using System.Globalization;

namespace Xpense.Utility
{
    internal class Formatter : IFormatter
    {
        public string FormatAsEuro(object value)
        {
            return ((double)value).ToString("C", CultureInfo.CreateSpecificCulture("it-IT"));
        }
    }

    internal interface IFormatter
    {
        string FormatAsEuro(object value);
    }
}
