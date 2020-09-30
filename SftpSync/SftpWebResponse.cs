using System;
using Renci.SshNet;
using System.Net;
using System.IO;

namespace SftpSync
{
    public class SftpWebResponse : WebResponse
    {
        private Stream m_sResponse = null;
        private readonly string m_method = String.Empty;
        private readonly BaseClient m_sftpClient = null;
        private readonly Stream m_sReqStream = null;

        private long m_lSize = 0;
        public override long ContentLength
        {
            get { return m_lSize; }
            set { throw new InvalidOperationException(); }
        }

        public override string ContentType
        {
            get { return "application/octet-stream"; }
            set { throw new InvalidOperationException(); }
        }

        private Uri m_uriResponse;
        private Uri m_uriMoveTo;

        public override Uri ResponseUri
        {
            get { return m_uriResponse; }
        }

        private WebHeaderCollection m_whc = new WebHeaderCollection();

        public override WebHeaderCollection Headers
        {
            get { return m_whc; }
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="p_sftpcl">sftp client object</param>
        /// <param name="p_method">method, value: post or move</param>
        /// <param name="uriResponse"> uri to get response</param>
        /// <param name="p_InStream"> input stream, if is not null then upload, else download</param>
        public SftpWebResponse(BaseClient p_sftpcl, string p_method, Uri uriResponse, Uri uriMoveTo, Stream p_InStream)
        {
            m_uriResponse = uriResponse;
            m_method = p_method;
            m_sftpClient = p_sftpcl;
            if (!m_sftpClient.IsConnected) m_sftpClient.Connect();
            m_sReqStream = p_InStream;
            m_uriMoveTo = uriMoveTo;
            m_whc.Add("ServerInfo", m_sftpClient.ConnectionInfo.ServerVersion.ToString());
            m_sResponse = doAction();
            m_sftpClient.Disconnect();

        }

        private Stream doAction()
        {
            m_sResponse = new MemoryStream();

            if (m_method == KeePassLib.Serialization.IOConnection.WrmDeleteFile)
            {
                if (m_sftpClient.GetType() == typeof(ScpClient)) throw new Exception("SCP not support method DELETE");

                if (((SftpClient)m_sftpClient).Exists(m_uriResponse.LocalPath)) ((SftpClient)m_sftpClient).DeleteFile(m_uriResponse.LocalPath);
            }
            else if (m_method == KeePassLib.Serialization.IOConnection.WrmMoveFile)
            {
                if (m_sftpClient.GetType() == typeof(ScpClient)) throw new Exception("SCP not support method MoveTo");
                if (m_uriMoveTo == null) throw new ArgumentNullException("uriMoveTo");
                ((SftpClient)m_sftpClient).RenameFile(m_uriResponse.LocalPath, m_uriMoveTo.LocalPath);
            }
            else if (m_sReqStream == null && m_method != "POST")
            {
                if (m_sftpClient.GetType() == typeof(SftpClient))
                    ((SftpClient)m_sftpClient).DownloadFile(m_uriResponse.LocalPath, m_sResponse);                
                else                
                    ((ScpClient)m_sftpClient).Download(m_uriResponse.LocalPath, m_sResponse);

                
                m_lSize = m_sResponse.Length;


            }
            else if (m_method == "POST")
            {
                if (m_sReqStream == null) throw new ArgumentNullException("m_sReqStream");
                m_lSize = 0;
                if (m_sftpClient.GetType() == typeof(SftpClient))
                    ((SftpClient)m_sftpClient).UploadFile(m_sReqStream, m_uriResponse.LocalPath);
                else
                    ((ScpClient)m_sftpClient).Upload(m_sReqStream, m_uriResponse.LocalPath);            

            } else
            {
                throw new Exception("mode not support");
            }

            string strTempFile = Path.GetTempFileName();
            File.WriteAllBytes(strTempFile, ((MemoryStream)m_sResponse).ToArray());

            return m_sResponse.Length > 0 ? (Stream)File.Open(strTempFile, FileMode.Open) : (Stream)m_sResponse;
        }

        public override Stream GetResponseStream()
        {
            return m_sResponse ?? doAction();
        }

        public override void Close()
        {
            if (m_sResponse != null) { m_sResponse.Close(); m_sResponse = null; }
        }
    }
}
