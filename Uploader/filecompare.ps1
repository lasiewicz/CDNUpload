$subject="Wrap complete "
$Body="files wrapped"
$ret= [char]13
function sendresults
    {

    $From = "William.Lasiewicz@bydeluxe.com"
$To = @( "<Shalaleh.Shabnam@bydeluxe.com>","<William.Lasiewicz@bydeluxe.com>" )
#$To = "William.Lasiewicz@bydeluxe.com"
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
    Body = $Sout
    } 

Send-MailMessage @param

    }

$Folder1=$args[0]
$Folder2=$args[1]
$fso = Get-ChildItem -Recurse -path $Folder1
$fsoBU = Get-ChildItem -Recurse -path $Folder2

Compare-Object -ReferenceObject $fso -DifferenceObject $fsoBU
$Sout=[string]$fsoBU
Write-Host ($Sout)
 $subject="Automated file compare  "
     
         sendresults

