using System.Diagnostics;
using System.IO;

namespace TestProcessWrapper
{
    internal class DotnetProcessFactory : IProcessFactory
    {
        private bool _isCoverletEnabled;
        private TestProjectInfo _testProjectInfo;
        private BuildConfiguration _buildConfiguration;

        private string BinFolder => Path.Combine("bin", _buildConfiguration.ToString(), "net7.0");

        public ITestProcess Create(
            string appProjectName,
            BuildConfiguration buildConfiguration,
            bool isCoverletEnabled
        )
        {
            _buildConfiguration = buildConfiguration;
            _testProjectInfo = new TestProjectInfo(appProjectName);
            _isCoverletEnabled = isCoverletEnabled;

            var process = new TestProcess();

            process.StartInfo = CreateProcessStartInfo();

            return process;
        }

        private ProcessStartInfo CreateProcessStartInfo()
        {
            ProcessStartInfo processStartInfo;

            if (!_isCoverletEnabled)
            {
                processStartInfo = CreateProcessStartInfo("dotnet", _testProjectInfo.AppDllName);
            }
            else
            {
                processStartInfo = CreateProcessStartInfoWithCoverletWrapper();
            }

            return processStartInfo;
        }

        private ProcessStartInfo CreateProcessStartInfo(string processName, string processArguments)
        {
            var processStartInfo = new ProcessStartInfo(processName)
            {
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                Arguments = processArguments,
                WorkingDirectory = Path.Combine(
                    _testProjectInfo.ProjectDir,
                    _testProjectInfo.AppProjectName,
                    BinFolder
                )
            };

            return processStartInfo;
        }

        private ProcessStartInfo CreateProcessStartInfoWithCoverletWrapper()
        {
            var arguments =
                $"\".\" --target \"dotnet\" --targetargs \"{_testProjectInfo.AppDllName}\" --output {_testProjectInfo.CoverageReportPath} --format cobertura";

            return CreateProcessStartInfo("coverlet", arguments);
        }
    }
}
