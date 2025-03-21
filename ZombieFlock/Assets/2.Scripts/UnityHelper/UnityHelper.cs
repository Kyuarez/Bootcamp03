using System.Collections.Generic;
using UnityEngine;

public static class UnityHelper
{
    public static bool IsTargetInFront(this Transform caller, Transform target)
    {
        Vector3 playerForward = new Vector3(caller.forward.x, 0f, caller.forward.z).normalized;
        Vector3 toTarget = new Vector3((target.position.x - caller.position.x), 0, (target.position.z - caller.position.z)).normalized;
        float dot = Vector3.Dot(playerForward, toTarget);
        return dot > 0;
    }
    public static bool IsTargetInBack(this Transform caller, Transform target)
    {
        Vector3 playerBack = new Vector3(caller.forward.x, 0f, caller.forward.z).normalized * -1f;
        Vector3 toTarget = new Vector3((target.position.x - caller.position.x), 0, (target.position.z - caller.position.z)).normalized;
        float dot = Vector3.Dot(playerBack, toTarget);
        return dot > 0;
    }
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

    public static Transform FindRecursiveChild(this Transform root, string name)
    {
        foreach (Transform trans in root.transform)
        {
            if(trans.name == name)
            {
                return trans;
            }

            var ret = FindRecursiveChild(trans, name);
            if(ret != null)
            {
                return ret;
            }
        }

        return null;
    }


}
