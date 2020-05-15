using System;
using System.Collections.Generic;

using KeePass.Plugins;
using KeePassLib.Serialization;
using KeePass.Ecas;
using KeePassLib;

namespace SftpSync
{
	public sealed class SftpSyncExt : Plugin
	{
		private IPluginHost m_host = null;
        private static bool m_bPropRegistered = false;
        private SftpWebRequestCreator m_sftpCr = null;

        public override bool Initialize(IPluginHost host)
		{
			m_host = host;
      
            m_sftpCr = new  SftpWebRequestCreator();
            m_sftpCr.Register();
            RegisterIocProperties();   

            return true;
		}

		public override void Terminate()
        {
            if (m_host != null)
                m_host = null;
        }

        private static void RegisterIocProperties()
		{
			if(m_bPropRegistered) return;
			m_bPropRegistered = true;

			string[] vScpSftp = new string[] { "SCP", "SFTP" };

            IocPropertyInfoPool.Add(new IocPropertyInfo("HostKey",
                            typeof(string), "Fingerprint of expected SSH host key", vScpSftp));
		}
    }
}