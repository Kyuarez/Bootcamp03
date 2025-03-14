using System.Collections.Generic;
using UnityEngine;

public class PatrolPointManager : MonoBehaviour
{
    protected List<Transform> pointList;

    private void OnEnable()
    {
        pointList = new List<Transform>();

        PatrolPoint[] points = transform.GetComponentsInChildren<PatrolPoint>();
        foreach (var point in points)
        {
            pointList.Add(point.transform);
        }
    }

    public List<Transform> GetRandomPointList()
    {
        return UnityHelper.Shuffle(pointList);
    }

    
}
