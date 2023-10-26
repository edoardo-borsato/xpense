using System.Globalization;

namespace Xpense.Utility
{
    internal class Formatter : IFormatter
    {
        public string FormatAsEuro(object value)
        {
            return ((double)value).ToString("C", CultureInfo.CreateSpecificCulture("it-IT"));
        }

        public string FormatAsMonth(object value)
        {
            return value switch
            {
                DateTime dateTime => dateTime.ToString("MMM"),
                DateTimeOffset dateTimeOffset => dateTimeOffset.ToString("MMM"),
                int intValue => ((Month)intValue).ToString()[..3],
                string stringValue => ((Month)(int.Parse(stringValue))).ToString()[..3],
                _ => value is not null ? Convert.ToDateTime(value).ToString("MMM") : string.Empty
            };
        }
    }

    internal interface IFormatter
    {
        string FormatAsEuro(object value);
        string FormatAsMonth(object value);
    }

    public enum Month
    {
        AllMonths,
        January,
        February,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December
    }
}
