FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["instock-server-application.csproj", "./"]
RUN dotnet restore "instock-server-application.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "instock-server-application.csproj" -c Release -o /app/build

FROM build AS unit-test
RUN dotnet test "instock-server-application.csproj" -c Release -o /app/publish

FROM build AS publish
RUN dotnet publish "instock-server-application.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "instock-server-application.dll"]
