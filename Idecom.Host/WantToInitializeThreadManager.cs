using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Idecom.Host.Interfaces;
using Idecom.Host.Utility;
using Topshelf;

namespace Idecom.Host
{
    public class WantToInitializeThreadManager
    {
        readonly IContainerAdapter _containerAdapter;
        List<Type> _types;

        public WantToInitializeThreadManager(IContainerAdapter containerAdapter)
        {
            _containerAdapter = containerAdapter;
            _types = AssemblyScanner.GetScannableAssemblies().SelectMany(x => x.GetTypes()).Implementing<IWantToInitializeAfterServiceStarts>().ToList();
        }


        List<IWantToInitializeAfterServiceStarts> _resolvedServicesCache;
        public Task BeforeStart(HostStartedContext hostStartedContext)
        {
            return Task.Factory.StartNew(() =>
            {
                lock (_types)
                {
                    _resolvedServicesCache = _types.Select(type =>
                    {
                        var service = _containerAdapter.Resolve<IWantToInitializeAfterServiceStarts>(type);
                        if (service == null)
                        {
                            _containerAdapter.Configure(type, Lifecycle.Singleton);
                            service = _containerAdapter.Resolve<IWantToInitializeAfterServiceStarts>(type);
                        }
                        return service;
                    }).ToList();
                }

                foreach (var service in _resolvedServicesCache)
                {
                    try
                    {
                        service.Initialize();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error starting IWantToInitializeAfterServiceStarts service: " + e);
                    }
                }
            });
        }

        public Task BeforeStop(HostStopContext hostStopContext)
        {
            return Task.Factory.StartNew(() =>
            {
                foreach (var service in _resolvedServicesCache)
                {
                    try
                    {
                        service.Destroy();
                        _containerAdapter.Release(service);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    _types = null;
                }
            });
        }
    }
}