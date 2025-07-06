using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walleto.Shared.Calendar;

public static class Calendar
{
    private static readonly PersianCalendar _persianCalendar = new() ;


    public static string ToShamsi(DateTime gregorian)
    {
        return $"{_persianCalendar.GetYear(gregorian):0000}/" +
               $"{_persianCalendar.GetMonth(gregorian):00}/" +
               $"{_persianCalendar.GetDayOfMonth(gregorian):00}";
    }

    public static DateTime ToGregorian(string shamsi)
    {
        var parts = shamsi.Split('/', '-');
        if (parts.Length != 3) throw new ArgumentException("Invalid Shamsi date.");
        int year = int.Parse(parts[0]);
        int month = int.Parse(parts[1]);
        int day = int.Parse(parts[2]);
        return _persianCalendar.ToDateTime(year, month, day, 0, 0, 0, 0);
    }


    public static bool IsValidShamsi(string shamsi)
    {
        try
        {
            ToGregorian(shamsi);
            return true;
        }
        catch { return false; }
    }



    /// <summary>
    /// بررسی 18 سال تمام سن یک فرد
    /// </summary>
    /// <param name="birthdate"></param>
    /// <returns></returns>
    public static bool IsAgeGreatherThan18(string birthdate)
    {
        bool result;
        var gregBirthdate = Calendar.ToGregorian(birthdate);
        TimeSpan difference = DateTime.Today - gregBirthdate;
        int age = (int)(difference.TotalDays / 365.25);
        if (age >= 18)
        {
            result = true;
        }
        else
        {
            result = false;
        }
        return result;
    }

    /// <summary>
    /// محاسبه سن بر اساس تاریخ تولد
    /// </summary>
    /// <param name="birthdate"></param>
    /// <returns></returns>
    public static int CalcuteAge(string birthdate)
    {
        int result;
        var gregBirthdate = Calendar.ToGregorian(birthdate);
        TimeSpan difference = DateTime.Today - gregBirthdate;
        result = (int)(difference.TotalDays / 365.25);

        return result;

    }

}
