using System.Linq;
using UnityEngine;

public class Bot : MonoBehaviour
{
    [SerializeField] private Road _road = null;


    public string Word => _road._dictionary.GetRandomWord();


    public void Begin()
    {
        string word = Word;
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
