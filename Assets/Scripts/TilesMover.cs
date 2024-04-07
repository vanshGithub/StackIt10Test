using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesMover : MonoBehaviour
{
    public SnapPoint[] snapPoints;
    [HideInInspector] public List<TilesHolder> tilesHolder;
    private bool isAnyThrowTiles;
    private TilesHolder matchedTileHolder;

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Invoke(nameof(CheckMoveTiles), .5f);
        }
    }

    public void CheckMoveTiles()
    {
        RefillTilesHolders();
        StartCoroutine(CoroutineCheckMoveTiles());
    }

    private void RefillTilesHolders()
    {
        foreach (SnapPoint snapPoint in snapPoints)
        {
            if (snapPoint.tileHolder)
            {
                tilesHolder.Add(snapPoint.tileHolder);
            }
        }
    }

    bool checkTileHol;

    IEnumerator CoroutineCheckMoveTiles()
    {
        foreach (TilesHolder tileHolder in tilesHolder)
        {
            checkTileHol = false;

            foreach (GameObject obj in tileHolder.nearbyTile)
            {
                if (obj.GetComponent<TilesHolder>().nearbyTile.Count > 1)
                {
                    checkTileHol = true;
                }
            }

            if (tileHolder.nearbyTile.Count > 1)
            {
                checkTileHol = true;
            }

            if (checkTileHol)
            {
                foreach (GameObject obj in tileHolder.nearbyTile)
                {
                    if (obj.GetComponent<TilesHolder>().nearbyTile.Count == 1)
                    {
                        obj.GetComponent<TilesHolder>().ThrowTiles(tileHolder);
                        tilesHolder = new List<TilesHolder>();

                        isAnyThrowTiles = true;
                        break;
                    }
                }

                foreach (GameObject obj in tileHolder.nearbyTile)
                {
                    if (obj.GetComponent<TilesHolder>().nearbyTile.Count > 1)
                    {
                        tileHolder.ThrowTiles(obj.GetComponent<TilesHolder>());
                        yield return new WaitForSeconds(.5f);

                        tilesHolder = new List<TilesHolder>();

                        isAnyThrowTiles = true;
                        break;
                    }
                }
            }

            if (isAnyThrowTiles)
            {
                break;
            }
        }

        tilesHolder = new List<TilesHolder>();
        print("Finish");

        Invoke(nameof(CheckAgainInvoke), .5f);

        yield return null;
    }

    private void CheckAgainInvoke()
    {
        if (isAnyThrowTiles)
        {
            isAnyThrowTiles = false;
            foreach (TilesHolder _tilesHolder in FindObjectsOfType<TilesHolder>())
            {
                _tilesHolder.CheckMissingNearbyTiles();
            }

            CheckMoveTiles();
        }
    }

    public void ThrowNextTile(TilesHolder _matchedTileHolder)
    {
        matchedTileHolder = _matchedTileHolder;
        Invoke(nameof(InvokeThrowNextTile), .5f);
    }

    private void InvokeThrowNextTile()
    {
        foreach (TilesHolder _tilesHolder in FindObjectsOfType<TilesHolder>())
        {
            if (!_tilesHolder.isNewTileHolder)
            {
                if (!_tilesHolder.GetComponentInChildren<ForwardTileDetector>().isForwardTile)
                {
                    _tilesHolder.rb.isKinematic = false;
                    _tilesHolder.Move();
                }
            }
        }

        foreach (TilesHolder _tilesHolder in FindObjectsOfType<TilesHolder>())
        {
            _tilesHolder.CheckMissingNearbyTiles();
        }
        print(matchedTileHolder.nearbyTile.Count + " TilesCount");

        if (matchedTileHolder.nearbyTile.Count == 0)
            return;

        matchedTileHolder.ThrowTiles(matchedTileHolder.nearbyTile[0].GetComponent<TilesHolder>());
    }
}
