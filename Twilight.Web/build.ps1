 param (
    [Parameter(Mandatory=$true)][string]$version
 )
dotnet publish --os linux --arch x64 -c Release /p:PublishProfile=DefaultContainer /p:Version=$version
docker tag twilight-web:$version cr.yandex/crp3fr717nr1rn78qeij/twilight-web:$version
docker push cr.yandex/crp3fr717nr1rn78qeij/twilight-web:$version
#ฯฮฬลอ฿าÜ ย DEPLOYMENT.YML
#kubectl apply -f .\deployment.yml -n twilight