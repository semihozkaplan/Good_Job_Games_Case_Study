using UnityEngine;

[CreateAssetMenu(menuName = "Game/BlockData")]
public class BlockData : ScriptableObject
{
    public Color BlockColor;
    public int ColorID;
    public Sprite DefaultIcon;
    public Sprite IconA;
    public Sprite IconB;
    public Sprite IconC;
}
