using KeePass.Plugins;
using KeePassLib.Serialization;

namespace SftpSync
{
	public sealed class SftpSyncExt : Plugin
	{
		private IPluginHost m_host = null;
        private static bool m_bPropRegistered = false;
        private SftpWebRequestCreator m_sftpCr = null;

        public override string UpdateUrl
        {
            get { return "https://raw.githubusercontent.com/shellster/keepass-sftp-sync/master/version.txt"; }
        }

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

			string[] vScpSftp = new string[] { "SFTP" };

            IocPropertyInfoPool.Add(new IocPropertyInfo("HostKey",
                            typeof(string), "Fingerprint of expected SSH host key", vScpSftp));

            IocPropertyInfoPool.Add(new IocPropertyInfo("SSHTimeout",
                            typeof(string), "SSH Connection Timeout [ms]", vScpSftp));

            IocPropertyInfoPool.Add(new IocPropertyInfo("SSHKey",
                            typeof(string), "SSH Private Key", vScpSftp));
        }
    }
}