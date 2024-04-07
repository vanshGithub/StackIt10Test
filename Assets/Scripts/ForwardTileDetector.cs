using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardTileDetector : MonoBehaviour
{
    public bool isForwardTile;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("TileHolder") || other.CompareTag("Wall"))
        {
            isForwardTile = true;
        }
        else
        {
            isForwardTile = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
