using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private BoardManager _boardManager;
    [SerializeField]
    private GameSettings _gameSettings;
    [SerializeField]
    private GameObject _backgroundObject;

    private void Awake()
    {
        // Singleton Pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetupScene();
    }
    private void SetupScene()
    {
        float cellSize = 1.2f; 
        int rows = _gameSettings.Rows;
        int cols = _gameSettings.Columns;

        // Center the camera over the grid
        Camera.main.transform.position = new Vector3((cols - 1) * cellSize / 2, -(rows - 1) * cellSize / 2, -10);

        // Set the camera size 
        Camera.main.orthographicSize = (rows * cellSize + 2) / 2;

        // Set the background sprite
        _backgroundObject.GetComponent<SpriteRenderer>().sprite = _gameSettings.BackgroundSprite;
        _backgroundObject.transform.position = new Vector3((cols - 1) * cellSize / 2, -(rows - 1) * cellSize / 2, 0);
    }

    // Handle the block click event
    public void OnBlockClicked(Block block)
    {
        _boardManager.OnBlockClicked(block);
        Debug.Log("Block Clicked");
    }
}
