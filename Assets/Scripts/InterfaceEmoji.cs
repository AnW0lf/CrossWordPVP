using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class InterfaceEmoji : MonoBehaviour
{
    [SerializeField] private Image _icon = null;
    private RectTransform _rect = null;
    private Vector2 _size = Vector2.zero;
    private Emoji _data = null;
    public Emoji Data
    {
        get => _data;
        set
        {
            _data = value;
            if (_data != null)
                _icon.sprite = _data.Icon;
            else
                _icon.sprite = null;
        }
    }

    public bool CheckWord(string word)
    {
        if (Data != null) return Data.CheckWord(word);
        else return false;
    }

    public void Show()
    {
        StartCoroutine(Utils.CrossFading(Vector2.zero, _size, 0.4f, (size) => _rect.sizeDelta = size, (a, b, c) => Vector2.Lerp(a, b, c)));
    }

    public void Delete()
    {
        StartCoroutine(Utils.CrossFading(_size, Vector2.zero, 0.4f, (size) => _rect.sizeDelta = size, (a, b, c) => Vector2.Lerp(a, b, c)));
        StartCoroutine(Utils.DelayedCall(0.45f, () => Destroy(gameObject)));
    }

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _size = _rect.sizeDelta;
    }

    private void Start()
    {
        Show();
    }
}
