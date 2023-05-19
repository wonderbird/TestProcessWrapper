using System.Diagnostics;
using System.IO;
using Xunit.Abstractions;

namespace TestProcessWrapper
{
    internal class TestProcessBuilder : IProcessFactory
    {
        public bool IsCoverletEnabled { get; set; }
        public BuildConfiguration BuildConfiguration { get; set; }

        private string _appProjectName;
        private TestProjectInfo _testProjectInfo;

        private string BinFolder => Path.Combine("bin", BuildConfiguration.ToString(), "net7.0");

        public TestProcessBuilder(
            string appProjectName,
            BuildConfiguration buildConfiguration,
            bool isCoverletEnabled
        )
        {
            _appProjectName = appProjectName;
            BuildConfiguration = buildConfiguration;
            IsCoverletEnabled = isCoverletEnabled;
        }

        public ITestProcess Build()
        {
            _testProjectInfo = new TestProjectInfo(_appProjectName);

            var process = new TestProcess();

            process.StartInfo = CreateProcessStartInfo();

            return process;
        }

        public ITestProcess Create(
            string appProjectName,
            BuildConfiguration buildConfiguration,
            bool isCoverletEnabled
        )
        {
            _appProjectName = appProjectName;
            BuildConfiguration = buildConfiguration;
            IsCoverletEnabled = isCoverletEnabled;

            return Build();
        }

        private ProcessStartInfo CreateProcessStartInfo()
        {
            ProcessStartInfo processStartInfo;

            if (!IsCoverletEnabled)
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
