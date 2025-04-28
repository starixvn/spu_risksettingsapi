FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-alpine AS builder

WORKDIR /src

COPY ["./src/Fanex.NAP.SPU.RiskSettingAPI", "Fanex.NAP.SPU.RiskSettingAPI/"]

COPY ["./src/Fanex.NAP.SPU.RiskSettingAPI.JobService/Fanex.NAP.SPU.RiskSettingAPI.JobService.csproj", "Fanex.NAP.SPU.RiskSettingAPI.JobService/"]

WORKDIR "/src/Fanex.NAP.SPU.RiskSettingAPI.JobService"

RUN dotnet nuget add source http://nuget.nexdev.net/api/v2

RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore -v n -r linux-x64

COPY ./src/Fanex.NAP.SPU.RiskSettingAPI.JobService .

RUN dotnet publish -r linux-x64 --self-contained false -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:8.0-alpine AS base

WORKDIR /app

COPY --from=builder /app .

ENTRYPOINT ["dotnet", "Fanex.NAP.SPU.RiskSettingAPI.JobService.dll"]

ENV TZ="America/La_Paz"
