FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY ["StarWars.ConsoleClient/StarWars.ConsoleClient.csproj", "StarWars.ConsoleClient/"]
RUN dotnet restore "StarWars.ConsoleClient/StarWars.ConsoleClient.csproj"

# Copy the rest of the source code
COPY . .
WORKDIR "/src/StarWars.ConsoleClient"
RUN dotnet build "StarWars.ConsoleClient.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StarWars.ConsoleClient.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StarWars.ConsoleClient.dll"]