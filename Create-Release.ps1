if (Test-Path Lyra-Release.zip) { 
    Remove-Item Lyra-Release.zip --Force -ErrorAction Ignore
}

Remove-Item .\LyraShell\bin\Release\store\ -Force -Recurse -ErrorAction Ignore

Compress-Archive -Path .\LyraShell\bin\Release -DestinationPath Lyra-Release.zip