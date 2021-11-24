dotnet pack --configuration Release -o artifacts

$version = (get-item .\artifacts\*.nupkg).Name -replace "pingct.","" -replace ".nupkg",""

dotnet tool uninstall pingct --global

dotnet tool install --global --add-source .\artifacts pingct --version $version