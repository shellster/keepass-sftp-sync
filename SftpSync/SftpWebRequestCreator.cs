using System;
using System.Net;
using System.Windows.Forms;

namespace SftpSync
{
	public sealed class SftpWebRequestCreator: IWebRequestCreate	
	{
		public void Register() {
            try
            {
                WebRequest.RegisterPrefix("sftp", this);
            } catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error on Register", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

		}
		
		public WebRequest Create(Uri uri) {
            try
            {
                return new SftpWebRequest(uri);
            } catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error on Create", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
		}
	}
}
