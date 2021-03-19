using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dictionary : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _label = null;
    [SerializeField] private Transform _container = null;
    [SerializeField] private GameObject _emojiPrefab = null;
    [SerializeField] private int _emojiCount = 3;
    [SerializeField] private Theme[] _themes = null;

    private Theme _theme = null;
    private Emoji[] _emojies = null;
    private List<InterfaceEmoji> list = new List<InterfaceEmoji>();

    public void AddEmoji()
    {
        InterfaceEmoji emoji = Instantiate(_emojiPrefab, _container).GetComponent<InterfaceEmoji>();
        List<Emoji> available = new List<Emoji>(_emojies);

        for(int i = available.Count - 1; i >= 0; i--)
        {
            foreach(var l in list)
            {
                if(l.Data.Words[0] == available[i].Words[0])
                {
                    available.RemoveAt(i);
                    break;
                }
            }
        }

        emoji.Data = available[Random.Range(0, available.Count)];
        list.Add(emoji);
    }

    public List<InterfaceEmoji> Emojies => new List<InterfaceEmoji>(list);

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

    private void Awake()
    {
        _theme = _themes[Random.Range(0, _themes.Length)];
        _emojies = _theme.Emojies;
        _label.text = _theme.Name;
    }

    private void Start()
    {
        while (list.Count < _emojiCount)
            AddEmoji();
    }

    public string GetDefaultWord()
    {
        List<string> words = new List<string>();
        foreach (var emoji in list)
            words.Add(emoji.Data.Words[0]);
        return words[Random.Range(0, words.Count)];
    }

    public string GetRandomWord()
    {
        List<string> words = new List<string>();
        foreach(var emoji in list)
            words.AddRange(emoji.Data.Words);
        return words[Random.Range(0, words.Count)];
    }

    public void Hide()
    {
        StartCoroutine(Utils.CrossFading(_label.rectTransform.anchoredPosition, _label.rectTransform.anchoredPosition + Vector2.up * 1000f, 1f,
            (pos) => _label.rectTransform.anchoredPosition = pos, (a, b, c) => Vector2.Lerp(a, b, c)));

        RectTransform containerRect = _container.GetComponent<RectTransform>();
        StartCoroutine(Utils.CrossFading(containerRect.anchoredPosition, containerRect.anchoredPosition + Vector2.down * 1000f, 1f,
            (pos) => containerRect.anchoredPosition = pos, (a, b, c) => Vector2.Lerp(a, b, c)));
    }
}
