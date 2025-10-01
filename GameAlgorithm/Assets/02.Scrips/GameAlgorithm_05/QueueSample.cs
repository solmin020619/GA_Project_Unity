using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueSample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Queue<string> queue = new Queue<string>();

        // Enqueue ������ �ֱ�
        queue.Enqueue("1����");
        queue.Enqueue("2����");
        queue.Enqueue("3����");

        Debug.Log("Queue 1");
        foreach (string item in queue)
            Debug.Log(item);
        Debug.Log("========");

        // peak : �� �� ������ Ȯ��
        Debug.Log("Peak :" + queue.Peek());    // 1����

        // Dequeue : �� �� ������
        Debug.Log("Pop :" + queue.Dequeue());   // 1����
        Debug.Log("Pop :" + queue.Dequeue());   // 2��°

        Debug.Log("���� ������ ��" + queue.Count);    // 1
        Debug.Log("Queue 2");
        foreach (string item in queue)
            Debug.Log(item);
        Debug.Log("=========================");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
