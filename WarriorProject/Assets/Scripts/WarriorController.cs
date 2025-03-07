using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


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

    [Header("Sonido")]
    public AudioClip swordAttackSound;
    private AudioSource audioSource; 

    [Header("Combate")]
    public GameObject rangoEspada;
    public int attackDamage = 2;

    [Header("Salud")]
    public int maxHealth = 20;
    private int currentHealth;
    private bool isDead = false;

    [Header("Caída")]
    public Transform fallCheck;
    public float fallThreshold = -10f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            Debug.LogError("No se encontró AudioSource en el Warrior");


        currentHealth = maxHealth;
        rangoEspada.SetActive(false);

    }


    void Update()
    {
        if (isDead) return;

        // Verificamos si el Warrior se cae
        if (transform.position.y < fallThreshold)
        {
            Die();
        }

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
        if (isDead) return;
        rb.linearVelocity = new Vector2(horizontal * Speed, rb.linearVelocity.y);
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
    }

    private void Atacar()
    {
        atacando = true;
        animator.SetBool("Atacando", true);

        if (swordAttackSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(swordAttackSound, 0.3f);
        }

        ActivarHitbox();
    }


    public void DesactivaAtaque()
    {
        atacando = false;
        animator.SetBool("Atacando", false);
    }

    public void ActivarHitbox()
    {
        rangoEspada.SetActive(true);
    }

    public void Hit()
    {
        if (isDead) return;

        currentHealth -= 1;
        Debug.Log("Warrior Herido. Salud: " + currentHealth);

        if (currentHealth > 0)
        {
            animator.SetTrigger("Hurt");
        }
        else
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("¡El Warrior ha muerto!");
        animator.SetTrigger("Die");


        rb.simulated = false;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0;

        StartCoroutine(RestartGame());
    }

    private IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("FallZone")) 
        {
            Die();
        }
    }

}
