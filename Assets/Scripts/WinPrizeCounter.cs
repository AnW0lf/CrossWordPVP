using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinPrizeCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _label = null;

    void Update()
    {
        _label.text = $"Win prize x{LevelData.Instance.PlayerCrystals + 1}";
    }
}
