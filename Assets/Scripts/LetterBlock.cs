using TMPro;
using UnityEngine;

public class LetterBlock : MonoBehaviour
{
    [SerializeField] private Renderer _renderer = null;
    [SerializeField] private TextMeshPro _label = null;
    [SerializeField] private GameObject _star = null;
    [SerializeField] private Material _emptyMaterial = null;
    [SerializeField] private Material _playerMaterial = null;
    [SerializeField] private Material _enemyMaterial = null;
    //[SerializeField] private Material _winMaterial = null;

    public Team team = Team.Empty;
    public string Letter
    {
        get => _label.text;
        set
        {
            _label.text = value.ToUpper();
            if (Letter != string.Empty)
            {
                _star.SetActive(false);
            }
            else if (IsWinBlock)
            {
                _star.SetActive(true);
            }
        }
    }

    public Color LetterColor
    {
        get => _label.color;
        set => _label.color = value;
    }

    public IBonus Bonus { get; set; } = null;

    private bool _isWinBlock = false;
    public bool IsWinBlock
    {
        get => _isWinBlock;
        set
        {
            _isWinBlock = value;
            _star.SetActive(_isWinBlock && _label.text == string.Empty);
        }
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
                //_renderer.material = IsWinBlock ? _winMaterial : _playerMaterial;
                _renderer.material = _playerMaterial;
                break;
            case Team.Enemy:
                //_renderer.material = IsWinBlock ? _winMaterial : _enemyMaterial;
                _renderer.material = _enemyMaterial;
                break;
        }

        LetterColor = new Color(1f, 1f, 0f);

        Visible = true;

        if (Bonus != null)
        {
            Bonus.Perform(team);
        }
    }
}