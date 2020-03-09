using UnityEngine;
using System;
using TMPro;

public class Wallet : MonoBehaviour
{
    public event Action Changed;

    public TextMeshProUGUI text;

    private int _amount;

    public void AddCoin()
    {
        _amount++;
        text.text = string.Format("X {0}", _amount);
        Changed?.Invoke();
    }
}


