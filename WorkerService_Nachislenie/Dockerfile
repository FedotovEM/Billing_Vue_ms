FROM mcr.microsoft.com/dotnet/sdk:7.0 AS base
WORKDIR /app
COPY *.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /publish

ARG BillingPostgreSQL
ENV BillingPostgreSQL="${BillingPostgreSQL}"

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /publish
COPY --from=base /publish .
ENTRYPOINT ["dotnet", "WorkerService_Nachislenie.dll"]