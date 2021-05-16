using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class DijkstraData
{
    public MazeGeneratorCell Previous { get; set; }
    public int Price { get; set; }
}

public class Greed
{
    private bool InsideMaze(MazeGeneratorCell cell)
    {
        return cell.X > -1 && cell.X < 15 && cell.Y > -1 && cell.Y < 23;
    }

    private IEnumerable<MazeGeneratorCell> GetNextPoints(MazeGeneratorCell current, MazeGeneratorCell[,] maze)
    {
        for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;
                if (x * y == 0)
                {
                    var nextPoint = maze[current.X + x, current.Y + y];
                    if (InsideMaze(nextPoint) && (!nextPoint.WallBottom || !nextPoint.WallLeft))
                        yield return nextPoint;
                }
            }
    }

    public IEnumerable<List<MazeGeneratorCell>> GetPathsByDijkstra(MazeGeneratorCell[,] maze, MazeGeneratorCell start,
    IEnumerable<MazeGeneratorCell> targets)
    {
        var visited = new HashSet<MazeGeneratorCell>();
        var canBeVisited = new HashSet<MazeGeneratorCell>();
        var track = new Dictionary<MazeGeneratorCell, DijkstraData>();
        canBeVisited.Add(start);
        track.Add(start, new DijkstraData { Price = 0, Previous = new MazeGeneratorCell(-1, -1) });

        while (true)
        {
            var toOpen = GetOpeningPoint(canBeVisited, track);
            if (toOpen == new MazeGeneratorCell(-1, -1)) yield break;
            if (targets.Contains(toOpen))
                yield return GetPath(toOpen, track);
            foreach (var point in GetNextPoints(toOpen, maze))
            {
                int currentPrice = track[toOpen].Price + 1;
                if (!track.ContainsKey(point) || track[point].Price > currentPrice)
                    track.Add(point, new DijkstraData { Previous = toOpen, Price = currentPrice });
                if (!visited.Contains(point))
                    canBeVisited.Add(point);
            }
            canBeVisited.Remove(toOpen);
            visited.Add(toOpen);
        }
    }

    private MazeGeneratorCell GetOpeningPoint(HashSet<MazeGeneratorCell> canBeVisited, Dictionary<MazeGeneratorCell, DijkstraData> track)
    {
        var toOpen = new MazeGeneratorCell(-1, -1);
        var bestPrice = double.PositiveInfinity;
        foreach (var point in canBeVisited)
        {
            if (track.ContainsKey(point) && track[point].Price < bestPrice)
            {
                bestPrice = track[point].Price;
                toOpen = point;
            }
        }
        return toOpen;
    }

    private List<MazeGeneratorCell> GetPath(MazeGeneratorCell currentPoint, Dictionary<MazeGeneratorCell, DijkstraData> track)
    {
        var result = new List<MazeGeneratorCell>();
        int cost = track[currentPoint].Price;
        while (currentPoint != new MazeGeneratorCell(-1, -1))
        {
            result.Add(currentPoint);
            currentPoint = track[currentPoint].Previous;
        }
        result.Reverse();
        return result;
    }
}
