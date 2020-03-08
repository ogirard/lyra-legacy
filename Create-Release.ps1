param(
    [String] $ReleaseName = "Lyra-Release.zip"
)
if (Test-Path $ReleaseName) { 
    Remove-Item $ReleaseName -ErrorAction Ignore
}

Remove-Item .\LyraShell\bin\Release\store\ -Force -Recurse -ErrorAction Ignore

Compress-Archive -Path .\LyraShell\bin\Release -DestinationPath .\$ReleaseName