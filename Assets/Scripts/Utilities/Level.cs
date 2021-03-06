﻿using System;
using UnityEngine;

[System.Serializable]
public class Level
{
    public TimeSpan time;
    public int deaths;
    public bool available;

    public Level()
    {
        time = FromFloatToTimeSpan(0);
        deaths = -1;
        available = false;
    }

    public static TimeSpan FromFloatToTimeSpan(float seconds)
    {
        return new TimeSpan(0, 0, 0, 0, (int)(seconds * 1000));
    }
}
