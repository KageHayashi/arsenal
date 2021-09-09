<# This Powershell Script tries to unravel users in nested groups within a given root Group. #>

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
$Searcher.filter = "name=Secret_Group"
$Result = $Searcher.FindAll()

function find_nested($result)
{
    Foreach($obj in $result)
    {
        $s_1 = $($obj.Properties.member -split "=")[1]
        if ($s_1)
        {
            $nested_group = $($s_1 -split ",")[0]
            $nested_group
            $Searcher.filter = "name=" + $nested_group
            $Result = $Searcher.FindAll()
            find_nested($Result)
        }
    }
}

find_nested($Result)