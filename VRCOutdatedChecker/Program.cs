using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VRCOutdatedChecker
{

    class Project
    {
        static public string LatestUnityVersion = "2018.4.20f1";
        static public string LatestVRCSDKVersion = "2020.05.06.12.14";

        private string directoryName;
        private string unityVersion;
        private string VRCSDKVersion;

        public Project(string directoryName, string unityVersion, string VRCSDKVersion)
        {
            this.directoryName = directoryName;
            this.unityVersion = unityVersion;
            this.VRCSDKVersion = VRCSDKVersion;
        }

        public void PrintInfo()
        {
            int directoryNameWidth = 30;
            Console.Write(directoryName.PadRight(directoryNameWidth));

            if (unityVersion == LatestUnityVersion)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }

            ColorizePrint("Unity", unityVersion, LatestUnityVersion);
            ColorizePrint("VRCSDK", VRCSDKVersion, LatestVRCSDKVersion);
            Console.WriteLine();
        }

        private void ColorizePrint(string title, string current, string latest)
        {
            if (current == latest)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.Write(" [" + title + ": " + current + "]");
            Console.ResetColor();
        }
    }
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Enter Projects Directory:");
            string projectDirPath = System.Console.ReadLine();

            if (!Directory.Exists(projectDirPath))
            {
                Console.WriteLine(projectDirPath + "が存在しません");
                Environment.Exit(1);
            }

            Console.WriteLine("--------------------------");
            Console.WriteLine("Latest Unity Version: " + Project.LatestUnityVersion);
            Console.WriteLine("Latest VRCSDK Version: " + Project.LatestVRCSDKVersion);
            Console.WriteLine("--------------------------");

            string[] dirs = Directory.GetDirectories(projectDirPath);
            List<Project> projects = new List<Project>();

            foreach (var dir in dirs)
            {
                string unityVersion = ObtainProjectUnityVersion(dir);
                if (unityVersion == null)
                {
                    continue;
                }

                string VRCSDKVersion = ObtainProjectVRCSDKVersion(dir);

                Project pro = new Project(Path.GetFileName(dir), unityVersion, VRCSDKVersion);

                projects.Add(pro);
            }

            foreach (var project in projects)
            {
                project.PrintInfo();
            }
            ShowSummary(projects);

            Console.ReadKey();
        }

        static string ObtainProjectUnityVersion(string projectPath)
        {
            string versionFilePath = @"ProjectSettings\ProjectVersion.txt";
            string versionFileFullPath = Path.Combine(projectPath, versionFilePath);

            if (!File.Exists(versionFileFullPath))
            {
                return null;
            }

            string firstLine = File.ReadLines(versionFileFullPath).Take(1).First();
            // m_EditorVersion: 2017.4.28f1 -> 2017.4.28f1
            return firstLine.Substring(17);
        }

        static string ObtainProjectVRCSDKVersion(string projectPath)
        {
            string versionFilePath = @"Assets\VRCSDK\version.txt";
            string versionFileFullPath = Path.Combine(projectPath, versionFilePath);

            if (!File.Exists(versionFileFullPath))
            {
                return "";
            }

            return File.ReadLines(versionFileFullPath).Take(1).First();
        }
        
        static void ShowSummary(List<Project> projects)
        {
            Console.WriteLine("--------------------------");
            Console.WriteLine("Unity プロジェクト数: " + projects.Count);
            Console.WriteLine("--------------------------");
        }
    }
}
