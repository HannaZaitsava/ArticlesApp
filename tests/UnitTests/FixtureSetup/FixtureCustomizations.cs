using ArticlesApp.Application.Common.Caching;
using AutoFixture;

namespace ArticlesApp.Tests.UnitTests.FixtureSetup
{
    public class FixtureCustomizations : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Register<ICacheInvalidationContext>(() => new CacheInvalidationContext());
        }
    }
}
