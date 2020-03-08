param(
    [String] $Version
)

$Date = [System.DateTime]::Now.ToString("yyyyMMddHHmmss")
$util = Get-Content .\LyraShell\Util.cs -Raw
$util = $util.Replace("{Version}", $Version).Replace("{BuildDate}", $Date);

Set-Content -Path .\LyraShell\Util.cs $util