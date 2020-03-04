using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GridInitializer : MonoBehaviour
{
    public int[,] blockTypeGrid =  {{0,1,1,0,0,0,0,0,0,0,1,0,0,0,1,1}, 
                                    {0,1,0,0,0,0,0,0,0,0,1,1,0,0,0,0}, 
                                    {0,1,1,0,0,0,0,0,0,0,1,1,1,0,0,0},
                                    {0,0,1,0,0,0,0,0,0,0,0,1,1,1,0,0},
                                    {0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0},
                                    {0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0}}; //1 for water, 0 for ground
    public GameObject[] blocks;

    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();

    }

    void CreateGrid()
    {
        //blockTypeGrid = new bool[amountOfRows,amountOfColumns];
        int amountOfRows = blockTypeGrid.GetUpperBound(0) + 1;
        int amountOfColumns = blockTypeGrid.GetUpperBound(1) + 1;

        for(int x = 0; x < amountOfRows; x++) 
        {
            for(int z = 0; z < amountOfColumns; z++) 
            {
                Vector3 spawnPoint = new Vector3(x * 1f, 0, z * 1f);

                SpawnBlock(blockTypeGrid[x,z], spawnPoint, Quaternion.identity, x, z);
            }
        }
    }

    void SpawnBlock(int blockType, Vector3 spawnPoint, Quaternion rotation, int x, int y)
    {
        GameObject clone = Instantiate(blocks[blockType], spawnPoint, rotation);
        clone.tag = "plane";
        if(blockType == 0) 
        {
            //clone.GetComponent<Renderer>().material.color = Color.green;

            
            int distance = CalculateDistance(x,y);
            if(distance < 2) clone.GetComponent<Renderer>().material.color = Color.yellow;
            else clone.GetComponent<Renderer>().material.color = new Color(1f/distance, 1, 1f/distance);
            
        }
        else
        {
            clone.GetComponent<Renderer>().material.color = Color.blue;
        }
    }

    int CalculateDistance(int x1, int y1)
    {
        int rows = blockTypeGrid.GetUpperBound(0) + 1;
        int columns = blockTypeGrid.GetUpperBound(1) + 1;

        Queue queue = new Queue();
        bool[,] visited = new bool[rows, columns];

        queue.Enqueue(x1);
        queue.Enqueue(y1);

        while(queue.Count != 0)
        {
            int x2 = Convert.ToInt32(queue.Peek()); queue.Dequeue();
            int y2 = Convert.ToInt32(queue.Peek()); queue.Dequeue();
            
            if(x2 >= 0 && y2 >= 0 && x2 < rows && y2 < columns) 
            {
                if(blockTypeGrid[x2,y2] == 1)
                {
                    int distanceX = x1 - x2;
                    int distanceY = y1 - y2;
                    int distance = Convert.ToInt32(Math.Sqrt((distanceX * distanceX) + (distanceY * distanceY)));
                    return distance;
                }
                else
                {
                    if(!visited[x2,y2])
                    {
                        visited[x2,y2] = true;
                        queue.Enqueue(x2); queue.Enqueue(y2 + 1);
                        queue.Enqueue(x2); queue.Enqueue(y2 - 1);
                        queue.Enqueue(x2 + 1); queue.Enqueue(y2);
                        queue.Enqueue(x2 - 1); queue.Enqueue(y2);
                        queue.Enqueue(x2 + 1); queue.Enqueue(y2 + 1);
                        queue.Enqueue(x2 + 1); queue.Enqueue(y2 - 1);
                        queue.Enqueue(x2 - 1); queue.Enqueue(y2 + 1);
                        queue.Enqueue(x2 - 1); queue.Enqueue(y2 - 1);
                    }
                }
            }
        }

        return -1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}