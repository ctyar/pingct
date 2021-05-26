# pingct

[![Build Status](https://dev.azure.com/ctyar/pingct/_apis/build/status/ctyar.pingct?branchName=main)](https://dev.azure.com/ctyar/pingct/_build/latest?definitionId=5&branchName=main)
[![pingct](https://img.shields.io/nuget/v/pingct.svg)](https://www.nuget.org/packages/pingct/)

A simple Ping like tool to check the network connection issues. `pingct` is designed to constantly check the network connectivity and in case of any issues run a set of tests to facilitate troubleshooting.

## Get started

Download the [.NET SDK](https://get.dot.net/).

Once installed, run this command:

```
$ dotnet tool install --global pingct
```

And run the tool with:
```
$ pingct
```

## Usage

```
  pingct [options] [command]

Options:
  --version         Show version information
  -?, -h, --help    Show help and usage information

Commands:
  config    Prints the path to the config file
```

Test types:
1. Ping
1. DNS lookup
1. HTTP GET request


### Sample output:
![image](https://user-images.githubusercontent.com/1432648/114851430-133a4180-9df7-11eb-9954-55db3fe9d93f.png)

## Pre-release builds
Get the package from [here](https://github.com/ctyar/pingct/packages/48026).

## Build
Install [.NET SDK](https://get.dot.net/).

Run:
```
$ dotnet tool restore
$ dotnet-cake
```
