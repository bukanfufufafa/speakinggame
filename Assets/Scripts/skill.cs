using UnityEngine;

public class skill : MonoBehaviour
{
    private characterStat characterStat;

    public string[] spells;

    // efek sesuai index
    // contoh: [-30, +20, -10]
    public int[] effects;

    void Start()
    {
        characterStat = GetComponent<characterStat>();
    }

    public void spellingCast(string word)
    {
        for (int i = 0; i < spells.Length; i++)
        {
            if (spells[i].ToLower() == word.ToLower())
            {
                ApplyEffect(i);
                return;
            }
        }

        Debug.Log("Kata tidak cocok");
    }

    void ApplyEffect(int index)
    {
        int value = effects[index];

        characterStat.health += value;

        // clamp biar tidak minus
        if (characterStat.health < 0)
            characterStat.health = 0;

        Debug.Log("Spell index " + index + " efek: " + value +
                  " | Health sekarang: " + characterStat.health);
    }
}