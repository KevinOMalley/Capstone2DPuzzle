using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    public int respawn;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
    }


    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        
        if(currentHealth > 0 )
        {
            // player hurt
            anim.SetTrigger("hurt");
        }
        else
        {
            // player dead
            anim.SetTrigger("die");

            //Needs to pause for 0.45 seconds here. It's the duration of the death animation.
            //I'll probably have to make it longer and lock the player or something

            SceneManager.LoadScene(respawn);
        }
    }


}
