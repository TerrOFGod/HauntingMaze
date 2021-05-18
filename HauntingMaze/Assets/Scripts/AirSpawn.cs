using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirSpawn : MonoBehaviour
{
    public GameObject air;
    public int countOfAir;

    public float checkRadius;
    public LayerMask whatIsWater;

    public int selectXMax;
    public int selectYMax;

    public static List<MazeGeneratorCell> targets;

    private Vector2 whereSpawn;
    private bool isWater;

    private System.Random rnd = new System.Random();
    private float nextSpawn = 0f;
    private float spawnRate = 0.5f;

    private int X;
    private int Y;

    // Start is called before the first frame update
    void Start()
    {
        targets = new List<MazeGeneratorCell>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextSpawn)
        {
            nextSpawn = Time.time + spawnRate;
            X = rnd.Next(selectXMax);
            Y = rnd.Next(selectYMax);
            whereSpawn = new Vector2(X * MazeGenerator.Shift, Y * MazeGenerator.Shift);
            isWater = Physics2D.OverlapCircle(whereSpawn, checkRadius, whatIsWater);

            if (countOfAir > 0 && isWater)
            {
                Instantiate(air, whereSpawn, Quaternion.identity);
                air.SetActive(true);
                if (!targets.Contains(MazeSpawner.Maze[X, Y]))
                    targets.Add(MazeSpawner.Maze[X, Y]);
                countOfAir--;
            }
        }
    }
}
