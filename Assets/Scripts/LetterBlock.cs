using TMPro;
using UnityEngine;

public class LetterBlock : MonoBehaviour
{
    [SerializeField] private Renderer _renderer = null;
    [SerializeField] private TextMeshPro _label = null;
    [SerializeField] private GameObject _star = null;
    [SerializeField] private Material _emptyMaterial = null;
    [SerializeField] private Material _playerMaterial = null;
    [SerializeField] private Material _enemyMaterial = null;
    [SerializeField] private Material _winMaterial = null;
    [SerializeField] private GameObject _confettiPrefab = null;
    [SerializeField] private Transform _coin = null;
    [SerializeField] private Transform _body = null;

    public Team team = Team.Empty;
    public string Letter
    {
        get => _label.text;
        set
        {
            _label.text = value.ToUpper();
            if (Letter != string.Empty)
            {
                _star.SetActive(false);
            }
            else if (IsWinBlock)
            {
                _star.SetActive(true);
            }
        }
    }

    public Color LetterColor
    {
        get => _label.color;
        set => _label.color = value;
    }

    public IBonus Bonus { get; set; } = null;

    private bool _isWinBlock = false;
    public bool IsWinBlock
    {
        get => _isWinBlock;
        set
        {
            _isWinBlock = value;
            _star.SetActive(_isWinBlock && _label.text == string.Empty);

            if (!Visible)
            {
                if (IsWinBlock)
                    _renderer.material = _winMaterial;
                else
                    _renderer.material = _emptyMaterial;
            }
        }
    }

    public bool Visible { get; private set; } = false;

    private static bool effectVisible = false;

    private void Start()
    {
        StartCoroutine(Utils.CrossFading(Vector3.zero, Vector3.one, 0.3f,
            (scale) => transform.localScale = scale, (a, b, c) => Vector3.Lerp(a, b, c)));
    }

    public void Show()
    {
        switch (team)
        {
            case Team.Empty:
                _renderer.material = _emptyMaterial;
                break;
            case Team.Player:
                _renderer.material = _playerMaterial;
                break;
            case Team.Enemy:
                _renderer.material = _enemyMaterial;
                break;
        }

        LetterColor = new Color(1f, 0.82f, 0f);

        Visible = true;

        if (Bonus != null)
        {
            Bonus.Perform(team);
        }

        if (IsWinBlock && !effectVisible)
        {
            effectVisible = true;
            Instantiate(_confettiPrefab, transform).transform.position = transform.position;
        }
    }

    public void FlyTo(RewardCounter counter)
    {
        _coin.gameObject.SetActive(true);
        _coin.LookAt(Camera.main.transform);

        float duration = Vector3.Distance(_coin.position, counter.IconPosition) / 20f;

        StartCoroutine(Utils.DelayedCrossFading(0.2f, _coin.position, counter.IconPosition, duration,
            (pos) =>
            {
                _coin.position = pos;
                _coin.LookAt(Camera.main.transform);
            }, (a, b, c) => Vector3.Lerp(a, b, c)));

        StartCoroutine(Utils.DelayedCrossFading(0.2f, _body.position, _body.position + Vector3.down * 10f, 3f,
            (pos) => _body.position = pos, (a, b, c) => Vector3.Lerp(a, b, c)));

        StartCoroutine(Utils.DelayedCrossFading(0.2f, _label.color.a, 0f, 0.3f,
            (alpha) => {
                Color color = _label.color;
                color.a = alpha;
                _label.color = color;
            }, (a, b, c) => Mathf.Lerp(a, b, c)));

        StartCoroutine(Utils.DelayedCrossFading(duration, _coin.localScale, Vector3.zero, 0.2f, (scale) => _coin.localScale = scale, (a, b, c) => Vector3.Lerp(a, b, c)));

        StartCoroutine(Utils.DelayedCall(0.3f + duration,
            () =>
            {
                _coin.gameObject.SetActive(false);
                counter.Count++;
            }));
    }
}