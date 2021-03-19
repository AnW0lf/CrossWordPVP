using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTurnPanel : MonoBehaviour
{
    [SerializeField] private Direction _direction = Direction.RIGHT;
    [SerializeField] private Team _team = Team.Empty;
    [SerializeField] private Road _road = null;

    private bool _visible = false;
    private Coroutine _moving = null;
    public bool Visible
    {
        get => _visible;
        private set
        {
            _visible = value;

            if (_moving != null) StopCoroutine(_moving);

            Vector2 endPosition = _visible ? Vector2.zero : Vector2.right * _rect.sizeDelta.x * (_direction == Direction.RIGHT ? 1f : -1f);

            _moving = StartCoroutine(Utils.CrossFading(_rect.anchoredPosition, endPosition, 0.3f, (pos) => _rect.anchoredPosition = pos, (a, b, c) => Vector2.Lerp(a, b, c)));
        }
    }

    private RectTransform _rect = null;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        Visible = Visible;
    }

    private void Update()
    {
        if (_road.Round == _team && !Visible) Visible = true;
        else if (_road.Round != _team && Visible) Visible = false;
    }
}

public enum Direction { RIGHT, LEFT }