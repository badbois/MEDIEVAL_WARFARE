using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.GameFoundation;

public class CoinPickup : MonoBehaviour
{
    private Currency m_CoinDefinition;
    public int coin_value = 1; // The value of the coin

    void Start()
    {
        // Get the Currency definition for the Coin
        m_CoinDefinition = GameFoundationSdk.catalog.Find<Currency>("goldCoin");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the Player moves onto the Coin
        if (collision.CompareTag("Player"))
        {
            GameFoundationSdk.wallet.Add(m_CoinDefinition, coin_value); // Add the coin to the Player's wallet
            transform.parent.destroy();
        }
    }
}
