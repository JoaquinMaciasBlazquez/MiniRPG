using UnityEngine;

public abstract class Damageable : MonoBehaviour {

    [SerializeField] protected Shake shakeEffect;
    [SerializeField] protected float maxHealth;

    protected float currentHealth;

    protected virtual void Awake() {

        // Actualizamos la vida actual con la vida máxima
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Efectos al momento de recibir daño
    /// </summary>
    protected virtual void DamageEffects() {

        // Activamos el shake
        shakeEffect.Activate();
    }

    /// <summary>
    /// Se lanzará en el momento que muera
    /// </summary>
    protected virtual void Die() {

        DieEffects();
    }

    /// <summary>
    /// Efectos al morir
    /// </summary>
    protected virtual void DieEffects() {

    }

    /// <summary>
    /// Método por el que se recibirá daño
    /// </summary>
    /// <param name="damage"></param>
    public virtual void TakeDamage(float damage) {

        // Modificamos la vida actual
        currentHealth -= damage;
        // Ejecutamos los efectos de daño
        DamageEffects();

        // Si la vida actual es menor o igual que cero...
        if (currentHealth <= 0) {
            // Muere
            Die();
        }
    }
}