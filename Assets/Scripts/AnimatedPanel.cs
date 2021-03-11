using TMPro;
using UnityEngine;

public class AnimatedPanel : MonoBehaviour
{
    [SerializeField] private bool _visible = false;
    [SerializeField] private RectTransform _window = null;
    [SerializeField] private TMP_InputField _input = null;

    public bool Visible
    {
        get => _visible;
        set
        {
            _visible = value;

            if (Visible)
            {
                _window.gameObject.SetActive(true);

                StartCoroutine(Utils.CrossFading(Vector3.down * 2000f, Vector3.zero, 0.15f, (pos) => _window.anchoredPosition = pos, (a, b, c) => Vector3.Lerp(a, b, c)));

                _input.text = string.Empty;
                _input.Select();
            }
            else
            {
                StartCoroutine(Utils.CrossFading(Vector3.zero, Vector3.down * 2000f, 0.15f, (pos) => _window.anchoredPosition = pos, (a, b, c) => Vector3.Lerp(a, b, c)));

                StartCoroutine(Utils.DelayedCall(0.2f, () => _window.gameObject.SetActive(false)));
            }
        }
    }

    public string Word => _input.text;
}
