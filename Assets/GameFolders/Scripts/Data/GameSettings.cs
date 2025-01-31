using UnityEngine;

[CreateAssetMenu(menuName = "Game/GameSettings")]
public class GameSettings : ScriptableObject
{
    [Range(2, 10)] 
    public int Rows = 10;
    [Range(2, 10)]
    public int Columns = 12;
    [Range(1, 6)]
    public int Colors = 6;

    public int A = 4, B = 7, C = 9;

    public float CollapseDuration = 0.3f;

    public Sprite BackgroundSprite;

    public float BlastDuration = 0.3f;
}
