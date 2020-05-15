@echo off
echo "%CD%\..\bins\KeePass.exe" --plgx-create %cd%\SftpSync --plgx-prereq-kp:2.18 --plgx-prereq-net:4.0 --debug
call "%CD%\..\bins\KeePass.exe" --plgx-create %cd%\SftpSync --plgx-prereq-kp:2.18 --plgx-prereq-net:4.0 --debug