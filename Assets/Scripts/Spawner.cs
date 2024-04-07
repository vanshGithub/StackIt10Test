using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject tileHolderPrefab;
    [HideInInspector] public int randNumTiles;

    void Awake()
    {
        SpawnTilesHolder();
    }

    public void SpawnTilesHolder()
    {
        randNumTiles = Random.Range(1, 5);
        for (int i = 0; i < randNumTiles; i++)
        {
            GameObject tileHolderToSpawn = tileHolderPrefab;
            GameObject tileHolder = Instantiate(tileHolderToSpawn, spawnPoints[i].position, Quaternion.identity, spawnPoints[i]);
            tileHolder.name += "  " + Random.Range(0, 50); 
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }
}
