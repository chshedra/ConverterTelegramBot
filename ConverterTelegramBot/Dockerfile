FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS build
WORKDIR /app
COPY *.csproj ./
RUN dotnet restore ConverterTelegramBot.csproj
COPY . ./
RUN dotnet publish ConverterTelegramBot.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "ConverterTelegramBot.dll"]