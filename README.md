# MSI File Creator For Dotnet API Using Wixsharp
This projetc will helps developers to create the MSI file for any dotnet API (dotnetcore, dotnet6, dotnet7,..) to deploy API to IIS server.  

## Steps to follow:
1. Create .net framework 4.8 console aplicaition with name such as 'SetupMSI'.
2. Add Wixsharp nuget package using following command: 
            
            dotnet add package WixSharp --version 1.20.3
3. Right click on SetupMSI project go to: properties > build events > post build event command line' and add below code
            
            "$(TargetPath)" $(SolutionDir) $(SolutionName) $(ConfigurationName)
4. Now, change name Program.cs to Script.cs add the code shown in repository Script.cs file
5. In script.cs class, replace the directory path for which project you want to create the MSI as shown below (path until project.sln file placed):
            
            dirProject = @"C:\dev\SampleAPI\main\";
            nameProject = "SampleAPI";
6. We can also modify the ManagedProject object according to our requirment, we can add path InstallDir path of server where we want to install the project. Also can        change IISVirtualDir objects field values according to requirement as shown below, also you can set  WindowsAuthentication to 'yes' or 'no'

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

7. In API Project, add web.config file if it not there, with reference code like below:

            <?xml version="1.0" encoding="utf-8"?>
            <configuration>
                        <system.webServer>
                                    <modules>
                                                <remove name="WebDAVModule" />
                                    </modules>
                                    <handlers>
                                                <remove name="WebDAV" />
                                                <remove name="aspNetCore" />
                                                <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModuleV2" resourceType="Unspecified" />
                                    </handlers>
                                    <aspNetCore processPath="dotnet" arguments=".\SampleAPI.dll" stdoutLogEnabled="true" stdoutLogFile=".\logs\stdout" hostingModel="InProcess" />
                        </system.webServer>
            </configuration>
            
## How to create MSI:
1. Go to .csproj file of API project for which you want to create MSI, and add the below lines: 
            
            <Authors>IT Team</Authors>
            <Company>Name</Company>
            <Version>1.0.0.1</Version>
            <AssemblyVersion>1.0.0.1</AssemblyVersion>
            <FileVersion>1.0.0.1</FileVersion>
  
2. Set API project to Release mode to get MSI in release mode. 
3. Reabuild the API project.
4. Now, go to the 'SetupMSI' and build the project, it will start building and start generating msi file for you (you can check all the details in output wwindow).
5. You can find the msi file in API project diretory with name like: 
              
              SampleAPI_1.0.0.1_Release
6. Now, your msi is ready to install, Once you install this msi, it will create web application in IIS and create application pool. You can uninstall it through control panel.
   

