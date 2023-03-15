using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.GameFoundation;
//using UnityEngine.GameFoundation.Components;

public class CoinPickup : MonoBehaviour
{

    private Currency m_CoinDefinition;
    
    // Quantity of coins to add when the Player moves onto a dropped Coin
    public const int find_quantity = 1;

    private void Start()
    {
        // Get the Currency definition for the Coin
        m_CoinDefinition = GameFoundationSdk.catalog.Find<Currency>("goldCoin");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object is the Player
        if (collision.CompareTag("Player"))
        {
            // Add the coin to the Player's wallet
            GameFoundationSdk.wallet.Add(m_CoinDefinition, find_quantity);
            //IWalletManager.Add(goldCoin, 1);

            // Destroy the Coin GameObject
            Destroy(gameObject);
        }
    }
}
