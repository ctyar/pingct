var configuration = Argument("configuration", "Release");
var projectDir = Directory("./src/pingct");
var outputDirectory = "./artifacts/";
var framework = "netcoreapp3.0";
var nugetApiKey = Argument("nugetApiKey", string.Empty);

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

Task("PublishNuget")
	.IsDependentOn("Build")
	.Does(() => {
		if (string.IsNullOrEmpty(nugetApiKey))
		{
			Information("Skipping Nuget deploy.");
			return;
		}

		var packageOutputPath = MakeAbsolute(Directory(outputDirectory));
		var settings = new DotNetCoreNuGetPushSettings
		{
			 Source = "https://api.nuget.org/v3/index.json",
			 ApiKey = nugetApiKey
		};
		
		foreach(var file in GetFiles($"{packageOutputPath}/*.nupkg"))
		{
			DotNetCoreNuGetPush(file.FullPath, settings);
		}
});


Task("Default")
  .IsDependentOn("PublishNuget");


var target = Argument("target", "Default");
RunTarget(target);