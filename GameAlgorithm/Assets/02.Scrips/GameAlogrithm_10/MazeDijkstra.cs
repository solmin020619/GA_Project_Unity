using System.Collections.Generic;
using UnityEngine;
public class MazeDijkstra : MonoBehaviour
{
    [Header("Map Settings")]
    public int width = 31;     // x 크기 (홀수 추천)
    public int height = 31;    // y 크기 (홀수 추천)
    public float tileSize = 1f;

    // 0=벽, 1=평지, 2=숲, 3=진흙
    int[,] map;
    GameObject[,] tileObjs;

    Vector2Int start = new Vector2Int(1, 1);
    Vector2Int goal;

    [Header("Tile Colors")]
    public Color wallColor = new Color(0.25f, 0.1f, 0.05f);
    public Color groundColor = Color.white;
    public Color forestColor = new Color(0.6f, 0.9f, 0.6f);
    public Color mudColor = new Color(1f, 0.7f, 0.7f);
    public Color pathColor = Color.yellow;      // 최단 경로 색

    List<Vector2Int> lastPath; // 마지막으로 계산한 최단 경로

    void Start()
    {
        GenerateNewMaze();
    }

    void Update()
    {
        // Space : 미로 재생성
        if (Input.GetKeyDown(KeyCode.Space))
            GenerateNewMaze();

        // R : 최단거리 경로 표시
        if (Input.GetKeyDown(KeyCode.R))
            ShowShortestPath();
    }

    // 1) 새 미로 생성 (탈출 가능할 때까지 반복)
    public void GenerateNewMaze()
    {
        // 기존 타일 삭제
        ClearOldTiles();

        goal = new Vector2Int(width - 2, height - 2);
        map = new int[width, height];

        // 탈출 가능할 때까지 랜덤 생성 + DFS 검사
        for (int tries = 0; tries < 1000; tries++)
        {
            RandomFillMap();

            if (HasPathDFS(start, goal))
                break;
        }

        BuildVisualTiles();

        // Dijkstra로 최단거리 미리 계산
        lastPath = Dijkstra(map, start, goal);
    }

    void ClearOldTiles()
    {
        if (tileObjs == null) return;

        for (int x = 0; x < tileObjs.GetLength(0); x++)
        {
            for (int y = 0; y < tileObjs.GetLength(1); y++)
            {
                if (tileObjs[x, y] != null)
                    Destroy(tileObjs[x, y]);
            }
        }

        tileObjs = null;
    }

    // ---------------------------
    // 2) 맵 랜덤 생성 (벽/평지/숲/진흙)
    // ---------------------------
    void RandomFillMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // 가장 외곽은 무조건 벽
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                {
                    map[x, y] = 0;
                }
                else
                {
                    int r = Random.Range(0, 100);

                    // 적당히 비율 조정 (원하면 바꿔도 됨)
                    if (r < 25) map[x, y] = 0; // 25% 벽
                    else if (r < 60) map[x, y] = 1; // 35% 평지
                    else if (r < 85) map[x, y] = 2; // 25% 숲
                    else map[x, y] = 3; // 15% 진흙
                }
            }
        }

        // 시작, 도착은 반드시 평지
        map[start.x, start.y] = 1;
        map[goal.x, goal.y] = 1;
    }

    // ---------------------------
    // 3) DFS로 탈출 가능 여부 체크
    // ---------------------------
    bool HasPathDFS(Vector2Int s, Vector2Int g)
    {
        bool[,] visited = new bool[width, height];
        Stack<Vector2Int> st = new Stack<Vector2Int>();

        st.Push(s);

        Vector2Int[] dirs =
        {
            new Vector2Int( 1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int( 0, 1),
            new Vector2Int( 0,-1),
        };

        while (st.Count > 0)
        {
            Vector2Int cur = st.Pop();
            if (!InBounds(map, cur.x, cur.y)) continue;
            if (visited[cur.x, cur.y]) continue;
            if (map[cur.x, cur.y] == 0) continue; // 벽이면 못 감

            visited[cur.x, cur.y] = true;

            if (cur == g)
                return true; // 탈출 가능

            foreach (var d in dirs)
            {
                Vector2Int next = cur + d;
                if (InBounds(map, next.x, next.y) &&
                    !visited[next.x, next.y] &&
                    map[next.x, next.y] != 0)
                {
                    st.Push(next);
                }
            }
        }

        return false; // 탈출 불가능
    }

    // ---------------------------
    // 4) Cube로 맵 시각화
    // ---------------------------
    void BuildVisualTiles()
    {
        tileObjs = new GameObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                obj.transform.position = new Vector3(x * tileSize, 0, y * tileSize);

                // 벽은 조금 높게, 바닥은 낮게
                float heightScale = (map[x, y] == 0) ? 1.5f : 0.2f;
                obj.transform.localScale = new Vector3(tileSize, heightScale, tileSize);

                var rend = obj.GetComponent<Renderer>();
                rend.material = new Material(Shader.Find("Standard"));

                switch (map[x, y])
                {
                    case 0: rend.material.color = wallColor; break;
                    case 1: rend.material.color = groundColor; break;
                    case 2: rend.material.color = forestColor; break;
                    case 3: rend.material.color = mudColor; break;
                }

                // 시작/목표 표시
                if (x == start.x && y == start.y)
                    rend.material.color = Color.blue;
                else if (x == goal.x && y == goal.y)
                    rend.material.color = Color.red;

                tileObjs[x, y] = obj;
            }
        }
    }

    // ---------------------------
    // 5) Dijkstra 알고리즘 (원래 코드 기반)
    // ---------------------------
    List<Vector2Int> Dijkstra(int[,] map, Vector2Int start, Vector2Int goal)
    {
        int w = map.GetLength(0);
        int h = map.GetLength(1);

        int[,] dist = new int[w, h];        // 최소 비용
        bool[,] visited = new bool[w, h];   // 방문 여부
        Vector2Int?[,] parent = new Vector2Int?[w, h]; // 경로 복원용

        // 초기 거리 MaxValue로 설정
        for (int x = 0; x < w; x++)
            for (int y = 0; y < h; y++)
                dist[x, y] = int.MaxValue;

        dist[start.x, start.y] = 0;

        Vector2Int[] dirs =
        {
            new Vector2Int( 1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int( 0, 1),
            new Vector2Int( 0,-1),
        };

        List<Vector2Int> open = new List<Vector2Int>();
        open.Add(start);

        while (open.Count > 0)
        {
            // 1) 가장 비용이 낮은 노드 선택 (간이 우선순위 큐)
            int bestIndex = 0;
            int bestDist = dist[open[0].x, open[0].y];

            for (int i = 1; i < open.Count; i++)
            {
                var c = open[i];
                int d = dist[c.x, c.y];
                if (d < bestDist)
                {
                    bestDist = d;
                    bestIndex = i;
                }
            }

            Vector2Int cur = open[bestIndex];
            open.RemoveAt(bestIndex);

            // 이미 방문한 곳이면 스킵
            if (visited[cur.x, cur.y]) continue;
            visited[cur.x, cur.y] = true;

            // 목표 도달하면 경로 반환
            if (cur == goal)
                return ReconstructPath(parent, start, goal);

            // 이웃 검사
            foreach (var d in dirs)
            {
                int nx = cur.x + d.x;
                int ny = cur.y + d.y;

                if (!InBounds(map, nx, ny)) continue;
                if (map[nx, ny] == 0) continue; // 벽

                int moveCost = TileCost(map[nx, ny]); // 이동 비용
                if (moveCost == int.MaxValue) continue;

                int newDist = dist[cur.x, cur.y] + moveCost;

                // 더 싼 비용이면 갱신
                if (newDist < dist[nx, ny])
                {
                    dist[nx, ny] = newDist;
                    parent[nx, ny] = cur;

                    Vector2Int next = new Vector2Int(nx, ny);
                    if (!visited[nx, ny] && !open.Contains(next))
                        open.Add(next);
                }
            }
        }

        return null; // 경로 없음
    }

    int TileCost(int tile)
    {
        switch (tile)
        {
            case 1: return 1; // 평지
            case 2: return 3; // 숲
            case 3: return 5; // 진흙
            default: return int.MaxValue;
        }
    }

    bool InBounds(int[,] map, int x, int y)
    {
        return x >= 0 && y >= 0 &&
               x < map.GetLength(0) &&
               y < map.GetLength(1);
    }

    List<Vector2Int> ReconstructPath(Vector2Int?[,] parent, Vector2Int start, Vector2Int goal)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int? cur = goal;

        while (cur.HasValue)
        {
            path.Add(cur.Value);
            if (cur.Value == start) break;

            cur = parent[cur.Value.x, cur.Value.y];
        }

        path.Reverse();
        return path;
    }

    // ---------------------------
    // 6) 최단 경로 시각화 (버튼에서도 호출 가능)
    // ---------------------------
    public void ShowShortestPath()
    {
        if (lastPath == null || tileObjs == null) return;

        foreach (var p in lastPath)
        {
            var rend = tileObjs[p.x, p.y].GetComponent<Renderer>();
            rend.material.color = pathColor;
        }

        // 시작, 목표 색 다시 덮어쓰기
        tileObjs[start.x, start.y].GetComponent<Renderer>().material.color = Color.blue;
        tileObjs[goal.x, goal.y].GetComponent<Renderer>().material.color = Color.red;
    }
}
