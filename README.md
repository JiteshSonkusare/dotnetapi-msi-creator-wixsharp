# dotnetapi-msi-creator-wixsharp
Creates a MSI of dotnet API using Wixsharp

Steps to follow:
1. Create .net framework 4.8 console aplicaition with name like 'SetupMSI'.
2. Add Wixsharp nuget package using following command: 
            <dotnet add package WixSharp --version 1.20.3>

3. Right click on project go to: 'build events > post build event command line' and add below code
            "$(TargetPath)" $(SolutionDir) $(SolutionName) $(ConfigurationName)

4. Now, chnage name Program.cs to Script.cs add the code shown in repository Script.cs file
5. In script.cs class, replace the directory path for which project you want to create the MSI as shown below: 
            
            dirProject = @"C:\dev\SampleAPI\main\";
            nameProject = "SampleAPI";

6. We can also modify the ManagedProject object according to our requirment, we can add path InstallDir path of server where we want to install the project. Alos can        change IISVirtualDir objects data according to requirement as show below, also you can set  WindowsAuthentication to 'yes' or 'no'

             var project = new ManagedProject(nameProject,
                        new InstallDir(@"C:\inetpub\wwwroot\" + nameProject,
                                new IISVirtualDir
                                {
                                    Name = "SampleAPI",
                                    AppName = "SampleAPI",
                                    WebSite = new WebSite("Default Web Site", "*:80") { InstallWebSite = false },
                                    WebAppPool = new WebAppPool("SampleAPIPool"),
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
7. How to create MSI: 
  a. Go to .csproj file of API project for which you want to create MSI, and add the below lines: 
            	<Authors>IT Team</Authors>
              <Company>Name</Company>
              <Version>1.0.0.1</Version>
              <AssemblyVersion>1.0.0.1</AssemblyVersion>
              <FileVersion>1.0.0.1</FileVersion>
  b. Set API project to Release mode to get MSI in release mode. 
  c. Reabuld the API project.
  d. Now, go to the 'SetupMSI' and build the project, it will start building and start generating msi file for you (you can checck all the details in output wwindow).
  e. You can find the msi file in API project diretory with name like: 
              SampleAPI_1.0.0.1_Release
8. Now, your msi is ready to install, it create web application in IIS and create application pool as well. You can uninstall it through control panel.
   

