using TMPro;
using UnityEngine;

public class AnimatedPanel : MonoBehaviour
{
    [SerializeField] private bool _visible = false;
    [SerializeField] private RectTransform _window = null;
    [SerializeField] private TMP_InputField _input = null;
    [SerializeField] private Road _road = null;

    public bool Visible
    {
        get => _visible;
        set
        {
            _visible = value;

            _window.gameObject.SetActive(_visible);
            if (Visible)
            {
                _input.text = string.Empty;
                _input.Select();
            }
        }
    }

    public void SetWord()
    {
        _road.Word = _input.text;
    }

    public void Submit()
    {
        if (!_road.Submit()) _input.Select();
    }
}
