using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEmoji", menuName = "Custom/Emoji", order = 1)]
public class Emoji : ScriptableObject
{
    [SerializeField] private Sprite _icon = null;
    [SerializeField] private string[] _words = null;

    public Sprite Icon => _icon;

    public bool CheckWord(string word)
    {
        return _words.Contains(word.ToLower());
    }

    public List<string> Words => new List<string>(_words);
}
