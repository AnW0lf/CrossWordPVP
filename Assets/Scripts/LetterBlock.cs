using TMPro;
using UnityEngine;

public class LetterBlock : MonoBehaviour
{
    [SerializeField] private Renderer _renderer = null;
    [SerializeField] private TextMeshPro _label = null;
    [SerializeField] private Material _emptyMaterial = null;
    [SerializeField] private Material _playerMaterial = null;
    [SerializeField] private Material _enemyMaterial = null;
    [SerializeField] private Material _winMaterial = null;

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

    public IBonus Bonus { get; set; } = null;

    public bool IsWinBlock { get; set; } = false;

    public bool Visible { get; private set; } = false;

    public void Show()
    {
        switch (team)
        {
            case Team.Empty:
                _renderer.material = _emptyMaterial;
                break;
            case Team.Player:
                _renderer.material = IsWinBlock ? _winMaterial : _playerMaterial;
                break;
            case Team.Enemy:
                _renderer.material = IsWinBlock ? _winMaterial : _enemyMaterial;
                break;
        }

        LetterColor = Color.black;

        Visible = true;

        if (Bonus != null)
        {
            Bonus.Perform(team);
        }
    }
}