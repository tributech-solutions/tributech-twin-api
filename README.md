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

## Using models

Models can be added to the [Catalog API](https://github.com/tributech-solutions/tributech-catalog-api) via REST, a default set of models gets loaded by default. These models can be found in the following repositories:

[Tributech Data-Asset Models](https://github.com/tributech-solutions/data-asset-twin)

[Tributech GAIA-X Self Description Models](https://github.com/tributech-solutions/gaia-x-self-descriptions)

## Development

### Docker

#### Build images
```powershell
docker-compose -f ./docker-compose.yml -f ./docker-compose.ci.build.yml build
```
