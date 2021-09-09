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

<# Filter search. Refer to samAccountType filter: https://docs.microsoft.com/en-us/windows/win32/adschema/a-samaccounttype?redirectedfrom=MSDN#>
<# Current filter enumerates for all users in the AD #>
$Searcher.filter="samAccountType=805306368"

<# We could also use the following to filter for a specific user name #>
# $Searcher.filter="name=admin"

$Result = $Searcher.FindAll()

Foreach($obj in $Result)
{
    Foreach($prop in $obj.Properties)
    {
    	$prop
    }

    Write-Host "----------------------------"
}