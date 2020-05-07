﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public PlayerController controller;

    public float runspeed = 40f;

    float horizontalMove = 0f;
    bool jump = false;

    bool isBlocking = false;
    public HealthBar healthBar;

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runspeed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButton("Jump"))
        {
            jump = true;
            animator.SetBool("isJumping", true);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            animator.SetTrigger("isBlocking");
        }

        if (Input.GetButton("Fire2"))
        {
            isBlocking = true;
            //animator.SetTrigger("block");
            //animator.SetTrigger("isBlocking");
        }
        if (Input.GetButtonUp("Fire2"))
        {
            isBlocking = false;
            animator.SetTrigger("stopBlocking");
        }
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;
    }

    public void OnLanding()
    {
        animator.SetBool("isJumping", false);
    }

    protected override void Die()
    {
        base.Die();
        currentHealth = maxHealth;
        Invoke("Respawn", 0.5f);
    }

    protected override void Respawn()
    {
        base.Respawn();
        healthBar.SetHealth(maxHealth);
    }

    protected override void TakeDamage(int damage)
    {
        if (isBlocking == false)
        {
            base.TakeDamage(damage);
            healthBar.SetHealth(currentHealth);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Respawn"))
        {
            Debug.Log("Player fell");
            Die();
        }
    }
}