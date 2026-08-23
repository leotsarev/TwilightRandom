 param (
    [Parameter(Mandatory=$true)][string]$version
 )
dotnet publish --os linux --arch x64 -c Release /p:PublishProfile=DefaultContainer /p:Version=$version
# Обычно деплой делает CI (.github/workflows/deploy-prod-auto.yml) автоматически
# после сборки master. Этот скрипт нужен только для ручной публикации образа;
# для ручного деплоя манифестов пришлось бы вручную подставить в
# ..\manifests\twilight\kustomization.yaml секции images:/secretGenerator:
# (см. .github/workflows/deploy-common.yml), а не просто kubectl apply -k.