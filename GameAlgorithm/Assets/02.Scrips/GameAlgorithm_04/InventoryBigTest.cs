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
        // 10����
        for (int i = 0; i < 100000; i++)
        {
            string name = $"Item_{i:D5}";   // Item_00001 ����
            int qty = rand.Next(1, 100);    // 1~99 ���� ����
            items.Add(new Item(name, qty));
        }

        // ���� Ž�� �׽�Ʈ
        string target = "Item_45672";
        Stopwatch sw = Stopwatch.StartNew();
        Item foundLinear = FindItemLinear(target);
        sw.Stop();
        UnityEngine.Debug.Log($"[���� Ž��] {target} ���� : {foundLinear?.quantity}, �ð� {sw.ElapsedMilliseconds} ms");

        // ���� Ž���� ���� ����
        items.Sort((a, b) => a.itemName.CompareTo(b.itemName));

        // ���� Ž�� �׽�Ʈ
        sw.Restart();
        Item foundBinary = FindItemBinary(target);
        sw.Stop();
        UnityEngine.Debug.Log($"[���� Ž��] {target} ���� : {foundBinary?.quantity}, �ð� {sw.ElapsedMilliseconds} ms");
    }

    // ���� Ž��
    public Item FindItemLinear(string targetName)
    {
        foreach (var item in items)
        {
            if (item.itemName == targetName) return item;     // �߰� �� ��ȯ
        }
        return null;    // ��ã���� null
    }

    // ���� Ž��
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
                return items[mid];  // ã��
            }
            else if ((compare < 0))
            {
                left = mid + 1;     // ������ Ž��
            }
            else
            {
                right = mid - 1;    // ���� Ž��
            }
        }
        return null;
    }
}
