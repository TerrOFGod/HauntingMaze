using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGeneratorCell
{
    public int X;
    public int Y;

    public bool WallLeft = true;
    public bool WallBottom = true;

    public bool Visited = false;
}

public class MazeGenerator
{
    public int Width = 15;
    public int Height = 23;
    public static float Shift = 2.00113f;


    public MazeGeneratorCell[,] GetGenerateMaze()
    {
        MazeGeneratorCell[,] maze = new MazeGeneratorCell[Width, Height];

        for (int x = 0; x < maze.GetLength(0); x++)
        {
            for (int y = 0; y < maze.GetLength(1); y++)
            {
                maze[x, y] = new MazeGeneratorCell { X = x, Y = y };
            }
        }

        for (int x = 0; x < maze.GetLength(0); x++)
            maze[x, Height - 1].WallLeft = false;
        for (int y = 0; y < maze.GetLength(1); y++)
            maze[Width - 1, y].WallBottom = false;

        RemoveWallsWithBacktracker(maze);

        return maze;
    }

    private void RemoveWallsWithBacktracker(MazeGeneratorCell[,] maze)
    {
        MazeGeneratorCell current = maze[0, 0];
        current.Visited = true;

        Stack<MazeGeneratorCell> stack = new Stack<MazeGeneratorCell>();
        do
        {
            List<MazeGeneratorCell> unvisited = new List<MazeGeneratorCell>();

            int x = current.X;
            int y = current.Y;

            if (x > 0 && !maze[x - 1, y].Visited) unvisited.Add(maze[x - 1, y]);
            if (y > 0 && !maze[x, y - 1].Visited) unvisited.Add(maze[x, y - 1]);
            if (x < (Width - 2) && !maze[x + 1, y].Visited) unvisited.Add(maze[x + 1, y]);
            if (y < (Height - 2) && !maze[x, y + 1].Visited) unvisited.Add(maze[x, y + 1]);

            if (unvisited.Count > 0)
            {
                MazeGeneratorCell chosen = unvisited[Random.Range(0, unvisited.Count)];
                RemoveWall(current, chosen);

                chosen.Visited = true;  
                stack.Push(chosen);
                current = chosen;
            }
            else
            {
                current = stack.Pop();
            }
        } while (stack.Count > 0);
    }

    private void RemoveWall(MazeGeneratorCell a, MazeGeneratorCell b)
    {
        if (a.X == b.X)
        {
            if (a.Y > b.Y) a.WallBottom = false;
            else b.WallBottom = false;
        }
        else
        {
            if (a.X > b.X) a.WallLeft = false;
            else b.WallLeft = false;
        }
    }
}
