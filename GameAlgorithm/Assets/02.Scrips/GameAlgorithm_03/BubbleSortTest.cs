using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BubbleSortTest : MonoBehaviour
{
    void Start()
    {
        int[] data2 = GenerateRandomArray(100);

        StartBubbleSort(data2);      

        foreach (var item in data2)
        {
            Debug.Log(item);
        }
    }
    int[] GenerateRandomArray(int size)
    {
        int[] arr = new int[size];
        System.Random rand = new System.Random();
        for(int i = 0; i < size; i++)
        {
            arr[i] = rand.Next(0,10000);
        }
        return arr;
    }

    public static void StartBubbleSort(int[] arr)
    {
        int n = arr.Length;
        for(int i = 0; i < n - 1; i++)
        {
            bool swapped = false;
            for(int j = 0; j < n-i -1; j++)
            {
                if(arr[j] > arr[j + 1])
                {
                    //swap
                    int temp = arr[j];
                    arr[j] = arr[j + 1];
                    arr[j + 1] = temp;
                    swapped = true;
                }
            }
            if(!swapped) break;
        }
    }
}
