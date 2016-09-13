powershell Set-ExecutionPolicy RemoteSigned

net start "BuyDRM_KeyOS_Windows_Packager_Service" 
del d:\KeyOSEncryptionServer\ServerTester\test\in\* /F /Q
del D:\KeyOSEncryptionServer\ServerTester\test\out\* /F /Q
del C:\KeyOSEncryptionServer\Working\Incoming\* /F /Q
del c:\logs\* /F /Q
powershell c:\scripts\DRMWrap.ps1 %Type% %Disney% %StartDate% %FinalFolder% %Airline%




