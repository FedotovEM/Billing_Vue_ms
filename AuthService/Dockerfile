FROM mcr.microsoft.com/dotnet/sdk:7.0 AS base
WORKDIR /app
COPY *.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /publish

ARG AbonentDataAccountingService_URL
ENV AbonentDataAccountingService_URL="${AbonentDataAccountingService_URL}"

ARG BillingPostgreSQL
ENV BillingPostgreSQL="${BillingPostgreSQL}"

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /publish
COPY --from=base /publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "AuthService.dll"]