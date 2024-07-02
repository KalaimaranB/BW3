using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Identification : MonoBehaviour
{
    public enum Faction { NoFaction, SolarEmpire, AngloIsles, WesternFrontier, Tundran, IronLegion, Xylvanian };
    public Faction faction;

    public enum UnitType { Infantry, Helicopter, Aviation, LightVehicle, HeavyVehicle, LightShip, HeavyShip, Building, Facility, Environment};
    public enum Status { PlayerControlled, PlayerOrdered, Captured, Facility, Environment, AIControlled};
    public enum UnitClass {RifleGrunt,AssaultVeteran,FlameVeteran,AntiAirVeteran,MortarVeteran,BazookaVeteran,
                            Recon,AntiAirVehicle,Artillery,LightTank,HeavyTank,Battlestation,
                            AirTransport,Gunship,Fighter,Bomber,StratoDestroyer,
                            NavalTransport,Submarine, Frigate,Battleship,Dreadnought};

    [Tooltip("Building refersr to towers such as MG Tower, RPG Tower etc., while facility refers to HQ, Barracks etc")]
    public UnitType unitType;
    public bool isAUnit;
    [ShowIf("isAUnit")]
    public UnitClass unitClass;

    [ShowIf("isAUnit")]
    public FactionTypes.Unit.UnitStatus unitStatus = FactionTypes.Unit.UnitStatus.Waiting;
    public Status currentStatus;
  
    public string Name;

    [HideInInspector]
    public FactionTypes.Unit unit;

    public void Start()
    {
        StartCoroutine(DelayedStart());
    }

    public IEnumerator DelayedStart()
    {
        yield return null;
        //If player
        if (faction == PlayerManager.PlayerFaction)
        {
            if (GetComponent<TargetingSystem>())
            {
                GetComponent<TargetingSystem>().enemyFactions = Finder.PlayerManager.enemyFactions;
            }
            Finder.LevelManager.CallOnUnitCreated(this, this, gameObject);
        }
        //Or an enemy
        else if (Finder.PlayerManager.enemyFactions.Contains(faction))
        {
            Finder.LevelManager.CallOnUnitCreated(this, this, gameObject);
        }
    }

    public void OnDestroy()
    {
        if (faction == PlayerManager.PlayerFaction)
        {
            Finder.LevelManager.CallOnUnitDestroyed(this, this, gameObject);
        }
        else if (Finder.PlayerManager.enemyFactions.Contains(faction))
        {
            Finder.LevelManager.CallOnUnitDestroyed(this, this, gameObject);
        }
    }
}
