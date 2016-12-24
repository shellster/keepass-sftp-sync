/*
 * Created by SharpDevelop.
 * User: Vitaly
 * Date: 20.12.2016
 * Time: 21:00
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Net;
using System.Threading;
using Renci.SshNet;
using KeePassLib.Serialization; 

namespace SftpSync
{
	/// <summary>
	/// Description of SftpWebRequestCreator.
	/// </summary>
	public sealed class SftpWebRequestCreator: IWebRequestCreate
		
	{
		private static readonly string[] m_vSupportedSchemes = new string[] {
			"sftp" , "scp"
		};
		
		public void Register() {
		
			foreach(string strPrefix in m_vSupportedSchemes)
				WebRequest.RegisterPrefix(strPrefix, this);
            

            // scp not support operation move and delete. Then sync via scp, do withot transaction (direct write to target remote file)
            FileTransactionEx.Configure("scp", false);
			
		}
		
		public WebRequest Create(Uri uri){
			
			return new SftpWebRequest(uri);
			
		}
		
		
	}
}
