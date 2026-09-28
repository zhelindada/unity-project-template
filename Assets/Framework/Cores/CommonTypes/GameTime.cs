namespace Dada.Cores;

public struct GameTime
{
    public int Year;
    public int Month;
    public int Day;
    
    public int Hour;
    public int Minute;
    public int Second;

    public int YearToMonth;
    public int MonthToDay;
    public int DayToHour;
    public int HourToMinute;
    public int MinuteToSecond;

    public override string ToString()
    {
        return $"{Year:0000}/{Month:00}/{Day:00} {Hour:00}:{Minute:00}:{Second:00}";
    }
}
