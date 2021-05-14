using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AirBar : MonoBehaviour
{
    public Slider slider;
    public float checkRadius;

    public Transform waterCheck;
    public LayerMask whatIsWater;

    [SerializeField] private bool playerInWater;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInWater == true)
        {
            slider.gameObject.SetActive(true);
            slider.value -= 0.01f;
        }
        if (slider.value <= 0)
        {
            Console.WriteLine("Game Over");
        }
        if (playerInWater == false)
        {
            slider.value += 0.5f;
            if (slider.value == 100)
            {
                slider.gameObject.SetActive(false);
            }
        }
    }



    private void FixedUpdate()
    {
        playerInWater = Physics2D.OverlapCircle(waterCheck.position, checkRadius, whatIsWater);
    }
}
