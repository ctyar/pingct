# pingct

[![Build Status](https://dev.azure.com/ctyar/pingct/_apis/build/status/ctyar.pingct?branchName=master)](https://dev.azure.com/ctyar/pingct/_build/latest?definitionId=3&branchName=master)
[![pingct](https://img.shields.io/nuget/v/pingct.svg)](https://www.nuget.org/packages/pingct/)
[![CodeFactor](https://www.codefactor.io/repository/github/ctyar/pingct/badge)](https://www.codefactor.io/repository/github/ctyar/pingct)

A simple Ping like tool to check the network connection issues. `pingct` is designed to constantly check the network connectivity and in case of any issues run a set of tests to facilitate troubleshooting.

## Get started

Download the [.NET Core SDK](https://get.dot.net/).

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

Currently, the tests are:
1. Pinging the gateway IP address
1. Pinging `aparat.com` to see if the country's internal network is accessible
1. Testing DNS
1. Trying to access a restricted website to see if our restriction bypass solution is working


### Sample output:
![image](https://user-images.githubusercontent.com/1432648/94337799-148e5300-fffa-11ea-92fd-ab1c278413ce.png)

## Pre-release builds
Get the package from [here](https://github.com/ctyar/pingct/packages/48026).

## Build
Install [.NET Core SDK](https://get.dot.net/).

Run:
```
$ dotnet tool restore
$ dotnet-cake
```
