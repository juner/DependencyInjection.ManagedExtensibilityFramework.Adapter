using System;
using System.Collections.Generic;
using System.Text;
using System.Composition;
using System.Composition.Hosting.Core;
using System.Linq;

namespace DependencyInjection.ManagedExtensibilityFramework.Adapter
{
    public class MefServiceProvider : IServiceProvider
    {
        public CompositionContext Context;
        public MefServiceProvider(CompositionContext Context)
            => this.Context = Context;
        public object GetService(Type serviceType)
        {
            if (TryGetEnumerableElementType(serviceType, out Type EnumerableElementType))
                return Context.GetExports(EnumerableElementType);
            return Context.GetExport(serviceType);
        }
        bool TryGetEnumerableElementType(Type serviceType, out Type enumerableElementType)
        {
            enumerableElementType = default!;
            if (serviceType
                .GetInterfaces()
                .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                .Select(t => t.GetGenericArguments()[0]) is Type export)
            {
                enumerableElementType = export;
                return true;
            }
            return false;
        }
    }
}
