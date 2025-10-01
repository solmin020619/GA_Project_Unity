using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 내가 한 입력을 넣어놓고 스페이스 바 누르면 입력한 것을 반환
public class QueueMoving : MonoBehaviour
{
    public float speed = 5f;

    private Queue<Vector3> moveQueue;

    private bool isMoving = false;

    private Vector3 targetPos;
    // Start is called before the first frame update
    void Start()
    {
        moveQueue = new Queue<Vector3>();
        targetPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if (!isMoving)
        {
            if (x != 0f || y != 0f)
            {
                Vector3 move = new Vector3(x, y, 0f).normalized * speed * Time.deltaTime;
                targetPos += move;
                moveQueue.Enqueue(targetPos);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!isMoving && moveQueue.Count > 0)
                {
                    isMoving = true;
                }
            }
        }
        else
        {
            if(moveQueue.Count > 0)
            {
                transform.position = moveQueue.Dequeue();
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }
        }
    }
}
