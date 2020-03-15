using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Source: https://github.com/SebLague/Pathfinding

public class Grid : MonoBehaviour
{
    public Transform animal;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;

    public List<Node> path;
    float nodeDiameter;
    int gridSizeX, gridSizeY;

    public GridInitializer gridInit;

    void Start()
    {
        gridInit = GameObject.FindObjectOfType(typeof(GridInitializer)) as GridInitializer;
        gridInit.CreateGridEco();
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }
    
    //reduce node radius to reduce the sphere that checks for walkable objects
    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius - .01f, unwalkableMask));
                Debug.Log("is this walkable" + walkable + "(" + worldPoint.x + "," + worldPoint.z + ")");
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    //Subtract 2.5f on x-axis and 7.5f on z-axis to account for offset
    //where grid is located on top of ecosystem plane
    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = ((worldPosition.x-2.5f) + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = ((worldPosition.z-7.5f) + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

   
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid != null)
        {
            Node animalNode = NodeFromWorldPoint(animal.position);
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                if(animalNode == n)
                {
                    Gizmos.color = Color.cyan;
                }
                if (path != null)
                {
                    Debug.Log("path entered");
                    if (path.Contains(n))
                        Gizmos.color = Color.black;
                }
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
}