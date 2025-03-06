using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorController : MonoBehaviour
{
    [Header("Movimiento")]
    public float Speed = 5f;
    public float JumpForce = 10f;
    private Rigidbody2D rb;
    private Animator animator;
    private float horizontal;
    private bool grounded;
    private bool atacando;

    [Header("Salud")]
    public int maxHealth = 20;
    private int currentHealth;
    private bool isDead = false; // Bandera para saber si el Warrior está muerto

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        if (rb == null)
            Debug.LogError("El componente Rigidbody2D no está asignado en el Warrior.");
        if (animator == null)
            Debug.LogError("El componente Animator no está asignado en el Warrior.");
    }

    void Update()
    {
        if (isDead) return; // No se procesa nada si el Warrior ha muerto

        // Movimiento Horizontal
        horizontal = Input.GetAxisRaw("Horizontal");

        // Ajustamos la dirección dependiendo de donde nos movamos
        if (horizontal < 0f)
            transform.localScale = new Vector3(-1f, 1f, 1f);
        else if (horizontal > 0f)
            transform.localScale = new Vector3(1f, 1f, 1f);

        // Actualizamos las Animaciones
        animator.SetBool("Running", horizontal != 0f);
        animator.SetBool("Atacando", atacando);

        // Detectamos el suelo para el salto con un Raycast
        grounded = Physics2D.Raycast(transform.position, Vector2.down, 1.5f);

        // Salto
        if (Input.GetKeyDown(KeyCode.W) && grounded)
            Jump();

        // Ataque
        if (Input.GetKeyDown(KeyCode.Z) && !atacando && grounded)
            Atacar();
    }

    void FixedUpdate()
    {
        if (isDead) return; // Evitar movimiento si está muerto
        rb.linearVelocity = new Vector2(horizontal * Speed, rb.linearVelocity.y);
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
    }

    private void Atacar()
    {
        atacando = true;
    }

    public void DesactivaAtaque()
    {
        atacando = false;
    }

    public void Hit()
    {
        if (isDead) return;  // Evita procesar daño si ya está muerto

        currentHealth -= 1;
        Debug.Log("Warrior Herido. Salud: " + currentHealth);

        if (currentHealth > 0)
        {
            // Reproducir animación de daño
            animator.SetTrigger("Hurt");
        }
        else
        {
            Die();
        }
    }


    private void Die()
    {
        isDead = true;  // Marcamos al Warrior como muerto
        Debug.Log("¡El Warrior ha muerto!");
        animator.SetTrigger("Die");

        // Desactivamos la física para evitar movimientos
        rb.simulated = false;

        // Desactivamos otros componentes (por ejemplo, colliders) si es necesario
        // o bien esperamos a que termine la animación y luego destruimos el objeto.
        Destroy(gameObject, 1.5f); // Ajusta el tiempo según la duración de tu animación
    }
}
