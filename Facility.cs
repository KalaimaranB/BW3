using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FactionTypes;

[RequireComponent(typeof(Timer))]
[RequireComponent(typeof(Identification))]
public class Facility : MonoBehaviour
{
    public enum BuildingType { Barracks, Factory, Airbase, Dock, Headquarters };

    public BuildingType buildingType;

    public List<GameObject> SpawnPoints;

    [Tooltip("Time to wait for new unit to spawn")]
    public float SpawnTime = 30f;

    private Timer timer;

    private int LastSpawnPointIndex = 0;

    public GameObject FacilityAppearExplosion;
    public GameObject Flag;
    public AudioClip onFactionSwitch;

    [Header("Faction Information")]
    public GameObject SolarEmpire;
    public GameObject SEExplosion;
    private Animator SEAnimator;

    public GameObject AngloIsles;
    public GameObject AIExplosion;
    private Animator AIAnimator;

    public GameObject IronLegion;
    public GameObject ILExplosion;
    private Animator ILAnimator;

    public GameObject Tundran;
    public GameObject TExplosion;
    private Animator TAnimator;

    public GameObject WesternFrontier;
    public GameObject WFExplosion;
    private Animator WFAnimator;

    public GameObject Xylvanian;
    public GameObject XExplosion;
    private Animator XAnimator;

    private Identification id;

    private List<GameObject> factionFacilities = new List<GameObject>();
    private GameObject activeFacility;

    private AudioSource ad;

    // Start is called before the first frame update
    void Start()
    {
        ad = gameObject.GetComponent<AudioSource>();
        if (Flag.GetComponent<Flagpole>() == false)
        {
            Debug.LogError("Flag must require FlagPole Component!");
        }
        timer = gameObject.GetComponent<Timer>();
        timer.PauseTimer();
        id = gameObject.GetComponent<Identification>();
        id.faction = Flag.GetComponent<Flagpole>().StartFaction;


        factionFacilities.Add(SolarEmpire);
        SEAnimator = SolarEmpire.GetComponent<Animator>();
        factionFacilities.Add(AngloIsles);
        AIAnimator = AngloIsles.GetComponent<Animator>();
        factionFacilities.Add(IronLegion);
        ILAnimator = IronLegion.GetComponent<Animator>();
        factionFacilities.Add(Tundran);
        TAnimator = Tundran.GetComponent<Animator>();
        factionFacilities.Add(WesternFrontier);
        WFAnimator = WesternFrontier.GetComponent<Animator>();
        factionFacilities.Add(Xylvanian);
        XAnimator = Xylvanian.GetComponent<Animator>();

        Flag.GetComponent<Flagpole>().OnFacilityCaptured += ShrinkBuilding;

        SetAnimatorTrigger(id.faction,"Grow Building");
        
    }

    private GameObject SetAnimatorTrigger(Identification.Faction faction, string triggerName)
    {
        switch (faction)
        {
            case Identification.Faction.SolarEmpire:
                SEAnimator.SetTrigger(triggerName);
                AssignAllButOneGameObjectAs(factionFacilities, SolarEmpire, false);
                return SolarEmpire;

            case Identification.Faction.AngloIsles:
                AIAnimator.SetTrigger(triggerName);
                AssignAllButOneGameObjectAs(factionFacilities, AngloIsles, false);
                return AngloIsles;

            case Identification.Faction.IronLegion:
                ILAnimator.SetTrigger(triggerName);
                AssignAllButOneGameObjectAs(factionFacilities, IronLegion, false);
                return IronLegion;

            case Identification.Faction.Tundran:
                TAnimator.SetTrigger(triggerName);
                AssignAllButOneGameObjectAs(factionFacilities, Tundran, false);
                return Tundran;

            case Identification.Faction.WesternFrontier:
                WFAnimator.SetTrigger(triggerName);
                AssignAllButOneGameObjectAs(factionFacilities, WesternFrontier, false);
                return WesternFrontier;

            case Identification.Faction.Xylvanian:
                XAnimator.SetTrigger(triggerName);
                AssignAllButOneGameObjectAs(factionFacilities, Xylvanian, false);
                return Xylvanian;

            case Identification.Faction.NoFaction:
                Debug.LogError("No Faction can not be the faction type for facility animation");
                return null;
            default:
                Debug.Log(faction.ToString() + " is not a valid faction");
                return null;

        }
    }

    // Update is called once per frame
    void Update()
    {
        id.faction = Flag.GetComponent<Flagpole>().lastFaction;
        if (id.faction == PlayerManager.PlayerFaction)
        {
            switch (buildingType)
            {
                case BuildingType.Barracks:
                    ProduceInfantry();
                    break;

                case BuildingType.Factory:
                    ProduceVehicles();
                    break;


                case BuildingType.Airbase:
                    ProduceAviation();
                    break;

                case BuildingType.Dock:
                    ProduceFleet();
                    break;

                case BuildingType.Headquarters:
                    ProduceHeadquarters();
                    break;

                default:
                    break;
            }
        }

        foreach (GameObject fac in factionFacilities)
        {
            if (fac.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsTag("Grow Idle"))
            {
                activeFacility = fac;
            }
        }

    }

    private void AssignAllButOneGameObjectAs(List<GameObject> gameObjects, GameObject Exception, bool NormalBool)
    {
        foreach (GameObject obj in gameObjects)
        {
            if (obj != Exception)
            {
                obj.SetActive(NormalBool);
            }
            if (obj == Exception)
            {
                if (NormalBool == true)
                {
                    obj.SetActive(false);
                }
                else if (NormalBool == false)
                {
                    obj.SetActive(true);
                }
            }
        }
    }

    #region Produce Units
    private void ProduceInfantry()
    {
        foreach(Unit infantry in Finder.PlayerManager.playerUnits.infantryUnits)
        {
            ProduceUnit(infantry);
        }

        ResetSpawnPointIndex();
    }

    private void ProduceVehicles()
    {
        foreach (Unit vehicle in Finder.PlayerManager.playerUnits.vehicleUnits)
        {
            ProduceUnit(vehicle);
        }

        ResetSpawnPointIndex();
    }

    private void ProduceAviation()
    {
        foreach (Unit aviation in Finder.PlayerManager.playerUnits.aviationUnits)
        {
            //You can't produce air transports!
            if (aviation.unitClass != Identification.UnitClass.AirTransport)
            {
                ProduceUnit(aviation);
            }
        }

        ResetSpawnPointIndex();
    }

    private void ProduceFleet()
    {
        foreach (Unit navy in Finder.PlayerManager.playerUnits.navalUnits)
        {
            //You can't produce air transports!
            if (navy.unitClass != Identification.UnitClass.NavalTransport)
            {
                ProduceUnit(navy);
            }
        }


        ResetSpawnPointIndex();
    }

    private void ProduceHeadquarters()
    {
        foreach (Unit infantry in Finder.PlayerManager.playerUnits.infantryUnits)
        {
            //Headquarters can only produce grunts
            if (infantry.unitClass == Identification.UnitClass.RifleGrunt)
            {
                ProduceUnit(infantry);
            }
        }

        ResetSpawnPointIndex();
    }

    private void ProduceUnit(FactionTypes.Unit Unit)
    {
        if (Unit.Units.Count < Unit.MaxNumberOfUnits && timer.timerIsRunning == false)
        {
            Instantiate(Unit.UnitPrefab, SpawnPoints[LastSpawnPointIndex].transform.position, SpawnPoints[LastSpawnPointIndex].transform.rotation);
            LastSpawnPointIndex++;
            timer.SetTimer(SpawnTime);
            timer.StartTimer();
        }
    }
    #endregion Produce Units

    private void ShrinkBuilding(object sender, EventArgs e)
    {
        StartCoroutine(SwitchFacilityFaction());
    }

    private IEnumerator SwitchFacilityFaction()
    {
        GameObject lastFac = activeFacility;
        lastFac.GetComponent<Animator>().SetTrigger("Shrink Building");
        do
        {
            yield return null;
        } while (lastFac.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsTag("Building Shrink") || lastFac.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsTag("Grow Idle"));

        lastFac.SetActive(false);
        switch (id.faction)
        {
            case Identification.Faction.AngloIsles:
                AngloIsles.SetActive(true);
                Instantiate(AIExplosion, gameObject.transform.position, gameObject.transform.rotation);
                break;

            case Identification.Faction.IronLegion:
                IronLegion.SetActive(true);
                Instantiate(ILExplosion, gameObject.transform.position, gameObject.transform.rotation);
                break;

            case Identification.Faction.SolarEmpire:
                SolarEmpire.SetActive(true);
                Instantiate(SEExplosion, gameObject.transform.position, gameObject.transform.rotation);
                break;

            case Identification.Faction.Tundran:
                Tundran.SetActive(true);
                Instantiate(TExplosion, gameObject.transform.position, gameObject.transform.rotation);
                break;

            case Identification.Faction.WesternFrontier:
                WesternFrontier.SetActive(true);
                Instantiate(WFExplosion, gameObject.transform.position, gameObject.transform.rotation);
                break;

            case Identification.Faction.Xylvanian:
                Xylvanian.SetActive(true);
                Instantiate(XExplosion, gameObject.transform.position, gameObject.transform.rotation);
                break;

            default:
                break;
        }
        GameObject factionGO = SetAnimatorTrigger(id.faction, "Grow Building");
        do
        {
            yield return null;
        } while (factionGO.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsTag("Building Grow") || factionGO.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsTag("Shrunk Idle"));
        Instantiate(FacilityAppearExplosion, gameObject.transform.position, gameObject.transform.rotation);
        ad.PlayOneShot(onFactionSwitch);
    }



    private void ResetSpawnPointIndex()
    {
        if (LastSpawnPointIndex == SpawnPoints.Count)
        {
            LastSpawnPointIndex = 0;
        }
    }

}
