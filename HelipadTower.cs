using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class HelipadTower : MonoBehaviour
{
    [SerializeField] private GameObject Light;
    [SerializeField] private GameObject Cylinder;
    private Transform lightTransform;
    [SerializeField] private VolumetricLines.VolumetricLineBehavior lineBehavior;

    [SerializeField] private Identification.Faction capturingFaction;

    [ReadOnly]
    [SerializeField] private Identification.Faction poleFaction;

    [SerializeField] private float MinPoleHeight;
    [SerializeField] private float MaxPoleHeight;
    [SerializeField] private float PoleHeightChangeSpeed = 0.01f;


    [Header("Factions")]
    [SerializeField] private Color SEColor;
    [SerializeField] private Material SEMaterial;
    public bool SETransportCalled = false;

    [SerializeField] private Color AIColor;
    [SerializeField] private Material AIMaterial;
    public bool AITransportCalled = false;

    [SerializeField] private Color ILColor;
    [SerializeField] private Material ILMaterial;
    public bool ILTransportCalled = false;

    [SerializeField] private Color WFColor;
    [SerializeField] private Material WFMaterial;
    public bool WFTransportCalled = false;

    [SerializeField] private Color TColor;
    [SerializeField] private Material TMaterial;
    public bool TTransportCalled = false;

    [SerializeField] private Color XColor;
    [SerializeField] private Material XMaterial;
    public bool XTransportCalled = false;

    private List<Identification.Faction> factionsCalled = new List<Identification.Faction>();

    public event EventHandler<OnHelipadActivatedEventArgs> OnHelipadActivated;

    public class OnHelipadActivatedEventArgs : EventArgs
    {
        public Identification.Faction activatedFaction;
    }

    // Start is called before the first frame update
    void Start()
    {
        AssignPoleColor(capturingFaction);
        AssignPoleFaction();
        lightTransform = Light.transform;
    }

    private void AssignPoleColor(Identification.Faction faction)
    {
        switch (faction)
        {
            case Identification.Faction.AngloIsles:
                Cylinder.GetComponent<MeshRenderer>().material = AIMaterial;
                lineBehavior.LineColor = AIColor;
                break;

            case Identification.Faction.IronLegion:
                Cylinder.GetComponent<MeshRenderer>().material = ILMaterial;
                lineBehavior.LineColor = ILColor;
                break;

            case Identification.Faction.SolarEmpire:
                Cylinder.GetComponent<MeshRenderer>().material = SEMaterial;
                lineBehavior.LineColor = SEColor;
                break;

            case Identification.Faction.Tundran:
                Cylinder.GetComponent<MeshRenderer>().material = TMaterial;
                lineBehavior.LineColor = TColor;
                break;

            case Identification.Faction.WesternFrontier:
                Cylinder.GetComponent<MeshRenderer>().material = WFMaterial;
                lineBehavior.LineColor = WFColor;
                break;

            case Identification.Faction.Xylvanian:
                Cylinder.GetComponent<MeshRenderer>().material = XMaterial;
                lineBehavior.LineColor = XColor;
                break;

            case Identification.Faction.NoFaction:
                break;

            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (capturingFaction)
        {
            case Identification.Faction.AngloIsles:
                FactionCapturingPole(Identification.Faction.AngloIsles, AITransportCalled);
                break;

            case Identification.Faction.IronLegion:
                FactionCapturingPole(Identification.Faction.IronLegion, ILTransportCalled);
                break;

            case Identification.Faction.SolarEmpire:
                FactionCapturingPole(Identification.Faction.SolarEmpire, SETransportCalled);
                break;

            case Identification.Faction.Tundran:
                FactionCapturingPole(Identification.Faction.Tundran, TTransportCalled);
                break;

            case Identification.Faction.WesternFrontier:
                FactionCapturingPole(Identification.Faction.WesternFrontier, WFTransportCalled);
                break;

            case Identification.Faction.Xylvanian:
                FactionCapturingPole(Identification.Faction.Xylvanian, XTransportCalled);
                break;
            default:
                break;
        }

        AssignPoleFaction();
    }

    private void FactionCapturingPole(Identification.Faction faction, bool poleActivated)
    {
        //The pole has not been raised yet
        if (lightTransform.localScale.y <= 0)
        {
            //Set pole faction and raise it
            poleFaction = faction;
            RaisePole();
            //Set color
            AssignPoleColor(faction);

        }
        //The pole has been partially raised by the same faction
        else if (lightTransform.localScale.y > 0 && lightTransform.localScale.y < 7 && capturingFaction == poleFaction)
        {
            RaisePole();
            //Raise pole
        }
        //The pole has been raised and the pole faction is the capturing faction
        else if (lightTransform.localScale.y >= 7 && capturingFaction == poleFaction && factionsCalled.Contains(capturingFaction) == false)
        {
            ActivatePole(capturingFaction);
            factionsCalled.Add(capturingFaction);
        }
        //The pole has been raised, but by a different faction
        else if (lightTransform.localScale.y > 0 && capturingFaction != poleFaction)
        {
            //Bring pole down
            BringPoleDown();
        }
    }

    private void ActivatePole(Identification.Faction faction)
    {
        switch (faction)
        {
            case Identification.Faction.AngloIsles:
                AITransportCalled = true;
                break;
            case Identification.Faction.IronLegion:
                ILTransportCalled = true;
                break;
            case Identification.Faction.SolarEmpire:
                SETransportCalled = true;
                break;
            case Identification.Faction.Tundran:
                TTransportCalled = true;
                break;
            case Identification.Faction.WesternFrontier:
                WFTransportCalled = true;
                break;
            case Identification.Faction.Xylvanian:
                XTransportCalled = true;
                break;
            default:
                break;
        }
        OnHelipadActivated?.Invoke(this, new OnHelipadActivatedEventArgs { activatedFaction = faction }); ;
    }

    private void RaisePole()
    {
        lightTransform.localScale += new Vector3(0, PoleHeightChangeSpeed, 0);
    }
    private void BringPoleDown()
    {
        lightTransform.localScale -= new Vector3(0, PoleHeightChangeSpeed, 0);
    }

    private void AssignPoleFaction()
    {
        if (lineBehavior.LineColor == SEColor)
        {
            poleFaction = Identification.Faction.SolarEmpire;
        }
        else if (lineBehavior.LineColor == ILColor)
        {
            poleFaction = Identification.Faction.IronLegion;
        }
        else if (lineBehavior.LineColor == TColor)
        {
            poleFaction = Identification.Faction.Tundran;
        }
        else if (lineBehavior.LineColor == WFColor)
        {
            poleFaction = Identification.Faction.WesternFrontier;
        }
        else if (lineBehavior.LineColor == XColor)
        {
            poleFaction = Identification.Faction.Xylvanian;
        }
        else if (lineBehavior.LineColor == AIColor)
        {
            poleFaction = Identification.Faction.AngloIsles;
        }
    }
}
