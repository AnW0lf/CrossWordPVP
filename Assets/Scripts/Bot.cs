using System.Linq;
using UnityEngine;

public class Bot : MonoBehaviour
{
    [SerializeField] private Road _road = null;


    public string Word
    {
        get
        {
            string[] words = _road.dictionary.Where((word) => !_road._words.Contains(word)).ToArray();
            return words[Random.Range(0, words.Length)];
        }
    }


    public void Begin()
    {
        StartCoroutine(Utils.DelayedCall(Random.Range(1.5f, 3f), () => _road.Submit()));
    }
}
