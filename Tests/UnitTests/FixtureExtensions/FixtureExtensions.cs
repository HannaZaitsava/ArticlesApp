using ArticlesApp.Tests.UnitTests.Common;
using AutoFixture;
using MapsterMapper;

namespace ArticlesApp.Tests.UnitTests.FixtureExtensions
{
    public static class FixtureExtensions
    {
        public static IFixture UseMapster(this IFixture fixture)
        {
            var mapper = new Mapper(TestMappingConfig.Instance);
            fixture.Inject<IMapper>(mapper);
            return fixture;
        }
    }
}
