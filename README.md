Idecom.Host - yet another lightweight opinionated windows service host
--------

The hosting process which comes as a replacement for NServiceBus.Host

Create an empty library project in VisualStudio. Please don't create a console app as this is not compatible with the hosting process.

Installation
--------

Install NuGet package from NuGet repository:

````
Install-Package Idecom.Host
````

 


After installing a package the install script would configure your project to execute the host on F5 run command.

NuGet installer script would add HostConfig.cs with a sample host configuration so you'd have a ready to roll service.

Visual Studio would ask you to re-load project as we'd make modifications to it.
 

 


This host process is used to host a single service only.

If you'd like to host multiple services please achieve this by using threads.
 

Command-line parameters
--------
````
Idecom.Host.exe help
Idecom.Host.exe install
Idecom.Host.exe uninstall
````

Other parameters could be found here: http://docs.topshelf-project.com/en/latest/overview/commandline.html

Configuration
--------

Adding CastleWindsor support
--------

````
Install-Package Idecom.Host.CastleWindsor
````

After installing the package you need to implement IWantToSpecifyContainer interface on a class of your choosing (this can be a service class)

````csharp
public class SampleService : HostedService, IWantToSpecifyContainer
{

   public IContainerAdapter ConfigureContainer()
   {
       var container = new WindsorContainer();
       container.Install(FromAssembly.InDirectory(new AssemblyFilter(""))); 
       return new CastleWindsorContainerAdapter(container);
   }

}
```
 

Sample service configuration
--------
````csharp
namespace MyServiceNamespace
{
    using System;
    using Topshelf;
    using Topshelf.HostConfigurators;
    using Topshelf.Logging;
    using Idecom.Bus.Interfaces;
    using Idecom.Host.Interfaces;
    using Idecom.Host.CastleWindsor;

    public class SampleService : HostedService, IWantToSpecifyContainer
    {
        public ILog Log { get; set; }

        public override bool Start(HostControl hostControl)
        {
            Log.Info("SampleService Starting...");
            hostControl.RequestAdditionalTime(TimeSpan.FromSeconds(10));  //requests more time from Windows Service Manager for service operation
            Log.Info("SampleService Started");
            return true;
        }
        public override bool Stop(HostControl hostControl)
        {
            Log.Info("SampleService Stopped");
            return true;
        }
        public override void OverrideDefaultConfiguration(HostConfigurator configurator) //use this override if you want to update the default configuration
        {
            Console.Write("Hook to extendconfiguration");
        }
        public bool Pause(HostControl hostControl)
        {
            Log.Info("SampleService Paused");
            return true;
        }
        public bool Continue(HostControl hostControl)
        {
            Log.Info("SampleService Continued");
            return true;
        }

        public IContainerAdapter ConfigureContainer()
        {
            var container = new WindsorContainer();
            container.Install(FromAssembly.InDirectory(new AssemblyFilter(""))); 
            return new CastleWindsorContainerAdapter(container);
        }

    }
}
``` 
