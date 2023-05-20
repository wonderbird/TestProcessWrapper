using System;
using System.IO;

namespace TestProcessWrapper;

internal class TestProjectInfo
{
    public TestProjectInfo(string appProjectName)
    {
        var projectRelativeDir = Path.Combine("..", "..", "..", "..");
        ProjectDir = Path.GetFullPath(projectRelativeDir);
        AppProjectName = appProjectName;
    }

    public string ProjectDir { get; }
    public string AppProjectName { get; }
    public string AppDllName => AppProjectName + ".dll";

    public string CoverageReportPath
    {
        get
        {
            var coverageReportFileName = $"{AppProjectName}.{Guid.NewGuid().ToString()}.xml";
            var coverageReportRelativeDir = Path.Join("..", "..", "..", "TestResults");
            var coverageReportDir = Path.GetFullPath(coverageReportRelativeDir);
            return Path.Combine(coverageReportDir, coverageReportFileName);
        }
    }
}