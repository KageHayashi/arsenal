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

<# Filter search. Refer to samAccountType filter #>
$Searcher.filter="samAccountType=805306368"

$Searcher.FindAll()