using AutoFixture.Xunit2;

namespace ArticlesApp.Tests.UnitTests.Attributes
{
    public class InlineAutoMoqDataAttribute : InlineAutoDataAttribute
    {        
        public InlineAutoMoqDataAttribute(params object[] values)
            : base(new AutoMoqDataAttribute(), values)
        {
        }
    }
}
