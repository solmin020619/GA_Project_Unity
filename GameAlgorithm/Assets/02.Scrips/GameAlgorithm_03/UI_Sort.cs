using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;  // UI ��ư, �ؽ�Ʈ ���ῡ �ʿ�

public class UI_Sort : MonoBehaviour
{
    public Text resultText; // ��� ǥ���� UI Text (Canvas �ȿ� Text ����� �巡��)

    // [��ư �̺�Ʈ] ���� ����
    public void OnClickSelectionSort()
    {
        int[] data = GenerateRandomArray(10000);
        Stopwatch sw = new Stopwatch();

        sw.Start();
        SelectionSort(data);
        sw.Stop();

        long time = sw.ElapsedMilliseconds;
        resultText.text = $"���� ����: {time} ms";
        UnityEngine.Debug.Log($"���� ����: {time} ms");
    }

    // [��ư �̺�Ʈ] ���� ����
    public void OnClickBubbleSort()
    {
        int[] data = GenerateRandomArray(10000);
        Stopwatch sw = new Stopwatch();

        sw.Start();
        BubbleSort(data);
        sw.Stop();

        long time = sw.ElapsedMilliseconds;
        resultText.text = $"���� ����: {time} ms";
        UnityEngine.Debug.Log($"���� ����: {time} ms");
    }

    // [��ư �̺�Ʈ] �� ����
    public void OnClickQuickSort()
    {
        int[] data = GenerateRandomArray(10000);
        Stopwatch sw = new Stopwatch();

        sw.Start();
        QuickSort(data, 0, data.Length - 1);
        sw.Stop();

        long time = sw.ElapsedMilliseconds;
        resultText.text = $"�� ����: {time} ms";
        UnityEngine.Debug.Log($"�� ����: {time} ms");
    }

    // ���� �迭 ����
    int[] GenerateRandomArray(int size)
    {
        int[] arr = new int[size];
        System.Random rand = new System.Random();

        for (int i = 0; i < size; i++)
        {
            arr[i] = rand.Next(0, 10000);
        }
        return arr;
    }

    // ���� ����
    public static void SelectionSort(int[] arr)
    {
        int n = arr.Length;

        for (int i = 0; i < n - 1; i++)
        {
            int minIndex = i;

            for (int j = i + 1; j < n; j++)
            {
                if (arr[j] < arr[minIndex])
                {
                    minIndex = j;
                }
            }

            // swap
            int temp = arr[minIndex];
            arr[minIndex] = arr[i];
            arr[i] = temp;
        }
    }

    // ���� ����
    public static void BubbleSort(int[] arr)
    {
        int n = arr.Length;
        for (int i = 0; i < n - 1; i++)
        {
            bool swapped = false;
            for (int j = 0; j < n - i - 1; j++)
            {
                if (arr[j] > arr[j + 1])
                {
                    // swap
                    int temp = arr[j];
                    arr[j] = arr[j + 1];
                    arr[j + 1] = temp;
                    swapped = true;
                }
            }
            if (!swapped) break;
        }
    }

    // �� ����
    public static void QuickSort(int[] arr, int low, int high)
    {
        if (low < high)
        {
            int pivotIndex = Partition(arr, low, high);

            QuickSort(arr, low, pivotIndex - 1);   // �ǹ� ���� ����
            QuickSort(arr, pivotIndex + 1, high);  // �ǹ� ������ ����
        }
    }

    private static int Partition(int[] arr, int low, int high)
    {
        int pivot = arr[high];
        int i = (low - 1);

        for (int j = low; j < high; j++)
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

        // pivot �ڸ� ��ȯ
        int temp2 = arr[i + 1];
        arr[i + 1] = arr[high];
        arr[high] = temp2;

        return i + 1;
    }
}
