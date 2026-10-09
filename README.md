# Microservices E-Commerce API

A containerized e-commerce backend built with **ASP.NET Core** and a
microservices architecture. The solution separates key business
capabilities into independent services and brings them together through
API gateways and supporting infrastructure.

> **Repository:**
> [Ahmed010Ashraf/Microservices.Ecommerce.Api](https://github.com/Ahmed010Ashraf/Microservices.Ecommerce.Api)

## Table of Contents

-   [Overview](#overview)
-   [Architecture](#architecture)
-   [Services](#services)
-   [Technology Stack](#technology-stack)
-   [Repository Structure](#repository-structure)
-   [Prerequisites](#prerequisites)
-   [Run with Docker Compose](#run-with-docker-compose)
-   [Access the Services](#access-the-services)
-   [How the Components Work
    Together](#how-the-components-work-together)
-   [Configuration and Security](#configuration-and-security)
-   [Troubleshooting](#troubleshooting)
-   [Future Improvements](#future-improvements)
-   [Author](#author)

## Overview

This project demonstrates how an e-commerce backend can be organized as
a set of focused services rather than one monolithic API. Each service
owns a particular responsibility and can use infrastructure suited to
its data and communication needs.

The repository includes product catalog, shopping basket, discount,
order processing, and identity capabilities, along with API gateway
components and containerized infrastructure.

The solution is intended as a practical learning project for distributed
backend development, service-to-service communication, container
orchestration, and API security.

## Architecture

``` mermaid
flowchart TB
    Client[Client / API Consumer]
    Nginx[Nginx Reverse Proxy]
    Ocelot[Ocelot API Gateway]
    Identity[Identity Server]
    Catalog[Catalog API]
    Basket[Basket API]
    Discount[Discount API]
    Ordering[Ordering API]
    Mongo[(MongoDB)]
    Redis[(Redis)]
    Postgres[(PostgreSQL)]
    SQL[(SQL Server)]
    RabbitMQ[(RabbitMQ)]
    Elastic[(Elasticsearch)]
    Kibana[Kibana]

    Client --> Nginx
    Client --> Ocelot
    Nginx --> Identity
    Nginx --> Catalog
    Nginx --> Basket
    Nginx --> Discount
    Nginx --> Ordering
    Ocelot --> Catalog
    Ocelot --> Basket
    Ocelot --> Discount
    Ocelot --> Ordering
    Catalog --> Mongo
    Basket --> Redis
    Basket --> Discount
    Discount --> Postgres
    Ordering --> SQL
    Basket <--> RabbitMQ
    Ordering <--> RabbitMQ
    Catalog --> Elastic
    Basket --> Elastic
    Ordering --> Elastic
    Elastic --> Kibana
    Client --> Identity
```

The diagram represents the main components declared in the Docker
Compose configuration. Exact request routes and event flows depend on
the gateway configuration and service implementation.

## Services

  -----------------------------------------------------------------------
  Component               Responsibility          Main technology /
                                                  dependency
  ----------------------- ----------------------- -----------------------
  **Catalog API**         Product catalog         ASP.NET Core, MongoDB
                          operations              

  **Basket API**          Shopping basket         ASP.NET Core, Redis
                          operations              

  **Discount API**        Discount-related        ASP.NET Core,
                          operations, exposed for PostgreSQL, gRPC
                          service-to-service use  configuration

  **Ordering API**        Order processing        ASP.NET Core, SQL
                                                  Server, RabbitMQ

  **Identity Server**     Identity and token      ASP.NET Core
                          issuance for API        identity-server
                          authentication          component

  **Ocelot API Gateway**  Gateway routing to      Ocelot
                          backend APIs            

  **Nginx reverse proxy** Reverse-proxy entry     Nginx
                          point configured by     
                          Docker Compose          

  **RabbitMQ**            Message broker for      RabbitMQ Management
                          asynchronous            image
                          communication           

  **Elasticsearch**       Search/log data         Elasticsearch
                          infrastructure          
                          configured in the stack 

  **Kibana**              UI for exploring        Kibana
                          Elasticsearch data      

  **pgAdmin**             PostgreSQL              pgAdmin
                          administration UI       
  -----------------------------------------------------------------------

## Technology Stack

-   **Language and framework:** C#, .NET / ASP.NET Core
-   **API gateway:** Ocelot
-   **Reverse proxy:** Nginx
-   **Databases and storage:** MongoDB, Redis, PostgreSQL, SQL Server
-   **Service communication:** HTTP, configured gRPC integration,
    RabbitMQ messaging
-   **Search and observability infrastructure:** Elasticsearch and
    Kibana
-   **Containerization:** Docker and Docker Compose
-   **API documentation artifacts:** Swagger/OpenAPI JSON files included
    in the repository

## Repository Structure

``` text
Microservices.Ecommerce.Api/
├── ApiGateway/
│   ├── OcelotApiGateway/       # Ocelot gateway application
│   └── nginx/                  # Nginx reverse-proxy configuration/image
├── Infrastructure/
│   └── eShop.Identity/         # Identity server application
│   └── CommonLogging/          # seriallog
│   └── EventBusMessages/       # Rabbitmq
├── services/
│   ├── Catalog/
│   │   └── Catalog.Api/
│   ├── Basket/
│   │   └── Basket.Api/
│   ├── Discount/
│   │   └── Discount.Api/
│   └── Ordering/
│       └── Ordering.Api/
├── docker-compose.yml          # Main multi-container stack
├── docker-compose.override.yml # Local development overrides
├── docker-compose.dcproj
├── Solution1.sln
├── catalog_sw.json
├── catalog_swagger.json
├── basket_sw.json
├── basket_swagger.json
└── swagger.json
```

The tree above highlights the main components visible in the repository.
Individual services may contain additional projects and folders.

## Prerequisites

Install the following before running the stack:

-   [Git](https://git-scm.com/)
-   [Docker Desktop](https://www.docker.com/products/docker-desktop/)
    with Docker Compose
-   Enough free memory and disk space for the databases, broker, and
    Elasticsearch
-   An IDE such as [Visual Studio](https://visualstudio.microsoft.com/)
    or [Visual Studio Code](https://code.visualstudio.com/) if you want
    to inspect or run individual .NET services

## Run with Docker Compose

### 1. Clone the repository

``` bash
git clone https://github.com/Ahmed010Ashraf/Microservices.Ecommerce.Api.git
cd Microservices.Ecommerce.Api
```

### 2. Start Docker Desktop

Make sure the Docker engine is running.

### 3. Build and start the stack

From the repository root, run:

``` bash
docker compose up --build
```

To start in the background:

``` bash
docker compose up --build -d
```

The Compose files define the application services and their supporting
infrastructure. The first startup can take several minutes while images
are downloaded, services are built, and databases initialize.

### 4. Check container status

``` bash
docker compose ps
```

### 5. View logs

``` bash
# All services
docker compose logs -f

# One service (replace the name with a Compose service name)
docker compose logs -f catalog.api
```

### 6. Stop the stack

``` bash
docker compose down
```

To remove named volumes as well (this **deletes persisted local database
data**):

``` bash
docker compose down -v
```

## Access the Services

The following host ports are declared in the current
`docker-compose.yml`. A port being mapped does not guarantee that the
corresponding endpoint is healthy or that a particular route exists.

  Component                Local address
  ------------------------ ---------------------------
  Nginx reverse proxy      `https://localhost:44344`
  Ocelot API Gateway       `http://localhost:8010`
  Catalog API              `http://localhost:8080`
  Basket API               `http://localhost:8001`
  Discount API             `http://localhost:8002`
  Ordering API             `http://localhost:8003`
  Identity Server          `http://localhost:9011`
  MongoDB                  `localhost:27017`
  Redis                    `localhost:6379`
  PostgreSQL               `localhost:5432`
  SQL Server               `localhost:1433`
  RabbitMQ management UI   `http://localhost:15672`
  Elasticsearch            `http://localhost:9200`
  Kibana                   `http://localhost:5601`
  pgAdmin                  `http://localhost:5050`

Use the gateway routes and Swagger/OpenAPI files that match the current
service configuration. If the application exposes Swagger UI only in the
Development environment, check the service's `Program.cs` and launch
settings for the exact path.

## How the Components Work Together

A typical request path in a microservices e-commerce system is:

1.  A client sends an HTTP request to a gateway or a service endpoint.
2.  The gateway routes the request to the appropriate backend service
    according to its routing configuration.
3.  The service applies its application logic and accesses its own
    configured data store or calls another service where needed.
4.  For operations that use asynchronous messaging, a service publishes
    or consumes messages through RabbitMQ rather than requiring every
    interaction to be a synchronous HTTP call.
5.  The relevant service returns an HTTP response to the caller.

This is a conceptual overview; the exact sequence for a particular
operation should be confirmed by tracing that endpoint through the
source code and gateway configuration.

## Configuration and Security

Before using this project outside a local development environment:

-   **Do not use development credentials in production.** The Compose
    file contains sample/default database and administration
    credentials. Replace them with strong secrets.
-   Move credentials and sensitive configuration to environment
    variables, Docker secrets, or a proper secret manager. Never commit
    real passwords, signing keys, or production connection strings.
-   Review the identity-server configuration, token validation,
    issuer/audience settings, HTTPS requirements, and allowed clients.
-   Review the Compose configuration for development-only settings,
    exposed database ports, and disabled Elasticsearch security.
-   Configure TLS certificates and production-ready reverse-proxy
    settings before exposing the application publicly.
-   Pin container image versions instead of relying on mutable tags
    where reproducible deployments are required.
-   Add health checks and readiness handling for application services,
    not only infrastructure containers.

The supplied Compose setup should be treated as a local development
environment until it has been reviewed and hardened.

## Troubleshooting

### Docker Compose fails to start

``` bash
docker compose ps
docker compose logs -f
```

Check that Docker Desktop is running and that the required ports are not
already in use.

### A service starts before its dependency is ready

`depends_on` controls startup ordering, but does not always mean a
dependency is ready to accept requests. Check health checks, service
logs, and retry behavior.

### The gateway returns 404

Verify that the requested path matches the Ocelot route configuration
and that the downstream service is running. A gateway root URL may not
map to an endpoint.

### A service cannot connect to a database or broker

When running inside Docker Compose, use the Compose service name and
container port (for example, `CatalogDb:27017` or `rabbitmq:5672`)
rather than `localhost`. Inside a container, `localhost` refers to that
same container.

### An API endpoint is unavailable

Inspect the relevant service's route/controller configuration and its
Swagger/OpenAPI definition. The JSON files in the repository can help
with API discovery, but may need to be regenerated if they are outdated.

## Future Improvements

Possible next steps for making the project more production-oriented
include:

-   Add automated unit and integration tests for each service.
-   Add health checks, readiness endpoints, and more resilient
    dependency handling.
-   Standardize structured logging and distributed tracing with
    correlation IDs.
-   Add CI workflows for build, test, and container-image validation.
-   Centralize secret management and harden authentication and TLS
    configuration.
-   Document each gateway route and provide reproducible API examples or
    a Postman collection.
-   Add architecture decision records explaining service boundaries,
    data ownership, and messaging choices.

## Author

**Ahmed Ashraf Samy Aboshady**

-   GitHub: [@Ahmed010Ashraf](https://github.com/Ahmed010Ashraf)
-   Project:
    [Microservices.Ecommerce.Api](https://github.com/Ahmed010Ashraf/Microservices.Ecommerce.Api)

------------------------------------------------------------------------

If you find the project useful for learning or reviewing microservices
concepts, feel free to explore the code and suggest improvements.
