FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

#copy csproj and restore as distinct layer
COPY *.csproj ./
RUN dotnet restore

#copy evrything else and build
COPY . ./
RUN dotnet publish -c Release -o out

#build a runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT [ "dotnet", "API.dll" ]