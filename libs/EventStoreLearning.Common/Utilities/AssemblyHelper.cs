using System;
using System.Linq;
using System.Reflection;

namespace EventStoreLearning.Common.Utilities
{
    public static class AssemblyHelper
    {
        public static Assembly[] GetAllOriginalAssembliesAroundType(Type startupType)
        {
            var executingAssembly = startupType.Assembly;
            var executingAssemblyPrefix = executingAssembly.FullName.Split('.')[0];

            var assembliesList = executingAssembly.GetReferencedAssemblies().Select(Assembly.Load).ToList();
            assembliesList.Add(executingAssembly);

            var assemblies = assembliesList
                .Where(assembly => assembly.FullName.StartsWith(executingAssemblyPrefix, StringComparison.CurrentCulture))
                .ToArray();

            return assemblies;
        }

        public static Assembly[] AddAssemblyFromType(this Assembly[] assemblies, Type type)
        {
            var results = assemblies.ToList();
            results.Add(type.Assembly);

            return results.ToArray();
        }
    }
}
