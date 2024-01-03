#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["QuizMaster.API.Monitoring/QuizMaster.API.Monitoring.csproj", "QuizMaster.API.Monitoring/"]
COPY ["QuizMaster.Library.Common/QuizMaster.Library.Common.csproj", "QuizMaster.Library.Common/"]
RUN dotnet restore "QuizMaster.API.Monitoring/QuizMaster.API.Monitoring.csproj"
COPY . .
WORKDIR "/src/QuizMaster.API.Monitoring"
RUN dotnet build "QuizMaster.API.Monitoring.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "QuizMaster.API.Monitoring.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY QuizMaster.API.Monitoring/localhost_cert.pfx ./localhost_cert.pfx
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QuizMaster.API.Monitoring.dll", "https://*:6004"]