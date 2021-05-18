using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    public GameObject ghost;

    private System.Random rnd = new System.Random();
    private float nextSpawn = 0f;
    public float spawnRate = 0.5f;

    public int selectXMax;
    public int selectYMax;

    private int X;
    private int Y;
    public static Vector2 whereSpawn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (AirSpawn.targets.Count > 0)
            if (Time.time > nextSpawn)
            {
                nextSpawn = Time.time + spawnRate;
                X = rnd.Next(selectXMax);
                Y = rnd.Next(selectYMax);
                if (!AirSpawn.targets.Contains(MazeSpawner.Maze[X, Y]))
                {
                    whereSpawn = new Vector2(X * MazeGenerator.Shift, Y * MazeGenerator.Shift);
                    Instantiate(ghost, whereSpawn, Quaternion.identity);
                }
            }
    }
}
