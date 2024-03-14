using CSI.IBTA.Shared.Entities;
using System.Text;

namespace CSI.IBTA.Shared.Extensions
{
    public static class ClaimStatusExtensions
    {
        public static string ToCustomString(this ClaimStatus value)
        {
            StringBuilder result = new();

            foreach (char c in value.ToString())
            {
                if (char.IsUpper(c))
                {
                    result.Append(' ');
                }

                result.Append(c);
            }

            return result.ToString().Trim();
        }
    }
}
