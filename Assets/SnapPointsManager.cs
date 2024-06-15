using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapPointsManager : MonoBehaviour
{
    public SnapPoint GetSnapPoint(int row, int column)
    {
        foreach (SnapPoint snapPoint in GetComponentsInChildren<SnapPoint>())
        {
            if(snapPoint.row == row && snapPoint.column == column)
            {
                return snapPoint;
            }
        }

        Debug.Log("Can't Find Snap Point");
        return null;
    }
}
