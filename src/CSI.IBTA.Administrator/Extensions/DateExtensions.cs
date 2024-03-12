
namespace CSI.IBTA.Administrator.Extensions
{
    public static class DateExtensions
    {
        public static string ToAmericanDateFormat (this DateOnly date)
        {
            return $"{date.Day}/{date.Month}/{date.Year}";
        }
    }
}
