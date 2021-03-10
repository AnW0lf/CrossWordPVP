using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Road : MonoBehaviour
{
    [SerializeField] private AnimatedPanel _playerPanel = null;
    [SerializeField] private Bot _bot = null;
    [SerializeField] public string[] dictionary = null;
    [SerializeField] private LetterBlock[] _blocks = null;

    private Team _round = Team.Player;
    [HideInInspector] public List<string> _words = new List<string>();


    private int ChainLength
    {
        get
        {
            int length = 0;
            foreach (var word in _words) length += word.Length;
            return length;
        }
    }

    private Transform LastLetterBlock
    {
        get
        {
            if (!_blocks[0].Visible) return _blocks[0].transform;

            for (int i = 0; i < _blocks.Length; i++)
                if (!_blocks[i].Visible) return _blocks[i - 1].transform;

            return _blocks[_blocks.Length - 1].transform;
        }
    }

    private void Start()
    {
        StartCoroutine(Utils.DelayedCall(1f, () => BeginRound(_round)));
    }

    private void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, LastLetterBlock.localRotation, 0.05f);
    }

    public void NextRound()
    {
        if (_round == Team.Player) _round = Team.Enemy;
        else _round = Team.Player;
    }

    private void BeginRound(Team round)
    {
        if(_round == Team.Player)
        {
            _playerPanel.Visible = true;
        }
        else
        {
            _bot.Begin();
        }
    }

    public void Submit()
    {
        string word = string.Empty;
        if (_round == Team.Player)
        {
            word = _playerPanel.Word;
            if (!CheckWord(word)) return;
            _playerPanel.Visible = false;    
        }
        else
        {
            word = _bot.Word;
            if (!CheckWord(word)) return;
        }

        int oldChainLength = ChainLength;
        _words.Add(word.ToLower());
        int chainLength = ChainLength;

        for(int i = oldChainLength; i < chainLength && i < _blocks.Length; i++)
        {
            int index = i - oldChainLength;
            LetterBlock block = _blocks[i];
            block.letter = word[index].ToString();
            block.team = _round;

            StartCoroutine(Utils.DelayedCall(0.4f + 0.1f * index, () => block.Show()));
        }

        if (CheckWin()) return;

        StartCoroutine(Utils.DelayedCall((chainLength - oldChainLength) * 0.1f + 1f, () =>
        {
            NextRound();
            BeginRound(_round);
        }));
    }

    private bool CheckWin() => _blocks[_blocks.Length - 1].letter != string.Empty;

    private bool CheckWord(string word) => dictionary.Contains(word.ToLower()) && !_words.Contains(word.ToLower());
}

public enum Team { Empty = 0, Player = 1, Enemy = 2 }
