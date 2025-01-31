using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D), typeof(Animator))]
public class Block : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    private BlockData _blockData;
    private Vector2Int _gridPosition;

    public BlockData BlockData => _blockData;

    public int ColorID { get; private set; }
    public Vector2Int GridPosition => _gridPosition;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Initialize the block with the given block data and grid position
    public void Initialize(BlockData blockData, Vector2Int gridPosition)
    {
        _blockData = blockData;
        _gridPosition = gridPosition;
        _spriteRenderer.sprite = _blockData.DefaultIcon;
        _spriteRenderer.color = _blockData.BlockColor;
        ColorID = _blockData.ColorID;
    }

    // Update the grid position of the block
    public void UpdateGridPosition(Vector2Int newPos)
    {
        _gridPosition = newPos;
    }

    // Update the icon of the block based on the group size and game settings
    public void UpdateIcon(int groupSize, GameSettings settings) 
    {
        _spriteRenderer.sprite = groupSize > settings.C ? _blockData.IconC :
                                 groupSize > settings.B ? _blockData.IconB :
                                 groupSize > settings.A ? _blockData.IconA :
                                 _blockData.DefaultIcon;
    }

    // Handle the block click event
    private void OnMouseDown() => GameManager.Instance.OnBlockClicked(this);

    // Play the blast animation
    public void BlastAnimation()
    {
        _animator.SetTrigger("Blast");
        Invoke("ReturnToPool", 0.2f);
    }

    // Return the block to the pool
    private void ReturnToPool()
    {
        gameObject.SetActive(false);
    }

}
