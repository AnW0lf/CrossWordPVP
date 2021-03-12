using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dictionary : MonoBehaviour
{
    [SerializeField] private Transform _container = null;
    [SerializeField] private GameObject _emojiPrefab = null;
    [SerializeField] private int _emojiCount = 3;
    [SerializeField] private Emoji[] _emojis = null;

    private List<InterfaceEmoji> list = new List<InterfaceEmoji>();

    public void AddEmoji()
    {
        InterfaceEmoji emoji = Instantiate(_emojiPrefab, _container).GetComponent<InterfaceEmoji>();
        emoji.Data = _emojis[Random.Range(0, _emojis.Length)];
        list.Add(emoji);
    }

    public bool DeleteAt(int index)
    {
        if (index > 0 && index <= list.Count)
        {
            InterfaceEmoji emoji = list[index - 1];
            list.RemoveAt(index - 1);
            emoji.Delete();
            return true;
        }
        return false;
    }

    public int IndexOf(string word)
    {
        for (int i = 0; i < list.Count; i++)
            if (list[i].Data.CheckWord(word)) return i + 1;
        return 0;
    }

    public bool CheckWord(string word)
    {
        int index = IndexOf(word);
        if (DeleteAt(index))
        {
            StartCoroutine(Utils.DelayedCall(0.5f, AddEmoji));
            return true;
        }
        return false;
    }

    private void Start()
    {
        while (list.Count < _emojiCount)
            AddEmoji();
    }

    public string GetRandomWord()
    {
        List<string> words = new List<string>();
        foreach(var emoji in list)
            words.AddRange(emoji.Data.Words);
        return words[Random.Range(0, words.Count)];
    }
}
