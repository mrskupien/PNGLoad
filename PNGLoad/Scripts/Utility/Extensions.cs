using System.Collections.Generic;

public static class Extensions
{
    public static bool HasSameFile(this List<ListElement> list, ElementInfo elementToCompare)
    {
        foreach (var element in list)
        {
            if (element.ElementInfo == elementToCompare)
            {
                return true;
            }
        }
        return false;
    }
}
