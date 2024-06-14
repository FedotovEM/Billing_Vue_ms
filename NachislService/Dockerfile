FROM mcr.microsoft.com/dotnet/sdk:7.0 AS base
WORKDIR /app
COPY *.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /publish

ARG AuthService_URL
ENV AuthService_URL="${AuthService_URL}"
ARG PayService_URL
ENV PayService_URL="${PayService_URL}"

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /publish
COPY --from=base /publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "NachislService.dll"]