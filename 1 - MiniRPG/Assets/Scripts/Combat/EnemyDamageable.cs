using UnityEngine;
using UnityEngine.UI;

public class EnemyDamageable : Damageable {

    [SerializeField] private Image healthImage;

    protected override void Awake() {
        base.Awake();
        healthImage.fillAmount = currentHealth / maxHealth;
    }

    public override void TakeDamage(float damage) {
        base.TakeDamage(damage);
        healthImage.fillAmount = currentHealth / maxHealth;
    }
}