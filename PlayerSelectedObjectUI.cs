using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;

public class PlayerSelectedObjectUI : MonoBehaviour
{
    public GameObject PlayerTarget;
    [Header("Bar & Text")]
    public Image FillBar;
    public Text TargetDetails;
    public GameObject BarBackground;

    private float targetCurrentHealth;

    private float targetMaxHealth;

    [ReadOnly]
    public string targetName;

    // Start is called before the first frame update
    void Start()
    {
        BarBackground.SetActive(false);
        TargetDetails.gameObject.SetActive(false);
        FillBar.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerTarget == null)
        {
            ClearValues();
        }
    }

    public void ClearValues()
    {
        PlayerTarget = null;
        BarBackground.SetActive(false);
        TargetDetails.gameObject.SetActive(false);
        FillBar.gameObject.SetActive(false);
        FillBar.fillAmount = 0;
        TargetDetails.text = "";
        PlayerTarget = null;
    }

    public void AssignTargetDetails(GameObject target)
    {
        PlayerTarget = target;

        BarBackground.SetActive(true);
        TargetDetails.gameObject.SetActive(true);
        FillBar.gameObject.SetActive(true);
        if (target.GetComponent<Identification>() == true && target.GetComponent<Health>() == true)
        {
            Health targetHealth = target.GetComponent<Health>();
            targetCurrentHealth = targetHealth.CurrentHealth;
            targetMaxHealth = targetHealth.MaxHealth;

            float healthPercent = (targetCurrentHealth / targetMaxHealth);
            FillBar.fillAmount = healthPercent;
            TargetDetails.text = target.GetComponent<Identification>().Name;

        }

        else
        {
            Debug.Log("Target needs both an ID and Health script to be displayed! The target supplied was : "+target.name);
            FillBar.fillAmount = 0;
            TargetDetails.text = "";
            ClearValues();
        }
    }

}
