using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Player/Data")]
public class PlayerDataSo : ScriptableObject
{
    [Header("Controls")]
    public KeyCode Jump = KeyCode.Space;
    public KeyCode Left = KeyCode.A;
    public KeyCode Right = KeyCode.D;

    [Header("Configs")]
    public float Speed;
    public float JumpSpeed;
    public float GravityScaleJump;
    public float GravityScaleFall;
    public float GravityScaleDead;

}