using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTheme", menuName = "Custom/Theme", order = 2)]
public class Theme : ScriptableObject
{
    [SerializeField] private string _name = "Theme name";
    [SerializeField] private Emoji[] _emojies = null;

    public string Name => _name;
    public Emoji[] Emojies => new List<Emoji>(_emojies).ToArray();
}
