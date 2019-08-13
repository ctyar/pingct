var configuration = Argument("configuration", "Release");
var projectDir = Directory("./src/pingct");
var outputDirectory = "./artifacts/";
var framework = "netcoreapp3.0";


Task("Clean")
    .Does(() =>
{
    var settings = new DotNetCoreCleanSettings
    {
        Framework = framework,
        Configuration = configuration,
        OutputDirectory = outputDirectory
    };

    DotNetCoreClean(projectDir, settings);
});


Task("Build")
    .IsDependentOn("Clean")
    .Does(() =>
{
    var settings = new DotNetCorePublishSettings
    {
        Framework = framework,
        Configuration = configuration,
        OutputDirectory = outputDirectory
    };

    DotNetCorePublish(projectDir, settings);
}); 


Task("Default")
  .IsDependentOn("Build");


var target = Argument("target", "Default");
RunTarget(target);