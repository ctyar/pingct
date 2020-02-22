var configuration = Argument("configuration", "Release");
var projectDir = Directory("./src/pingct");
var outputDirectory = "./artifacts/";

Task("Clean")
    .Does(() =>
{
    DotNetCoreClean(projectDir);

	var directoriesToClean = GetDirectories(outputDirectory);
	CleanDirectories(directoriesToClean);
});

Task("Build")
    .IsDependentOn("Clean")
    .Does(() =>
{
	var settings = new DotNetCorePackSettings
	{
		Configuration = configuration,
		OutputDirectory = outputDirectory
	};

	DotNetCorePack(projectDir, settings);
});

Task("Default")
  .IsDependentOn("Build");


var target = Argument("target", "Default");
RunTarget(target);