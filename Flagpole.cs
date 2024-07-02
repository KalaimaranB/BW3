using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Flagpole : MonoBehaviour
{
    [Header("Flag")]
    [SerializeField] private GameObject Flag;
    public Identification.Faction StartFaction;
    [Range(0,1)]
    [SerializeField] private float StartFactionControlValue;
    public Identification.Faction lastFaction;
    [SerializeField] private Identification.Faction capturingFaction;
    [SerializeField] private float FlagHeight;
    private MeshRenderer FlagMesh;
    [SerializeField] private float flagChangeHeight;
    [SerializeField] private Identification.Faction flagFaction;

    [SerializeField] private float minHeight=3.2f;
    [SerializeField] private float maxHeight=10.2f;

    [Range(0,1)]
    [SerializeField] private float CurrentControlValue;

    [Header("Factions")]

    [SerializeField] private FlagControlValue SolarEmpire = new FlagControlValue(Identification.Faction.SolarEmpire);

    [SerializeField] private FlagControlValue AngloIsles = new FlagControlValue(Identification.Faction.AngloIsles);

    [SerializeField] private FlagControlValue IronLegion = new FlagControlValue(Identification.Faction.IronLegion);

    [SerializeField] private FlagControlValue Tundra = new FlagControlValue(Identification.Faction.Tundran);

    [SerializeField] private FlagControlValue WesternFrontier = new FlagControlValue(Identification.Faction.WesternFrontier);

    [SerializeField] private FlagControlValue Xylvanian = new FlagControlValue(Identification.Faction.Xylvanian);

    private FlagControlValue currentFlagControlValue;


    public event EventHandler OnFacilityCaptured;

    // Start is called before the first frame update
    void Start()
    {

        FlagMesh = Flag.GetComponent<MeshRenderer>();
        switch (StartFaction)
        {
            case Identification.Faction.AngloIsles:
                AssignStartFlag(AngloIsles);
                break;

            case Identification.Faction.IronLegion:
                AssignStartFlag(IronLegion);
                break;

            case Identification.Faction.SolarEmpire:
                AssignStartFlag(SolarEmpire);
                break;

            case Identification.Faction.Tundran:
                AssignStartFlag(Tundra);
                break;

            case Identification.Faction.WesternFrontier:
                AssignStartFlag(WesternFrontier);
                break;

            case Identification.Faction.Xylvanian:
                AssignStartFlag(Xylvanian);
                break;

            case Identification.Faction.NoFaction:
                Debug.LogError("Flag needs a start faction!");
                break;

            default:
                Debug.LogError("Invalid Faction --> "+lastFaction);
                break;
        }
    }

    private void AssignStartFlag(FlagControlValue value)
    {
        AssignFlag(value.Flag);
        lastFaction = value.faction;
        CurrentControlValue = StartFactionControlValue;
        AssignFlagHeight(StartFactionControlValue);
        flagFaction = value.faction;
    }

    // Update is called once per frame
    void Update()
    {
        if (capturingFaction!= Identification.Faction.NoFaction)
        {
            switch (capturingFaction)
            {
                case Identification.Faction.SolarEmpire:
                    FactionCapture(Identification.Faction.SolarEmpire, SolarEmpire.Flag);
                    break;

                case Identification.Faction.AngloIsles:
                    FactionCapture(Identification.Faction.AngloIsles, AngloIsles.Flag);
                    break;

                case Identification.Faction.IronLegion:
                    FactionCapture(Identification.Faction.IronLegion, IronLegion.Flag);
                    break;

                case Identification.Faction.Tundran:
                    FactionCapture(Identification.Faction.Tundran, Tundra.Flag);
                    break;

                case Identification.Faction.WesternFrontier:
                    FactionCapture(Identification.Faction.WesternFrontier, WesternFrontier.Flag);
                    break;

                case Identification.Faction.Xylvanian:
                    FactionCapture(Identification.Faction.Xylvanian, Xylvanian.Flag);
                    break;

                default:
                    Debug.LogWarning(capturingFaction +" --> Is an invalid capture faction type");
                    break;
            }
        }
        AssignFlagHeight(CurrentControlValue);
    }

    private void FactionCapture(Identification.Faction faction, Material flag)
    {
        //If the solar Empire already controls it, but the flag isn't raised
        if (lastFaction == faction && CurrentControlValue < 1)
        {
            CurrentControlValue += flagChangeHeight;
        }
        //If they don't then bring down the flag
        else if (CurrentControlValue > 0 && flagFaction != faction)
        {
            CurrentControlValue -= flagChangeHeight;
        }

        //If flag is brought down then replace the flag
        else if (CurrentControlValue <= 0)
        {
            flagFaction = faction;
            AssignFlag(flag);
            CurrentControlValue += flagChangeHeight;
        }

        //If the flag has been changed, but they don't control the facility raise the flag
        else if (flagFaction == faction && lastFaction != faction && CurrentControlValue < 1)
        {
            CurrentControlValue += flagChangeHeight;
        }

        //If the flag has been changed, and completely raised, then capture it
        else if (flagFaction == faction && CurrentControlValue >= 1 && lastFaction!=faction)
        {
            lastFaction = faction;
            //Faction changed so trigger event
            OnFacilityCaptured?.Invoke(this, EventArgs.Empty);
        }

        //If the flag has been changed, completley raised and it is captured
        else if (flagFaction == faction && lastFaction == faction && CurrentControlValue>=1)
        {
            lastFaction = faction;
        }
    }


    private void AssignFlagHeight(float control)
    {
        Flag.transform.position = new Vector3(Flag.transform.position.x, ConvertControlFloatToFlagHeight(control,minHeight,maxHeight), Flag.transform.position.z);
    }

    private void AssignFlag(Material factionMaterial)
    {
        FlagMesh.material = factionMaterial;
    }

    private float ConvertControlFloatToFlagHeight(float floatToConvert, float rangeMin, float rangeMax)
    {
        return (rangeMax - rangeMin) * floatToConvert+rangeMin;
    }

    [Serializable]
    public class FlagControlValue
    {
        public Identification.Faction faction;
        public Material Flag;

        public FlagControlValue(Identification.Faction factionName)
        {
            faction = factionName;
        }
    }
}
