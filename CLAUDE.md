# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## О проекте

TwilightRandom — рандомизатор фракций/цветов/порядка хода для настольной игры «Сумерки Империи» (Twilight Imperium, по домашним правилам): каждый игрок получает две фракции на выбор. Решение на .NET 10: веб-приложение на Razor Pages и слой доступа к данным на Postgres.

## Структура решения

- `TwilightRandom/` — ядро алгоритма рандомизации (`Randomiser`, `GameRequest`, `RandomizeResult`). Чистая логика, без ввода-вывода.
- `Twilight.Domain/` — общая доменная модель: `Game`, `Player`, `Faction`, `PlayerColor`, `AllianceMode`, `DefaultData` (данные по умолчанию), `SlugGenerator`, `IGameRepository`.
- `Twilight.Dal/` — доступ к данным через EF Core (`TwilightDbContext`) и Npgsql, `GameRepository`, миграции в `Migrations/`. `Registration.AddTwilightDal(services, configuration)` подключает `DbContext` по строке подключения `TwilightDb`.
- `Twilight.Web/` — основное деплоимое приложение: ASP.NET Core Razor Pages, health-check на `/health/live`, использует `AddTwilightDal`. Здесь же `build.ps1` для ручной публикации контейнера.
- `manifests/` — манифесты Kubernetes (kustomize): `base/` (Deployment, Service, HTTPRoute Gateway API в `ingress.yaml`, Job миграций) и оверлей окружения `twilight/`. Маршрут цепляется к Gateway `main` в неймспейсе `infra` (traefik); TLS и сертификаты — на этом Gateway, не в манифестах приложения.
- `Twilight.Migrator/` — отдельный воркер, который применяет EF Core миграции при старте (`MigrationsLauncher`, `IMigratorService`/`MigrateEfCoreHostService<TwilightDbContext>`); собирается в отдельный контейнер и запускается перед `Twilight.Web` (или вместе с ним) как шаг деплоя.

Поток данных: `Randomiser` (из `TwilightRandom`) принимает `GameRequest` + список `Faction` + `AllianceMode` и возвращает `RandomizeResult`; `Twilight.Web` — интерфейс над этим ядром алгоритма, дополнительно сохраняющий результат через `Twilight.Dal`.

Тестовых проектов в решении сейчас нет.

## Сборка и запуск

Требуется .NET SDK `10.0.100` (зафиксирован в `global.json`, `rollForward: latestFeature` — SDK не нужно обновлять в файле вручную, Dependabot обновляет только NuGet-пакеты).

```
dotnet restore
dotnet build
dotnet format --no-restore --verify-no-changes --severity error   # проверка стиля, как в CI
dotnet test -c Release --logger trx --results-directory ./TestResults
```

Postgres для локальной разработки:
```
docker-compose up -d db      # postgres:15.18, db=twilight, user=twilightuser/twilightpass, порт 5432
```

Запуск веб-приложения: `dotnet run --project Twilight.Web`.

### Миграции EF Core

`dotnet-ef` — локальный tool, зафиксирован в `.config/dotnet-tools.json` (сначала `dotnet tool restore`). Команды миграций выполняются из `Twilight.Dal/` (или с `--project Twilight.Dal --startup-project Twilight.Migrator`), например:
```
dotnet tool restore
dotnet ef migrations add <Name> --project Twilight.Dal --startup-project Twilight.Migrator
```
В деплое `Twilight.Migrator` применяет накопившиеся миграции автоматически при старте против `TwilightDbContext` — он должен отрабатывать один раз перед `Twilight.Web` (или вместе с ним), а не наоборот.

## CI/CD (`.github/workflows/build-and-publish.yml`)

При push/PR: restore → проверка `dotnet format` → сборка `Twilight.Web` и `Twilight.Migrator` под `linux-x64` → `dotnet test`. При сборках не из PR (push в `master`, теги `v**`) дополнительно выполняется вход в Selectel Container Registry (`cr.selcloud.ru`, секреты `SELECTEL_CR_USERNAME`/`SELECTEL_CR_PASSWORD`) и публикация `Twilight.Web` и `Twilight.Migrator` как контейнеров: `dotnet publish /t:PublishContainer` собирает образы `joinrpg/<имя>` в локальный docker, затем `docker push` отправляет их в `cr.selcloud.ru/joinrpg/<имя>` (прямой пуш из SDK в Selectel не работает: реестр отдаёт внутренний адрес в Location). Деплой берёт образы уже из Selectel, но сборка пока по-прежнему дублирует их в Yandex Container Registry (`cr.yandex/crpais914lemf2m60cja/*`, секрет `YANDEX_CR_SA_KEY_JSON`) — этот шаг осталось убрать. Версия вычисляется внешним скриптом в стиле GitVersion (`SemVerCalc_GitHubFlow_Actions.ps1`), а `ContainerImageTags` в `Directory.Build.props` всегда проставляет образам теги `latest` и версию.

Контейнеры деплоятся в Kubernetes вручную: workflow `.github/workflows/deploy-prod.yml` (workflow_dispatch, автодеплоя после сборки нет) вызывает `.github/workflows/deploy-reusable.yml`. Образы берутся из `cr.selcloud.ru/joinrpg/*` (digest по тегу), кластер тянет их через секрет `craas-auth` интеграции Selectel Managed Kubernetes с Container Registry. Деплой идёт из манифестов в `manifests/` (kustomize; строка подключения берётся из секрета `twilight-conn`, liveness probe на `/health/live`): сначала Job `twilight-migrator`, затем Deployment. `Twilight.Web/build.ps1` — вспомогательный скрипт для ручной публикации контейнера (`dotnet publish`).

## Заметки по проекту

- `Directory.Build.props` и `Directory.Packages.props` задают общие для решения настройки (централизованное управление версиями пакетов, `net10.0`, nullable/implicit usings включены, `UseArtifactsOutput`, пакет `ReferenceTrimmer`). Версии пакетов добавляй в `Directory.Packages.props`, а не в отдельные `.csproj`.
- CI проверяет `dotnet format` с `--severity error` — запускай его перед коммитом, чтобы не ловить падение CI.

## Общие правила работы

- **Регулярные выражения**: избегать по возможности, искать решение без regex.
- **Ветки и PR**: не коммитить и не пушить напрямую в `master`. Изменения делать в отдельной ветке и открывать PR через `gh pr create`, не дожидаясь отдельной просьбы (даже в интерактивной сессии).
- **Коммиты**: создавать через `git commit --edit -m "предложенное сообщение"`, чтобы редактор открылся и сообщение можно было поправить перед сохранением.
- **Фикс багов**: при исправлении бага рассматривать добавление юнит-теста, воспроизводящего баг до фикса и проходящего после. В решении сейчас нет тестового проекта — если для теста его ещё не существует, сначала нужно создать (например `TwilightRandom.Tests`) и подключить к `TwilightRandom.slnx`.
