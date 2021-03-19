using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CrystalCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _counter = null;

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
        _counter.text = $"{LevelData.Instance.PlayerCrystals}<sprite=0>";
    }

    private void Start()
    {
        Subscribe();

        _counter.text = $"{LevelData.Instance.PlayerCrystals}<sprite=0>";
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    public void Hide()
    {
        var _rect = GetComponent<RectTransform>();
        StartCoroutine(Utils.CrossFading(_rect.anchoredPosition, _rect.anchoredPosition + Vector2.left * 1000f, 1f, (pos) => _rect.anchoredPosition = pos, (a, b, c) => Vector2.Lerp(a, b, c)));
    }
}
