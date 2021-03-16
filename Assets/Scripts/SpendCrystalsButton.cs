using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class SpendCrystalsButton : MonoBehaviour
{
    [SerializeField] private Button _button = null;
    [SerializeField] private Road _road = null;

    private RectTransform _rect = null;

    private bool _visibe = false;
    private Coroutine _moving = null;
    public bool Visible
    {
        get => _visibe;
        private set
        {
            _visibe = value;

            if (_moving != null) StopCoroutine(_moving);
            _moving = StartCoroutine(Utils.CrossFading(_rect.anchoredPosition, Visible ? Vector2.zero : Vector2.right * 1000f, 0.4f,
                                                       (pos) => _rect.anchoredPosition = pos, (a, b, c) => Vector2.Lerp(a, b, c)));
        }
    }


    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }

    private void Subscribe()
    {
        LevelData.Instance.OnCrystalCountChanged += CrystalCountChanged;
    }

    private void Unsubscribe()
    {
        LevelData.Instance.OnCrystalCountChanged -= CrystalCountChanged;
    }

    private void CrystalCountChanged(Team team, int count)
    {
        _button.interactable = LevelData.Instance.CanSpend(Team.Player);
    }

    public void Spend()
    {
        LevelData.Instance.Spend(Team.Player);
    }

    private void Start()
    {
        _button.interactable = LevelData.Instance.CanSpend(Team.Player);
        Subscribe();

        Visible = _road.Round == Team.Player;
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    private void Update()
    {
        if(_road.Round == Team.Player)
        {
            if (!Visible) Visible = true;
        }
        else
        {
            if (Visible) Visible = false;
        }
    }
}
