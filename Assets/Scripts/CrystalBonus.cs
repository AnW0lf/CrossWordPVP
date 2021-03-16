using UnityEngine;

public class CrystalBonus : MonoBehaviour, IBonus
{
    public Team Team { get; set; } = Team.Empty;
    public Road Road { get; set; } = null;

    private bool _deleted = false;
    public void Delete()
    {
        if (_deleted) return;
        _deleted = true;
        StartCoroutine(Utils.CrossFading(Vector3.one, Vector3.zero, 0.2f, (scale) => transform.localScale = scale, (a, b, c) => Vector3.Lerp(a, b, c)));
        StartCoroutine(Utils.DelayedCall(0.3f, () => Destroy(gameObject)));
    }

    public void Perform(Team team)
    {
        Team = team;
        if (Team == Team.Player)
        {
            var crystalCounter = FindObjectOfType<CrystalCounter>();
            StartCoroutine(Utils.CrossFading(transform.position, crystalCounter.transform.position, 0.5f, (pos) => transform.position = pos, (a, b, c) => Vector3.Lerp(a, b, c)));
            StartCoroutine(Utils.DelayedCall(0.3f, Delete));
            StartCoroutine(Utils.DelayedCall(0.5f, () => LevelData.Instance.AddCrystal(this)));
        }
        else
        {
            Delete();
            LevelData.Instance.AddCrystal(this);
        }
    }
}
