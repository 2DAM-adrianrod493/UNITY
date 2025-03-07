using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFinal : MonoBehaviour
{
    public Transform Warrior;
    public GameObject BulletPrefab;
    private Animator animator;

    private int Health = 25;
    private float LastShoot;
    private Vector3 defaultScale = new Vector3(3.5f, 3.5f, 1f);
    private bool isDead = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        transform.localScale = defaultScale;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    void Update()
    {
        if (Warrior == null || isDead) return;

        Vector3 direction = Warrior.position - transform.position;

        // Ajustamos la escala para mirar al Warrior
        if (direction.x >= 0.0f)
            transform.localScale = new Vector3(Mathf.Abs(defaultScale.x), defaultScale.y, defaultScale.z);
        else
            transform.localScale = new Vector3(-Mathf.Abs(defaultScale.x), defaultScale.y, defaultScale.z);

        float distance = Mathf.Abs(Warrior.position.x - transform.position.x);

        // Dispara si el Warrior está dentro del rango cada 2s
        if (distance < 4.0f && Time.time > LastShoot + 2f)
        {
            Attack();
            LastShoot = Time.time;
        }
    }

    private void Attack()
    {
        animator.SetTrigger("EnemyAttack");

        Vector3 direction = (Warrior.position - transform.position).normalized;
        Debug.Log("EnemyFinal dispara");
        GameObject bullet = Instantiate(BulletPrefab, transform.position + direction * 0.1f, Quaternion.identity);
        bullet.GetComponent<BulletScript>().SetDirection(direction);

    }

    public void Hit(int damage)
    {
        if (Health <= 0 || isDead) return;

        Health -= damage;
        Debug.Log("EnemyFinal recibe daño. Salud: " + Health);

        if (Health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("EnemyFinal ha muerto");

        animator.SetTrigger("EnemyDeath");

        // Desactivamos la física para evitar que se caiga
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.simulated = false;
            rb.gravityScale = 0;
        }

        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        this.enabled = false;

        Destroy(gameObject, 2f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("FallZone"))
        {
            Die();
        }
    }
}
