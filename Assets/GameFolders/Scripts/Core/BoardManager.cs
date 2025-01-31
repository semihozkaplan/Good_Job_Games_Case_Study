using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField]
    private GameSettings _gameSettings;
    [SerializeField]
    private BlockData[] _blockData;
    [SerializeField]
    private BlockPool _blockPool;

    private Block[,] _grid;
    private bool _isProcessing;
    private DeadlockResolver _deadlockResolver = new DeadlockResolver();

    private void Start()
    {
        InitializeGrid();
    }

    // Initializing blocks by spawning them and check for deadlock
    private void InitializeGrid()
    {
        _grid = new Block[_gameSettings.Rows, _gameSettings.Columns];

        for(int row = 0; row < _gameSettings.Rows; row++)
        {
            for(int col = 0; col < _gameSettings.Columns; col++) 
            {
                SpawnBlock(row, col);
            }
        }
        CheckForDeadlock();
    }

    // Spawning blocks in the grid
    private void SpawnBlock(int row, int col) 
    {
        Block block = _blockPool.GetBlock();
        block.Initialize(_blockData[Random.Range(0, _gameSettings.Colors)], new Vector2Int(row, col));
        block.transform.position = GridUtility.GridToWorld(row, col);
        _grid[row, col] = block;
    }

    // Handling block click event
    public void OnBlockClicked(Block block)
    {
        if (_isProcessing) return;

        List<Block> blockGroup = FloodFillUtility.FindConnectedBlocks(_grid, block);
        if (blockGroup != null && blockGroup.Count >= 2)
        {
            StartCoroutine(CollapseProcessing(blockGroup));
        }
    }

    // Processing the collapse of blocks
    private IEnumerator CollapseProcessing(List<Block> blocks)
    {
        _isProcessing = true;

        foreach (Block block in blocks)
        {
            block.BlastAnimation();
        }
        yield return new WaitForSeconds(_gameSettings.BlastDuration);

        // Disable block and return to pool
        foreach (Block block in blocks)
        {           
            _grid[block.GridPosition.x, block.GridPosition.y] = null;
            _blockPool.ReturnBlock(block);
        }

        // Shift columns down
        yield return StartCoroutine(ShiftColumnsDown());

        // Fill empty spaces with new blocks
        for (int col = 0; col < _gameSettings.Columns; col++)
        {
            for (int row = 0; row < _gameSettings.Rows; row++)
            {
                if (_grid[row, col] == null)
                {
                    SpawnBlock(row, col);
                }
            }
        }

        // Update all icons of blocks
        UpdateAllIcons();

        // Check for deadlock
        CheckForDeadlock();

        _isProcessing = false;
    }

    private IEnumerator ShiftColumnsDown()
    {
        // Search each column
        for (int col = 0; col < _gameSettings.Columns; col++)
        {
            // Search each row from bottom to top
            for (int row = _gameSettings.Rows - 1; row >= 0; row--)
            {
                // If the block is null, search for the first non-null block in above rows
                if (_grid[row, col] == null)
                {
                    // This search for the first non-null block in above rows
                    for (int aboveRow = row - 1; aboveRow >= 0; aboveRow--)
                    {
                        // If the found block is not null, shift it down
                        if (_grid[aboveRow, col] != null)
                        {   
                            Block movedBlocked = _grid[aboveRow, col];
                            _grid[row, col] = movedBlocked;
                            _grid[aboveRow, col] = null;
                            movedBlocked.UpdateGridPosition(new Vector2Int(row, col));

                            // Update the world position of the block in the world
                            StartCoroutine(SmoothShiftTranslate(movedBlocked, GridUtility.GridToWorld(aboveRow, col), GridUtility.GridToWorld(row, col)));
                            break;
                        }
                    }
                }
            }
        }
        yield return new WaitForSeconds(_gameSettings.CollapseDuration);
    }

    // Smoothly shift the block to the new position
    private IEnumerator SmoothShiftTranslate(Block block, Vector2 startPos, Vector2 endPos)
    {
        float duration = _gameSettings.CollapseDuration;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float smoothTime = Mathf.Clamp01(elapsed / duration * 1.2f);
            block.transform.position = Vector2.Lerp(startPos, endPos, smoothTime);
            yield return null;
        }

        // Ensure the final position
        block.transform.position = GridUtility.GridToWorld(block.GridPosition.x, block.GridPosition.y);
    }

    // Update all icons of blocks based on the group size
    private void UpdateAllIcons()
    {
        for (int row = 0; row < _gameSettings.Rows; row++)
        {
            for (int col = 0; col < _gameSettings.Columns; col++)
            {
                List<Block> connectedBlocks = FloodFillUtility.FindConnectedBlocks(_grid, _grid[row, col]);
                if (connectedBlocks != null) _grid[row, col].UpdateIcon(connectedBlocks.Count, _gameSettings);
            }
        }
    }

    // Check for deadlock and resolve it
    private void CheckForDeadlock()
    {
        if (_deadlockResolver.IsDeadlock(_grid))
        {
            _deadlockResolver.IntelligentShuffle(_grid);
            UpdateAllIcons();
        }
    }
}
 