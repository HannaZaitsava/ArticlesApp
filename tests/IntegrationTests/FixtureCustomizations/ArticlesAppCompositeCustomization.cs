using ArticlesApp.Tests.Shared.FixtureCustomizations;
using AutoFixture;

namespace ArticlesApp.Tests.IntegrationTests.FixtureCustomizations
{
    public class ArticlesAppCompositeCustomization : CompositeCustomization
    {
        public ArticlesAppCompositeCustomization()
            : base(
                new CommonCustomization(),
                new TagsCustomization())
        {
        }
    }   
}
