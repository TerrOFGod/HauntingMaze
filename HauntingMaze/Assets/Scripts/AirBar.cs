using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AirBar : MonoBehaviour
{
    public Slider slider;
    public float checkRadius;
    private float maxValue;
    private float currentValue;

    public Transform waterCheck;
    public LayerMask whatIsWater;

    [SerializeField] private bool playerInWater;

    // Start is called before the first frame update
    void Start()
    {
        maxValue = slider.maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        currentValue = slider.maxValue;

        if (playerInWater == true)
        {
            slider.gameObject.SetActive(true);
            slider.value -= 0.01f;
            if (currentValue - slider.value > 9)
                slider.maxValue -= 10;
            if (slider.maxValue < maxValue)
                slider.maxValue = maxValue;
        }
        if (slider.value <= 0)
        {
            Console.WriteLine("Game Over");
        }
        if (playerInWater == false)
        {
            slider.maxValue = 100;
            slider.value += 0.05f;
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
