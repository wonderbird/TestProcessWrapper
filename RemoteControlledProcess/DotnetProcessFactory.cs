using System.Diagnostics;
using System.IO;

namespace RemoteControlledProcess
{
    internal class DotnetProcessFactory : IProcessFactory
    {
        private bool _isCoverletEnabled;
        private TestProjectInfo _testProjectInfo;

        private static string BinFolder
        {
            get
            {
#if DEBUG
                var binFolder = Path.Combine("bin", "Debug", "net7.0");
#else
                var binFolder = Path.Combine("bin", "Release", "net7.0");
#endif
                return binFolder;
            }
        }

        public IProcess Create(string appProjectName, bool isCoverletEnabled)
        {
            _testProjectInfo = new TestProjectInfo(appProjectName);
            _isCoverletEnabled = isCoverletEnabled;

            var process = new DotnetProcess();

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
