using UnityEngine;

public class PlayerDamageable : Damageable {

    // Referencia al hud
    [SerializeField] private HUD hud;

    protected override void Awake() {
        base.Awake();
        hud.UpdateTargetFillAmount(1f);
    }

    public override void TakeDamage(float damage) {
        base.TakeDamage(damage);
        hud.UpdateTargetFillAmount(currentHealth / maxHealth);
    }
}