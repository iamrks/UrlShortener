# URL Shortener

## Supported Development Environments
* Visual Studio (Windows)
* Visual Studio for Mac (MacOS)
* Visual Studio Code (Windows/MacOS/Linux)

## Environment Setup
1. Install [.NET Core](https://dotnet.microsoft.com/en-us/download)
2. Install [SQL Server](https://www.microsoft.com/en-in/sql-server/sql-server-downloads)
3. Install [Docker](https://docs.docker.com/engine/install/)

4. Set user secrets, if you want to run the application outside docker for debugging and other scenarios

```Shell
$ cd <repo_path>/UrlShortener
$ dotnet user-secrets set 'HttpClient:Github:Token' '<Token>' --project .\UrlShortener.Api\UrlShortener.Api.csproj
$ dotnet user-secrets set "ConnectionStrings:Database" "<Database_Connection_String>" --project .\UrlShortener.Api\UrlShortener.Api.csproj
```

## Development with Visual Studio

1. Install [Visual Studio](https://visualstudio.microsoft.com/downloads/)
2. Open Solution file

## Development with Visual Studio Code

1. Install [VS Code](https://code.visualstudio.com/download).
2. Install C# extension (`ms-vscode.csharp`) in VS Code.
3. Open the folder containing the .sln file

# Running the App

## Run Locally

```Shell
$ cd <repo_path>/UrlShortener
$ cd dotnet run --project .\UrlShortener.Api\UrlShortener.Api.csproj --environment "Development" --launch-profile https
```

## Run with Docker

1. Set environment variables in docker-compose file

```Shell
environment:
      - ConnectionStrings__Database=<Database_Connection_String>
      - HttpClient__Github__Token=<Token>
```

```Shell
$ cd <repo_path>/UrlShortener
$ docker-compose up
# if changes have been made, you will need to build them before running docker-compose up:
$ docker-compose build
```


## Other useful docker commands

```Shell
# Command to build
$ docker build --rm -t iamrks/urls:latest .

# Command to run
$ docker run -p 5000:8080 iamrks/urls

# To run in background
$ docker run -d -p 5000:8080 iamrks/urls

# To Stop
$ docker ps
$ docker stop <container_id>

# To list docker images
$ docker images

# To remove a image
$ docker rmi iamrks/urls --force
```








