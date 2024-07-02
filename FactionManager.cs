using UnityEngine;
using FactionTypes;

public class FactionManager : MonoBehaviour
{
    public FactionUnits SolarEmpire = new FactionUnits(Identification.Faction.SolarEmpire);
    public FactionUnits AngloIsles = new FactionUnits(Identification.Faction.AngloIsles);
    public FactionUnits WesternFrontier = new FactionUnits(Identification.Faction.WesternFrontier);
    public FactionUnits Xylvanian = new FactionUnits(Identification.Faction.Xylvanian);
    public FactionUnits Tundra = new FactionUnits(Identification.Faction.Tundran);
    public FactionUnits IronLegion = new FactionUnits(Identification.Faction.IronLegion);

    public void Start()
    {
        Finder.LevelManager.OnUnitCreated += LevelManager_OnUnitCreated;
        Finder.LevelManager.OnUnitDestroyed += LevelManager_OnUnitDestroyed;
    }

    private void LevelManager_OnUnitDestroyed(object sender, LevelManager.OnUnitStatusChangedEventArgs e)
    {
        Unit unit = findUnit(e.id.faction, e.id.unitClass);

        if (unit != null)
        {
            unit.Units.Remove(e.unit);
        }
        else
        {
            Debug.Log("A null object was about to be added!");
        }
    }

    private void LevelManager_OnUnitCreated(object sender, LevelManager.OnUnitStatusChangedEventArgs e)
    {
        switch (e.id.faction)
        {
            case Identification.Faction.AngloIsles:
                AngloIsles.UnitCreated(e.id, e.unit);
                break;

            case Identification.Faction.IronLegion:
                IronLegion.UnitCreated(e.id, e.unit);
                break;

            case Identification.Faction.SolarEmpire:
                SolarEmpire.UnitCreated(e.id, e.unit);
                break;

            case Identification.Faction.Tundran:
                Tundra.UnitCreated(e.id, e.unit);
                break;

            case Identification.Faction.WesternFrontier:
                WesternFrontier.UnitCreated(e.id, e.unit);
                break;

            case Identification.Faction.Xylvanian:
                Xylvanian.UnitCreated(e.id, e.unit);
                break;
            default:
                break;
        }
    }

    private Unit findUnit(Identification.Faction faction, Identification.UnitClass go)
    {
        switch (faction)
        {
            case Identification.Faction.SolarEmpire:
                return SolarEmpire.FindUnit(go);

            case Identification.Faction.AngloIsles:
                return AngloIsles.FindUnit(go);

            case Identification.Faction.IronLegion:
                return IronLegion.FindUnit(go);

            case Identification.Faction.Tundran:
                return Tundra.FindUnit(go);

            case Identification.Faction.WesternFrontier:
                return WesternFrontier.FindUnit(go);

            case Identification.Faction.Xylvanian:
                return Xylvanian.FindUnit(go);
            default:
                Debug.Log(go + " was not found!");
                return null;
        }
    }
}