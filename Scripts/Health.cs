using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int currentHealth;
    public int startingHealth;

    public void SetHealth(int value)
    {
        currentHealth = value;
        GetComponent<PlayerMovement>().UpdateHpText();
    }

    private void Start()
    {
        currentHealth = startingHealth;
    }
    
}
