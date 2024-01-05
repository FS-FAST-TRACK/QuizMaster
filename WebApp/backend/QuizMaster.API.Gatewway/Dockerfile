#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["QuizMaster.API.Gatewway/QuizMaster.API.Gateway.csproj", "QuizMaster.API.Gatewway/"]
COPY ["QuizMaster.API.Account/QuizMaster.API.Account.csproj", "QuizMaster.API.Account/"]
COPY ["QuizMaster.API.Monitoring/QuizMaster.API.Monitoring.csproj", "QuizMaster.API.Monitoring/"]
COPY ["QuizMaster.Library.Common/QuizMaster.Library.Common.csproj", "QuizMaster.Library.Common/"]
COPY ["QuizMaster.API.Authentication/QuizMaster.API.Authentication.csproj", "QuizMaster.API.Authentication/"]
COPY ["QuizMaster.API.Media/QuizMaster.API.Media.csproj", "QuizMaster.API.Media/"]
COPY ["QuizMaster.API.QuizSession/QuizMaster.API.QuizSession.csproj", "QuizMaster.API.QuizSession/"]
COPY ["QuizMaster.API.Quiz/QuizMaster.API.Quiz.csproj", "QuizMaster.API.Quiz/"]
RUN dotnet restore "QuizMaster.API.Gatewway/QuizMaster.API.Gateway.csproj"
COPY . .
WORKDIR "/src/QuizMaster.API.Gatewway"
RUN dotnet build "QuizMaster.API.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "QuizMaster.API.Gateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY QuizMaster.API.Gatewway/localhost_cert.pfx ./localhost_cert.pfx
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QuizMaster.API.Gateway.dll", "https://*:80"]