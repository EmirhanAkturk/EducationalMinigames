using System;
public static class Timestamp
{

    public static int Now()
    {
        return GetByDate(DateTime.UtcNow);
    }

    public static int GetByDate(DateTime date)
    {
        DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        int currentEpochTime = (int)(date - epochStart).TotalSeconds;

        return currentEpochTime;
    }
}

