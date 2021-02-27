FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

# Copy csproj as distinct layers
COPY *.sln .
COPY Taskr.Api/*.csproj ./Taskr.Api/
COPY Taskr.Commands/*.csproj ./Taskr.Commands/
COPY Taskr.Domain/*.csproj ./Taskr.Domain/
COPY Taskr.Dtos/*.csproj ./Taskr.Dtos/
COPY Taskr.Handlers/*.csproj ./Taskr.Handlers/
COPY Taskr.Infrastructure/*.csproj ./Taskr.Infrastructure/
COPY Taskr.IntegrationTests/*.csproj ./Taskr.IntegrationTests/
COPY Taskr.MappingProfiles/*.csproj ./Taskr.MappingProfiles/
COPY Taskr.Persistance/*.csproj ./Taskr.Persistance/
COPY Taskr.Queries/*.csproj ./Taskr.Queries/
COPY Taskr.Validation/*.csproj ./Taskr.Validation/

RUN dotnet restore

# copy everything else and bulid app

COPY Taskr.Api/. ./Taskr.Api/
COPY Taskr.Commands/. ./Taskr.Commands/
COPY Taskr.Domain/. ./Taskr.Domain/
COPY Taskr.Dtos/. ./Taskr.Dtos/
COPY Taskr.Handlers/. ./Taskr.Handlers/
COPY Taskr.Infrastructure/. ./Taskr.Infrastructure/
COPY Taskr.MappingProfiles/. ./Taskr.MappingProfiles/
COPY Taskr.Persistance/. ./Taskr.Persistance/
COPY Taskr.Queries/. ./Taskr.Queries/
COPY Taskr.Validation/. ./Taskr.Validation/

WORKDIR /app/Taskr.Api
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app

COPY --from=build /app/Taskr.Api/out ./
ENTRYPOINT ["dotnet", "Taskr.Api.dll"]
