using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Word", menuName = "Scriptable Objects/SO_Word")]
public class SO_Word : ScriptableObject
{
    public enum Effect
    {
        ROCK,
        JUMP,
        CLIMB
    }

    [Tooltip("What text will be printed")]
    [SerializeField] string word = default;


    public string GetWord()
    {
        return word;
    }
}
