using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Helipad : MonoBehaviour
{
    [SerializeField] private HelipadTower tower;

    public GameObject LandingPos;
    public GameObject FlyToPos;
    public GameObject LastLookAtObject;
    public List<GameObject> tankMoveToPos;
    public GameObject tankInitialMoveToPos;
    public List<GameObject> infantryMoveToPos;

    [SerializeField] private bool AngleIslesCanCall = false;
    [ShowIf("AngleIslesCanCall")]
    [SerializeField] private GameObject AngloIslesTransport;

    [SerializeField] private bool TundraCanCall = false;
    [ShowIf("TundraCanCall")]
    [SerializeField] private GameObject TundranTransport;

    [SerializeField] private bool WesternFrontierCanCall = false;
    [ShowIf("WesternFrontierCanCall")]
    [SerializeField] private GameObject WesternFrontierTransport;

    [SerializeField] private bool XylvanianCanCall = false;
    [ShowIf("XylvanianCanCall")]
    [SerializeField] private GameObject XylvanianTransport;

    [SerializeField] private bool SolarEmpireCanCall = false;
    [ShowIf("SolarEmpireCanCall")]
    [SerializeField] private GameObject SolarEmpireTransport;

    // Start is called before the first frame update
    void Start()
    {
        tower.OnHelipadActivated += CallTransport;
        if (AngleIslesCanCall == false)
        {
            tower.AITransportCalled = true;
        }
        if (TundraCanCall == false)
        {
            tower.TTransportCalled = true;
        }
        if (WesternFrontierCanCall == false)
        {
            tower.WFTransportCalled = true;
        }
        if (XylvanianCanCall == false)
        {
            tower.XTransportCalled = true;
        }
        if (SolarEmpireCanCall == false)
        {
            tower.SETransportCalled = true;
        }
    }

    public void CallTransport(object sender,HelipadTower.OnHelipadActivatedEventArgs activatedEventArgs)
    {
        if (activatedEventArgs.activatedFaction == Identification.Faction.AngloIsles)
        {
            AngloIslesTransport.GetComponent<AirTransport>().TransferToHelipadAndReturn(this);
        }
        else if (activatedEventArgs.activatedFaction == Identification.Faction.Tundran)
        {
            TundranTransport.GetComponent<AirTransport>().TransferToHelipadAndReturn(this);
        }
        else if (activatedEventArgs.activatedFaction == Identification.Faction.WesternFrontier)
        {
            WesternFrontierTransport.GetComponent<AirTransport>().TransferToHelipadAndReturn(this);
        }
        else if (activatedEventArgs.activatedFaction == Identification.Faction.Xylvanian)
        {
            XylvanianTransport.GetComponent<AirTransport>().TransferToHelipadAndReturn(this);
        }
        else if (activatedEventArgs.activatedFaction == Identification.Faction.SolarEmpire)
        {
            SolarEmpireTransport.GetComponent<AirTransport>().TransferToHelipadAndReturn(this);
        }
    }
}
