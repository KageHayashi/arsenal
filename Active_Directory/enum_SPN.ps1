# This script attempts to find all SPNs and resolve their IPs with nslookup

$domainObj = [System.DirectoryServices.ActiveDirectory.Domain]::GetCurrentDomain()

<#Primary Domain Controller#>
$PDC = ($domainObj.PdcRoleOwner).Name

<# Contruct search string in the format 'LDAP://HostName[:PortNumber][/DistinguishedName]'#>
$SearchString = "LDAP://"
$SearchString += $PDC + "/"

$DistinguishedName = "DC=$($domainObj.Name.Replace('.', ',DC='))"
$SearchString += $DistinguishedName

<# Create DirectorySearcher object #>
$Searcher = New-Object System.DirectoryServices.DirectorySearcher([ADSI]$SearchString)

$objDomain = New-Object System.DirectoryServices.DirectoryEntry

$Searcher.SearchRoot = $objDomain

# Finds any SPN with HTTP in the name
$Searcher.filter="serviceprincipalname=*"

$Result = $Searcher.FindAll()

Foreach($obj in $Result)
{
    $spn = $obj.Properties.serviceprincipalname
    $spn
    $server = $spn | Select-String "\/(.+)$"
    $server = $server.Matches.Groups[1].Value
}