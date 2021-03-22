using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinPrizeCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _label = null;

    void Update()
    {
        _label.text = $"Victory Prize: <color=#ffff00><size=100>X{LevelData.Instance.PlayerCrystals + 1}</size></color>";
    }
}
