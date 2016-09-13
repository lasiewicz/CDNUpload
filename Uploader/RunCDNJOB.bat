powershell Set-ExecutionPolicy RemoteSigned
IF %JobNumber%=="No Jobs to Run" echo No Jobs to Run
IF %JobNumber%=="No Jobs to Run" goto end
echo running job %%JobNumber%
del c:\logs\CNDUploader.log
C:\Scripts\Uploader.exe %JobNumber%


type c:\logs\CNDUploader.log
C:\Scripts\Uploader.exe %JobNumber% compare
powershell c:\scripts\wait.ps1

copy c:\jobs\%JobNumber% c:\completedjobs\%JobNumber%
del c:\jobs\%JobNumber%
copy c:\logs\FileCompare.log c:\completedjobs\%JobNumber%FileCompare.log
powershell c:\scripts\sendresults.ps1 %JobNumber%
powershell c:\scripts\wait.ps1
del c:\logs\FileCompare.log
del c:\logs\CNDUploader.log
powershell c:\scripts\wait.ps1
:end
