using System.Collections.Generic;
using UnityEngine;

public class BlockPool : MonoBehaviour
{
    [SerializeField]
    private Block _blockPrefab;
    [SerializeField]
    private int _initialPoolSize = 100;

    private Queue<Block> _pool = new Queue<Block>();

    private void Awake()
    {
        InitializePool();
    }

    // Initialize the block pool with the initial pool size
    private void InitializePool()
    {
        for (int i = 0; i < _initialPoolSize; i++)
        {
            Block block = Instantiate(_blockPrefab, transform);
            block.gameObject.SetActive(false);
            _pool.Enqueue(block);
        }
    }

    // Get a block from the pool
    public Block GetBlock()
    {
        if (_pool.Count == 0)
            ExpandPool();

        Block block = _pool.Dequeue();
        block.gameObject.SetActive(true);
        return block;
    }

    // Return a block to the pool
    public void ReturnBlock(Block block)
    {
        block.gameObject.SetActive(false);
        _pool.Enqueue(block);
    }

    // Expand the pool by instantiating new blocks
    private void ExpandPool()
    {
        for (int i = 0; i < _initialPoolSize; i++)
        {
            Block block = Instantiate(_blockPrefab, transform);
            block.gameObject.SetActive(false);
            _pool.Enqueue(block);
        }
    }
}
