<h1 align="center">smart-meter-collector</h1>

<p align="center">
  <a href="#about">About</a> •
  <a href="#features">Features</a> •
  <a href="#dependencies">Dependencies</a> •
  <a href="#building">Building</a> •
  <a href="#installation">Installation</a> •
  <a href="#usage">Usage</a> •
  <a href="#configuration">Configuration</a>
</p>

---

## About

The smart-meter-collector is a C# application designed to collect and process data from smart meters. It reads data from various energy sources, processes it, and stores it in a MongoDB database.

## Features

- Collects data from smart meters.
- Supports multiple energy sources (Solar, Wind, Other).
- Stores data in MongoDB.
- Provides JSON serialization and file export functionality.
- Configurable via JSON configuration files.

## Dependencies

- .NET 6.0 or later
- MongoDB.Driver
- Microsoft.Extensions.Hosting
- Microsoft.Extensions.Configuration
- Microsoft.Extensions.Logging

## Building


To build the project, use the following command:
```bash
dotnet build
```

## Installation

To install the project, clone the repository and navigate to the project directory:
```bash
git clone https://github.com/yourusername/smart-meter-collector.git
```
```bash
cd smart-meter-collector
```

## Usage

To run the application, use the following command:
```bash
dotnet run
```

## Configuration

The application can be configured using a config.json file. Below is an example configuration:
```JSON
{
  "MongoDB": {
    "ConnectionString": "your-mongodb-connection-string",
    "DatabaseName": "your-database-name",
    "CollectionName": "your-collection-name"
  },
  "AppSettings": {
    "Logging": {
      "LogLevel": {
        "Default": "Information"
      }
    }
  }
}
```

## Shoutout

Special thanks to the developers of MongoDB.Driver and Microsoft.Extensions libraries for providing the necessary tools to build this application.
