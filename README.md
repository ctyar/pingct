# pingct

[![Build Status](https://dev.azure.com/ctyar/pingct/_apis/build/status/ctyar.pingct?branchName=main)](https://dev.azure.com/ctyar/pingct/_build/latest?definitionId=5&branchName=main)
[![pingct](https://img.shields.io/nuget/v/pingct.svg)](https://www.nuget.org/packages/pingct/)

A simple Ping like tool to check the network connection issues. `pingct` is designed to constantly check the network connectivity and in case of any issues run a set of tests to facilitate troubleshooting.

## Get started

[Install](https://get.dot.net) the [required](global.json) .NET SDK.

Run this command:

```
$ dotnet tool install --global pingct
```

And run the tool with:
```
$ pingct
```

## Usage

```
  pingct [command] [options]

Options:
  -?, -h, --help  Show help and usage information
  --version       Show version information

Commands:
  config  Prints the path to the config file
```

#### Config file

```json
{
  "Ping": "4.2.2.4",
  "Delay": 1500,
  "MaxPingSuccessTime": 120,
  "MaxPingWarningTime": 170,
  "Tests": [
    {
      "Type": "ping",
      "Host": "192.168.1.1"
    },
    {
      "Type": "ping",
      "Host": "bt.com"
    },
    {
      "Type": "dns",
      "Host": "facebook.com"
    },
    {
      "Type": "get",
      "Host": "https://twitter.com"
    }
  ],
  "OnConnected": "",
  "OnConnectedArgs": "",
  "OnDisconnected": "",
  "OnDisconnectedArgs": ""
}
```

#### Supported test types:
1. `ping` Ping
1. `dns` DNS lookup
1. `get` HTTP GET request


#### Sample output:
![image](https://user-images.githubusercontent.com/1432648/151763302-146eb6f9-999d-4ea4-a528-875ae55b0be9.png)

## Pre-release builds
Get the package from [here](https://github.com/ctyar/pingct/packages/48026).

## Build
[Install](https://get.dot.net) the [required](global.json) .NET SDK.

Run:
```
$ dotnet build
```
