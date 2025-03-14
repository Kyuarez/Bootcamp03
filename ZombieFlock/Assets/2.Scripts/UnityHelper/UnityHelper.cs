using System.Collections.Generic;
using UnityEngine;

public static class UnityHelper
{
    public static List<T> Shuffle<T>(List<T> originalList)
    {
        List<T> list = new List<T>(originalList);
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
        return list;
    }
}
