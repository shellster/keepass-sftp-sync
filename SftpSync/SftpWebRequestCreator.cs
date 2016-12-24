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

namespace SftpSync
{
	/// <summary>
	/// Description of SftpWebRequestCreator.
	/// </summary>
	public sealed class SftpWebRequestCreator: IWebRequestCreate
		
	{
		private static readonly string[] m_vSupportedSchemes = new string[] {
			"sftp"
		};
		
		public void Register() {
		
			foreach(string strPrefix in m_vSupportedSchemes)
				WebRequest.RegisterPrefix(strPrefix, this);
			
		}
		
		public WebRequest Create(Uri uri){
			
			return new SftpWebRequest(uri);
			
		}
		
		
	}
}
