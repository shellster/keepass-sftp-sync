Keepass SftpSync
================

This tool is based on (and primarily composed of) the work done by: Vitaly Burkut.
The original tool's source can be found here: https://sourceforge.net/projects/keepass-sftp-sync/

This version of the tool contains minor code clean-ups, and one major change.
The previous verions will timeout on a slow network connection, this version adds a 2 minute timeout, so even on an extremely slow network connection, you can still access your KeePass wallet.
In addition, this version is compiled against .NET 4.0.

I hope to continue development of this addon, as the original developer has not made any updates since 2016.

Change Log
--------------
16.05.2020	2.1: Added configurable SSH connection timeout under advanced options, added update checking capabilities.

14.05.2020: 2.0: Fixed some spelling errors, updated SSHNet Lib version, added extended timeout for connections.

24.12.2016:	1.0: Support sftp protocol, for sync keepass db

27.12.2016: 1.1: Add support scp, add keyboard authenticate, add check host key