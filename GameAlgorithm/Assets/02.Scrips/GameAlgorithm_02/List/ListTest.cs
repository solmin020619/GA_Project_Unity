using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListTest : MonoBehaviour
{
    private void Start()
    {
        List<string> inventory = new List<string>();
        inventory.Add("Potion");
        inventory.Add("Sword");

        Debug.Log(inventory[0]);    // Potion
        Debug.Log(inventory[1]);    // Sword
        Debug.Log(inventory[2]);    // Null Reference Exception
    }
}
