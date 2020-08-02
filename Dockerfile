FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build

ARG BUILDCONFIG=Release
ARG VERSION=1.0.0

COPY UserManagement.sln /build/
COPY UserManagement.API/*.csproj /build/UserManagement.API/
COPY UserManagement.Domain/*.csproj /build/UserManagement.Domain/
COPY UserManagement.Infrastructure/*.csproj /build/UserManagement.Infrastructure/
COPY UserManagement.Services/*.csproj /build/UserManagement.Services/
COPY Common/*.csproj /build/Common/

WORKDIR /build
RUN dotnet restore
COPY . .

WORKDIR /build/UserManagement.Domain
RUN dotnet build -c $BUILDCONFIG -o /app /p:Version=$VERSION

WORKDIR /build/UserManagement.Infrastructure
RUN dotnet build -c $BUILDCONFIG -o /app /p:Version=$VERSION

WORKDIR /build/UserManagement.Services
RUN dotnet build -c $BUILDCONFIG -o /app /p:Version=$VERSION

WORKDIR /build/UserManagement.API
RUN dotnet build -c $BUILDCONFIG -o /app /p:Version=$VERSION

WORKDIR /build/Common
RUN dotnet build -c $BUILDCONFIG -o /app /p:Version=$VERSION

FROM build AS publish
#Publish automatically sets Environment to Production
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
ENV defusername=admin
ENV defpass=adminadmin
ENV defemail=admin@admin.com
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY --from=publish /app .
ENTRYPOINT ["dotnet", "UserManagement.API.dll"] 