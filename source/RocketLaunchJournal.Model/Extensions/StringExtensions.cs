using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public static class StringExtensions
{
    /// <summary>
    /// Truncates the string to the specified length
    /// </summary>
    /// <param name="str">string to truncate</param>
    /// <param name="maxLength">max length of the string</param>
    /// <returns>truncated string</returns>
    public static string Truncate(this string str, int maxLength)
    {
        if ((String.IsNullOrEmpty(str)) || (maxLength <= 0)) return str;
        if (str.Length > maxLength) return str.Substring(0, maxLength);
        return str;
    }

    /// <summary>
    /// Parses the csv line into a line array.
    /// </summary>
    /// <param name="line">line to parse</param>
    /// <returns>List of strings</returns>
    public static List<string> ParseCSVLine(this string line)
    {
        var lineArray = new List<string>();
        var index = 0;
        var nextIndex = line.IndexOf(',');
        while (index != -1)
        {
            if (nextIndex > -1 && line.Substring(index, nextIndex - index).Count(w => w == '"') == 1)
                nextIndex = line.IndexOf(',', nextIndex + 1);
            else
            {
                if (nextIndex == -1)
                {
                    lineArray.Add(line.Substring(index));
                    index = nextIndex;
                }
                else
                {
                    lineArray.Add(line.Substring(index, nextIndex - index));
                    index = nextIndex + 1;
                    nextIndex = line.IndexOf(',', index);
                }
            }
        }

        return lineArray;
    }

    /// <summary>
    /// PhoneFormats
    /// </summary>
    public enum PhoneFormats
    {
        /// <summary>
        /// 123 456 7890
        /// </summary>
        USPhone,
        /// <summary>
        /// 123-456-7890
        /// </summary>
        USPhoneDashes,
        /// <summary>
        /// (123) 456-7890
        /// </summary>
        USPhoneParens

    }
    /// <summary>
    /// Format String as USPhone
    /// </summary>
    /// <param name="phoneFormat"></param>
    /// <returns>Formatted string</returns>
    public static string FormatAsPhone(this string str, PhoneFormats phoneFormat)
    {
        if (String.IsNullOrEmpty(str))
            return "";

        //first we must remove all non numeric characters
        str = str.Replace("(", "").Replace(")", "").Replace("-", "");
        string results = string.Empty;
        string formatPattern = @"(\d{3})(\d{3})(\d{4})";
        string resultPattern;
        switch (phoneFormat)
        {
            case PhoneFormats.USPhone:
                resultPattern = "$1 $2 $3";
                break;
            case PhoneFormats.USPhoneDashes:
                resultPattern = "$1-$2-$3";
                break;
            case PhoneFormats.USPhoneParens:
                resultPattern = "($1) $2-$3";
                break;
            default:
                resultPattern = "$1$2$3";
                break;
        }

        results = Regex.Replace(str, formatPattern, resultPattern);
        //now return the formatted phone number
        return results;
    }

    /// <summary>
    /// Replaces all non numerics with empty string
    /// </summary>
    /// <param name="str"></param>
    /// <param name="allowNumericCharacters">allows numeric characters, decimal ".", comma ",", etc.</param>
    /// <param name="allowDecimal">this will change (100) to -100</param>
    /// <returns>String</returns>
    public static string RemoveNonNumerics(this string str, RemoveNonNumericsEnum removeNonNumerics = RemoveNonNumericsEnum.Digits)
    {
        if (String.IsNullOrEmpty(str))
            return str;

        switch (removeNonNumerics)
        {
            case RemoveNonNumericsEnum.Digits:
                return Regex.Replace(str, "[^0-9]", "");
            case RemoveNonNumericsEnum.DigitsWithDash:
                return Regex.Replace(str, "[^0-9-]", "");
            case RemoveNonNumericsEnum.DigitsWithDashAndDecimal:
                return Regex.Replace(str, "[^0-9-.]", "");
            default:
                return str;
        }
    }

    public enum RemoveNonNumericsEnum
    {
        Digits,
        DigitsWithDash,
        DigitsWithDashAndDecimal
    }
}
