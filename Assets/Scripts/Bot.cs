using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{
    [SerializeField] private Road _road = null;

    public void Begin()
    {
        //while (LevelData.Instance.CanSpend(Team.Enemy) && Random.Range(0f, 1f) > 0.5f)
        //{
        //    LevelData.Instance.Spend(Team.Enemy);
        //}

        StartCoroutine(ChooseWord());
    }

    private IEnumerator ChooseWord()
    {
        List<InterfaceEmoji> emojies = _road._dictionary.Emojies;
        int i = 0;
        while (true)
        {
            i++;
            int index = Random.Range(0, emojies.Count);
            if (i > 4)
            {
                emojies[index].Cast();
                yield break;
            }

            if (Random.Range(0f, 1f) > 0.5f)
            {
                emojies[index].Cast();
                yield break;
            }
            else
            {
                float delay = Random.Range(0.8f, 2.3f);
                emojies[index].View(delay);
                yield return new WaitForSeconds(delay + Random.Range(0.2f, 0.5f));
            }
        }
    }
}
