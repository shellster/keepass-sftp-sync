Keepass SftpSync
================

This tool is based on (and primarily composed of) the work done by: Vitaly Burkut.
The original tool's source can be found here: https://sourceforge.net/projects/keepass-sftp-sync/

This version of the tool contains minor code clean-ups, and one major change.
The previous verions will timeout on a slow network connection, this version adds a 2 minute timeout, so even on an extremely slow network connection, you can still access your KeePass wallet.
In addition, this version is compiled against .NET 4.0.

I hope to continue development of this addon, as the original developer has not made any updates since 2016.

SSH Private Key Support
-----------------------

As of verion 2.2, this plugin supports authentication via SSH Private Key. There are some pitfalls with this authentication method. Please read this section carefully.

The SSHNet library (the SSH library this plugin uses) only supports an older SSH Private Key format.  If you use ssh-keygen to create your key IT MOST LIKELY WILL NOT WORK directly.

To convert your key to a format that this plugin can use, load your private key into PuttyGen (https://www.chiark.greenend.org.uk/~sgtatham/putty/latest.html), then click `Conversions -> Export OpenSSH key`.

The new key is now in a format you can use with this plugin.  To use it, copy the contents of the exported key file into the new `SSH Private Key` field under the `Advanced` tab. Though not visible in the field, the newlines need to be present as they appeared in the key file (and should be if you copied and pasted the key file content directly).  Alternatively, you can replace the new line characters with "\n" in the key string that you pasted.  

If your key is password protected, you should place the key password in the normal `Connection` tab `Password` field.  

Not all SSH Private Key files may be supported.  A list of known working types can be found here: https://github.com/sshnet/SSH.NET/tree/546e2b9ece47f1982811a0bcdafa93fec7c5d0e3/src/Renci.SshNet.Tests/Data

SSH Pageant Support
-------------------

If you are using Windows (Currently only Windows is supported), this plugin suppports Pageant (as of version 2.3). The plugin will automatically attempt to use Pageant if a SFTP url is entered, and no SSH key or password is specified.

Change Log
--------------
14.07.2020      2.3: Updated SSH.NET verion.  Added Pageant Support for Windows (Thanks to @kins-dev)

28.05.2020	2.2: Added SSH Private Key Support.

16.05.2020	2.1: Added configurable SSH connection timeout under advanced options, added update checking capabilities.

14.05.2020:     2.0: Fixed some spelling errors, updated SSHNet Lib version, added extended timeout for connections.

24.12.2016:	1.0: Support sftp protocol, for sync keepass db

27.12.2016: 1.1: Add support scp, add keyboard authenticate, add check host key
