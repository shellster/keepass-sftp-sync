Keepass SftpSync
================

ARCHIVED 05/20/2024:
====================
As of the above date, I'm archiving this repo.  I no longer use this plugin myself, and as such do not have an easy way to test or maintain it.
Furthermore, the upstream project that this relies on SSH.NET does not seem to be interested in supporting non-Windows platforms, meanwhile the list of features and bugfixes continues
and I have no way to keep a unified codebase for all OS's.  I've attempted to patch upstream library to support the features I need that aren't compatible, but have no succeeded.  
Another issue is that at one point we supplied Putty support, but had to drop that because upstream patches were rejected.  In all this is becoming too painful for me to maintain
and I lack the time or interest to continue.  KeepassXC now supports KDBX v4 databases and has a built-in SSH agent.  I recommned people who want this functionality checkout that option.
If someone wants to take this project over, I'm open to the idea of transfering this repo, but I'm leery of someone of unknown reput hijacking it.  Feel free to open an issue to discuss.


This tool is based on (and primarily composed of) the work done by: Vitaly Burkut.
The original tool's source can be found here: https://sourceforge.net/projects/keepass-sftp-sync/

This version of the tool contains minor code clean-ups, and one major change.
The previous verions will timeout on a slow network connection, this version adds a 2 minute timeout, so even on an extremely slow network connection, you can still access your KeePass wallet.
In addition, this version is compiled against .NET 4.0.

I hope to continue development of this addon, as the original developer has not made any updates since 2016.

Connecting to your database through sftp can be set up with `Open URL...` menu option

SSH Private Key Support
-----------------------

As of verion 2.2, this plugin supports authentication via SSH Private Key. There are some pitfalls with this authentication method. Please read this section carefully.

The SSHNet library (the SSH library this plugin uses) only supports an older SSH Private Key format.  If you use ssh-keygen to create your key without specifying supported type IT MOST LIKELY WILL NOT WORK directly.

A list of known working types can be found here: https://github.com/sshnet/SSH.NET/tree/546e2b9ece47f1982811a0bcdafa93fec7c5d0e3/src/Renci.SshNet.Tests/Data

For example, u can create new private key with supported type like this: 
`ssh-keygen -t ed25519`

Or you can convert your key to a format that this plugin can use, just load your private key into PuttyGen (https://www.chiark.greenend.org.uk/~sgtatham/putty/latest.html), then click `Conversions -> Export OpenSSH key`. The new key is now in a format you can use with this plugin.

To use private key, copy the contents of the exported key file into the new `SSH Private Key` field under the `Advanced` tab. Though not visible in the field, the newlines need to be present as they appeared in the key file (and should be if you copied and pasted the key file content directly).  Alternatively, you can replace the new line characters with "\n" in the key string that you pasted.  

If your key is password protected, you should place the key password in the normal `Connection` tab `Password` field. 

SSH Pageant Support
-------------------

If you are using Windows (Currently only Windows is supported), this plugin suppports Pageant (as of version 2.3). The plugin will automatically attempt to use Pageant if a SFTP url is entered, and no SSH key or password is specified.

Change Log
--------------
26.10.2020  2.4.1: Minor Update to fix previous version bump mistake (This should stop you from getting alerts to update.)

27.09.2020  2.4: Explicitly close SSH session to prevent session hangs. 

14.07.2020  2.3: Updated SSH.NET verion.  Added Pageant Support for Windows (Thanks to @kins-dev)

28.05.2020	2.2: Added SSH Private Key Support.

16.05.2020  2.1: Added configurable SSH connection timeout under advanced options, added update checking capabilities.

14.05.2020  2.0: Fixed some spelling errors, updated SSHNet Lib version, added extended timeout for connections.

27.12.2016  1.1: Add support scp, add keyboard authenticate, add check host key

24.12.2016  1.0: Support sftp protocol, for sync keepass db
