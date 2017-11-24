using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    public Transform tilePrefab;
    public Vector3 mapSize;
 
    public void GenerateMap()
    {
        for (int i = 0; i < mapSize.x; i++)
        {
            for (int j = 0; j < mapSize.y; j++)
            {
                Vector3 tilePosition = new Vector3(-mapSize.x / 2 + 0.5f + i, -mapSize.y + 0.5f + j ,0);
                Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right), transform);
            }
        }
    }
}
