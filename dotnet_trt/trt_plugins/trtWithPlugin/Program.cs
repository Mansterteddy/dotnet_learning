using McMaster.NETCore.Plugins;
using PluginBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace trtWithPlugin
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var loaders = new List<PluginLoader>();

            string[] pluginPaths = new string[]
            {
                @"C:\Yuan\Git\dotnet_learning\dotnet_trt\trt_plugins\TRT8Plugin\bin\Release\net5.0\TRT8Plugin.dll",
                @"C:\Yuan\Git\dotnet_learning\dotnet_trt\trt_plugins\TRT7Plugin\bin\Release\net5.0\TRT7Plugin.dll"
            };

            var loader_0 = PluginLoader.CreateFromAssemblyFile(pluginPaths[0], sharedTypes: new[] { typeof(ICommand) });
            
            foreach (var pluginType in loader_0
                .LoadDefaultAssembly()
                .GetTypes()
                .Where(t => typeof(ICommand).IsAssignableFrom(t) && !t.IsAbstract))
            {
                // This assumes the implementation of IPlugin has a parameterless constructor
                var plugin = Activator.CreateInstance(pluginType) as ICommand;

                Console.WriteLine($"Created plugin instance '{plugin?.Name}'.");
                Console.WriteLine(plugin?.Name);
                plugin?.InitEngine();
            }

            var loader_1 = PluginLoader.CreateFromAssemblyFile(pluginPaths[1], sharedTypes: new[] { typeof(ICommand) });

            foreach (var pluginType in loader_1
                .LoadDefaultAssembly()
                .GetTypes()
                .Where(t => typeof(ICommand).IsAssignableFrom(t) && !t.IsAbstract))
            {
                // This assumes the implementation of IPlugin has a parameterless constructor
                var plugin = Activator.CreateInstance(pluginType) as ICommand;

                Console.WriteLine($"Created plugin instance '{plugin?.Name}'.");
                Console.WriteLine(plugin?.Name);
                plugin?.InitEngine();
            }

        }
    }
}
