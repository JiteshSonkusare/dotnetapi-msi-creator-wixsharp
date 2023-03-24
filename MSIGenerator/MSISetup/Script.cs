using System;
using WixSharp;
using WixSharp.Forms;
using System.Diagnostics;

namespace MSISetup
{
    internal class Script
    {
        static void Main(string[] args)
        {
            Console.WriteLine("In Main : Arguments:");
            for (int i = 0; i < args.Length; i++) { Console.WriteLine(i + " = [" + args[i] + "]"); }

            string dirProject = args[0];
            string nameProject = args[1];
            string environment = args[2];
            Console.WriteLine("Project name: " + nameProject);
            Console.WriteLine("Project directory: " + dirProject);
            Console.WriteLine("Environment: " + environment);
            Console.WriteLine(" ");
            dirProject = @"C:\dev\MSIGenerator\";
            nameProject = "SampleAPI";

            if (environment.IndexOf("Configuration:") > 0)
            {
                environment = environment.Substring(15);
                Console.WriteLine("Environment2: " + environment);
            }

            string nameBinary = nameProject + ".exe";
            string dirSourceBase = nameProject + @"\" + "bin";
            string dirBinary = dirSourceBase + @"\" + environment + @"\net6.0\";
            string dirSource = dirProject + dirSourceBase + @"\" + environment + @"\";
            string pathBinary = dirProject + dirBinary + nameBinary;

            FileVersionInfo assemblyInfo;
            Version version;

            Console.WriteLine("Project name: " + nameProject);
            Console.WriteLine("Project directory: " + dirProject);
            Console.WriteLine("Source directory: " + dirSource);
            Console.WriteLine("Binary directory: " + dirBinary);
            Console.WriteLine("Binary full path: " + pathBinary);

            assemblyInfo = FileVersionInfo.GetVersionInfo(pathBinary);
            version = new Version(assemblyInfo.FileVersion);

            Console.WriteLine("Version: " + version);
            Console.WriteLine("Manufacturer: " + assemblyInfo.CompanyName);

            var project = new ManagedProject(nameProject,
                new InstallDir(@"C:\inetpub\wwwroot\" + nameProject,
                    new IISVirtualDir
                    {
                        Name = "SampleAPI",
                        AppName = "SampleAPI",
                        WebSite = new WebSite("Default Web Site", "*:80") { InstallWebSite = false },
                        WebAppPool = new WebAppPool("SampleAPI"),
                        WebDirProperties = new WebDirProperties("AnonymousAccess=yes;WindowsAuthentication=no;AuthenticationProviders=Negotiate,NTLM"),
                    },
                    new Files(dirSource + @"net6.0\*.*", f => f.EndsWith(".exe") ||
                                                              f.EndsWith(".dll") ||
                                                              f.EndsWith(".json") ||
                                                              f.EndsWith(".config") ||
                                                              f.EndsWith(".xml") ||
                                                              f.EndsWith(".pdb")
                                                              )
                )
            );

            project.Version = version;
            project.GUID = new Guid("E550D54B-94E2-41CC-956B-E97C7B3A3EFF");
            project.ResolveWildCards(pruneEmptyDirectories: true);
            project.MajorUpgradeStrategy = MajorUpgradeStrategy.Default;
            project.OutFileName = dirProject + nameProject + "_" + project.Version + "_" + environment;
            project.InstallScope = InstallScope.perMachine;
            project.ControlPanelInfo.Contact = "IT Team";
            project.ControlPanelInfo.Manufacturer = "Jitesh Sonkusare";
            project.ControlPanelInfo.Comments = "Sample API component";

            project.ManagedUI = new ManagedUI();
            project.ManagedUI.InstallDialogs.Add(Dialogs.Welcome)
                                            .Add(Dialogs.InstallDir)
                                            .Add(Dialogs.Progress)
                                            .Add(Dialogs.Exit);

            project.ManagedUI.ModifyDialogs.Add(Dialogs.MaintenanceType)
                                            .Add(Dialogs.Progress)
                                            .Add(Dialogs.Exit);

            Compiler.BuildMsi(project);
        }
    }
}
