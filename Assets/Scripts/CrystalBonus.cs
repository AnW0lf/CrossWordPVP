using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalBonus : MonoBehaviour, IBonus
{
    public Team Team { get; set; } = Team.Empty;

    private bool _deleted = false;
    public void Delete()
    {
        if (_deleted) return;
        _deleted = true;
        StartCoroutine(Utils.CrossFading(Vector3.one, Vector3.zero, 0.4f, (scale) => transform.localScale = scale, (a, b, c) => Vector3.Lerp(a, b, c)));
        StartCoroutine(Utils.DelayedCall(0.5f, () => Destroy(gameObject)));
    }

    public void Perform(Team team)
    {
        Team = team;
        LevelData.Instance.AddCrystal(this);
    }
}
