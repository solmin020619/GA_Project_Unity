using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class QuickSortTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int[] data3 = GenerateRandomArray(100);

        StartQuickSort(data3,0,data3.Length - 1);

        foreach (var item in data3)
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
            arr[i] = rand.Next(0, 10000);
        }
        return arr;
    }

    public static void StartQuickSort(int[] arr, int low, int high)
    {
        if(low < high)
        {
            int pivotIndex = Partition(arr, low, high);

            StartQuickSort(arr,low, pivotIndex - 1); // 피벗 왼쪽 정렬
            StartQuickSort(arr,pivotIndex + 1 , high);  // 피벗 오른쪽 정렬
        }
    }

    private static int Partition(int[] arr, int low, int high)
    {
        int pivot = arr[high];
        int i = (low - 1);

        for(int j = low;  j < high; j++)
        {
            if (arr[j] <= pivot)
            {
                i++;

                // swap
                int temp = arr[i];
                arr[i] = arr[j];
                arr[j] = temp;
            }
        }

        // pivot 자리 교환
        int temp2 = arr[i + 1];
        arr[i + 1] = arr[high];
        arr[high] = temp2;

        return i + 1;
    }
}
