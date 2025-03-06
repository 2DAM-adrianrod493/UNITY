using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
    public int maxHealth = 100;
    private int currentHealth;

    private int Health = 100;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        if (rb == null)
        {
            Debug.LogError("El componente Rigidbody2D no está asignado en el Warrior.");
        }
        if (animator == null)
        {
            Debug.LogError("El componente Animator no está asignado en el Warrior.");
        }
    }

    void Update()
    {
        // Movimiento horizontal
        horizontal = Input.GetAxisRaw("Horizontal");

        // Ajustar dirección según el movimiento
        if (horizontal < 0f)
            transform.localScale = new Vector3(-1f, 1f, 1f);
        else if (horizontal > 0f)
            transform.localScale = new Vector3(1f, 1f, 1f);

        // Actualizar animaciones
        animator.SetBool("Running", horizontal != 0f);
        animator.SetBool("Atacando", atacando);

        // Detectar suelo usando un Raycast
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
        rb.linearVelocity = new Vector2(horizontal * Speed, rb.linearVelocity.y);
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
    }

    private void Atacar()
    {
        atacando = true;
        // Aquí se activa la animación de ataque.
        // Puedes usar un evento de animación para llamar a DesactivaAtaque()
        // y/o para sincronizar el momento en que se aplique daño.
    }

    // Este método puede llamarse desde la animación al finalizar el ataque
    public void DesactivaAtaque()
    {
        atacando = false;
    }

    // Método para aplicar daño al Warrior
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Salud del Warrior: " + currentHealth);
        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        Debug.Log("¡El Warrior ha muerto!");
        // Aquí puedes agregar animación de muerte, reiniciar la escena, etc.
        gameObject.SetActive(false);
    }

    public void Hit()
    {
        Health -= 1;
        if (Health == 0) Destroy(gameObject);
    }
}
