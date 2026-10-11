 param (
    [Parameter(Mandatory=$true)][string]$version
 )
dotnet publish --os linux --arch x64 -c Release /p:PublishProfile=DefaultContainer /p:Version=$version
