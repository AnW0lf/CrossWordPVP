using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RewardCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _counter = null;
    [SerializeField] private RectTransform _icon = null;

    public Vector3 IconPosition => _icon.transform.position + Vector3.left;

    private int _count = 0;
    public int Count
    {
        get => _count;
        set
        {
            _count = value;
            _counter.text = $"+{_count}";
        }
    }

    private void OnEnable()
    {
        StartCoroutine(Utils.CrossFading(_counter.rectTransform.anchoredPosition, Vector2.zero, 0.25f, (pos) => _counter.rectTransform.anchoredPosition = pos, (a, b, c) => Vector2.Lerp(a, b, c)));
        StartCoroutine(Utils.CrossFading(_icon.anchoredPosition, Vector2.right * 50f, 0.25f, (pos) => _icon.anchoredPosition = pos, (a, b, c) => Vector2.Lerp(a, b, c)));
    }
}
