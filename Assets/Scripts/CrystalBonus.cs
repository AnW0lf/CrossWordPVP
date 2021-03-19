using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalBonus : MonoBehaviour, IBonus
{
    [SerializeField] private Transform _body = null;
    [SerializeField] private Transform _shadow = null;

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

    private bool _performed = false;

    public void Perform(Team team)
    {
        _performed = true;
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

        StartCoroutine(Utils.CrossFading(_shadow.localScale, Vector3.zero, 0.3f, (scale) => _shadow.localScale = scale, (a, b, c) => Vector3.Lerp(a, b, c)));
    }
    private IEnumerator UpDown()
    {
        if (_performed) yield break;
        yield return new WaitForSeconds(Random.Range(2f, 7f));
        if (_performed) yield break;
        StartCoroutine(Utils.CrossFading(_body.localPosition, _body.localPosition + Vector3.up * 0.1f, 1f, (pos) => _body.localPosition = pos, (a, b, c) => Vector3.Lerp(a, b, c)));
        yield return StartCoroutine(Utils.CrossFading(_shadow.localScale, _shadow.localScale - Vector3.one * 0.1f, 1f, (scale) => _shadow.localScale = scale, (a, b, c) => Vector3.Lerp(a, b, c)));
        if (_performed) yield break;
        StartCoroutine(Utils.CrossFading(_body.localPosition, _body.localPosition + Vector3.down * 0.1f, 1f, (pos) => _body.localPosition = pos, (a, b, c) => Vector3.Lerp(a, b, c)));
        yield return StartCoroutine(Utils.CrossFading(_shadow.localScale, _shadow.localScale + Vector3.one * 0.1f, 1f, (scale) => _shadow.localScale = scale, (a, b, c) => Vector3.Lerp(a, b, c)));
        if (_performed) yield break;
        yield return new WaitForSeconds(Random.Range(0f, 3f));
        StartCoroutine(UpDown());
    }

    private void Start()
    {
        StartCoroutine(UpDown());
    }
}
