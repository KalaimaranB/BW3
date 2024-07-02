using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarPing : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float disappearTimer;
    private float disapperTimeMax;
    private Color color;


    private void Awake()
    {
        disappearTimer = 0;
        disapperTimeMax = 1;
        spriteRenderer = GetComponent<SpriteRenderer>();
        color = new Color(1, 1, 1, 1);
    }

    private void Update()
    {
        disappearTimer += Time.deltaTime;

        color.a = Mathf.Lerp(disapperTimeMax, 0, disappearTimer / disapperTimeMax);
        spriteRenderer.color = color;

        if (disappearTimer >= disapperTimeMax)
        {
            Destroy(gameObject);
        }
    }

    public void SetColor(Color color)
    {
        this.color = color;
    }

    public void SetDisappearTimer(float disappearTimerMax)
    {
        this.disapperTimeMax = disappearTimerMax;
        disappearTimer = 0f;
    }
}
