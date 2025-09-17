using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayTest : MonoBehaviour
{
    private void Start()
    {
        string[] inventory = new string[5];
        inventory[0] = "Potion";
        inventory[1] = "Sword";

        Debug.Log(inventory[0]);    // Potion
        Debug.Log(inventory[1]);    // Sword
        Debug.Log(inventory[2]);    // Null
    }


}
