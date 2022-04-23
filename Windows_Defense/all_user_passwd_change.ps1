# Ask for user input
$passwd = Read-Host "Enter a password for ALL users on local system" -AsSecureString

# Get list of all local users
$users = Get-LocalUser | Select Name

# Change passwords for users
ForEach ($user in $users) {
    $username = ($user.name)
    Set-LocalUser -Name $username -Password $passwd
    Write-Output "[+] $username Password Updated."
}