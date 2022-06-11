using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] int coinValue = 100;
    [SerializeField] AudioClip coinPickupSFX;
    bool wasCollected = false;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !wasCollected)
        {
            wasCollected = true;
            AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position);
            FindObjectOfType<GameSession>().AddToScore(coinValue);
            Debug.Log("Coin Collected");
            Destroy(gameObject);
        }
    }


}
