using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

[CreateAssetMenu(fileName = "PokeData", menuName = "Scriptable/PokeData")]
public class PokeData : ScriptableObject
{
    public PokeInfo[] PokeInfos;
    [Space]
    public PrimaryMove[] PrimaryMoves;
    public SecondaryMove[] SecondaryMoves;
    public Color[] PokeTypeColors;
    public TypeChart TypeChart;
    public Sprite[] PokeTypeSprites;

    public int GetPokeInfoIdByName(string pokemonName)
    {
        for (int i = 0; i < PokeInfos.Length; i++)
        {
            if (PokeInfos[i].Name.Equals(pokemonName))
                return i;
        }
        
        Debug.LogError("No PokeInfo with given pokemon name");
        return 0;
    }

    public int GetPrimaryMoveId(string moveName)
    {
        if (string.IsNullOrEmpty(moveName))
        {
            Debug.LogWarning("PokeType in Move is empty");
            return 0;
        }

        for (int i = 0; i < PrimaryMoves.Length; i++)
        {
            if (PrimaryMoves[i].Name.Equals(moveName))
            {
                return i;
            }
        }

        Debug.LogError("No such PokeType in Move: " + moveName);
        return 0;
    }

    public int GetSecondaryMoveId(string moveName)
    {
        if (string.IsNullOrEmpty(moveName))
        {
            Debug.LogWarning("PokeType in Move is empty");
            return 0;
        }

        for (int i = 0; i < SecondaryMoves.Length; i++)
        {
            if (SecondaryMoves[i].Name.Equals(moveName))
            {
                return i;
            }
        }

        Debug.LogError("No such PokeType in Move: " + moveName);
        return 0;
    }
}

[Serializable]
public class PokeInfo
{
    public string Name;
    public int Id;
    public PokeType[] Type;

    [Space]
    public float Weight;
    public float Height;
    public float MaxCp;
    public float CpMultiFromEvo;

    [Space]
    public int BaseAttack;
    [Range(0f, 1f)]
    public float AttackRate;
    public int BaseDefense;
    [Range(0f, 1f)]
    public float DefenseRate;
    public int BaseStamina;
    [Range(0f, 1f)]
    public float StaminaRate;
    public int BaseHp;

    [Space]
    public float Rarity;
    public float CaptureRate;
    public float FleeRate;

    [Space]
    public PokeClass Class;
    public int CandyToEvolve;
    public EggType EggDistanceType;

    [Space]
    public int[] PrimaryMovesIds;
    public int[] SecondaryMovesIds;

    [Space]
    public int EvoFromId;
    public int EvoToId;

    [Space]
    public PokeType[] Resistance;
    public PokeType[] Weaknesses;

    [Space]
    public Sprite Image;
}

[Serializable]
public class PrimaryMove
{
    public string Name;
    public PokeType Type;

    [Space]
    public int Attack;
    public float Cooldown;
    public float Dps;

    [Space]
    [Range(0f, 1f)]
    public float AttackRate;
    [Range(0f, 1f)]
    public float CooldownRate;
    [Range(0f, 1f)]
    public float DpsRate;

    public PokeType[] StrongAgaints;
    public PokeType[] WeakAgains;

    public int EnergyGain;
}

[Serializable]
public class SecondaryMove : PrimaryMove
{
    public int ChargeCount;
    public float CritChance;
    public float DidgeWindow;
}

[Serializable]
public class TypeChart
{
    public TypeParameter[] TypeParameters;

    public TypeChart(int elementsCount)
    {
        TypeParameters = new TypeParameter[elementsCount];
    }

    public PokeType[] GetStrenghts(PokeType pokeType)
    {
        for (int i = 0; i < TypeParameters.Length; i++)
        {
            if (TypeParameters[i].BaseType == pokeType)
                return TypeParameters[i].StrenghtsTypes;
        }

        return null;
    }

    public PokeType[] GetWeaknesses(PokeType pokeType)
    {
        for (int i = 0; i < TypeParameters.Length; i++)
        {
            if (TypeParameters[i].BaseType == pokeType)
                return TypeParameters[i].WeaknessesTypes;
        }

        return null;
    }

    public PokeType[] GetResistance(PokeType[] baseTypes)
    {
        List<PokeType> resistance = new List<PokeType>();

        for (int i = 0; i < baseTypes.Length; i++)
        {
            for (int j = 0; j < TypeParameters.Length; j++)
            {
                if (TypeParameters[j].WeaknessesTypes != null)
                {
                    if (TypeParameters[j].WeaknessesTypes.Contains(baseTypes[i]) && !resistance.Contains(TypeParameters[j].BaseType))
                        resistance.Add(TypeParameters[j].BaseType);
                }
            }
        }

        for (int i = 0; i < baseTypes.Length; i++)
        {
            for (int j = 0; j < TypeParameters.Length; j++)
            {
                if (TypeParameters[j].StrenghtsTypes != null)
                {
                    if (TypeParameters[j].StrenghtsTypes.Contains(baseTypes[i]))
                    {
                        resistance.Remove(TypeParameters[j].BaseType);
                    }
                }
            }
        }

        return resistance.ToArray();
    }

    public PokeType[] GetWeaknesses([NotNull] PokeType[] baseTypes)
    {
        List<PokeType> weaknesses = new List<PokeType>();

        for (int i = 0; i < baseTypes.Length; i++)
        {
            for (int j = 0; j < TypeParameters.Length; j++)
            {
                if (TypeParameters[j].StrenghtsTypes != null)
                {
                    if (TypeParameters[j].StrenghtsTypes.Contains(baseTypes[i]) && !weaknesses.Contains(TypeParameters[j].BaseType))
                        weaknesses.Add(TypeParameters[j].BaseType);
                }
            }
        }

        for (int i = 0; i < baseTypes.Length; i++)
        {
            for (int j = 0; j < TypeParameters.Length; j++)
            {
                if (TypeParameters[j].WeaknessesTypes != null)
                {
                    if (TypeParameters[j].WeaknessesTypes.Contains(baseTypes[i]))
                    {
                        weaknesses.Remove(TypeParameters[j].BaseType);
                    }
                }
            }
        }

        return weaknesses.ToArray();
    }
}

[Serializable]
public class TypeParameter
{
    public PokeType BaseType;
    public PokeType[] StrenghtsTypes;
    public PokeType[] WeaknessesTypes;
}

[Serializable]
public enum EggType
{
    None = 0, km2 = 2, km5 = 5, km10 = 10
}

[Serializable]
public enum PokeClass
{
    Normal = 0, Legendary = 1
}

[Serializable]
public enum PokeType
{
    Bug = 0, Dark, Dragon, Electric, Fairy, Fighting, Fire, Flying, Ghost, Grass, Ground, Ice, Normal, Poison, Psychic, Rock, Steel, Water
}