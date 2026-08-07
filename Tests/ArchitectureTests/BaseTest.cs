using Application;
using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArticlesAPI;
using ArticlesApp.Infrastructure.Cache;
using ArticlesApp.Infrastructure.Common;
using ArticlesApp.Infrastructure.DataAccess;
using ArticlesApp.Infrastructure.Logging;
using Domain;
using ReflectionAssembly = System.Reflection.Assembly;

namespace ArchitectureTests
{
    public abstract class BaseTest
    {
        protected static readonly ReflectionAssembly DomainAssembly = typeof(IDomainAssemblyMarker).Assembly;
        protected static readonly ReflectionAssembly ApplicationAssembly = typeof(IApplicationAssemblyMarker).Assembly;
        protected static readonly ReflectionAssembly PresentationAssembly = typeof(IPresentationAssemblyMarker).Assembly;
                
        protected static readonly ReflectionAssembly InfrastructureCommonAssembly = typeof(IInfrastructureCommonAssemblyMarker).Assembly;
        protected static readonly ReflectionAssembly InfrastructureCacheAssembly = typeof(IInfrastructureCacheAssemblyMarker).Assembly;
        protected static readonly ReflectionAssembly InfrastructureDataAccessAssembly = typeof(IInfrastructureDataAccessAssemblyMarker).Assembly;
        protected static readonly ReflectionAssembly InfrastructureLoggingAssembly = typeof(IInfrastructureLoggingAssemblyMarker).Assembly;
               
        protected static readonly Architecture Architecture = new ArchLoader()
            .LoadAssemblies(
                DomainAssembly,
                ApplicationAssembly,
                PresentationAssembly,
                InfrastructureCommonAssembly,
                InfrastructureCacheAssembly,
                InfrastructureDataAccessAssembly,
                InfrastructureLoggingAssembly)
            .Build();
    }
}
