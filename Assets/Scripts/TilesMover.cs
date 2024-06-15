using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesMover : MonoBehaviour
{
    public List<SnapPoint> snapPoints;
    //[HideInInspector] public List<TilesHolder> tilesHolder;
    //private bool isAnyThrowTiles;
    //private TilesHolder matchedTileHolder;
    public SnapPointsManager snapPointsManager;
    public bool fixedTileHolder, nearbyTileMatched;

    private void Start()
    {
       snapPointsManager = FindObjectOfType<SnapPointsManager>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Invoke(nameof(CheckMoveTilesInvoke), .6f);
        }
    }

    public void CheckMoveTilesInvoke()
    {
        CheckMoveTiles(0);
    }

    public void CheckMoveTiles(int checkVal)
    {
        foreach (SnapPoint snapPoint in snapPoints)
        {
            if (snapPoint.tileHolder)
            {
                if (snapPoint.tileHolder.AnyNearbyTileHolderMatching() > checkVal)
                {
                    print(snapPoint.tileHolder.gameObject.name + " CheckNearby");
                    snapPoint.tileHolder.CheckNearby(true);
                    nearbyTileMatched = true;
                    break;
                }
            }
        }

        //Debug.Log("NotMatchingAny");

        //if (nearbyTileMatched)
        //{
        //    nearbyTileMatched = false;
        //    FixTileHolders();
        //}
    }

    private void FixTileHolders()
    {
        List<SnapPoint> reverseSnapPoints = new List<SnapPoint>();
        reverseSnapPoints = snapPoints;
        reverseSnapPoints.Reverse();

        foreach (SnapPoint snapPoint in reverseSnapPoints)
        {
            if (snapPoint.tileHolder)
            {           
                SnapPoint backSnapPoint = snapPointsManager.GetSnapPoint(snapPoint.column + 1, snapPoint.row);
               
                if (backSnapPoint == null)
                    return;
               
                if (backSnapPoint.tileHolder)
                {
                    return;
                }

                for (int i = 2; i < 8; i++)
                {
                    if (backSnapPoint.tileHolder)
                    {
                        backSnapPoint = snapPointsManager.GetSnapPoint(backSnapPoint.column - 1, snapPoint.row);
                        snapPoint.tileHolder.MoveToSnapPoint(backSnapPoint);
                        fixedTileHolder = true;
                        break;
                    }

                    backSnapPoint = snapPointsManager.GetSnapPoint(backSnapPoint.column + 1, snapPoint.row);
                }
            }
        }

        if (fixedTileHolder)
        {
            fixedTileHolder = false;
            CheckMoveTiles(1);
        }
    }

    //private void RefillTilesHolders()
    //{
    //    foreach (SnapPoint snapPoint in snapPoints)
    //    {
    //        if (snapPoint.tileHolder)
    //        {
    //            tilesHolder.Add(snapPoint.tileHolder);
    //        }
    //    }
    //}

    //bool checkTileHol;

    //IEnumerator CoroutineCheckMoveTiles()
    //{
    //    foreach (TilesHolder tileHolder in tilesHolder)
    //    {
    //        checkTileHol = false;

    //        foreach (GameObject obj in tileHolder.nearbyTile)
    //        {
    //            if (obj.GetComponent<TilesHolder>().nearbyTile.Count > 1)
    //            {
    //                checkTileHol = true;
    //            }
    //        }

    //        if (tileHolder.nearbyTile.Count > 1)
    //        {
    //            checkTileHol = true;
    //        }

    //        if (checkTileHol)
    //        {
    //            foreach (GameObject obj in tileHolder.nearbyTile)
    //            {
    //                if (obj.GetComponent<TilesHolder>().nearbyTile.Count == 1)
    //                {
    //                    obj.GetComponent<TilesHolder>().ThrowTiles(tileHolder);
    //                    tilesHolder = new List<TilesHolder>();

    //                    isAnyThrowTiles = true;
    //                    break;
    //                }
    //            }

    //            foreach (GameObject obj in tileHolder.nearbyTile)
    //            {
    //                if (obj.GetComponent<TilesHolder>().nearbyTile.Count > 1)
    //                {
    //                    tileHolder.ThrowTiles(obj.GetComponent<TilesHolder>());
    //                    yield return new WaitForSeconds(.5f);

    //                    tilesHolder = new List<TilesHolder>();

    //                    isAnyThrowTiles = true;
    //                    break;
    //                }
    //            }
    //        }

    //        if (isAnyThrowTiles)
    //        {
    //            break;
    //        }
    //    }

    //    tilesHolder = new List<TilesHolder>();
    //    print("Finish");

    //    Invoke(nameof(CheckAgainInvoke), .5f);

    //    yield return null;
    //}

    //private void CheckAgainInvoke()
    //{
    //    if (isAnyThrowTiles)
    //    {
    //        isAnyThrowTiles = false;
    //        foreach (TilesHolder _tilesHolder in FindObjectsOfType<TilesHolder>())
    //        {
    //            _tilesHolder.CheckMissingNearbyTiles();
    //        }

    //        CheckMoveTiles();
    //    }
    //}

    //public void ThrowNextTile(TilesHolder _matchedTileHolder)
    //{
    //    matchedTileHolder = _matchedTileHolder;
    //    Invoke(nameof(InvokeThrowNextTile), .5f);
    //}

    //private void InvokeThrowNextTile()
    //{
    //    foreach (TilesHolder _tilesHolder in FindObjectsOfType<TilesHolder>())
    //    {
    //        if (!_tilesHolder.isNewTileHolder)
    //        {
    //            _tilesHolder.detectForwardObject = true;
    //            //if (!_tilesHolder.GetComponentInChildren<ForwardTileDetector>().isForwardTile)
    //            //{
    //            //    _tilesHolder.rb.isKinematic = false;
    //            //    _tilesHolder.Move();
    //            //}
    //        }
    //    }

    //    foreach (TilesHolder _tilesHolder in FindObjectsOfType<TilesHolder>())
    //    {
    //        _tilesHolder.CheckMissingNearbyTiles();
    //    }
    //    print(matchedTileHolder.nearbyTile.Count + " TilesCount");

    //    if (matchedTileHolder.nearbyTile.Count == 0)
    //        return;

    //    matchedTileHolder.ThrowTiles(matchedTileHolder.nearbyTile[0].GetComponent<TilesHolder>());
    //}
}
