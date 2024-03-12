using System.Globalization;

namespace CSI.IBTA.Shared.Utils.Extensions
{
    public static class DateExtensions
    {
        public static string ToAmericanDateOnlyFormat(this DateOnly date)
        {
            var cultureInfo = new CultureInfo("en-US");
            return date.ToString("dd/MM/yyyy", cultureInfo);
        }

        public static string ToAmericanDateOnlyFormat(this DateTime date)
        {
            var cultureInfo = new CultureInfo("en-US");
            return date.ToString("dd/MM/yyyy", cultureInfo);
        }

        public static string? ToAmericanDateOnlyFormat(this DateOnly? date)
        {
            if (date == null) return null;
            var d = (DateOnly) date;
            return d.ToAmericanDateOnlyFormat();
        }
    }
}
