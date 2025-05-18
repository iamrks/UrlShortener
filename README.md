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
$ cd <repo_path>/UrlShortener/src/UrlShortener
$ dotnet user-secrets set 'HttpClient:Github:Token' '<Token>'
$ dotnet user-secrets set "ConnectionStrings:Database" "<Database_Connection_String>"
$ dotnet user-secrets set "Seq:ApiKey" "<Api_Key>"
$ dotnet user-secrets set "LaunchDarkly:SdkKey" "<SdkKey>"
$ dotnet user-secrets set "Redis:Endpoint" "<Endpoint>"
$ dotnet user-secrets set "Redis:Password" "<Password>"
$ dotnet user-secrets set "Redis:User" "<User>"
```

## Development with Visual Studio

1. Install [Visual Studio](https://visualstudio.microsoft.com/downloads/)
2. Open Solution file

## Development with Visual Studio Code

1. Install [VS Code](https://code.visualstudio.com/download).
2. Install C# extension (`ms-vscode.csharp`) in VS Code.
3. Open the folder containing the .sln file

## Install Seq for local development logging

1. Install [Seq](https://datalust.co/download)
2. After installing browse at default url http://localhost:5341/

# LaunchDarkly Service
1. Create your [LaunchDarkly][https://app.launchdarkly.com] account
2. Create Feature Flag


# Running the App

## Run Locally

```Shell
$ cd <repo_path>/UrlShortener
$ dotnet run --project .\src\UrlShortener\UrlShortener.csproj --environment "Development" --launch-profile https
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

## .Net Aspire
1. Add .Net Aspire Service Default Project to solution
2. Then add reference and update Program.cs
3. Check [Commit](https://github.com/iamrks/UrlShortener/commit/fcb8dff04bab50014f1c5fea374617fe72dbcd38)