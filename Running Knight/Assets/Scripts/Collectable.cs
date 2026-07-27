using UnityEngine;

public class Collectable : MonoBehaviour
{
    [Header("VARIABBLES")]
    public int point;
    [Header("REFERENCES"), SerializeField]
    Collider2D thisCollider;
    [SerializeField]
    SpriteRenderer spriteRenderer;
    [Header("SOUNDS")]
    [SerializeField] AudioClip pickUpCoin;

    [Header("FEEDBBACK")]

    public Color flashColor = Color.white;

    public float flashTTime = 0.4f;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerControllerEndLess playerEndLess))
        {
            FeedBBackEndLEss(playerEndLess);
            GameManager.Instance.PicupCollectable(point);
            Desactivate();
        }
        if (collision.TryGetComponent(out PlayerControllerPlatform playerPlatform))
        {
            FeedBBackPlatform(playerPlatform);
            Desactivate();
        }
    }

    private void FeedBBackEndLEss(PlayerControllerEndLess playerEndLess)
    {
        playerEndLess.StartColorFlash(flashColor, flashTTime);
    }
    private void FeedBBackPlatform(PlayerControllerPlatform playerPlatfrom)
    {
        playerPlatfrom.StartColorFlash(flashColor, flashTTime);
    }
    private void Desactivate()
    {
        thisCollider.enabled = false;
        spriteRenderer.enabled = false;
    }
}
