using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    public float speed;

    private Greed greed;
    private IEnumerable<MazeGeneratorCell> path;

    private System.Random rnd = new System.Random();
    private int X;
    private int Y;

    void Start()
    {
        greed = new Greed();
    }

    void Update()
    {
        if (AirSpawn.targets.Count > 0)
            path = greed.GetPathsByDijkstra(MazeSpawner.Maze, GetStartPosition(), AirSpawn.targets).FirstOrDefault();
        var commands = path.ToArray();
        Vector2 dir;
        for (int i = 1; i < commands.Length; i++)
        {
            dir = new Vector2(commands[i].X - commands[i - 1].X, commands[i].Y - commands[i - 1].Y);
            while (transform.position.x != commands[i].X * MazeGenerator.Shift && transform.position.y != commands[i].Y * MazeGenerator.Shift)
                MoveInDirection(dir);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Air"))
        {
            Destroy(gameObject);
        }
    }

    private MazeGeneratorCell GetStartPosition()
    {
        X = rnd.Next(14);
        Y = rnd.Next(22);
        return MazeSpawner.Maze[X, Y];
    }

    private void MoveInDirection(Vector2 vector)
    {
        transform.Translate(vector * speed * Time.deltaTime, Space.World);
    }
}
