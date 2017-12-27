/*
 * Created by SharpDevelop.
 * User: Vitaly
 * Date: 19.12.2016
 * Time: 20:12
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

using KeePass.Plugins;
using KeePassLib.Serialization;
using KeePass.Ecas;
using KeePassLib;

namespace SftpSync
{
	/// <summary>
	/// Description of MyClass.
	/// </summary>
	public sealed class SftpSyncExt : Plugin
	{
		private IPluginHost m_host = null;
        private static bool m_bPropRegistered = false;
        private SftpWebRequestCreator m_sftpCr = null;
       
   

        public override bool Initialize(IPluginHost host)
		{
			//try{
			m_host = host;
      
            m_sftpCr = new  SftpWebRequestCreator();
            m_sftpCr.Register();
            RegisterIocProperties();   

            return true;
			//}
			// catch (Exception e) 
		//	{
		//		MessageBox.Show(e.ToString(), "Plugin initialize error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
		//	}
			
		}
		public override void Terminate()
{
            if (m_host != null)
            {
                               m_host = null;
            }
        }
        private static void RegisterIocProperties()
		{
			if(m_bPropRegistered) return;
			m_bPropRegistered = true;

			string[] vScpSftp = new string[] { "SCP", "SFTP" };
       


            IocPropertyInfoPool.Add(new IocPropertyInfo("HostKey",
                            typeof(string), "Fingerprint of expected SSH host key", vScpSftp));

            /* later...
            IocPropertyInfoPool.Add(new IocPropertyInfo("PrivateKey",
				typeof(string), "SSH private key path", vScpSftp));
			IocPropertyInfoPool.Add(new IocPropertyInfo("Passphrase",
				typeof(string), "Passphrase for encrypted private keys and client certificates",
                vScpSftp));
                */
		}

}
}