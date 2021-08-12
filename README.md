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

### Docker

#### Build images
```powershell
docker-compose -f ./docker-compose.yml -f ./docker-compose.ci.build.yml build
```
