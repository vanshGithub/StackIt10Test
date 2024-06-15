using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardTileDetector : MonoBehaviour
{
    public bool isForwardTile;

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("TileHolder") || other.CompareTag("Wall"))
    //    {
    //        isForwardTile = true;
    //    }
    //    else
    //    {
    //        isForwardTile = false;
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{

    //}

    public float detectionDistance = 5f;
     
    private void Update()
    {

        // Cast a ray in the forward direction of the object
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // Check if the ray hits anything within detectionDistance
        if (Physics.Raycast(ray, out hit, detectionDistance))
        {
            // If it hits something, you can do whatever you want here
            isForwardTile = true;
            // You can access the object that was hit via hit.collider
            // For example, you can get its name:
        }
        else
        {
            isForwardTile = false;
            GetComponentInParent<TilesHolder>().Move();
        }
    }
}
