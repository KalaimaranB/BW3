using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class RadarPulse : MonoBehaviour
{
    [SerializeField]
    private GameObject pfRandarPing;

    private Transform pulseTransform;

    [SerializeField]
    [ReadOnly]
    private float range;

    public float rangeMax;
    [SerializeField]
    [Tooltip("This is recommended to be half of rangeMax")]
    private float rangeSpeed = 100;

    [SerializeField]
    private Vector3 pingSpawnOffset;

    private List<Collider> alreadyPingedColliderList;

    private float fadeRange;

    [Range(0, 1)]
    [Tooltip("The lower the value the later the fade starts")]
    [SerializeField]
    private float fadeStart;

    private Color pulseColor;
    private SpriteRenderer pulseSpriteRenderer; 

    [Header("Colors")]
    [SerializeField] private Color WesternFrontierColor;
    [SerializeField] private Color XylvanianColor;
    [SerializeField] private Color SolarEmpireColor;
    [SerializeField] private Color AngloIsleColor;
    [SerializeField] private Color IronLegionColor;
    [SerializeField] private Color TundraColor;

    private void Awake()
    {
        fadeRange = rangeMax * fadeStart;
        pulseTransform = transform.Find("Pulse");
        alreadyPingedColliderList = new List<Collider>();
        pulseSpriteRenderer = pulseTransform.GetComponent<SpriteRenderer>();
        pulseColor = pulseSpriteRenderer.color;
    }

    private void Update()
    {
        range += rangeSpeed * Time.deltaTime;

        if (range > rangeMax)
        {
            range = 0;
            alreadyPingedColliderList.Clear();
        }


        pulseTransform.localScale = new Vector3(range, range);


        //This may be off due to 2d vs 3d
        Collider[] hits = Physics.OverlapSphere(transform.position, range / 2);

        foreach (Collider col in hits)
        {
            if (alreadyPingedColliderList.Contains(col) == false)
            {
                //If it has an id
                if (col.gameObject.GetComponent<Identification>())
                {
                    Identification rpi = col.gameObject.GetComponent<Identification>();
                    if (rpi.faction!=Identification.Faction.NoFaction && rpi.isAUnit == true)
                    {
                        alreadyPingedColliderList.Add(col);
                        GameObject ping = Instantiate(pfRandarPing, col.gameObject.transform.position, Quaternion.identity);
                        ping.transform.rotation *= Quaternion.Euler(pingSpawnOffset);
                        ping.name = col.gameObject.name + " Ping";
                        RadarPing targetPing = ping.GetComponent<RadarPing>();
                        targetPing.SetDisappearTimer(rangeMax / rangeSpeed);
                        AssignPingColor(rpi.faction, targetPing);
                    }
                }
            }
        }

        //Begin fading
        ManageFading();
    }

    private void AssignPingColor(Identification.Faction faction, RadarPing targetPing)
    {
        switch (faction)
        {
            case Identification.Faction.WesternFrontier:
                targetPing.SetColor(WesternFrontierColor);
                break;

            case Identification.Faction.Xylvanian:
                targetPing.SetColor(XylvanianColor);
                break;

            case Identification.Faction.AngloIsles:
                targetPing.SetColor(AngloIsleColor);
                break;

            case Identification.Faction.IronLegion:
                targetPing.SetColor(IronLegionColor);
                break;

            case Identification.Faction.SolarEmpire:
                targetPing.SetColor(SolarEmpireColor);
                break;

            case Identification.Faction.Tundran:
                targetPing.SetColor(TundraColor);
                break;
            default:
                break;
        }
    }


    private void ManageFading()
    {
        if (range > rangeMax - fadeRange)
        {
            pulseColor.a = Mathf.Lerp(0, 1, (rangeMax - range) / fadeRange);
        }
        else
        {
            pulseColor.a = 1;
        }
        pulseSpriteRenderer.color = pulseColor;
    }
}
