using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{
    [SerializeField] private Road _road = null;

    private List<string> _dictionary = null;

    public string Word => _dictionary[Random.Range(0, _dictionary.Count)];

    private void Awake()
    {
        _dictionary = _road.Dictionary;
    }

    public void Begin()
    {
        StartCoroutine(Utils.DelayedCall(Random.Range(1.5f, 3f), () => _road.Submit()));
    }
}
