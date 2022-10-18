using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Borigran.OneData.Platform.Dependencies
{
    public class AssemblyScanner
    {
        public const string AssemblyNamePrefix = "Borigran.OneData";

        public static IEnumerable<Func<Assembly, bool>> DefaultAutoRegisterIgnoredAssemblies = new Func<Assembly, bool>[]
        {
            asm => asm.FullName.StartsWith("Microsoft.", StringComparison.InvariantCulture),
            asm => asm.FullName.StartsWith("System.", StringComparison.InvariantCulture),
            asm => asm.FullName.StartsWith("System,", StringComparison.InvariantCulture),
            asm => asm.FullName.StartsWith("CR_ExtUnitTest", StringComparison.InvariantCulture),
            asm => asm.FullName.StartsWith("mscorlib,", StringComparison.InvariantCulture),
            asm => asm.FullName.StartsWith("CR_VSTest", StringComparison.InvariantCulture),
            asm => asm.FullName.StartsWith("DevExpress.CodeRush", StringComparison.InvariantCulture),
            asm => asm.FullName.StartsWith("IronPython", StringComparison.InvariantCulture),
            asm => asm.FullName.StartsWith("IronRuby", StringComparison.InvariantCulture),
            asm => asm.FullName.StartsWith("xunit", StringComparison.InvariantCulture),
            asm => asm.FullName.StartsWith("Nancy", StringComparison.InvariantCulture),
            asm => asm.FullName.StartsWith("MonoDevelop", StringComparison.InvariantCulture),
            asm => asm.FullName.StartsWith("SMDiagnostics", StringComparison.InvariantCulture),
            asm => asm.FullName.StartsWith("CppCodeProvider", StringComparison.InvariantCulture),
            asm => asm.FullName.StartsWith("WebDev.WebHost40", StringComparison.InvariantCulture),
            asm => asm.FullName.StartsWith("vshost", StringComparison.InvariantCulture),
            asm => asm.FullName.StartsWith("Autofac", StringComparison.InvariantCulture),
            asm => asm.FullName.StartsWith("log4net", StringComparison.InvariantCulture),
            asm => asm.FullName.StartsWith("TopShelf", StringComparison.InvariantCulture),
            asm => asm.FullName.StartsWith("Common.Logging", StringComparison.InvariantCulture),
            asm => asm.FullName.StartsWith("DynamicProxyGenAssembly2", StringComparison.InvariantCulture),
        };

        public Assembly[] AssembliesToScan()
        {
            var assemblies = ScanForLinkedAssemblies().ToArray();
            return assemblies;
        }

        private IEnumerable<Assembly> ScanForLinkedAssemblies()
        {
            var projectAssemblies = new List<Assembly>() { Assembly.GetEntryAssembly() };
            projectAssemblies.AddRange(Assembly.GetEntryAssembly().GetReferencedAssemblies()
                .Select(x => Assembly.Load(x)));
            
            return projectAssemblies
                // exclude dynamic assemblies
                .Where(a => !a.IsDynamic)
                // filter the project namespace
                .Where(a => a.FullName.StartsWith(AssemblyNamePrefix))
                .ToArray();
        }

        protected virtual IEnumerable<Func<Assembly, bool>> AutoRegisterIgnoredAssemblies
        {
            get { return DefaultAutoRegisterIgnoredAssemblies; }
        }
    }
}
