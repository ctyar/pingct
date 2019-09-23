# pingct

[![Build Status](https://dev.azure.com/ctyar/pingct/_apis/build/status/ctyar.pingct?branchName=master)](https://dev.azure.com/ctyar/pingct/_build/latest?definitionId=3&branchName=master)
[![pingct](https://img.shields.io/nuget/v/pingct.svg)](https://www.nuget.org/packages/pingct/)

A simple Ping like tool to check the common network connection issues in Iran. `pingct` is designed to constantly check the network connection and in case of any issue run a set of tests to facilitate troubleshooting.

## Get started

Download the .NET Core SDK [3.0](https://dotnet.microsoft.com/download/dotnet-core/3.0).
Once installed, run this command:

```
$ dotnet tool install --global pingct
```

And run with:
```
$ pingct
```

## Usage

```
pingct [options] [command]

Options:
  --version    Display version information

Commands:
  config    Run a wizard to change the default settings.
```

Currently, the tests are:
1. Pinging the gateway IP address
1. Pinging `aparat.com` to see if the country's internal network is accessible
1. Testing DNS
1. Trying to access a restricted IP to see if our restriction bypass solution is working


### Sample output:
![image](https://user-images.githubusercontent.com/1432648/64917273-a76b9f00-d7a3-11e9-8c0c-d249224ec0c7.png)

## Build
Install .NET Core SDK [3.0](https://dotnet.microsoft.com/download/dotnet-core/3.0)

Install [Cake.Tool](https://www.nuget.org/packages/Cake.Tool)

Run:
```
$ dotnet-cake
```
