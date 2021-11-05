using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    private int numberOfPotions;

    public List<GameObject> potionsObjects;
    public GameObject graphics;
    private void Start()
    {
        numberOfPotions = 0;
        // potionsObjects = new List<GameObject>();
        // foreach (Transform child in transform)
        // {
        //     potionsObjects.Add(child.gameObject);
        // }
        
        RefreshPotions();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            graphics.SetActive(!graphics.activeSelf);
        }
    }

    public int GetNumberOfPotions()
    {
        return numberOfPotions;
    }
    
    public void SetNumberOfPotions(int value)
    {
        if (value < 0 || value > 3)
        {
            throw new Exception("Wrong value of potions");
        }
        numberOfPotions = value;
        RefreshPotions();
    }

    private void RefreshPotions()
    {
        foreach (var potion in potionsObjects)
        {
            potion.SetActive(false);
        }
        
        for (int i = 0; i < numberOfPotions; i++)
        {
            potionsObjects[i].SetActive(true);
        }
    }

    public void PickedUpPotion()
    {
        if (numberOfPotions < 3)
        {
            numberOfPotions += 1;
        }
        
        RefreshPotions();
    }

    public void UsePotion()
    {
        if (GameMaster.Instance.playerObject.GetComponent<Health>().currentHealth >= 3) return;
        numberOfPotions -= 1;
        GameMaster.Instance.playerObject.GetComponent<PlayerMovement>().PlayerHealthUp();
        RefreshPotions();
    }
}
