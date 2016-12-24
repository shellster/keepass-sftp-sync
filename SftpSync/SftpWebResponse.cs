/*
 * Created by SharpDevelop.
 * User: Vitaly
 * Date: 20.12.2016
 * Time: 23:30
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Renci.SshNet;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace SftpSync
{
    /// <summary>
    /// Description of SftpWebResponse.
    /// </summary>
    public class SftpWebResponse : WebResponse
    {
        private Stream m_sResponse = null;
        private readonly string m_method = String.Empty;
        private readonly SftpClient m_sftpClient = null;
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
        public SftpWebResponse(SftpClient p_sftpcl, string p_method, Uri uriResponse, Uri uriMoveTo, Stream p_InStream)
        {
            m_uriResponse = uriResponse;
            m_method = p_method;
            m_sftpClient = p_sftpcl;
            m_sReqStream = p_InStream;
            m_uriMoveTo = uriMoveTo;
            m_whc.Add("ServerInfo", m_sftpClient.ConnectionInfo.ServerVersion.ToString());
            m_sResponse = doAction();

        }
        private Stream doAction()
        {
            m_sResponse = new MemoryStream();

            if (m_method == KeePassLib.Serialization.IOConnection.WrmDeleteFile && m_sftpClient.Exists(m_uriResponse.LocalPath))
            {
                m_sftpClient.DeleteFile(m_uriResponse.LocalPath);
            }
            else if (m_method == KeePassLib.Serialization.IOConnection.WrmMoveFile)
            {
                if (m_uriMoveTo == null) throw new ArgumentNullException("uriMoveTo");
                m_sftpClient.RenameFile(m_uriResponse.LocalPath, m_uriMoveTo.LocalPath);
            }
            else if (m_sReqStream == null)
            {


                m_lSize = m_sftpClient.GetAttributes(m_uriResponse.LocalPath).Size;
                m_sftpClient.DownloadFile(m_uriResponse.LocalPath, m_sResponse);
                // Debug.Assert(m_sResponse.Length != m_lSize);				
            }
            else if (m_method == "POST")
            {
                if (m_sReqStream == null) throw new ArgumentNullException("m_sReqStream");
                m_lSize = 0;
              
                m_sftpClient.UploadFile(m_sReqStream, m_uriResponse.LocalPath);
                var s = m_sftpClient.GetAttributes(m_uriResponse.LocalPath).Size;
                //Debug.Assert(m_sReqStream.Length != s);

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
