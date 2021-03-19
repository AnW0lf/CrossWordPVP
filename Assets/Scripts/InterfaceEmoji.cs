using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class InterfaceEmoji : MonoBehaviour, IDragHandler, IBeginDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image _icon = null;
    [SerializeField] private Image _outline = null;
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
            {
                _icon.sprite = _data.Icon;
                _outline.sprite = _data.Icon;

            }
            else
            {
                _icon.sprite = null;
                _outline.sprite = null;
            }
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

    private Road _road = null;

    private void Start()
    {
        Show();
        _road = FindObjectOfType<Road>();
    }

    private void Update()
    {
        float y = _outline.rectTransform.anchoredPosition.y;

        Image[] images = GetComponentsInChildren<Image>();
        foreach (var image in images)
        {
            Color color = image.color;
            color.a = Mathf.Lerp(1f, 0f, Mathf.Clamp(y / 600f, 0f, 1f));
            image.color = color;
        }
    }

    private Coroutine _moving = null;

    Vector2 _startPos = Vector2.zero;


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_road.Round != Team.Player) return;

        if (_moving != null) StopCoroutine(_moving);

        _startPos = Input.mousePosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_road.Round != Team.Player) return;

        float y = Mathf.Max(0f, Input.mousePosition.y - _startPos.y) / Screen.height * 2160f;

        Vector2 position = _outline.rectTransform.anchoredPosition;
        position.y = y;
        _outline.rectTransform.anchoredPosition = position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_road.Round != Team.Player) return;

        if (_outline.rectTransform.anchoredPosition.y > 200f)
        {
            _road.Submit();
        }
        else
        {
            _road.Word = string.Empty;
            float duration = Vector2.Distance(Vector2.zero, _outline.rectTransform.anchoredPosition) / 1000f;
            _moving = StartCoroutine(Utils.CrossFading(_outline.rectTransform.anchoredPosition, Vector2.zero, duration,
                (pos) => _outline.rectTransform.anchoredPosition = pos, (a, b, c) => Vector2.Lerp(a, b, c)));
        }

        _outline.enabled = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_road.Round != Team.Player) return;

        _outline.enabled = true;
        _road.Word = _data.Words[0];
    }

    public void View(float delay)
    {
        _outline.enabled = true;
        _road.Word = _data.Words[0];

        StartCoroutine(Utils.DelayedCall(delay,
            () =>
            {
                _outline.enabled = false;
                _road.Word = string.Empty;
            }));
    }

    public void Cast()
    {
        _outline.enabled = true;
        _road.Word = _data.Words[0];

        float delay = Random.Range(0.3f, 0.7f);
        Vector2 startPos = _outline.rectTransform.anchoredPosition;
        Vector2 endPos = startPos + Vector2.up * 400f;
        float duration = Random.Range(0.3f, 0.8f);


        StartCoroutine(Utils.DelayedCrossFading(delay, startPos, endPos, duration, (pos) => _outline.rectTransform.anchoredPosition = pos, (a, b, c) => Vector2.Lerp(a, b, c)));
        StartCoroutine(Utils.DelayedCall(delay + duration, () => _road.Submit()));
    }
}
