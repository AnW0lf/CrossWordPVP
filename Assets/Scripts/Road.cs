using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Road : MonoBehaviour
{
    [SerializeField] private AnimatedPanel _playerPanel = null;
    [SerializeField] private Bot _bot = null;
    [SerializeField] public Dictionary _dictionary = null;
    [SerializeField] public DrawSpiral _spiral = null;
    [SerializeField] public int _defaultLength = 25;

    [Header("Final")]
    [SerializeField] public TextMeshProUGUI _finalLabel = null;
    [SerializeField] public GameObject _reloadSceneButton = null;

    [Header("Crystals")]
    [SerializeField] private GameObject _crystalPrefab = null;
    [SerializeField] private int _crystalCount = 8;
    [SerializeField] private int _startOffset = 3;
    [SerializeField] private int _endOffset = 3;

    [HideInInspector] public List<string> _words = new List<string>();

    public Team Round { get; private set; } = Team.Player;
    private LetterBlock[] _blocks = null;

    private int ChainLength
    {
        get
        {
            int length = 0;
            foreach (var word in _words) length += word.Length;
            return length;
        }
    }

    //private bool _submitted = false;

    public void SetBlocks(GameObject[] objects)
    {
        _blocks = objects.Select((o) => o.GetComponent<LetterBlock>()).ToArray();
        foreach (var block in _blocks) block.IsWinBlock = false;
        _blocks[_blocks.Length - 1].IsWinBlock = true;
    }

    public Transform LastLetterBlock
    {
        get
        {
            //if (_submitted)
            //{
            //    if (!_blocks[0].Visible) return _blocks[0].transform;

            //    for (int i = 0; i < _blocks.Length; i++)
            //        if (!_blocks[i].Visible) return _blocks[i - 1].transform;

            //    return _blocks[_blocks.Length - 1].transform;
            //}
            //else
            //{
            if (_blocks[0].Letter == string.Empty) return _blocks[0].transform;

            for (int i = 0; i < _blocks.Length; i++)
                if (_blocks[i].Letter == string.Empty) return _blocks[i - 1].transform;

            return _blocks[_blocks.Length - 1].transform;
            //}
        }
    }

    private void Start()
    {
        _spiral.Count = _defaultLength;
        StartCoroutine(Utils.DelayedCall(1f, () => BeginRound(Round)));

        List<LetterBlock> blocks = new List<LetterBlock>();
        for (int i = _startOffset; i < _defaultLength - _endOffset; i++)
            blocks.Add(_blocks[i]);

        for(int i = 0; i < _crystalCount && blocks.Count > 0; i++)
        {
            int index = Random.Range(0, blocks.Count);
            CrystalBonus crystal = Instantiate(_crystalPrefab, blocks[index].transform).GetComponent<CrystalBonus>();
            crystal.Road = this;
            blocks[index].Bonus = crystal;
            blocks.RemoveAt(index);
        }
    }

    public void NextRound()
    {
        if (Round == Team.Player) Round = Team.Enemy;
        else Round = Team.Player;
    }

    private void BeginRound(Team round)
    {
        if (Round == Team.Player)
        {
            _playerPanel.Visible = true;
        }
        else
        {
            _bot.Begin();
        }
    }

    private string _inputWord = string.Empty;
    public string Word
    {
        get => _inputWord;
        set
        {
            _inputWord = value;
            int chainLength = ChainLength;

            _spiral.Count = Mathf.Max(_defaultLength, chainLength + Word.Length);

            for (int i = chainLength; i < _blocks.Length; i++)
            {
                int index = i - chainLength;
                LetterBlock block = _blocks[i];
                if (index < Word.Length)
                    block.Letter = Word[index].ToString();
                else
                    block.Letter = string.Empty;
                block.IsWinBlock = i >= _defaultLength - 1;
            }
        }
    }

    public bool Submit()
    {
        string word = Word;
        _inputWord = string.Empty;
        if (Round == Team.Player)
        {
            if (!CheckWord(word))
            {
                Color normalColor = _blocks[_blocks.Length - 1].LetterColor;

                foreach (var block in _blocks.Where((b) => b.team == Team.Empty))
                    block.LetterColor = Color.red;

                StartCoroutine(Utils.DelayedCall(0.2f,
                    () =>
                    {
                        foreach (var block in _blocks.Where((b) => b.team == Team.Empty))
                            block.LetterColor = normalColor;
                    }));

                return false;
            }
            _playerPanel.Visible = false;
        }
        else
        {
            if (!CheckWord(word)) return false;
        }

        //_submitted = true;
        //StartCoroutine(Utils.DelayedCall(word.Length * 0.1f + 0.8f, () => _submitted = false));

        int oldChainLength = ChainLength;
        _words.Add(word.ToLower());
        int chainLength = ChainLength;

        if (chainLength > _blocks.Length) _spiral.Count = chainLength;

        for (int i = oldChainLength; i < chainLength && i < _blocks.Length; i++)
        {
            int index = i - oldChainLength;
            LetterBlock block = _blocks[i];
            block.Letter = word[index].ToString();
            block.team = Round;

            StartCoroutine(Utils.DelayedCall(0.4f + 0.1f * index, () => block.Show()));
        }

        if (CheckWin())
        {
            StartCoroutine(Utils.DelayedCall(2f, () => _reloadSceneButton.SetActive(true)));

            if (_blocks[_blocks.Length - 1].team == Team.Player) _finalLabel.text = "VICTORY";
            else _finalLabel.text = "DEFEAT";
        }
        else
        {
            StartCoroutine(Utils.DelayedCall((chainLength - oldChainLength) * 0.1f + 1f, () =>
            {
                NextRound();
                BeginRound(Round);
            }));
        }

        return true;
    }

    private bool CheckWin() => _blocks[_blocks.Length - 1].Letter != string.Empty;

    private bool CheckWord(string word) => _dictionary.CheckWord(word);

    public void ReloadScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}

public enum Team { Empty = 0, Player = 1, Enemy = 2 }
