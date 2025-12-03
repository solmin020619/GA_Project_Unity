using System.Collections.Generic;
using UnityEngine;

public class DijkstraSample : MonoBehaviour
{
    // 0=벽, 1=평지, 2=숲, 3=진흙
    int[,] map =
    {
        {0,0,0,0,0,0,0},
        {0,1,1,1,1,1,0},
        {0,1,2,2,1,1,0},
        {0,1,3,3,1,1,0},
        {0,1,1,2,1,1,0},
        {0,0,0,0,0,0,0},
    };

    Vector2Int start = new Vector2Int(1, 1);
    Vector2Int goal = new Vector2Int(4, 5);

    void Start()
    {
        var path = Dijkstra(map, start, goal);

        if (path == null)
            Debug.Log("경로 없음");
        else
        {
            Debug.Log($"경로 길이: {path.Count}");
            foreach (var p in path)
                Debug.Log(p);
        }
    }

    // ---------------------------------------
    // Dijkstra 알고리즘
    // ---------------------------------------
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

        // -----------------------------
        // Main Loop
        // -----------------------------
        while (open.Count > 0)
        {
            // 1) 가장 비용이 낮은 노드 선택
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

            // 2) 이미 방문한 곳이면 스킵
            if (visited[cur.x, cur.y]) continue;
            visited[cur.x, cur.y] = true;

            // 3) 목표 도달시 경로 반환
            if (cur == goal)
                return ReconstructPath(parent, start, goal);

            // 4) 이웃 검사
            foreach (var d in dirs)
            {
                int nx = cur.x + d.x;
                int ny = cur.y + d.y;

                if (!InBounds(map, nx, ny)) continue;
                if (map[nx, ny] == 0) continue;   // 벽

                int moveCost = TileCost(map[nx, ny]); // 이동 비용
                if (moveCost == int.MaxValue) continue;

                int newDist = dist[cur.x, cur.y] + moveCost;

                // 더 싼 비용이면 갱신
                if (newDist < dist[nx, ny])
                {
                    dist[nx, ny] = newDist;
                    parent[nx, ny] = cur;

                    if (!visited[nx, ny] && !open.Contains(new Vector2Int(nx, ny)))
                        open.Add(new Vector2Int(nx, ny));
                }
            }
        }

        return null; // 경로 없음
    }

    // ---------------------------------------
    // 타일 비용 계산
    // ---------------------------------------
    int TileCost(int tile)
    {
        switch (tile)
        {
            case 1: return 1;     // 평지
            case 2: return 3;     // 숲
            case 3: return 5;     // 진흙
            default: return int.MaxValue; // 벽 등 이동 불가
        }
    }

    // ---------------------------------------
    // 범위 체크
    // ---------------------------------------
    bool InBounds(int[,] map, int x, int y)
    {
        return x >= 0 && y >= 0 &&
               x < map.GetLength(0) &&
               y < map.GetLength(1);
    }

    // ---------------------------------------
    // 경로 복원
    // ---------------------------------------
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
}
