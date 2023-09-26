 param (
    [Parameter(Mandatory=$true)][string]$version
 )
dotnet publish --os linux --arch x64 -c Release /p:PublishProfile=DefaultContainer /p:Version=$version
#ฯฮฬลอ฿าÜ ย DEPLOYMENT.YML
#kubectl apply -f .\deployment.yml -n twilight