# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## О проекте

TwilightRandom — рандомизатор фракций/цветов/порядка хода для настольной игры «Сумерки Империи» (Twilight Imperium, по домашним правилам): каждый игрок получает две фракции на выбор. Решение на .NET 10: веб-приложение на Razor Pages, слой доступа к данным на Postgres и отдельная консольная версия.

## Структура решения

- `TwilightRandom/` — ядро алгоритма рандомизации (`Randomiser`, `GameRequest`, `RandomizeResult`). Чистая логика, без ввода-вывода.
- `Twilight.Domain/` — общая доменная модель: `Game`, `Player`, `Faction`, `PlayerColor`, `AllianceMode`, `DefaultData` (данные по умолчанию), `SlugGenerator`, `IGameRepository`.
- `Twilight.Dal/` — доступ к данным через EF Core (`TwilightDbContext`) и Npgsql, `GameRepository`, миграции в `Migrations/`. `Registration.AddTwilightDal(services, configuration)` подключает `DbContext` по строке подключения `TwilightDb`.
- `Twilight.Web/` — основное деплоимое приложение: ASP.NET Core Razor Pages, health-check на `/health/live`, использует `AddTwilightDal`. Здесь же `build.ps1` для ручного/локального деплоя.
- `Twilight.Migrator/` — отдельный воркер, который применяет EF Core миграции при старте (`MigrationsLauncher`, `IMigratorService`/`MigrateEfCoreHostService<TwilightDbContext>`); собирается в отдельный контейнер и запускается перед `Twilight.Web` (или вместе с ним) как шаг деплоя.
- `Twilight.Console/` — старая консольная версия: читает игроков из `twinlight_players.ini` через `ConfigLoader` и печатает результат в консоль вместо сохранения в БД.

Поток данных: `Randomiser` (из `TwilightRandom`) принимает `GameRequest` + список `Faction` + `AllianceMode` и возвращает `RandomizeResult`; `Twilight.Web` и `Twilight.Console` — это два разных интерфейса над одним и тем же ядром алгоритма, при этом `Twilight.Web` дополнительно сохраняет результат через `Twilight.Dal`.

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
docker-compose up -d db      # postgres:13.2, db=twilight, user=twilightuser/twilightpass, порт 5432
```

Запуск веб-приложения или консольного приложения: `dotnet run --project Twilight.Web` / `--project Twilight.Console`.

### Миграции EF Core

`dotnet-ef` — локальный tool, зафиксирован в `.config/dotnet-tools.json` (сначала `dotnet tool restore`). Команды миграций выполняются из `Twilight.Dal/` (или с `--project Twilight.Dal --startup-project Twilight.Migrator`), например:
```
dotnet tool restore
dotnet ef migrations add <Name> --project Twilight.Dal --startup-project Twilight.Migrator
```
В деплое `Twilight.Migrator` применяет накопившиеся миграции автоматически при старте против `TwilightDbContext` — он должен отрабатывать один раз перед `Twilight.Web` (или вместе с ним), а не наоборот.

## CI/CD (`.github/workflows/build-and-publish.yml`)

При push/PR: restore → проверка `dotnet format` → сборка `Twilight.Web` и `Twilight.Migrator` под `linux-x64` → `dotnet test`. При сборках не из PR (push в `master`, теги `v**`) дополнительно выполняется вход в Yandex Container Registry и публикация `Twilight.Web` и `Twilight.Migrator` как контейнеров через `dotnet publish -p:PublishProfile=DefaultContainer`. Версия вычисляется внешним скриптом в стиле GitVersion (`SemVerCalc_GitHubFlow_Actions.ps1`), а `ContainerImageTags` в `Directory.Build.props` всегда проставляет образам теги `latest` и версию.

Манифесты Kubernetes лежат в `manifests/` в виде kustomize-оверлея: `manifests/base/` (`deployment.yaml`, `service.yaml`, `ingress.yaml`, `migration-job.yaml`, заглушка `twilight-secret.env` для секрета) и `manifests/twilight/` — оверлей, который проставляет `namespace: twilight` и метку `managed-by-kustomize: "true"` поверх `../base`. В базовых манифестах образы указаны как плейсхолдеры без registry/тега (`twilight-web`, `twilight-migrator`), а строка подключения приходит из Secret `twilight-secret`, сгенерированного kustomize `secretGenerator` — реальные значения в оба места подставляет CI, локальная сборка `kubectl kustomize manifests/twilight` работает на пустых плейсхолдерах. `migration-job.yaml` — одноразовый `Job`, который прогоняет миграции перед обновлением `Twilight.Web`.

Деплой автоматический: `.github/workflows/deploy-prod-auto.yml` запускается по `workflow_run` после успешной сборки `master` (или вручную через `workflow_dispatch`) и вызывает переиспользуемый `.github/workflows/deploy-common.yml`, который: логинится в Yandex Container Registry, вычисляет digest только что собранных образов `twilight-web`/`twilight-migrator`, пишет секрет из GitHub Secrets с префиксом `KUBESECRET_` (например `KUBESECRET_ConnectionStrings__TwilightDb`) в `.env`, дописывает в `manifests/twilight/kustomization.yaml` блоки `secretGenerator`/`images` с реальными значениями, применяет `twilight-migrator` через `kubectl replace --force` и ждёт его завершения, затем `kubectl apply --prune -l managed-by-kustomize=true` остальных ресурсов и `kubectl rollout status`. Для работы нужны GitHub Secrets `YANDEX_CR_SA_KEY_JSON`, `KUBECONFIG` и `KUBESECRET_ConnectionStrings__TwilightDb`. `Twilight.Web/build.ps1` — вспомогательный скрипт для ручной локальной публикации образа (`dotnet publish`); ручной `kubectl apply -k manifests/twilight` без шагов CI не сработает, т.к. плейсхолдеры образов и пустой секрет не заменяются автоматически.

## Заметки по проекту

- `Directory.Build.props` и `Directory.Packages.props` задают общие для решения настройки (централизованное управление версиями пакетов, `net10.0`, nullable/implicit usings включены, `UseArtifactsOutput`, пакет `ReferenceTrimmer`). Версии пакетов добавляй в `Directory.Packages.props`, а не в отдельные `.csproj`.
- CI проверяет `dotnet format` с `--severity error` — запускай его перед коммитом, чтобы не ловить падение CI.

## Общие правила работы

- **Регулярные выражения**: избегать по возможности, искать решение без regex.
- **Коммиты**: создавать через `git commit --edit -m "предложенное сообщение"`, чтобы редактор открылся и сообщение можно было поправить перед сохранением.
- **PR в контексте GitHub Actions**: если задача выполняется через claude-code-action (т.е. в GitHub-контексте, не в интерактивной сессии), после коммита сразу создавать PR через `gh pr create`, не дожидаясь отдельной просьбы.
- **Фикс багов**: при исправлении бага рассматривать добавление юнит-теста, воспроизводящего баг до фикса и проходящего после. В решении сейчас нет тестового проекта — если для теста его ещё не существует, сначала нужно создать (например `TwilightRandom.Tests`) и подключить к `TwilightRandom.slnx`.
