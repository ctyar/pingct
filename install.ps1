$version = Read-Host -Prompt 'Enter the version'
dotnet tool install --global --add-source .\artifacts pingct --version $version