using System;
using System.IO;
using System.Linq;

namespace RaytraceAir
{
    public static class AppEnvironment
    {
        private static string _solutionFolder;

        private static string AppSolutionFileName { get; } = "RaytraceAir.sln";
        private static string OutputFolderName { get; } = "Output";

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

        private static bool DirectoryContainsAppSolutionFile(DirectoryInfo directory)
        {
            return directory.GetFiles()
                .All(file => string.Compare(file.Name, AppSolutionFileName, StringComparison.Ordinal) != 0);
        }
    }
}