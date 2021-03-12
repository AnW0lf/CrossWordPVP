using TMPro;
using UnityEngine;

public class LetterBlock : MonoBehaviour
{
    [SerializeField] private Renderer _renderer = null;
    [SerializeField] private TextMeshPro _label = null;
    [SerializeField] private Material _emptyMaterial = null;
    [SerializeField] private Material _playerMaterial = null;
    [SerializeField] private Material _enemyMaterial = null;

    public Team team = Team.Empty;
    public string Letter
    {
        get => _label.text;
        set => _label.text = value.ToUpper();
    }

    public Color LetterColor
    {
        get => _label.color;
        set => _label.color = value;
    }

    public bool Visible { get; private set; } = false;

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

        LetterColor = Color.black;

        Visible = true;
    }
}