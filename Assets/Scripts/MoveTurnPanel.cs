using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveTurnPanel : MonoBehaviour
{
    [SerializeField] private Direction _direction = Direction.RIGHT;
    [SerializeField] private Team _team = Team.Empty;
    [SerializeField] private Road _road = null;
    [SerializeField] private Slider _timer = null;

    private bool _visible = false;
    private Coroutine _moving = null;
    private float _time = 10f;

    public bool Visible
    {
        get => _visible;
        private set
        {
            _visible = value;

            if (_moving != null) StopCoroutine(_moving);

            Vector2 endPosition = _visible ? Vector2.zero : Vector2.right * _rect.sizeDelta.x * (_direction == Direction.RIGHT ? 1f : -1f);

            _moving = StartCoroutine(Utils.CrossFading(_rect.anchoredPosition, endPosition, 0.3f, (pos) => _rect.anchoredPosition = pos, (a, b, c) => Vector2.Lerp(a, b, c)));

            if (Visible) _timer.value = _time;
        }
    }

    private RectTransform _rect = null;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        Visible = Visible;

        _timer.maxValue = _time;
    }

    private void Update()
    {
        if (Visible)
        {
            if (_road.IsLevelEnd || _road.Round != _team || _road.Submitted) Visible = false;

            if (!_road.Submitted && !_road.IsLevelEnd) _timer.value -= Time.deltaTime;
        }
        else
        {
            if (!_road.IsLevelEnd && _road.Round == _team && !_road.Submitted) Visible = true;
        }

    }
}

public enum Direction { RIGHT, LEFT }