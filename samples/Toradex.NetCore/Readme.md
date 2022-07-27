#### Publish for linux-arm64

`dotnet publish -c Release -o ./publish -r linux-arm64 -p:PublishReadyToRun=true -p:PublishSingleFile=true -p:PublishTrimmed=true --self-contained true`

### Journalctl with kernel time
`journalctl -o short-monotonic -b`

`systemctl list-unit-files --state=enabled`
