/*
 * Created by SharpDevelop.
 * User: Vitaly
 * Date: 20.12.2016
 * Time: 21:43
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using Renci.SshNet;
using System.Net;
using KeePassLib.Serialization;
using System.Diagnostics;
using System.Collections.Generic;

namespace SftpSync
{
	/// <summary>
	/// Description of SftpWebRequest.
	/// </summary>
	public class SftpWebRequest: WebRequest, IHasIocProperties
    {
		private SftpClient m_SftpClient = null;
		private readonly Uri m_uri;
        private List<byte> m_reqBody = new List<byte>();
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
		//	m_SshClientc = new SftpClient
		
				
		}
		public override Stream GetRequestStream()
		{
            m_reqBody.Clear();
            return new CopyMemoryStream(m_reqBody);
		}
		//private WebResponse m_webresp = null;
		public override WebResponse GetResponse()
		{
			//if(m_wr != null) return m_wr;
			
			NetworkCredential cred = (m_cred as NetworkCredential);
			string strUser = ((cred != null) ? cred.UserName : null);
			string strPassword = ((cred != null) ? cred.Password : null);

            if (m_SftpClient == null)
            {
                m_SftpClient = m_uri.Port == -1 ? new SftpClient(m_uri.Host, strUser, strPassword) : new SftpClient(m_uri.Host, m_uri.Port, strUser, strPassword);
                m_SftpClient.Connect();
            }

            Uri uriTo = null;
            if (m_strMethod == KeePassLib.Serialization.IOConnection.WrmMoveFile) uriTo = new Uri(m_whcHeaders.Get(
                        IOConnection.WrhMoveFileTo));
            MemoryStream reqStream = null;
            if (m_reqBody.Count > 0)  reqStream = new MemoryStream(m_reqBody.ToArray());

            return  new SftpWebResponse(m_SftpClient, m_strMethod, m_uri, uriTo, reqStream);


        }

		
	}
}
