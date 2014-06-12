﻿using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

    public Transform healthBar;

    private float health = 0, maxHealth = 1;
    private float currentHealth = 0;

    public float change = 5.0f;

    public void Reset()
    {
        currentHealth = 0;
        maxHealth = 1;
        health = 0;
    }

    public void UpdateBar(float phealth, float pmaxHealth)
    {
        health = Mathf.Clamp(phealth, 0, pmaxHealth);
        maxHealth = pmaxHealth;
        if (currentHealth == 0)
        {
            currentHealth = maxHealth;
        }
    }

    public void UpdateInstant()
    {
        currentHealth = health;
    }

    void Update()
    {
        currentHealth = Mathf.Lerp(currentHealth, health, Time.deltaTime * change);
        healthBar.localScale = new Vector3(currentHealth / maxHealth, healthBar.localScale.y, healthBar.localScale.z);
    }
}