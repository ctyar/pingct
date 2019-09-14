if ($args.Length -eq 0)
{
	dotnet-cake
}
else
{
	$cake = "dotnet-cake -ScriptArgs '" + '-nugetApiKey="' + $args[0] + '"' + "'"
	Invoke-Expression $cake
}