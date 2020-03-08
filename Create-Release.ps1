Remove-Item .\LyraShell\bin\Release\store\ -Force -Recurse -ErrorAction Ignore
Compress-Archive -Path .\LyraShell\bin\Release -DestinationPath Lyra-Release.zip