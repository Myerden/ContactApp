# ContactApp

There is a Contact List App which developed in .NET 5 with microservice architecture.


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
3. At the root directory which include **docker-compose.yml** files, run below command:
```csharp
docker-compose up
```
4. Wait for docker compose all microservices. 

5. You can **launch microservices** as below urls:

* **Gateway API -> http://localhost:5000**
* **Contact API -> http://localhost:5002**
* **Report API -> http://localhost:5004**
* **pgAdmin PostgreSQL -> http://localhost:5050**   -- admin@admin.com / admin

If you want to test the API with Postman, click below link.

[![Run in Postman](https://run.pstmn.io/button.svg)](https://app.getpostman.com/run-collection/18286593-bd756cf0-fc87-4988-b848-cdf57e85aaf3?action=collection%2Ffork&collection-url=entityId%3D18286593-bd756cf0-fc87-4988-b848-cdf57e85aaf3%26entityType%3Dcollection%26workspaceId%3D7f095af1-3e08-4f71-a9e0-48d386d4dfc5)
