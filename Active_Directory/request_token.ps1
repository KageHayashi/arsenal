Add-Type -AssemblyName System.IdentityModel
New-Object System.IdentityModel.Tokens.KerberosRequestorSecurityToken -ArgumentList 'ldap/DC01.corp.com/ForestDnsZones.corp.com'