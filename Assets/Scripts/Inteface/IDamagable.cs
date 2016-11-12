public interface IDamagable {
    bool isAlive { get; }
    bool StopsBullet { get; }
    void ApplyDamage(float amount);
}