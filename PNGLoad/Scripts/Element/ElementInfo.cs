using System;
using UnityEngine;

public struct ElementInfo
{
    public string timeSinceBirth;
    public string name;
    public string imagePath;

    public ElementInfo(TimeSpan timeSpan, string name, string imagePath)
    {
        timeSinceBirth = $"{timeSpan.Days} d, {timeSpan.Hours} h, {timeSpan.Minutes} m ago";
        this.name = name;
        this.imagePath = imagePath;
    }

    public static bool operator== (ElementInfo first, ElementInfo second)
    {
        return first.imagePath.Equals(second.imagePath) && first.timeSinceBirth.Equals(second.timeSinceBirth);
    }
    public static bool operator!= (ElementInfo first, ElementInfo second)
    {
        return !(first == second);
    }
}
