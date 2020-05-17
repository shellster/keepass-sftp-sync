using System;
using System.Linq;
using System.IO;
using Renci.SshNet;
using System.Net;
using KeePassLib.Serialization;
using System.Diagnostics;
using System.Collections.Generic;
using Renci.SshNet.Common;

namespace SftpSync
{
	public class SftpWebRequest: WebRequest, IHasIocProperties
    {
		
		private readonly Uri m_uri;
        private List<byte> m_reqBody = new List<byte>();
        private byte[] m_fingerprint;

		public override Uri RequestUri {
			get {
				return m_uri;
			}
		}

		private string m_strMethod = string.Empty;

		public override string Method
		{
			get { return m_strMethod; }
			set
			{
				if(value == null) throw new ArgumentNullException("value");
				m_strMethod = value;
			}
		}

        private WebHeaderCollection m_whcHeaders = new WebHeaderCollection();

        public override WebHeaderCollection Headers
        {
            get { return m_whcHeaders; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                m_whcHeaders = value;
            }
        }

        private long m_lContentLength = 0;

		public override long ContentLength
		{
			get { return m_lContentLength; }
			set
			{
				if(value < 0) throw new ArgumentOutOfRangeException("value");
				m_lContentLength = value;
			}
		}

		private string m_strContentType = string.Empty;

		public override string ContentType
		{
			get { return m_strContentType; }
			set
			{
				if(value == null) throw new ArgumentNullException("value");
				m_strContentType = value;
			}
		}

		private ICredentials m_cred = null;
		public override ICredentials Credentials
		{
			get { return m_cred; }
			set { m_cred = value; }
		}

		private bool m_bPreAuth = true;

		public override bool PreAuthenticate
		{
			get { return m_bPreAuth; }
			set { m_bPreAuth = value; }
		}

		private IWebProxy m_prx = null;

		public override IWebProxy Proxy
		{
			get { return m_prx; }
			set { m_prx = value; }
		}

		private IocProperties m_props = new IocProperties();

		public IocProperties IOConnectionProperties
		{
			get { return m_props; }
			set
			{
				if(value == null) { Debug.Assert(false); return; }
				m_props = value;
			}
		}

		public SftpWebRequest(Uri uri)
		{
			if(uri == null) throw new ArgumentNullException("uri");
			m_uri = uri;
		}

		public override Stream GetRequestStream()
		{
            m_reqBody.Clear();
            return new CopyMemoryStream(m_reqBody);
		}
        
        public override WebResponse GetResponse()
        {
            NetworkCredential cred = (m_cred as NetworkCredential);
            string strUser = ((cred != null) ? cred.UserName : null);
            string strPassword = ((cred != null) ? cred.Password : null);

            BaseClient m_Client = null;

            int l_port = m_uri.Port == -1 ? 22 : m_uri.Port;

            Uri uriTo = null;
            
            if (m_strMethod == KeePassLib.Serialization.IOConnection.WrmMoveFile) uriTo = new Uri(m_whcHeaders.Get(
                        IOConnection.WrhMoveFileTo));

            MemoryStream reqStream = null;
            if (m_reqBody.Count > 0) reqStream = new MemoryStream(m_reqBody.ToArray());

            KeyboardInteractiveAuthenticationMethod v_kauth = new KeyboardInteractiveAuthenticationMethod(strUser);
            v_kauth.AuthenticationPrompt += SftpWebRequest_AuthenticationPrompt;
            PasswordAuthenticationMethod v_pauth = new PasswordAuthenticationMethod(strUser, strPassword);

            ConnectionInfo n_con_info = new ConnectionInfo(m_uri.Host, l_port, strUser, v_pauth, v_kauth);
            m_Client = new SftpClient(n_con_info);

            //Set timeout to reasonable setting of 30 seconds for default.
            int connectionTimeout = 30000;

            if (m_props.Get("SSHTimeout") != null)
            {
                int.TryParse(m_props.Get("SSHTimeout"), out connectionTimeout);
            }
            
            m_Client.ConnectionInfo.Timeout = new TimeSpan(0, 0, 0, 0, connectionTimeout);

            if (m_props.Get("HostKey") != null)
            {
                string[] v_ssh_dss_parts = m_props.Get("HostKey").Split(':');
                if (v_ssh_dss_parts.Length != 16) throw new Exception("Input incorrect host fingerprint. Check it. Must look like: 12:34:56:78:90:ab:cd:ef:12:34:56:78:90:ab:cd:ef");
                List<byte> v_ssh_dss_parts_b = new List<byte>();
                foreach (string str in v_ssh_dss_parts)
                    {
                    try
                    {
                        v_ssh_dss_parts_b.Add(byte.Parse(str, System.Globalization.NumberStyles.AllowHexSpecifier));
                    } 
                    catch (Exception)
                    {
                        throw new Exception("Input incorrect host fingerprint. Check it. Must look like: 12:34:56:78:90:ab:cd:ef:12:34:56:78:90:ab:cd:ef");

                    }
                }
                m_fingerprint = v_ssh_dss_parts_b.ToArray();
                m_Client.HostKeyReceived += M_Client_HostKeyReceived;

            }
            
            return new SftpWebResponse(m_Client, m_strMethod, m_uri, uriTo, reqStream);
        }

        private void SftpWebRequest_AuthenticationPrompt(object sender, Renci.SshNet.Common.AuthenticationPromptEventArgs e)
        {
            foreach (AuthenticationPrompt prompt in e.Prompts)
            {
                if (prompt.Request.IndexOf("Password:", StringComparison.InvariantCultureIgnoreCase) != -1)
                {
                    prompt.Response = (m_cred as NetworkCredential).Password;
                }
            }
        }

        private void M_Client_HostKeyReceived(object sender, Renci.SshNet.Common.HostKeyEventArgs e)
        {
            e.CanTrust = e.FingerPrint.SequenceEqual(m_fingerprint) ?true: false;
        }
    }
}
