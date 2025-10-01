using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player/Data", order = 1)]
public class PlayerData : ScriptableObject
{
    public KeyCode Jump = KeyCode.Space;
    public KeyCode Left = KeyCode.LeftArrow;
    public KeyCode Right = KeyCode.RightArrow;
    public float Speed;
    public float JumpSpeed;
    public float GravityScaleJump;
    public float GravityScaleFall;
    public float GravityScaleDead;
}
