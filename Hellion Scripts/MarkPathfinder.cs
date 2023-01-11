using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MarkPathfinder
{
    int[][] m;
    List<Vector2Int> origins = new List<Vector2Int>();
    public MarkPathfinder(int[][] map, int originBuildingIndex = 2) // unused -2, walkable -1, origin 0
    {
        Generate(map, originBuildingIndex);
    }

    public int[][] Generate(int[][] map, int originBuildingIndex = 2)
    {
        Debug.Log(originBuildingIndex);
        origins.Clear();
        m = new int[map.Length][];
        for (int i = 0; i < map.Length; i++)
        {
            m[i] = new int[map[i].Length];
            for (int j = 0; j < map[i].Length; j++)
            {
                if (map[i][j] == 0)
                {
                    m[i][j] = -2;
                }
                else if (map[i][j] == 1)
                {
                    m[i][j] = -1;
                }
                else if (map[i][j] == originBuildingIndex) //Attactor specific
                {
                    m[i][j] = 0;
                    origins.Add(new Vector2Int(i, j));
                }
                else 
                {
                    m[i][j] = -2;
                }
            }
        }

        for (int k = 0; k < 5; k++) {
            int rand = 0;
            bool done = false;

            int left = origins[rand].x;
            int right = origins[rand].x;
            int top = origins[rand].y;
            int bottom = origins[rand].y;

            do {
                int countToDone = 0;
                left--;
                right++;
                top++;
                bottom--;

                if (left < 0)
                {
                    left = 0;
                    countToDone++;
                }

                if (bottom < 0)
                {
                    bottom = 0;
                    countToDone++;
                }

                if (right > m.Length - 1)
                {
                    right = m.Length - 1;
                    countToDone++;
                }

                if (top > m[0].Length - 1)
                {
                    top = m[0].Length - 1;
                    countToDone++;
                }

                //way one
                for (int i = left + 1; i <= right - 1; i++) {
                    CheckCost(i, top);
                    CheckCost(i, bottom);
                }

                for (int i = bottom + 1; i <= top - 1; i++)
                {
                    CheckCost(left, i);
                    CheckCost(right, i);
                }

                //way two
                for (int i = right - 1; i >= left + 1; i--)
                {
                    CheckCost(i, top);
                    CheckCost(i, bottom);
                }

                for (int i = top - 1; i >= bottom + 1; i--)
                {
                    CheckCost(left, i);
                    CheckCost(right, i);
                }

                CheckCost(right, top);
                CheckCost(right, bottom);
                CheckCost(left, top);
                CheckCost(left, bottom);

                if (countToDone >= 4) {
                    done = true;
                }

                //Debug.Log(left.ToString() + "|" + right.ToString() + "|" + top.ToString() + "|" + bottom.ToString());

            } while (!done);
        }
        return m;
    }

    void CheckCost(int x, int y)
    {
        int smallestValue = 10000;
        if (m[x][y] == -1)
        {
            
            if (m[x + 1][y] <= smallestValue && m[x + 1][y] >= 0) 
            {
                smallestValue = m[x + 1][y];
            }

            if (m[x - 1][y] <= smallestValue && m[x - 1][y] >= 0)
            {
                smallestValue = m[x - 1][y];
            }

            if (m[x][y + 1] <= smallestValue && m[x][y + 1] >= 0)
            {
                smallestValue = m[x][y + 1];
            }

            if (m[x][y - 1] <= smallestValue && m[x][y - 1] >= 0)
            {
                smallestValue = m[x][y - 1];
            }

            if (smallestValue >= 0 && smallestValue < 10000)
            {
                m[x][y] = smallestValue + 1;
                //Debug.Log(smallestValue);
            }
            
            //m[x][y] = 255;
        }
        if (m[x][y] == 0)
        {
            m[x][y] = 0;
            //Debug.Log("run");
        }
    }

    public void SaveMap() {
        Texture2D tex = new Texture2D(m.Length, m[0].Length);
        for (int i = 0; i < tex.width; i++) 
        {
            for (int j = 0; j < tex.height; j++) 
            {
                tex.SetPixel(i, j, new Color((float)m[i][j] / 255f, (float)m[i][j] / 255f, (float)m[i][j] / 255f));
            }
        }
        tex.Apply();
        byte[] bytes = tex.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/../SavedScreen.png", bytes);
    }

    public int[][] GetPaths() {
        return m;
    }
}
