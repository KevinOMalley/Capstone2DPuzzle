using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class JetpackCollectible : MonoBehaviour
{
    [SerializeField] private float usesValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<PlayerMovement>().AddJetpackUses(usesValue);
            gameObject.SetActive(false);
        }
    }
}
