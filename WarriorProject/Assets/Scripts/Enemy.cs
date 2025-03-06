using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform Warrior;  // Asegúrate de asignar esto en el inspector
    public GameObject BulletPrefab;

    private int Health = 10;
    private float LastShoot;

    private Vector3 defaultScale = new Vector3(3.5f, 3.5f, 1f); // Escala del enemigo

    void Start()
    {
        transform.localScale = defaultScale; // Asegurar que el enemigo tenga la escala correcta al principio
    }

    void Update()
    {
        if (Warrior == null) return;

        Vector3 direction = Warrior.position - transform.position;

        // Corregir la escala según la dirección del Warrior, asegurando que se mantenga la escala original
        if (direction.x >= 0.0f)
            transform.localScale = new Vector3(Mathf.Abs(defaultScale.x), defaultScale.y, defaultScale.z); // Mirando hacia la derecha
        else
            transform.localScale = new Vector3(-Mathf.Abs(defaultScale.x), defaultScale.y, defaultScale.z); // Mirando hacia la izquierda

        float distance = Mathf.Abs(Warrior.position.x - transform.position.x);

        if (distance < 1.0f && Time.time > LastShoot + 0.25f)
        {
            Shoot();
            LastShoot = Time.time;
        }
    }

    private void Shoot()
    {
        // Dirección hacia el Warrior (hacia la posición del Warrior)
        Vector3 direction = (Warrior.position - transform.position).normalized;

        // Ajustamos la posición de la bala para que salga ligeramente frente al enemigo
        Vector3 spawnPosition = transform.position + new Vector3(Mathf.Sign(transform.localScale.x) * 0.5f, 0f, 0f); // Ajuste para que la bala salga "adelante"

        // Instanciamos la bala con la dirección correcta hacia el Warrior
        GameObject bullet = Instantiate(BulletPrefab, spawnPosition, Quaternion.identity);
        bullet.GetComponent<BulletScript>().SetDirection(direction); // Dirección corregida hacia el Warrior
    }

    public void Hit()
    {
        Health -= 1;
        if (Health == 0) Destroy(gameObject);
    }
}
