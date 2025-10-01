using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryBigTest : MonoBehaviour
{
    List<Item> items = new List<Item>();
    private System.Random rand = new System.Random();

    private void Start()
    {
        // 10만개
        for (int i = 0; i < 100000; i++)
        {
            string name = $"Item_{i:D5}";   // Item_00001 형식
            int qty = rand.Next(1, 100);    // 1~99 랜덤 개수
            items.Add(new Item(name, qty));
        }

        // 선형 탐색 테스트
        string target = "Item_45672";
        Stopwatch sw = Stopwatch.StartNew();
        Item foundLinear = FindItemLinear(target);
        sw.Stop();
        UnityEngine.Debug.Log($"[선형 탐색] {target} 개수 : {foundLinear?.quantity}, 시간 {sw.ElapsedMilliseconds} ms");

        // 이진 탐색을 위한 정렬
        items.Sort((a, b) => a.itemName.CompareTo(b.itemName));

        // 이진 탐색 테스트
        sw.Restart();
        Item foundBinary = FindItemBinary(target);
        sw.Stop();
        UnityEngine.Debug.Log($"[이진 탐색] {target} 개수 : {foundBinary?.quantity}, 시간 {sw.ElapsedMilliseconds} ms");
    }

    // 선형 탐색
    public Item FindItemLinear(string targetName)
    {
        foreach (var item in items)
        {
            if (item.itemName == targetName) return item;     // 발견 시 반환
        }
        return null;    // 못찾으면 null
    }

    // 이진 탐색
    public Item FindItemBinary(string targetName)
    {
        int left = 0;
        int right = items.Count - 1;

        while (left <= right)
        {
            int mid = (left + right) / 2;
            int compare = items[mid].itemName.CompareTo(targetName);

            if (compare == 0)
            {
                return items[mid];  // 찾음
            }
            else if ((compare < 0))
            {
                left = mid + 1;     // 오른쪽 탐색
            }
            else
            {
                right = mid - 1;    // 왼쪽 탐색
            }
        }
        return null;
    }
}
