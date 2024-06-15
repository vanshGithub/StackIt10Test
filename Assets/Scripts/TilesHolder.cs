using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TilesHolder : MonoBehaviour
{
    //public Rigidbody rb;
    //public float forwardForce;
    //private bool isColWithWallorTile;
    //public List<GameObject> nearbyTile;
    public Transform spawnPoint;
    private float yIncVal;
    [HideInInspector] public List<Tile> tiles;
    public GameObject tilePrefab;
    public Material[] tileMats;
    public bool isNewTileHolder = true;
    public float rayLength;
    public TilesHolder matchedTileHolder;
    //[HideInInspector] public bool frontObject;
    public int column, row;
    private SnapPointsManager snapPointsManager;
    public bool isBackTileHolder;
    public TilesMover tilesMover;

    private void Start()
    { 
        tilesMover = FindObjectOfType<TilesMover>();
        snapPointsManager = FindObjectOfType<SnapPointsManager>();
        //print(gameObject.layer);
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
        matchedTileHolder = _matchedTileHolder;
        //StartCoroutine(CoroutineThrowTiles());
        CoroutineThrowTiles();
    }

    void CoroutineThrowTiles()
    {
        int count = 0;
        for (int i = 1; i <= tiles.Count; i++)
        {
            print(i + "  " + gameObject.name);
            int index = tiles.Count - i;

            tiles[index].GotoTarget(matchedTileHolder);
            tiles[index].transform.parent = matchedTileHolder.transform;

            count++;

            if (count == tiles.Count)
            {
                gameObject.layer = 0;
                Destroy(gameObject);
                matchedTileHolder.CheckNearby(false);
                break; 
            }
        }
    }

    //IEnumerator CoroutineThrowTiles()
    //{
    //    int count = 0;
    //    for (int i = tiles.Count - 1; i >= 0; i--)
    //    {    
    //        print(i + "  " + gameObject.name);
    //        tiles[i].GotoTarget(matchedTileHolder);
    //        tiles[i].transform.parent = matchedTileHolder.transform;
    //        count++;

    //        if (count == tiles.Count - 1)
    //        {
    //            matchedTileHolder.CheckNearby(false);
    //            Destroy(gameObject);

    //            yield break; // Exit the coroutine
    //        }

    //        //DecreaseYSpawnPointYPos();
    //        yield return new WaitForSeconds(0.05f);
    //    }
    //}


    private void LateUpdate()
    {
        CastRay();

        if (Input.GetMouseButtonUp(0) && isNewTileHolder)
        {
            isNewTileHolder = false;

            if(isBackTileHolder)
                Invoke("Move", .01f);
            else
                Invoke("Move", 0);
        }
    }

    public TilesHolder tilesHolder = null;
    public SnapPoint snapPoint = null;

    public void Move()
    {      
        if (hit.collider.gameObject.CompareTag("TileHolder"))
        {
            tilesHolder = hit.collider.gameObject.GetComponent<TilesHolder>();
            snapPoint = snapPointsManager.GetSnapPoint(tilesHolder.row, tilesHolder.column - 1);
        }
        else if (hit.collider.gameObject.CompareTag("SnapPoint"))
        {
            snapPoint = hit.collider.gameObject.GetComponent<SnapPoint>();       
        }

        transform.DOMoveZ(snapPoint.transform.position.z, .5f);

        row = snapPoint.row;
        column = snapPoint.column;

        snapPoint.tileHolder = this;

        transform.parent = null;
    }

    public void MoveToSnapPoint(SnapPoint _snapPoint)
    {
        transform.DOMoveZ(_snapPoint.transform.position.z, .5f);
        row = snapPoint.row;
        column = snapPoint.column;
        snapPoint.tileHolder = this;
    }

    public int AnyNearbyTileHolderMatching()
    {
        int matchedCount = 0;
        SnapPoint rightSnapPoint = snapPointsManager.GetSnapPoint(row + 1, column);
        SnapPoint backSnapPoint = snapPointsManager.GetSnapPoint(row, column + 1);
        SnapPoint leftSnapPoint = snapPointsManager.GetSnapPoint(row - 1, column);  
        SnapPoint frontSnapPoint = snapPointsManager.GetSnapPoint(row, column - 1);  

        if (rightSnapPoint)
        {
            if (rightSnapPoint.tileHolder)
            {
                if (gameObject.layer == rightSnapPoint.tileHolder.gameObject.layer)
                {
                    matchedCount++;

                    //if(rightSnapPoint.tileHolder.AnyNearbyTileHolderMatching()>0)
                    //    matchedCount++;
                }
            }
        }

        if (backSnapPoint)
        {
            if (backSnapPoint.tileHolder)
            {
                if (gameObject.layer == backSnapPoint.tileHolder.gameObject.layer)
                {
                    matchedCount++;

                    //if (backSnapPoint.tileHolder.AnyNearbyTileHolderMatching() > 0)
                    //    matchedCount++;
                }
            }
        }

        if (leftSnapPoint)
        {
            if (leftSnapPoint.tileHolder)
            {
                if (gameObject.layer == leftSnapPoint.tileHolder.gameObject.layer)
                {
                    matchedCount++;

                    //if (leftSnapPoint.tileHolder.AnyNearbyTileHolderMatching() > 0)
                    //    matchedCount++;
                }
            }
        }

        if (frontSnapPoint)
        {
            if (frontSnapPoint.tileHolder)
            {
                if (gameObject.layer == frontSnapPoint.tileHolder.gameObject.layer)
                {
                    matchedCount++;

                }
            }
        }


        return matchedCount;
    }

    public void CheckNearby(bool checkFromStart)
    {
        SnapPoint rightSnapPoint = snapPointsManager.GetSnapPoint(row + 1, column);
        SnapPoint backSnapPoint = snapPointsManager.GetSnapPoint(row, column + 1);
        SnapPoint leftSnapPoint = snapPointsManager.GetSnapPoint(row - 1, column);

        if (rightSnapPoint && rightSnapPoint.tileHolder && gameObject.layer == rightSnapPoint.tileHolder.gameObject.layer)
        {
            if (rightSnapPoint.tileHolder.AnyNearbyTileHolderMatching() > 0)
            {
                if (gameObject.layer == backSnapPoint.tileHolder.gameObject.layer && backSnapPoint.tileHolder.AnyNearbyTileHolderMatching() > 0)
                {
                    if (checkFromStart)
                    {
                        if (rightSnapPoint.tileHolder.AnyNearbyTileHolderMatching() < 2)
                        {
                            print("Right Return");
                            return;
                        }
                    }
                    //else
                    //{
                    ThrowTiles(rightSnapPoint.tileHolder);
                    print("Right Throw");

                    return;
                    //}
                }

                //if (backSnapPoint && backSnapPoint.tileHolder && gameObject.layer == backSnapPoint.tileHolder.gameObject.layer)
                //{
                //    rightSnapPoint.tileHolder.CheckNearby();
                //}
                //else
                //rightSnapPoint.tileHolder.CheckNearby(false);
                ThrowTiles(rightSnapPoint.tileHolder);
                print("Right Throw End");

                return;
            }
        }
        else if (backSnapPoint && backSnapPoint.tileHolder && gameObject.layer == backSnapPoint.tileHolder.gameObject.layer)
        {
            if (checkFromStart)
            {
                if (backSnapPoint.tileHolder.AnyNearbyTileHolderMatching() < 2)
                {
                    print("Back Return");

                    return;
                }
            }
            //else
            //{
                ThrowTiles(backSnapPoint.tileHolder);
                print("Back Throw");

                return;
            //}
        }
        else if (leftSnapPoint && leftSnapPoint.tileHolder && gameObject.layer == leftSnapPoint.tileHolder.gameObject.layer)
        {
            if (checkFromStart)
            {
                if (leftSnapPoint.tileHolder.AnyNearbyTileHolderMatching() < 2)
                {
                    print("Left Return");

                    return;
                }
            }

            ThrowTiles(leftSnapPoint.tileHolder);
            print("Left Throw");

            return;
        }

        tilesMover.CheckMoveTiles(0);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("TileHolder"))
    //    {
    //        detectForwardObject = true;
    //        isColWithWallorTile = true;
    //        rb.isKinematic = true;
    //        transform.parent = null;
    //        Invoke(nameof(StartDetectNearByInvoke), 0);
    //    }
    //}

    //private void StartDetectNearByInvoke()
    //{
    //    canDetectNearbyTiles = true;
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("SnapPoint") && isColWithWallorTile)
    //        other.GetComponent<SnapPoint>().tileHolder = this;

    //    if (other.gameObject.CompareTag("OutSide") && isColWithWallorTile)
    //        print("Retry");     
    //}

    //private void OnTriggerStay(Collider other)
    //{
    //    if (!canDetectNearbyTiles)
    //        return;

    //    if (other.gameObject.layer == gameObject.layer)
    //    {
    //        if (!nearbyTile.Contains(other.gameObject))
    //            nearbyTile.Add(other.gameObject);
    //    }
    //}

    //public void CheckMissingNearbyTiles()
    //{
    //    // Remove any missing objects from the list
    //    nearbyTile.RemoveAll(tile => tile == null);
    //}

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

    //public bool detectForwardObject;
    //private bool canDetectNearbyTiles;

    //public float detectionDistance;
    //public float boxWidth; // Width of the box cast
    //public float boxHeight; // Height of the box cast
    //public bool isHit;
    //public string testName;
    private RaycastHit hit;
    public GameObject tempHit;

    void CastRay()
    {
        // Define the origin and direction of the ray
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;

        // Perform the raycast
        if (Physics.Raycast(origin, direction, out RaycastHit _hit, rayLength))
        {
            hit = _hit;
            tempHit = hit.collider.gameObject;
        }

        // Draw the ray in the Scene view
        Debug.DrawRay(origin, direction * rayLength, Color.red);
    }

    //// Visualize the box cast gizmo in the Scene view
    //void OnDrawGizmosSelected()
    //{
    //    Vector3 boxSize = new Vector3(boxWidth, boxHeight, detectionDistance);
    //    Gizmos.color = Color.yellow;
    //    Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
    //    Gizmos.DrawWireCube(Vector3.forward * (detectionDistance / 2), boxSize);
    //}
}
