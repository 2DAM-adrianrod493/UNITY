using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public int attackDamage = 3;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        EnemyFinal enemyFinal = other.GetComponent<EnemyFinal>();

        if (enemy != null)
        {
            Debug.Log("Enemy Da�ado");
            enemy.Hit(attackDamage);
        }
        else if (enemyFinal != null)
        {
            Debug.Log("EnemyFinal Da�ado");
            enemyFinal.Hit(attackDamage);
        }
    }

}
