using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSpiral : MonoBehaviour
{
    [SerializeField] private GameObject _prefab = null;
    [SerializeField] private int _count = 54;
    [SerializeField] private int _offset = 0;
    [SerializeField] private float _a = 0f;
    [SerializeField] private float _b = 0f;
    [SerializeField] private float _step = 0f;

    private GameObject[] _objects = null;

    private void Start()
    {
        _objects = new GameObject[_count];
        for (int i = 0; i < _count; i++)
            _objects[i] = Instantiate(_prefab, transform);
    }

    private void Update()
    {
        for(int i = 0; i < _count; i++)
        {
            Transform t = _objects[i].transform;
            float sigma = 2f * Mathf.PI * Mathf.Sqrt(2f * _step * (i + _offset) / _b);
            t.localPosition = new Vector3((_a + _b * sigma) * Mathf.Cos(sigma), 0f, (_a + _b * sigma) * Mathf.Sin(sigma));
            t.LookAt(t.position + (t.position - transform.position) * 1000f);
        }
    }
}
