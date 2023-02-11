FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build 
COPY dc-api.sln dc-api.sln
COPY src/AdminService src/AdminService
RUN dotnet restore src/AdminService/AdminService.csproj

FROM build AS publish
RUN dotnet publish -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0 as runtime
WORKDIR /publish
COPY --from=publish /publish .
ENTRYPOINT ["dotnet", "AdminService.dll"]