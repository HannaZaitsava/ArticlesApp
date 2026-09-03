using ArticlesApp.Application;
using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArticlesApp.ArticlesAPI;
using ArticlesApp.Domain;
using ArticlesApp.Infrastructure.Cache;
using ArticlesApp.Infrastructure.DataAccess;
using ArticlesApp.Infrastructure.Logging;
using ArticlesApp.SharedKernel;
using ReflectionAssembly = System.Reflection.Assembly;

namespace ArticlesApp.Tests.ArchitectureTests
{
    public abstract class BaseTest
    {
        protected static readonly ReflectionAssembly DomainAssembly = typeof(IDomainAssemblyMarker).Assembly;
        protected static readonly ReflectionAssembly ApplicationAssembly = typeof(IApplicationAssemblyMarker).Assembly;
        protected static readonly ReflectionAssembly PresentationAssembly = typeof(IPresentationAssemblyMarker).Assembly;
        protected static readonly ReflectionAssembly SharedKernelAssembly = typeof(ISharedKernelAssemblyMarker).Assembly;

        protected static readonly ReflectionAssembly InfrastructureCacheAssembly = typeof(IInfrastructureCacheAssemblyMarker).Assembly;
        protected static readonly ReflectionAssembly InfrastructureDataAccessAssembly = typeof(IInfrastructureDataAccessAssemblyMarker).Assembly;
        protected static readonly ReflectionAssembly InfrastructureLoggingAssembly = typeof(IInfrastructureLoggingAssemblyMarker).Assembly;
               
        protected static readonly Architecture Architecture = new ArchLoader()
            .LoadAssemblies(
                DomainAssembly,
                ApplicationAssembly,
                PresentationAssembly,
                SharedKernelAssembly,
                InfrastructureCacheAssembly,
                InfrastructureDataAccessAssembly,
                InfrastructureLoggingAssembly)
            .Build();
    }
}
