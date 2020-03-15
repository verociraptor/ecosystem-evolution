using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

/*
 * Code taken from youtube video
 * "Health Bars in Unity3D - Quick, Clean & Easy"
 * Ref: https://www.youtube.com/watch?time_continue=7&v=CA2snUe7ARM&feature=emb_title
 */

public class Health : MonoBehaviour
{
    [SerializeField]
    private int water = 100;

    private int currentHealth;
    public event Action<float> OnHealthPctChanged = delegate { };

    private void OnEnable()
    {
        currentHealth = water;
    }

    public void ModifyHealth(int amount)
    {
        currentHealth += amount;

        float currentHealthPct = (float)currentHealth / (float)water;
        OnHealthPctChanged(currentHealthPct);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ModifyHealth(-10);
    }
}
