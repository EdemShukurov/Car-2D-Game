﻿using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<CarTrigger>(out CarTrigger carTrigger))
        {
            carTrigger.AddCoin();
            Destroy(gameObject);
        }
    }
}