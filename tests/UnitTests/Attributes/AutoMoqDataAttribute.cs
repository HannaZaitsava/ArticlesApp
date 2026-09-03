using ArticlesApp.Tests.Shared.FixtureCustomizations;
using ArticlesApp.Tests.UnitTests.FixtureExtensions;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace ArticlesApp.Tests.UnitTests.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor)]
    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute() : base(() => 
            new Fixture()
            .Customize(new AutoMoqCustomization())
            .Customize(new CommonCustomization())
            .UseMapster())       
        {

        }
    }
}
