using System;
using System.IO;
using System.Linq;

namespace RaytraceAir
{
    public static class AppEnvironment
    {
        private static string _solutionFolder;
        private static string _tempFolder;

        private static string AppSolutionFileName { get; } = "RaytraceAir.sln";
        private static string OutputFolderName { get; } = "Output";
        private static string TestFolderName { get; } = "RaytraceAirTest";
        private static string TestResultsFolderName { get; } = "TestResults";
        private static string ReferenceImagesFolderName { get; } = "ReferenceImages";

        private static string SolutionFolder
        {
            get
            {
                if (_solutionFolder == null)
                {
                    var currentFolder = new DirectoryInfo(typeof(AppEnvironment).Assembly.Location);

                    do
                    {
                        currentFolder = currentFolder?.Parent;
                    } while (DirectoryContainsAppSolutionFile(currentFolder));

                    _solutionFolder = currentFolder?.FullName;

                    return _solutionFolder;
                }

                return _solutionFolder;
            }
        }

        public static string OutputFolder { get; } = Path.Combine(SolutionFolder, OutputFolderName);

        public static string TestReferenceFolder { get; } = Path.Combine(SolutionFolder, TestFolderName, ReferenceImagesFolderName);

        public static string TempFolderInTestResults => _tempFolder ?? (_tempFolder = NewTempFolderInTestResults);

        private static string TestResultsFolder { get; } = Path.Combine(SolutionFolder, TestFolderName, TestResultsFolderName);

        private static string NewTempFolderInTestResults => Path.Combine(TestResultsFolder, Guid.NewGuid().ToString());

        private static bool DirectoryContainsAppSolutionFile(DirectoryInfo directory)
        {
            return directory.GetFiles()
                .All(file => string.Compare(file.Name, AppSolutionFileName, StringComparison.Ordinal) != 0);
        }
    }
}