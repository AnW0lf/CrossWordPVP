using UnityEngine;
using UnityEngine.Events;

public class LevelData : MonoBehaviour
{
    public static LevelData Instance { get; private set; } = null;

    [SerializeField] private Road _road = null;

    public UnityAction<Team, int> OnCrystalCountChanged { get; set; } = null;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }

    public int PlayerCrystals { get; private set; } = 0;
    public int BotCrystals { get; private set; } = 0;

    public void AddCrystal(CrystalBonus crystal)
    {
        switch (crystal.Team)
        {
            case Team.Empty:
                return;
            case Team.Player:
                PlayerCrystals++;
                break;
            case Team.Enemy:
                BotCrystals++;
                break;
        }

        print($"Player {PlayerCrystals} Bot {BotCrystals}");

        OnCrystalCountChanged?.Invoke(crystal.Team, crystal.Team == Team.Player ? PlayerCrystals : BotCrystals);
    }

    public bool CanSpend(Team team)
    {
        switch (team)
        {
            case Team.Empty:
                return false;
            case Team.Player:
                return PlayerCrystals > 0;
            case Team.Enemy:
                return BotCrystals > 0;
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
                return;
            case Team.Player:
                PlayerCrystals--;
                break;
            case Team.Enemy:
                BotCrystals--;
                break;
        }

        OnCrystalCountChanged?.Invoke(team, team == Team.Player ? PlayerCrystals : BotCrystals);

        _road._defaultLength++;
        _road._spiral.Count = Mathf.Max(_road._spiral.Count, _road._defaultLength);
    }
}