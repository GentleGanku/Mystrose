namespace Mystrose.Utilities.Tools;

public static class NumberHelper
{

    #region Methods
    public static string ToOrdinal(int number)
    {
        if (number < 0)
        {
            return "";
        }

        int rem100 = number % 100;
        if (rem100 >= 11 && rem100 <= 13)
        {
            return $"{number}th";
        }

        int rem10 = number % 10;
        string suffix = rem10 switch
        {
            1 => "st",
            2 => "nd",
            3 => "rd",
            _ => "th"
        };

        return $"{number}{suffix}";
    }
    #endregion
    
}