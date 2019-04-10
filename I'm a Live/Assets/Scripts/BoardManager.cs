using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count (int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 8;
    public int rows = 8;
    public Count wallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);

    public GameObject exit; // unityengine내에 존재하는 객체이고 , 여러개 경우 배열로 정의해주면좋다.
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    // init map
    void InitializeList()
    {
        gridPositions.Clear();

        for(int x = 1; x < columns -1; x++)
        {
            for(int y = 1; y < rows -1; y++)
            {
                // Vector3 3D env -> x , y , z 
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    void BoardSetup()
    {
        // make Obj : Board 
        boardHolder = new GameObject("Board").transform;

        // make tiles
        for( int x = -1 ; x < columns + 1 ; x++)
        {
            for( int y = -1 ; y < rows ; y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if ( x == -1 || x == columns || y == -1 || y == rows )
                {
                    toInstantiate = outerWallTiles[Random.Range(0, floorTiles.Length)];
                }
                // Quaternion.identity : cannot rotate , cannot 3d 
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                // parent Hold
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    Vector3 RandomPostion()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        // 사용한 객체는 리스트에서 제거 : At 이 붙어서 index 로 적용
        gridPositions.RemoveAt(randomIndex);

        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maxmum)
    {
        int objectCount = Random.Range (minimum, maxmum + 1);
        
        for( int i = 0 ; i < objectCount ; i++)
        {
            Vector3 randomPosition = RandomPostion();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    // called GameManager
    public void SetupScene(int level)
    {
        BoardSetup();
        InitializeList();
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
        // log(), log of 6 in base 2
        // Mathf.Log(6,2);
        // 4레벨이면 2마리, 8레벨이면 3마리 ....
        int enemyCount = (int)Mathf.Log(level, 2F);
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        // Exit position
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0F), Quaternion.identity);
    }
}
