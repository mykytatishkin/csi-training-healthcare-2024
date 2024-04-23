using AutoFixture;
using AutoFixture.Xunit2;

namespace CSI.IBTA.Administrator.Tests.Utils
{
    public class AutoDataExAttribute : AutoDataAttribute
    {
        public static Fixture GetFixture()
        {
            var fixture = new Fixture();
            fixture.Customize<DateOnly>(composer => composer.FromFactory<DateTime>(DateOnly.FromDateTime));
            return fixture;
        }

        public AutoDataExAttribute()
          : base(GetFixture)
        { }
    }
}
