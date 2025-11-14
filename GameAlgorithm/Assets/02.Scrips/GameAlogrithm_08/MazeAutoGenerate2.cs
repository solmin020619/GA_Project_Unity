using System.Collections.Generic;
using UnityEngine;

public class MazeAutoGenerate2 : MonoBehaviour
{
    public int width = 15;
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
        GenerateValidMaze();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            GenerateValidMaze();
        if (Input.GetKeyDown(KeyCode.R))
            ShowSolution();
    }

    // -------------------- 미로 전체 생성 --------------------
    void GenerateValidMaze()
    {
        bool ok = false;

        for (int attempt = 0; attempt < 30; attempt++)
        {
            GenerateAndShowMaze();
            visited = new bool[height, width];
            solutionPath.Clear();

            if (FindPath(start.x, start.y))
            {
                ok = true;
                Debug.Log($"<color=green> 탈출 가능한 미로 생성됨 (시도 {attempt + 1}회)</color>");
                break;
            }
        }

        if (!ok)
            Debug.Log("<color=red> 탈출 가능한 미로 생성 실패</color>");
    }

    // -------------------- 미로 생성 --------------------
    void GenerateAndShowMaze()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        maze = new int[height, width];
        visited = new bool[height, width];
        solutionPath.Clear();

        // 전체 벽으로 초기화
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                maze[y, x] = 1;

        // DFS로 기본 길 뚫기
        Carve(1, 1);

        // 랜덤하게 일부 길 막아서 다양화
        AddRandomWalls(0.15f);

        goal = new Vector2Int(width - 2, height - 2);
        DrawMaze();
    }

    // DFS 길 뚫기
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

    // 일부 길을 랜덤하게 다시 막기 (0.15 = 15%)
    void AddRandomWalls(float blockChance)
    {
        for (int y = 1; y < height - 1; y++)
        {
            for (int x = 1; x < width - 1; x++)
            {
                if (maze[y, x] == 0 && Random.value < blockChance)
                    maze[y, x] = 1;
            }
        }

        // 시작과 목표는 반드시 길
        maze[start.y, start.x] = 0;
        maze[height - 2, width - 2] = 0;
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

    // -------------------- 시각화 --------------------
    void DrawMaze()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                obj.transform.position = new Vector3(x, 0, -y);
                obj.transform.localScale = maze[y, x] == 1 ? new Vector3(1, 1, 1) : new Vector3(1, 0.1f, 1);
                obj.GetComponent<Renderer>().material.color = maze[y, x] == 1 ? Color.red : Color.white;
                obj.transform.parent = transform;

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

    // -------------------- DFS 탐색 --------------------
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
