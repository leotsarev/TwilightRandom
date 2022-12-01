#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Twilight.Web/Twilight.Web.csproj", "Twilight.Web/"]
RUN dotnet restore "Twilight.Web/Twilight.Web.csproj"
COPY . .
WORKDIR "/src/Twilight.Web"
RUN dotnet build "Twilight.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Twilight.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Twilight.Web.dll"]