using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform Warrior;
    public GameObject BulletPrefab;
    private Animator animator;

    private int Health = 5;
    private float LastShoot;
    private Vector3 defaultScale = new Vector3(3.5f, 3.5f, 1f);

    void Start()
    {
        animator = GetComponent<Animator>();  // Obtiene el Animator
        transform.localScale = defaultScale;
    }

    void Update()
    {
        if (Warrior == null) return;

        Vector3 direction = Warrior.position - transform.position;

        // Ajustar la escala para mirar al Warrior
        if (direction.x >= 0.0f)
            transform.localScale = new Vector3(Mathf.Abs(defaultScale.x), defaultScale.y, defaultScale.z);
        else
            transform.localScale = new Vector3(-Mathf.Abs(defaultScale.x), defaultScale.y, defaultScale.z);

        // Usamos una distancia mayor para disparar
        float distance = Mathf.Abs(Warrior.position.x - transform.position.x);

        // Dispara si el Warrior está dentro del rango y con un delay de 2 segundos
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
        Debug.Log("Enemy dispara");
        GameObject bullet = Instantiate(BulletPrefab, transform.position + direction * 0.1f, Quaternion.identity);
        bullet.GetComponent<BulletScript>().SetDirection(direction);
    }

    public void Hit()
    {
        if (Health <= 0) return;  // Evita recibir daño si ya está muerto

        Health -= 1;
        Debug.Log("Enemy recibe daño. Salud restante: " + Health);

        animator.SetTrigger("EnemyHurt"); 

        if (Health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy ha muerto");

        animator.SetTrigger("EnemyDeath");

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false; 

        Destroy(gameObject, 1.5f);  
    }
}
