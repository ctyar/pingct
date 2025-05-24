Get-ChildItem -Path '.\artifacts' | Remove-Item -Force -Recurse

dotnet pack src\pingct\pingct.csproj --configuration Release -o artifacts -tl

$version = (get-item .\artifacts\*.nupkg).Name -replace "pingct.","" -replace ".nupkg",""

dotnet tool uninstall pingct --global

dotnet tool install --global --add-source .\artifacts pingct --version $version