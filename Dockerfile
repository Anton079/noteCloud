# syntax=docker/dockerfile:1

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY NoteCloud-api/NoteCloud-api.csproj NoteCloud-api/
RUN dotnet restore NoteCloud-api/NoteCloud-api.csproj

COPY . .
WORKDIR /src/NoteCloud-api
RUN dotnet publish NoteCloud-api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM --platform=$TARGETPLATFORM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "NoteCloud-api.dll"]
