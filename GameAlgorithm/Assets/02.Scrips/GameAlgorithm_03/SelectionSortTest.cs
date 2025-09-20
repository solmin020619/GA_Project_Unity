using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionSortTest : MonoBehaviour
{

    void Start()
    {
        int[] data1 = GenerateRandomArray(100);

        StartSelectionSort(data1);

        foreach (var item in data1)
        {
            Debug.Log(item);
        }
    }

    int[] GenerateRandomArray(int size)
    {
        int[] arr = new int[size];

        System.Random rand = new System.Random();

        for (int i = 0; i < size; i++)
        {
            arr[i] = rand.Next(0,10000);
        }
        return arr;
    }
    
    public static void StartSelectionSort(int[] arr)
    {

        int n = arr.Length;

        for (int i = 0; i < n - 1; i++)
        {
            int mindIndex = i;

            for(int j = i + 1; j < n; j++)
            {
                if (arr[j] < arr[mindIndex])
                {
                    mindIndex = j;
                }
            }

            //swap
            int temp = arr[mindIndex];
            arr[mindIndex] = arr[i];
            arr[i] = temp;
        }
    }
}
