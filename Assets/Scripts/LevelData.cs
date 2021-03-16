using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    public static LevelData Instance { get; private set; } = null;

    [SerializeField] private Road _road = null;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }

    private int _playerCrystals = 0;
    private int _botCrystals = 0;

    public void AddCrystal(CrystalBonus crystal)
    {
        switch (crystal.Team)
        {
            case Team.Empty:
                break;
            case Team.Player:
                _playerCrystals++;
                break;
            case Team.Enemy:
                _botCrystals++;
                break;
        }

        crystal.Delete();
    }

    public bool CanSpend(Team team)
    {
        switch (team)
        {
            case Team.Empty:
                return false;
            case Team.Player:
                return _playerCrystals > 0;
            case Team.Enemy:
                return _botCrystals > 0;
            default:
                return false;
        }
    }

    public void Spend(Team team)
    {
        if (!CanSpend(team)) return;
        switch (team)
        {
            case Team.Empty:
                break;
            case Team.Player:
                _playerCrystals--;
                break;
            case Team.Enemy:
                _botCrystals--;
                break;
        }
        _road._defaultLength++;
        _road._spiral.Count = Mathf.Max(_road._spiral.Count, _road._defaultLength);
    }
}