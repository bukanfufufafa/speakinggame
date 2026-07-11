using UnityEngine;

public class characterStat : MonoBehaviour
{
    [Header("Level")]
    public int level = 1;

    [Header("Base Stats")]
    [SerializeField] private int intelligence = 12;
    [SerializeField] private int vitality = 8;
    [SerializeField] private int agility = 6;

    public int Intelligence => intelligence;
    public int Vitality => vitality;
    public int Agility => agility;

    //final stats nya jadi kiye
    public int MagicAttack => intelligence * 3;

    public int MaxMana => intelligence * 15;

    public int MaxHealth => vitality * 12;

    public int Defense => vitality * 2;

    public float MoveSpeed => 5f + agility * 0.1f;

}
