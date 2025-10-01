using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueSample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Queue<string> queue = new Queue<string>();

        // Enqueue 데이터 넣기
        queue.Enqueue("1번쨰");
        queue.Enqueue("2번쨰");
        queue.Enqueue("3번쨰");

        Debug.Log("Queue 1");
        foreach (string item in queue)
            Debug.Log(item);
        Debug.Log("========");

        // peak : 맨 앞 데이터 확인
        Debug.Log("Peak :" + queue.Peek());    // 1번쨰

        // Dequeue : 맨 앞 꺼내기
        Debug.Log("Pop :" + queue.Dequeue());   // 1번쨰
        Debug.Log("Pop :" + queue.Dequeue());   // 2번째

        Debug.Log("남은 데이터 수" + queue.Count);    // 1
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
