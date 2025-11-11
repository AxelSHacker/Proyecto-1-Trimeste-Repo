using UnityEngine;

public class Collectable : MonoBehaviour
{
    [Header("VARIABBLES")]
    public int point;
    [Header("REFERENCES"), SerializeField]
    Collider2D thisCollider;
    [SerializeField]
    SpriteRenderer spriteRenderer;

    [Header("FEEDBBACK")]

    public Color flashColor = Color.white;

    public float flashTTime = 0.4f;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PayerControlle player))
        {
            FeedBBack(player);
            GameManager.Instance.PicupCollectable(point);
            Desactivate();
        }
    }

    private void FeedBBack(PayerControlle player)
    {
        
            player.StartColorFlash(flashColor, flashTTime);
        
    }
    private void Desactivate()
    {
        thisCollider.enabled = false;
        spriteRenderer.enabled = false;
    }
}
