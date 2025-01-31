using UnityEngine;

public class DeadlockResolver
{
    public bool IsDeadlock(Block[,] grid)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        // Check for deadlock in the grid for each row and column
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Block block = grid[row, col];
                if (block == null) continue;

                // Check right neighbor, if it's not null and has the same color, return false
                if (col < cols - 1 && grid[row, col + 1] != null &&
                    grid[row, col + 1].ColorID == block.ColorID)
                {
                    return false;
                }

                // Check bottom neighbor, if it's not null and has the same color, return false
                if (row < rows - 1 && grid[row + 1, col] != null &&
                    grid[row + 1, col].ColorID == block.ColorID)
                {
                    return false;
                }

            }
        }
        return true;
    }

    // Resolve deadlock by shuffling blocks
    public void IntelligentShuffle(Block[,] grid)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        // Swapping blocks to resolve deadlock, it checks every 2x2 block
        for (int row = 0; row < rows; row += 2)
        {
            for (int col = 0; col < cols; col += 2)
            {
                if(col < cols - 1 && row < rows - 1)
                {
                    SwapBlocks(grid, row, col, row, col + 1);               
                    SwapBlocks(grid, row, col, row + 1, col);
                }               
            }
        }

    }

    private void SwapBlocks(Block[,] grid, int row1, int col1, int row2, int col2)
    {
        // Swap blocks in the grid 
        Block temp = grid[row1, col1];
        grid[row1, col1] = grid[row2, col2];
        grid[row2, col2] = temp;

        // Update grid positions
        if (grid[row1, col1] != null) grid[row1, col1].UpdateGridPosition(new Vector2Int(row1, col1));
        if (grid[row2, col2] != null) grid[row2, col2].UpdateGridPosition(new Vector2Int(row2, col2));

        // Update block's world position
        if (grid[row1, col1] != null) grid[row1, col1].transform.position = GridUtility.GridToWorld(row1, col1);
        if (grid[row2, col2] != null) grid[row2, col2].transform.position = GridUtility.GridToWorld(row2, col2);
    }

}
