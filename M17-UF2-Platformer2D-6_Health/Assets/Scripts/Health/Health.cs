using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    private bool dead = false;
    public float currentHealth { get; private set; }
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float damage)
    {
        StartCoroutine(FlashRed());
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);
        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
        }
        else
        {
            if (!dead)
            {
                anim.SetTrigger("die");
                GetComponent<PlayerMovement>().enabled = false;
                dead = true;
            }
        }
    }

    private IEnumerator FlashRed()
    {
        float flashDuration = 1.5f;
        float flashSpeed = 0.1f;

        int numberOfFlashes = Mathf.FloorToInt(flashDuration / (2 * flashSpeed));

        Physics2D.IgnoreLayerCollision(gameObject.layer, 8, true);

        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(flashSpeed);

            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(flashSpeed);
        }

        spriteRenderer.color = Color.white;

        Physics2D.IgnoreLayerCollision(gameObject.layer, 8, false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeDamage(1);
        }
    }

    public void AddHealth(float value)
    {
        currentHealth = Mathf.Clamp(currentHealth + value, 0, startingHealth);
    }
}
