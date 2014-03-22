namespace Idecom.Host.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// Helpers for assembly scanning operations
    /// </summary>
    public static class AssemblyScanner
    {
        /// <summary>
        /// Gets a list with assemblies that can be scanned
        /// </summary>
        /// <returns></returns>
        [DebuggerNonUserCode] //so that exceptions don't jump at the developer debugging their app
        public static IEnumerable<Assembly> GetScannableAssemblies()
        {
            var assemblyFiles = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).GetFiles("*.dll", SearchOption.AllDirectories);


            foreach (var assemblyFile in assemblyFiles)
            {
                Assembly assembly;

                try
                {
                    assembly = Assembly.LoadFrom(assemblyFile.FullName);

                    //will throw if assembly cant be loaded
                    assembly.GetTypes();
                }

                catch (Exception)
                {
                    continue;
                }

                yield return assembly;
            }
        }
    }
}