
function sendresults
    {

    $From = "William.Lasiewicz@bydeluxe.com"
#$To = @("<Jim.Lichnerowicz@bydeluxe.com>" , "<Shalaleh.Shabnam@bydeluxe.com>","<William.Lasiewicz@bydeluxe.com>" , "<Stephen.Zupan@bydeluxe.com>", "<Abhay.Shekatkar@bydeluxe.com>")
$To = "William.Lasiewicz@bydeluxe.com"
$Cc = "William.Lasiewicz@bydeluxe.com"
$pwd = ConvertTo-SecureString ‘deluxemedia’ -AsPlainText -Force 
$cred = New-Object System.Management.Automation.PSCredential "DeluxeMediaWrap",$pwd
$param = @{ 
    SmtpServer = ‘smtp.gmail.com’ 
  #  Port = 587 
    UseSsl = $true 
    Credential = $cred 
    From = $From 
    To = $To
    Subject = $subject 
     Attachments=$atth
    Body = $Body
    } 

Send-MailMessage @param

    }

$JobNum=""
$JobNum=$args[0]

$atth="c:\logs\error.txt"
$logpath="C:\logs\FileCompare.log"
$txt= "job " + $JobNum
$subject=$txt
 $Body =   " File compare has errors " + $txt
  if (Test-Path $logpath)
  {
 $atth=$logpath
 $Body=""
 $test=Get-Content C:\logs\FileCompare.log | Select-String "perfect"
 if ($test)
 {
    $Body =   " File compare report is perfect " + $txt

 }


 }
 $subject="Job Results  "
 Write-Host $Body 
 Write-Host $subject

 sendresults