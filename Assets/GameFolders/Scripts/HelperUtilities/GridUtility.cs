using UnityEngine;

public class GridUtility : MonoBehaviour
{
    // Convert grid coordinates to world coordinates
    public static Vector2 GridToWorld(int row, int col, float cellSize = 1.2f) 
    {
        return new Vector2(col * cellSize, -row * cellSize);
    }

    // Convert world coordinates to grid coordinates
    public static void WorldToGrid(Vector3 worldPosition, out int row, out int col, float cellSize = 1.2f)
    {
        row = Mathf.RoundToInt(-worldPosition.y / cellSize);
        col = Mathf.RoundToInt(worldPosition.x / cellSize);
    }
}
