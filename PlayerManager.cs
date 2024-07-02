using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using Cinemachine;
using FactionTypes;

public class PlayerManager : MonoBehaviour
{
    public GameObject StartPlayer;

    public static GameObject CurrentPlayer;

    [SerializeField] private Identification.Faction playerFaction = Identification.Faction.NoFaction;

    public static Identification.Faction PlayerFaction;

    [SerializeField]private CinemachineVirtualCamera vcm;
    public static List<GameObject> playerTeam = new List<GameObject>();

    public List<Identification.Faction> friendlyFactions;
    public List<Identification.Faction> enemyFactions;

    public FactionUnits playerUnits;

    private void Awake()
    {
        PlayerFaction = playerFaction;
    }
    

    private void LevelManager_OnUnitCreated(object sender, LevelManager.OnUnitStatusChangedEventArgs e)
    {
        if (e.id.faction == playerFaction && playerTeam.Contains(e.unit) == false)
        {
            playerTeam.Add(e.unit);
        }
    }

    private void LevelManager_OnUnitDestroyed(object sender, LevelManager.OnUnitStatusChangedEventArgs e)
    {
        if (playerTeam.Contains(e.unit))
        {
            playerTeam.Remove(e.unit);
        }
    }

    private void Start()
    {
        Finder.LevelManager.OnUnitCreated += LevelManager_OnUnitCreated;
        Finder.LevelManager.OnUnitDestroyed += LevelManager_OnUnitDestroyed;
        PlayerFaction = playerFaction;
        switch (PlayerFaction)
        {
            case Identification.Faction.AngloIsles:
                playerUnits = Finder.FactionManager.AngloIsles;
                break;

            case Identification.Faction.IronLegion:
                playerUnits = Finder.FactionManager.IronLegion;
                break;

            case Identification.Faction.SolarEmpire:
                playerUnits = Finder.FactionManager.SolarEmpire;
                break;

            case Identification.Faction.Tundran:
                playerUnits = Finder.FactionManager.Tundra;
                break;

            case Identification.Faction.WesternFrontier:
                playerUnits = Finder.FactionManager.WesternFrontier;
                break;

            case Identification.Faction.Xylvanian:
                playerUnits = Finder.FactionManager.Xylvanian;
                break;

            default:
                break;
        }
        if (StartPlayer != null)
        {
            CurrentPlayer = StartPlayer;
            vcm.Follow = CurrentPlayer.transform;
            vcm.LookAt = CurrentPlayer.transform;
        }
        if (StartPlayer == null) {Debug.LogWarning("Start Player is blank! Please assign StartPlayer.");}
        if (vcm == null) {Debug.LogWarning("Virtural Camera is not assigned! Please assign it!");}
        if (PlayerFaction == Identification.Faction.NoFaction) {Debug.LogWarning("Player faction is currently NoFaction. Please check that PlayerFaction is an actual faction");}
    }

    void Update()
    {  
        playerTeam = playerTeam.Where(player => player && player.TryGetComponent<Health>(out var health) && health.CurrentHealth > 0).ToList();
        for (int i = 0; i < playerTeam.ToArray().Length; i++)
        {
            if (playerTeam[i] != CurrentPlayer)
            {
                playerTeam[i].layer = 0;
            }
        }

        ManageCurrentPlayer();
        ManagePlayerFactionUnit();
    }

    private void ManagePlayerFactionUnit()
    {
        switch (playerFaction)
        {
            case Identification.Faction.AngloIsles:
                playerUnits = Finder.FactionManager.AngloIsles;
                break;

            case Identification.Faction.IronLegion:
                playerUnits = Finder.FactionManager.IronLegion;
                break;

            case Identification.Faction.SolarEmpire:
                playerUnits = Finder.FactionManager.SolarEmpire;
                break;

            case Identification.Faction.Tundran:
                playerUnits = Finder.FactionManager.Tundra;
                break;

            case Identification.Faction.WesternFrontier:
                playerUnits = Finder.FactionManager.WesternFrontier;
                break;

            case Identification.Faction.Xylvanian:
                playerUnits = Finder.FactionManager.Xylvanian;
                break;
            default:
                break;
        }
    }
 
    //This method controls the CurrentPlayer
    private void ManageCurrentPlayer()
    {
        CurrentPlayer.layer = 6;
        vcm.Follow = CurrentPlayer.transform;
        vcm.LookAt = CurrentPlayer.transform;
        CinemachineTransposer transposer = vcm.GetCinemachineComponent<CinemachineTransposer>();
        if (CurrentPlayer.GetComponent<CameraInfo>())
        {
            transposer.m_FollowOffset = CurrentPlayer.GetComponent<CameraInfo>().FollowOffset;
        }
        else if (CurrentPlayer.GetComponent<CameraInfo>() == false)
        {
            Debug.LogError("Current Player does not have a CameraInfo Component! Please add a component to ensure that the camera follows the current player properly. ");
        }
    }

    public void UpdateUnit(Unit unit)
    {
        if (unit == null)
        {
            Debug.LogWarning("A null unit was told to be updated. Prevent his from happening!");
        }
        else
        {
            unit.Units = unit.Units.Where(p => p).ToList();
            foreach (GameObject gameObject in playerTeam)
            {
                if (gameObject.GetComponent<Identification>().unitClass == unit.unitClass && unit.Units.Contains(gameObject) == false)
                {
                    unit.Units.Add(gameObject);
                }
            }

            foreach (GameObject UNIT in unit.Units.ToArray())
            {
                if (UNIT != null)
                {
                    if (UNIT.gameObject.GetComponent<Health>().CurrentHealth <= 0 || UNIT.gameObject.activeSelf == false)
                    {
                        unit.Units.Remove(UNIT);
                    }

                    if (UNIT.GetComponent<TargetingSystem>())
                    {
                        UNIT.GetComponent<TargetingSystem>().enemyFactions = enemyFactions;
                    }
                }
                else
                {
                    unit.Units.Remove(UNIT);
                }
            }
        }
    }
    
}
