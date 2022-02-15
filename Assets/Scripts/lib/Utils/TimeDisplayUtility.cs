using System;
using UnityEngine;

public static class TimeDisplayUtility
{
	public static string GetTotalTimeText(float min)
	{
		string timeText;
        
		int minutes = Mathf.FloorToInt(min);
		int seconds = Mathf.FloorToInt(min * 60f);
		int hours = Mathf.FloorToInt(min / 60f);
        
		TimeSpan time = new TimeSpan(hours, minutes % 60, seconds % 60);

		if (hours > 0)
			timeText = time.ToString("%h") + "h";
		else if (minutes > 0)
            timeText = time.ToString("%m") + "min";
		else
            timeText = time.ToString("%s") + "sec";

		return timeText;
	}

	public static string GetClockTimeText(float sec)
	{
		float totalSeconds = sec;
		TimeSpan time = TimeSpan.FromSeconds(totalSeconds);
		return time.ToString("hh':'mm':'ss");
	}

	public static int GetMinute(float sec)
    {
		float totalSeconds = sec;
		TimeSpan time = TimeSpan.FromSeconds(totalSeconds);

		int min = time.Minutes + (time.Hours * 60);
		return min;
	}
	
	public static string GetClockTimeTextJustMinutes(float sec)
	{
		float totalSeconds = sec;
		TimeSpan time = TimeSpan.FromSeconds(totalSeconds);
		return time.ToString("mm':'ss");
	}
}
