using UnityEngine;

public class HorizontalDrag : MonoBehaviour
{
    private bool isDragging;
    private float minX, maxX;
    private float distanceToCamera;
    Vector3 newPosition;
    public float[] snapValues; // Array to store custom snap positions
    public float[] snapValuesFor1Tile; // Array to store custom snap positions
    public Spawner spawner;

    void Start()
    {      
        // Calculate the distance from the camera to the object
        distanceToCamera = Camera.main.transform.position.z - transform.position.z;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (spawner.randNumTiles == 1)
            {
                minX = -3.75f;
                maxX = 2.5f;
            }
            else
            {
                // Define the minimum and maximum x positions for dragging
                minX = -2.5f; // Change these values according to your scene
                maxX = 2.5f;  // Change these values according to your scene
            }

            // Set dragging flag to true when the left mouse button is pressed
            isDragging = true;
        }

        if (isDragging)
        {
            // Calculate the new position based on mouse position
            newPosition = GetMouseWorldPos();

            // Clamp the x position within the defined range
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);

            if (Input.GetMouseButtonUp(0))
            {
                // Snap the position horizontally
                newPosition.x = SnapToCustomValues(newPosition.x);
            }

            // Retain the original Y and Z positions
            newPosition.y = transform.position.y;
            newPosition.z = transform.position.z;

            // Update the object's position
            transform.position = newPosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            // Set dragging flag to false when the left mouse button is released
            isDragging = false;
            Invoke(nameof(InvokeResetPos), 1);
        }
    }

    private void InvokeResetPos()
    {
        transform.position = new Vector3(0.15f, 0.75f, -6.5f);
        spawner.SpawnTilesHolder();
    }

    Vector3 GetMouseWorldPos()
    {
        // Get mouse position in screen space
        Vector3 mousePos = Input.mousePosition;

        // Set the distance from camera to object
        mousePos.z = distanceToCamera;

        // Convert the screen space to world space
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    float SnapToCustomValues(float value)
    {
        if (spawner.randNumTiles == 1)
        {
            // Find the closest snap value from the array
            float closestSnap = snapValuesFor1Tile[0];
            float smallestDifference = Mathf.Abs(value - snapValuesFor1Tile[0]);
            foreach (float snapValue in snapValuesFor1Tile)
            {
                float difference = Mathf.Abs(value - snapValue);
                if (difference < smallestDifference)
                {
                    smallestDifference = difference;
                    closestSnap = snapValue;
                }
            }
            return closestSnap;
        }
        else
        {
            // Find the closest snap value from the array
            float closestSnap = snapValues[0];
            float smallestDifference = Mathf.Abs(value - snapValues[0]);
            foreach (float snapValue in snapValues)
            {
                float difference = Mathf.Abs(value - snapValue);
                if (difference < smallestDifference)
                {
                    smallestDifference = difference;
                    closestSnap = snapValue;
                }
            }
            return closestSnap;

        }
    }
}
