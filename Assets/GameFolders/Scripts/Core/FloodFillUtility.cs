using System;
using System.Collections.Generic;
using UnityEngine;

public static class FloodFillUtility
{
    private static Queue<Block> _queue = new Queue<Block>();
    private static bool[,] _visited;
    
    public static List<Block> FindConnectedBlocks(Block[,] grid, Block startBlock) 
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        // Initialize visited array and optimize memory usage with reducing garbage collection
        if (_visited == null || _visited.GetLength(0) != rows || _visited.GetLength(1) != cols)
        {
            _visited = new bool[rows, cols];
        }
        else
        {
            Array.Clear(_visited, 0, _visited.Length);
        }
        _queue.Clear();

        // List to store connected blocks and target color ID
        List<Block> connectedBlocks = new List<Block>();
        int targetColorID = startBlock.ColorID;

        // Enqueue the start block and mark it as visited
        _queue.Enqueue(startBlock);
        _visited[startBlock.GridPosition.x, startBlock.GridPosition.y] = true;

        // Breadth first search algorithm to find connected blocks
        while (_queue.Count > 0)
        {
            Block current = _queue.Dequeue();
            connectedBlocks.Add(current);

            // Check row neighbors
            CheckNeighbor(grid, current.GridPosition.x + 1, current.GridPosition.y, _visited, _queue, targetColorID);
            CheckNeighbor(grid, current.GridPosition.x - 1, current.GridPosition.y, _visited, _queue, targetColorID);
            // Check column neighbors
            CheckNeighbor(grid, current.GridPosition.x, current.GridPosition.y + 1, _visited, _queue, targetColorID);
            CheckNeighbor(grid, current.GridPosition.x, current.GridPosition.y - 1, _visited, _queue, targetColorID);
        }
        if (connectedBlocks.Count >= 2)
        {
            Debug.Log($"Founded {connectedBlocks.Count} connected blocks)");
            return connectedBlocks;
        }
        else
        {
            return null;
        }
    }

    public static void CheckNeighbor(Block[,] grid, int row, int col, bool[,] visited, Queue<Block> queue,
        int targetColorID)
    {
        // Check if the neighbor is within the grid boundaries
        if (row >= 0 && row < grid.GetLength(0) && col >= 0 && col < grid.GetLength(1))
        {
            // Check if the neighbor is not visited and has the same color
            Block neighbor = grid[row, col];
            if (neighbor != null && !visited[row, col] && neighbor.ColorID == targetColorID)
            {
                visited[row, col] = true;
                queue.Enqueue(neighbor);
            }
        }
    }
}
