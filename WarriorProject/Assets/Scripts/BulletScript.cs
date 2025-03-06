using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float Speed;
    public AudioClip Sound;

    private Rigidbody2D Rigidbody2D;
    private Vector3 Direction;

    private void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Camera.main.GetComponent<AudioSource>().PlayOneShot(Sound);
    }

    private void FixedUpdate()
    {
        Rigidbody2D.linearVelocity = Direction * Speed; // Corregimos de 'linearVelocity' a 'velocity' para mejor manejo
    }

    public void SetDirection(Vector3 direction)
    {
        Direction = direction;
    }

    public void DestroyBullet()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Comprobamos si la bala colisiona con el Warrior o el Enemy
        WarriorController warrior = other.GetComponent<WarriorController>();
        Enemy enemy = other.GetComponent<Enemy>();

        if (warrior != null)
        {
            warrior.TakeDamage(1);  // Llama al m�todo para hacerle da�o al Warrior
        }

        if (enemy != null)
        {
            enemy.Hit();  // Llama al m�todo para hacerle da�o al Enemy
        }

        DestroyBullet(); // Destruir la bala al colisionar
    }
}
