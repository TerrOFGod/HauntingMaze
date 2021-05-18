using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    public float speed;

    private bool facingRight = true;

    private Greed greed;
    [SerializeField] private MazeGeneratorCell[] path;

    private float maxDis = .1f;
    private IEnumerator<Vector2> enumerator;
    private Vector2[] vectors;

    private int X;
    private int Y;
    private int counter = 0;

    void Start()
    {
        greed = new Greed();
        path = greed.GetPathsByDijkstra(GetStartPosition(), AirSpawn.targets).FirstOrDefault().ToArray();
        vectors = new Vector2[path.Length];
        vectors = path.Select(cell => new Vector2(cell.X * MazeGenerator.Shift, cell.Y * MazeGenerator.Shift)).ToArray();
        enumerator = GetEnumerator();
        enumerator.MoveNext();
    }

    void Update()
    {
        if (enumerator == null || enumerator.Current == null)
            return;

        Vector2 pos = transform.position;

        transform.position = Vector2.MoveTowards(pos, enumerator.Current, Time.deltaTime * speed);

        var dis = (pos - enumerator.Current).sqrMagnitude;

        if (dis < maxDis * maxDis)
            enumerator.MoveNext();
    }

    private void FixedUpdate()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Air") || collision.tag.Equals("Ghost"))
        {
            Destroy(gameObject);
        }
        if (collision.tag.Equals("Ghost"))
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }

    private MazeGeneratorCell GetStartPosition()
    {
        X = (int)(GhostSpawner.whereSpawn.x / MazeGenerator.Shift);
        Y = (int)(GhostSpawner.whereSpawn.y / MazeGenerator.Shift);
        return MazeSpawner.Maze[X, Y];
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    public IEnumerator moveObject(MazeGeneratorCell current, MazeGeneratorCell next)
    {
        float totalMovementTime = 2f; //the amount of time you want the movement to take
        float currentMovementTime = 0f;//The amount of time that has passed
        var Destination = new Vector2(next.X * MazeGenerator.Shift, next.Y * MazeGenerator.Shift);
        var Origin = new Vector2(current.X * MazeGenerator.Shift, current.Y * MazeGenerator.Shift);
        while (Vector2.Distance(transform.localPosition, Destination) > 0)
        {
            currentMovementTime += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(Origin, Destination, currentMovementTime / totalMovementTime);
            yield return null;
        }
    }

    public IEnumerator<Vector2> GetEnumerator()
    {
        while (true)
        {
            yield return vectors[counter];
            counter++;
        }
    }
}
