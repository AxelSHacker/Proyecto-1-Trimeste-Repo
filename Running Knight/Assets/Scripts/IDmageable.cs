using UnityEngine;

public interface IDamageabe<T>
{
    public int Maxhealt { get; }
    public int Currentealt{ get; }
    public bool IsDead { get; }
    public void TakeDamag(T damage, Vector3 impactPoint = default(Vector3));
}
