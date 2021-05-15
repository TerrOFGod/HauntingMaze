using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirSpawn : MonoBehaviour
{
    public GameObject air;
    public int countOfAir;

    public float checkRadius;
    public LayerMask whatIsWater;

    public int selectXMin;
    public int selectXMax;
    public int selectYMin;
    public int selectYMax;

    private Vector2 whereSpawn;
    private bool isWater;

    private System.Random rnd = new System.Random();
    private float nextSpawn = 0f;
    private float spawnRate = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextSpawn)
        {
            nextSpawn = Time.time + spawnRate;
            whereSpawn = new Vector2(rnd.Next(selectXMin, selectXMax) * MazeGenerator.Shift, rnd.Next(selectYMin, selectYMax) * MazeGenerator.Shift);
            isWater = Physics2D.OverlapCircle(whereSpawn, checkRadius, whatIsWater);

            if (countOfAir > 0 && isWater)
            {
                Instantiate(air, whereSpawn, Quaternion.identity);
                air.SetActive(true);
                countOfAir--;
            }
        }
    }

    //private void FixedUpdate()
    //{
    //    isWater = Physics2D.OverlapCircle(whereSpawn, checkRadius, whatIsWater);
    //}
}
