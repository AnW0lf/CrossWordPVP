using System.Linq;
using UnityEngine;

public class Bot : MonoBehaviour
{
    [SerializeField] private Road _road = null;

    private float _chance = 0f;

    private void Start()
    {
        _chance = Random.Range(0f, 0.3f);
    }

    public void Begin()
    {
        string word = string.Empty;

        if(Random.Range(0f, 1f) > _chance)
        {
            word = _road._dictionary.GetDefaultWord();
        }
        else
        {
            word = _road._dictionary.GetRandomWord();
        }

        float delay = Random.Range(1.5f, 3f);
        StartCoroutine(Utils.CrossFading(string.Empty, word, delay, (str) => _road.Word = str,
            (a, b, c) =>
            {
                int length = Mathf.RoundToInt(Mathf.Lerp(0f, (float) word.Length, c));
                return word.Substring(0, length);
            }));


        StartCoroutine(Utils.DelayedCall(delay + 0.5f, () => _road.Submit()));
    }
}
