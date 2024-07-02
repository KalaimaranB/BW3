using UnityEngine;
using System.Collections.Generic;
using System;
using NaughtyAttributes;


public class LevelManager : MonoBehaviour
{
    public DialogueBox dialogueBox;

    [Header("Player")]
    [ReadOnly]
    public TeamData playerData;

    [Header("Enemy Data")]
    [ReadOnly]
    public TeamData EnemyData;

    public enum team {player, enemy};

    public enum LevelState {Incomplete, Failed, Complete};
    public LevelState currentLevelState;

    public event EventHandler<OnUnitStatusChangedEventArgs> OnUnitCreated;
    public event EventHandler<OnUnitStatusChangedEventArgs> OnUnitDestroyed;

    public class OnUnitStatusChangedEventArgs : EventArgs
    {
        public Identification id;
        public GameObject unit;
        public OnUnitStatusChangedEventArgs(Identification identification, GameObject Unit)
        {
            id = identification;
            unit = Unit;
        }
    }

    private ObjectiveManager om;

    public void Awake()
    {
        om = GetComponent<ObjectiveManager>();
        OnUnitCreated += LevelManager_OnUnitCreated;
        OnUnitDestroyed += LevelManager_OnUnitDestroyed;
    }

    public void Update()
    {
        switch (om.primaryObjectiveState())
        {
            case ObjectiveManager.Objective.ObjectiveState.Complete:
                currentLevelState = LevelState.Complete;
                break;

            case ObjectiveManager.Objective.ObjectiveState.Failed:
                currentLevelState = LevelState.Failed;
                break;

            case ObjectiveManager.Objective.ObjectiveState.InProgress:
                currentLevelState = LevelState.Incomplete;
                break;
            default:
                break;
        }
    }


    #region TeamData & Methods

    [System.Serializable]
    public class TeamData
    {
        [Header("Infantry")]
        public int totalInfantry;
        public int currentInfantry;
        [Header("Vehicles")]
        public int totalVehicles;
        public int currentVehicles;
        [Header("Aviation")]
        public int totalAviation;
        public int currentAviation;
        [Header("Navy")]
        public int totalShips;
        public int currentShips;
    }

    private void UpdateTeamInfantry(TeamData td, bool updateTotal, int value)
    {
        if (updateTotal == true)
        {
            td.totalInfantry += value;
            td.currentInfantry += value;
        }
        else
        {
            td.currentInfantry += value;
        }
    }

    private void UpdateTeamVehicle(TeamData td, bool updateTotal, int value)
    {
        if (updateTotal == true)
        {
            td.totalVehicles += value;
            td.currentVehicles += value;
        }
        else
        {
            td.currentVehicles += value;
        }
    }

    private void UpdateTeamAviation(TeamData td, bool updateTotal, int value)
    {
        if (updateTotal == true)
        {
            td.totalAviation += value;
            td.currentAviation += value;
        }
        else
        {
            td.currentAviation += value;
        }
    }

    private void UpdateTeamShips(TeamData td, bool updateTotal, int value)
    {
        if (updateTotal == true)
        {
            td.totalShips += value;
            td.currentShips += value;
        }
        else
        {
            td.currentShips += value;
        }
    }

    #endregion TeamData & Methods

    #region UnitStatusChangedEvents

    private void LevelManager_OnUnitCreated(object sender, OnUnitStatusChangedEventArgs e)
    {
        Identification.UnitType unitType = e.id.unitType;
        TeamData targetData = null;
        //It is a player
        if (e.id.faction == PlayerManager.PlayerFaction)
        {
            targetData = playerData;
        }

        //If it an enemy
        else if (Finder.PlayerManager.enemyFactions.Contains(e.id.faction))
        {
            targetData = EnemyData;
        }

        //Update the unit with targetData
        if (targetData!=null)
        {
            if (unitType == Identification.UnitType.Aviation || unitType == Identification.UnitType.Helicopter)
            {
                UpdateTeamAviation(targetData, true, 1);
            }
            else if (unitType == Identification.UnitType.HeavyVehicle || unitType == Identification.UnitType.LightVehicle)
            {
                UpdateTeamVehicle(targetData, true, 1);
            }
            else if (unitType == Identification.UnitType.LightShip || unitType == Identification.UnitType.HeavyShip)
            {
                UpdateTeamShips(targetData, true, 1);
            }
            else if (unitType == Identification.UnitType.Infantry)
            {
                UpdateTeamInfantry(targetData, true, 1);
            }
        }
    }

    private void LevelManager_OnUnitDestroyed(object sender, OnUnitStatusChangedEventArgs e)
    {
        Identification.UnitType unitType = e.id.unitType;
        TeamData targetData = null;
        //It is a player
        if (e.id.faction == PlayerManager.PlayerFaction)
        {
            targetData = playerData;
        }

        //If it an enemy
        else if (Finder.PlayerManager.enemyFactions.Contains(e.id.faction))
        {
            targetData = EnemyData;
        }

        //Update the unit with targetData
        if (targetData != null)
        {
            if (unitType == Identification.UnitType.Aviation || unitType == Identification.UnitType.Helicopter)
            {
                UpdateTeamAviation(targetData, false, -1);
            }
            else if (unitType == Identification.UnitType.HeavyVehicle || unitType == Identification.UnitType.LightVehicle)
            {
                UpdateTeamVehicle(targetData, false, -1);
            }
            else if (unitType == Identification.UnitType.LightShip || unitType == Identification.UnitType.HeavyShip)
            {
                UpdateTeamShips(targetData, false, -1);
            }
            else if (unitType == Identification.UnitType.Infantry)
            {
                UpdateTeamInfantry(targetData, false, -1);
            }
        }
    }


    public void CallOnUnitCreated(object sender, Identification identification, GameObject Unit)
    {
        OnUnitStatusChangedEventArgs eventArgs = new OnUnitStatusChangedEventArgs(identification, Unit);
        OnUnitCreated?.Invoke(sender, eventArgs);
    }
    public void CallOnUnitDestroyed(object sender, Identification identification, GameObject Unit)
    {
        OnUnitStatusChangedEventArgs eventArgs = new OnUnitStatusChangedEventArgs(identification, Unit);
        OnUnitDestroyed?.Invoke(sender, eventArgs);
    }
    #endregion UnitStatusChangedEvents
}
