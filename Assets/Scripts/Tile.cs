using DG.Tweening;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public void GotoTarget(TilesHolder tilesHolder)
    {
        if (tilesHolder.spawnPoint == null)
            return;
        // Move To Other Tile Stack
        transform.DOLocalJump(tilesHolder.spawnPoint.localPosition, 3, 1, .2f);

        tilesHolder.IncreaseYSpawnPointYPos();
        tilesHolder.tiles.Add(this);
    }
}