using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TilesHolder : MonoBehaviour
{
    public Rigidbody rb;
    public float forwardForce;
    private bool isColWithWallorTile;
    [HideInInspector] public List<GameObject> nearbyTile;
    public Transform spawnPoint;
    private float yIncVal;
    [HideInInspector] public List<Tile> tiles;
    public GameObject tilePrefab;
    public Material[] tileMats;
    [HideInInspector] public bool isNewTileHolder = true;
    public float rayLength;
    private TilesHolder matchedTileHolder;
    //[HideInInspector] public bool frontObject;

    private void Start()
    {
        print(gameObject.layer);
        SpawnTiles(Random.Range(2,5));
    }

    private void SpawnTiles(int tilesToSpawn)
    {
        if (tilesToSpawn == 0)
            return;

        int tileMatIndex = Random.Range(0, tileMats.Length);

        if (tileMatIndex == 0)
            gameObject.layer = 6;
        else if (tileMatIndex == 1)
            gameObject.layer = 7;
        else if (tileMatIndex == 2)
            gameObject.layer = 8;

        for (int i = 0; i < tilesToSpawn; i++)
        {
            GameObject tile = Instantiate(tilePrefab, spawnPoint.position, Quaternion.identity, transform);
            tile.GetComponentInChildren<TextMeshPro>().text = Random.Range(2, 4).ToString();
            tile.GetComponent<MeshRenderer>().material = tileMats[tileMatIndex];
            tiles.Add(tile.GetComponent<Tile>());
            IncreaseYSpawnPointYPos();
        }
    }

    public void ThrowTiles(TilesHolder _matchedTileHolder)
    {
        if (!gameObject.activeSelf)
            return;

        matchedTileHolder = _matchedTileHolder;
        StartCoroutine(CoroutineThrowTiles());
    }

    IEnumerator CoroutineThrowTiles()
    {
        for (int i = tiles.Count - 1; i >= 0; i--)
        {    
            print(i + "  " + gameObject.name);
            tiles[i].GotoTarget(matchedTileHolder);
            tiles[i].transform.parent = matchedTileHolder.transform;
            tiles.RemoveAt(i);

            if (tiles.Count == 0)
            {
                FindObjectOfType<TilesMover>().ThrowNextTile(matchedTileHolder);
                Destroy(gameObject);
                yield break; // Exit the coroutine
            }

            DecreaseYSpawnPointYPos();
            yield return new WaitForSeconds(0.05f);
        }
    }


    private void LateUpdate()
    {
        if (Input.GetMouseButtonUp(0) && isNewTileHolder)
        {
            isNewTileHolder = false;
            Move();
        }
    }

    public void Move()
    {
        rb.AddForce(Vector3.forward * forwardForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("TileHolder"))
        {
            rb.isKinematic = true;
            isColWithWallorTile = true;
            transform.parent = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SnapPoint") && isColWithWallorTile)
            other.GetComponent<SnapPoint>().tileHolder = this;

        if (other.gameObject.CompareTag("OutSide") && isColWithWallorTile)
            print("Retry");

        if(other.gameObject.layer == gameObject.layer && isColWithWallorTile)
        {
            if(!nearbyTile.Contains(other.gameObject))
                nearbyTile.Add(other.gameObject);
        }
    }

    public void CheckMissingNearbyTiles()
    {
        // Remove any missing objects from the list
        nearbyTile.RemoveAll(tile => tile == null);
    }

    public void IncreaseYSpawnPointYPos()
    {
        yIncVal += .25f;
        spawnPoint.localPosition = new Vector3(spawnPoint.localPosition.x, yIncVal, spawnPoint.localPosition.z);
    }

    public void DecreaseYSpawnPointYPos()
    {
        yIncVal -= .25f;
        spawnPoint.localPosition = new Vector3(spawnPoint.localPosition.x, yIncVal, spawnPoint.localPosition.z);
    }

    //void Update()
    //{
    //    // Calculate the starting point of the ray (middle of the cube)
    //    Vector3 rayOrigin = transform.position + transform.forward * (transform.localScale.z / 2f);

    //    // Set up layer mask to ignore the cube's layer
    //    int layerMask = ~(1 << gameObject.layer);

    //    // Cast a ray from the calculated origin in the forward direction of the cube
    //    RaycastHit hitInfo;
    //    if (Physics.Raycast(rayOrigin, transform.forward, out hitInfo, rayLength, layerMask))
    //    {
    //        // If the ray hits something, you can access the hit information
    //        Debug.DrawRay(rayOrigin, transform.forward * hitInfo.distance, Color.red);
    //        frontObject = true;
    //    }
    //    else
    //    {
    //        // If the ray doesn't hit anything, you can handle that here
    //        Debug.DrawRay(rayOrigin, transform.forward * rayLength, Color.green);
    //        frontObject = false;
    //    }
    //}
}
