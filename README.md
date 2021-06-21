# Tributech Twin API

## Description
The Tributech Twin API service gets used to manage instances of digital twins written in the [Digital Twin Definition Language](https://github.com/Azure/opendigitaltwins-dtdl/blob/master/DTDL/v2/dtdlv2.md).

Responsibilities:
- Validate instances
- Get instances
- Add/Edit instances
- Remove instances 
- Get relationships
- Add/Edit relationships
- Remove relationships


## Development

### dotnet
To access our private NuGet feed at Azure DevOps install [Azure Artifacts Credential Provider
](https://github.com/microsoft/artifacts-credprovider#setup) plugin and restore NuGet packages with:
```powershell
dotnet restore --interactive` # (interactive is only required at first time)
dotnet build
```
More info: [Guidelines - Azure DevOps - NuGet packages](https://tributech.atlassian.net/wiki/spaces/TM/pages/1005781050/NuGet+packages#dotnet-build-%28local%29)

### Docker

#### Build images
```powershell
docker-compose -f ./docker-compose.yml -f ./docker-compose.ci.build.yml build
```
The PAT (personal access token) must have read permissions for private NuGet feed at Azure DevOps for central package repository (see `nuget.config`).
More info: [Guidelines - Azure DevOps - NuGet packages](https://tributech.atlassian.net/wiki/spaces/TM/pages/1005781050/NuGet+packages#Docker-based-build-%28local%29)
