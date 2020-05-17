using System;
using System.Net;
using KeePassLib.Serialization;
using System.Windows.Forms;

namespace SftpSync
{
	public sealed class SftpWebRequestCreator: IWebRequestCreate	
	{
		private static readonly string[] m_vSupportedSchemes = new string[] {
			"sftp" , "scp"
		};
		
		public void Register() {
            try
            {
                foreach (string strPrefix in m_vSupportedSchemes)
                    WebRequest.RegisterPrefix(strPrefix, this);

                // scp not support operation move and delete. Then sync via scp, do without transaction (direct write to target remote file)
                FileTransactionEx.Configure("scp", false);
            } catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error on Register", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

		}
		
		public WebRequest Create(Uri uri) {
			return new SftpWebRequest(uri);
		}
	}
}
