using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSpiral : MonoBehaviour
{
    [Header("Spiral")]
    [SerializeField] private GameObject _prefab = null;
    [SerializeField] private int _count = 0;
    [SerializeField] private int _offset = 0;
    [SerializeField] private float _a = 0f;
    [SerializeField] private float _b = 0f;
    [SerializeField] private float _step = 0f;
    [SerializeField] private float _smoothness = 0.05f;
    [Header("Road")]
    [SerializeField] private Road _road;

    private GameObject[] _objects = new GameObject[0];

    public int Count
    {
        get => _count;
        set
        {
            _count = value;
            if (_objects.Length < _count)
            {
                GameObject[] newObjects = new GameObject[_count];
                for (int i = 0; i < _count; i++)
                {
                    if (i < _objects.Length) newObjects[i] = _objects[i];
                    else newObjects[i] = Instantiate(_prefab, transform);
                }
                _objects = newObjects;
            }
            else if (_objects.Length > _count)
            {
                GameObject[] newObjects = new GameObject[_count];
                for (int i = 0; i < _objects.Length; i++)
                {
                    if (i < newObjects.Length) newObjects[i] = _objects[i];
                    else Destroy(_objects[i]);
                }
                _objects = newObjects;
            }
            _road.SetBlocks(_objects);
        }
    }

    private void Awake()
    {
        Count = Count;

        for (int i = 0; i < _count; i++)
        {
            int k = _count - 1 - i;
            Transform t = _objects[i].transform;
            float sigma = 2f * Mathf.PI * Mathf.Sqrt(2f * _step * (k + _offset) / _b);
            t.localPosition = new Vector3((_a + _b * sigma) * Mathf.Cos(sigma), 0f, (_a + _b * sigma) * Mathf.Sin(sigma));
            t.LookAt(t.position + (t.position - transform.position) * 1000f);
        }
    }

    private void Update()
    {
        for (int i = 0; i < _count; i++)
        {
            int k = _count - 1 - i;
            Transform t = _objects[i].transform;
            float sigma = 2f * Mathf.PI * Mathf.Sqrt(2f * _step * (k + _offset) / _b);

            Vector3 newLocalPosition = new Vector3((_a + _b * sigma) * Mathf.Cos(sigma), 0f, (_a + _b * sigma) * Mathf.Sin(sigma));

            t.localPosition = Vector3.Lerp(t.localPosition, newLocalPosition, _smoothness);
            t.LookAt(t.position + (t.position - transform.position) * 1000f);
        }
    }
}
