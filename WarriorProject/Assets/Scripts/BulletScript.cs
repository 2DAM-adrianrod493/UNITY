using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float Speed;
    public AudioClip Sound;

    private Rigidbody2D rb2D;
    private Vector3 Direction;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        // Sonido Disparo
        Camera.main.GetComponent<AudioSource>().PlayOneShot(Sound);
    }

    private void FixedUpdate()
    {
        // Movemos la bala en el sentido que queremos
        rb2D.linearVelocity = Direction * Speed;
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
        // Ignoramos al enemigo como colisión
        if (other.GetComponent<Enemy>() != null)
        {
            return;
        }

        if (other.GetComponent<EnemyFinal>() != null)
        {
            return;
        }

        // Daño a nuestro Warrior si colisiona con él
        WarriorController warrior = other.GetComponent<WarriorController>();
        if (warrior != null)
        {
            warrior.Hit();
        }

        // Destruimos la bala al colisionar con cualquier objeto
        DestroyBullet();
    }
}
