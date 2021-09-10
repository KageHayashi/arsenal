# This script attempts to get the clear key of all WLAN profiles saved on the computer

$all_profiles = netsh wlan show profiles | Select-String '\:(.+)$'
$all_clear = @()
$no_clear = @()

foreach ($profile in $all_profiles) {
    $name = $profile.Matches.Groups[1].Value.Trim();
    $pass_match = netsh wlan show profile name=$name key=clear | Select-String 'Key Content\W+\:(.+)$';
    try {
        $pass = $pass_match.Matches.Groups[1].Value.Trim();
        $all_clear += ($name + ':' + $pass);
    }
    catch {
        $no_clear += ($name)
    }
}
Write-Host 'All Clear Keys Saved'
Write-Host '--------------------'
$all_clear
Write-Host 
Write-Host 'No Clear Keys Saved'
Write-Host '-------------------'
$no_clear