using System.Collections.Generic;
using UnityEngine;

public class MazeAutoGenerate : MonoBehaviour
{
    [Header("Maze Settings")]
    public int width = 15;   // 홀수로 권장
    public int height = 15;

    private int[,] maze;
    private bool[,] visited;
    private List<Vector2Int> solutionPath = new List<Vector2Int>();

    Vector2Int start = new Vector2Int(1, 1);
    Vector2Int goal;
    Vector2Int[] dirs = {
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, -1)
    };

    void Start()
    {
        GenerateAndShowMaze();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            GenerateAndShowMaze();

        if (Input.GetKeyDown(KeyCode.R))
            ShowSolution();
    }

    // -------------------- 미로 생성 --------------------
    void GenerateAndShowMaze()
    {
        // 기존 오브젝트 제거
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        maze = new int[height, width];
        visited = new bool[height, width];
        solutionPath.Clear();

        // 전체 벽으로 초기화
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                maze[y, x] = 1;

        // (1,1)부터 DFS로 길 뚫기
        Carve(1, 1);
        goal = new Vector2Int(width - 2, height - 2);

        DrawMaze();
    }

    void Carve(int x, int y)
    {
        maze[y, x] = 0;

        int[] dx = { 0, 0, -2, 2 };
        int[] dy = { -2, 2, 0, 0 };
        Shuffle(dx, dy);

        for (int i = 0; i < 4; i++)
        {
            int nx = x + dx[i];
            int ny = y + dy[i];
            if (nx > 0 && ny > 0 && nx < width - 1 && ny < height - 1 && maze[ny, nx] == 1)
            {
                maze[y + dy[i] / 2, x + dx[i] / 2] = 0;
                Carve(nx, ny);
            }
        }
    }

    void Shuffle(int[] dx, int[] dy)
    {
        for (int i = 0; i < dx.Length; i++)
        {
            int r = Random.Range(0, dx.Length);
            (dx[i], dx[r]) = (dx[r], dx[i]);
            (dy[i], dy[r]) = (dy[r], dy[i]);
        }
    }

    // -------------------- 미로 그리기 --------------------
    void DrawMaze()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject obj;

                if (maze[y, x] == 1)
                {
                    obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    obj.GetComponent<Renderer>().material.color = Color.red;
                    obj.transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    obj.GetComponent<Renderer>().material.color = Color.white;
                    obj.transform.localScale = new Vector3(1, 0.1f, 1);
                }

                obj.transform.position = new Vector3(x, 0, -y);
                obj.transform.parent = transform;

                // 시작/목표 색상 표시
                if (x == start.x && y == start.y)
                    obj.GetComponent<Renderer>().material.color = Color.green;
                else if (x == goal.x && y == goal.y)
                    obj.GetComponent<Renderer>().material.color = Color.blue;
            }
        }
    }

    // -------------------- 경로 표시 --------------------
    void ShowSolution()
    {
        solutionPath.Clear();
        visited = new bool[height, width];

        if (FindPath(start.x, start.y))
        {
            foreach (var p in solutionPath)
            {
                GameObject pathBlock = GameObject.CreatePrimitive(PrimitiveType.Cube);
                pathBlock.transform.position = new Vector3(p.x, 0.2f, -p.y);
                pathBlock.transform.localScale = new Vector3(0.5f, 0.3f, 0.5f);
                pathBlock.GetComponent<Renderer>().material.color = Color.green;
                pathBlock.transform.parent = transform;
            }
        }
    }

    bool FindPath(int x, int y)
    {
        if (x < 0 || y < 0 || x >= width || y >= height) return false;
        if (maze[y, x] == 1 || visited[y, x]) return false;

        visited[y, x] = true;

        if (x == goal.x && y == goal.y)
        {
            solutionPath.Add(new Vector2Int(x, y));
            return true;
        }

        foreach (var d in dirs)
        {
            if (FindPath(x + d.x, y + d.y))
            {
                solutionPath.Add(new Vector2Int(x, y));
                return true;
            }
        }

        return false;
    }
}
