using UnityEngine;
using UnityEngine.UI;
public class ProgressBarCircle : MonoBehaviour {

    [Header("Bar Setting")]
    public Gradient gradient;
    public Sprite BarBackGroundSprite;

    public Image bar;
    public Image barBackground;
    public float barValue;

    private void Start()
    {
        barBackground.color = gradient.Evaluate(1);
        barBackground.sprite = BarBackGroundSprite;
        UpdateValue(barValue);
    }

    public void UpdateValue(float val)
    {
        barValue = val;
    }


    private void Update()
    {
        bar.fillAmount = -(barValue / 100) + 1f;
        barBackground.color = gradient.Evaluate(barValue / 100f);
    }

}
