# ContactApp

There is a Contact List App which developed in .NET 5 with microservice architecture.

## Overview

This project includes following features

### Contact Microservice
Responsible for basic CRUD implementations over Contact Entity.

This service includes:
* .NET 5 Web Api implementation
* REST API Principles, CRUD operations
* PostgreSQL as Database Provider
* Repository Pattern implementation
* Containerization

### Report Microservice
Responsible for generating reports for Contacts.

This service includes:
* .NET 5 Web Api implementation
* REST API Principles, CRUD operations
* PostgreSQL as Database Provider
* Repository Pattern implementation
* Containerization
* RabbitMQ Message Broker Service
* HTTP Rest communication between Contact Service
* EPPlus for generationg Excel reports

### Gateway Microservice
Responsible for communication with microservices.

This service includes:
* Ocelot is used for Gateway implementation
* Containerization

## Run The Project
You will need the following tools:

* [Visual Studio 2019](https://visualstudio.microsoft.com/downloads/)
* [.Net Core 5 or later](https://dotnet.microsoft.com/download/dotnet-core/5)
* [Docker Desktop](https://www.docker.com/products/docker-desktop)

### Installing
Follow these steps to get your development environment set up: (Before Run Start the Docker Desktop)
1. Clone the repository
2. (Optional) If you want, you can limit the Wsl2's memory and cpu usage. ([see also](https://github.com/microsoft/WSL/issues/4166))
  - Go to User's Document Folder ( Press "WinKey + R" and run "%UserProfile%" )
  - Create a file named *.wslconfig*
  - Write its content as follows (Windows restart may be required)
```
[wsl2]
memory=8GB
processors=2
swap=0
localhostForwarding=true
```
3. At the root directory which include **docker-compose.yml** and **docker-compose.override.yml** files, run below command:
```
docker-compose -f docker-compose.yml -f docker-compose.override.yml up
```
4. Wait for docker compose all microservices. 

5. You can **launch microservices** as below urls:

* **Gateway API -> http://localhost:5000**
* **Contact API -> http://localhost:5002**
* **Report API -> http://localhost:5004**
* **pgAdmin PostgreSQL -> http://localhost:5050**   -- admin@admin.com / admin
* **RabbitMQ -> http://localhost:15672**   -- user / password

If you want to use the API with Postman, click below link.

[![Run in Postman](https://run.pstmn.io/button.svg)](https://app.getpostman.com/run-collection/18286593-bd756cf0-fc87-4988-b848-cdf57e85aaf3?action=collection%2Ffork&collection-url=entityId%3D18286593-bd756cf0-fc87-4988-b848-cdf57e85aaf3%26entityType%3Dcollection%26workspaceId%3D7f095af1-3e08-4f71-a9e0-48d386d4dfc5)

## Testing
To run unit tests, you have to run microservices with test environment. We will also use Docker for unit tests.
1. At the root directory which include **docker-compose-tests.yml** and **docker-compose-tests.override.yml** files, run below command:
```
docker-compose  -f docker-compose-tests.yml -f docker-compose-tests.override.yml up
```
2. Wait for docker compose all microservices.
3. From the Test menu item, run the all tests.

![image](https://user-images.githubusercontent.com/15304742/141701962-ea730d56-119a-42e7-9883-bead95ec21c4.png)

4. Open the Test Explorer window, and notice the results of the tests.

![image](https://user-images.githubusercontent.com/15304742/141702031-1ef9947a-0075-4729-9db9-9b1cdcedf774.png)

