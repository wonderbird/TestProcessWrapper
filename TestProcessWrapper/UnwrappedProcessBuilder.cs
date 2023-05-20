using System.Collections.Generic;

namespace TestProcessWrapper;

internal class UnwrappedProcessBuilder : TestProcessBuilder
{
    private readonly BuildConfiguration _buildConfiguration;

    public UnwrappedProcessBuilder(string appProjectName, BuildConfiguration buildConfiguration)
        : base(appProjectName) => _buildConfiguration = buildConfiguration;

    public override void CreateProcessStartInfo() =>
        ProcessStartInfo = CreateProcessStartInfo(
            "dotnet",
            TestProjectInfo.AppDllName,
            _buildConfiguration
        );

    public override void AddCommandLineArguments(Dictionary<string, string> arguments)
    {
        foreach (var (argument, value) in arguments)
        {
            ProcessStartInfo.Arguments += string.IsNullOrEmpty(value)
                ? $" {argument}"
                : $" {argument}={value}";
        }
    }
}
