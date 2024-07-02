using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace FactionTypes
{
    [System.Serializable]
    public class FactionUnits
    {
        public Identification.Faction Faction;
        public List<Unit> infantryUnits = new List<Unit>();
        public List<Unit> vehicleUnits = new List<Unit>();
        public List<Unit> aviationUnits = new List<Unit>();
        public List<Unit> navalUnits = new List<Unit>();

        //Assign Faction units
        public FactionUnits(Identification.Faction faction)
        {
            Faction = faction;
            //Add in the infantry
            infantryUnits.Add(new Unit("Rifle Grunt", Identification.UnitClass.RifleGrunt));
            infantryUnits.Add(new Unit("Assault Veteran", Identification.UnitClass.AssaultVeteran));
            infantryUnits.Add(new Unit("Flame Veteran", Identification.UnitClass.FlameVeteran));
            infantryUnits.Add(new Unit("AntiAirVeteran", Identification.UnitClass.AntiAirVeteran));
            infantryUnits.Add(new Unit("MortarVeteran", Identification.UnitClass.MortarVeteran));
            infantryUnits.Add(new Unit("BazookaVeteran", Identification.UnitClass.BazookaVeteran));

            //Add in the vehicles
            vehicleUnits.Add(new Unit("Recon", Identification.UnitClass.Recon));
            vehicleUnits.Add(new Unit("Anti-Air Vehicle", Identification.UnitClass.AntiAirVehicle));
            vehicleUnits.Add(new Unit("Artillery", Identification.UnitClass.Artillery));
            vehicleUnits.Add(new Unit("Light Tank", Identification.UnitClass.LightTank));
            vehicleUnits.Add(new Unit("Heavy Tank", Identification.UnitClass.HeavyTank));
            vehicleUnits.Add(new Unit("Battlestation", Identification.UnitClass.Battlestation));

            //Add in the aviation
            aviationUnits.Add(new Unit("Air Transport", Identification.UnitClass.AirTransport));
            aviationUnits.Add(new Unit("Gunship", Identification.UnitClass.Gunship));
            aviationUnits.Add(new Unit("Fighter", Identification.UnitClass.Fighter));
            aviationUnits.Add(new Unit("Bomber", Identification.UnitClass.Bomber));
            aviationUnits.Add(new Unit("Strato-Destroyer", Identification.UnitClass.StratoDestroyer));

            //Add in the navy
            navalUnits.Add(new Unit("Naval Transport", Identification.UnitClass.NavalTransport));
            navalUnits.Add(new Unit("Submarine", Identification.UnitClass.Submarine));
            navalUnits.Add(new Unit("Frigate", Identification.UnitClass.Frigate));
            navalUnits.Add(new Unit("Battleship", Identification.UnitClass.Battleship));
            navalUnits.Add(new Unit("Dreadnought", Identification.UnitClass.Dreadnought));
        }

        private Unit SearchList(Unit unit, List<Unit> listToSearch)
        {
            for (int i = 0; i < listToSearch.Count; i++)
            {
                if (unit == listToSearch[i])
                {
                    return listToSearch[i];
                }
            }
            return null;
        }

        //Find unit given unit
        public Unit FindUnit(Unit unit)
        {
            //First check the infantry
            if (SearchList(unit, infantryUnits)!=null)
            {
                return SearchList(unit, infantryUnits);
            }

            //Then the vehicles
            if (SearchList(unit, vehicleUnits)!=null)
            {
                return SearchList(unit, infantryUnits);
            }

            //Then the navy
            if (SearchList(unit, navalUnits)!=null)
            {
                return SearchList(unit, navalUnits);
            }

            //Then the aviation
            if (SearchList(unit, aviationUnits)!=null)
            {
                return SearchList(unit, aviationUnits);
            }

            //If you still can't find it, then return null and log it
            Debug.Log(unit+" --> This unit can not be found in the infantry, vehicle, naval or aviation units. It does not exist!");
            return null;

        }

        //Find unit given gameObject
        public Unit FindUnit(GameObject unit)
        {
            foreach(Unit inf in infantryUnits)
            {
                foreach(GameObject infUnit in inf.Units)
                {
                    if (infUnit == unit)
                    {
                        return inf;
                    }
                }
            }

            foreach (Unit veh in vehicleUnits)
            {
                foreach (GameObject vehUnit in veh.Units)
                {
                    if (vehUnit == unit)
                    {
                        return veh;
                    }
                }
            }

            foreach (Unit avi in aviationUnits)
            {
                foreach (GameObject aviUnit in avi.Units)
                {
                    if (aviUnit == unit)
                    {
                        return avi;
                    }
                }
            }

            foreach (Unit nav in navalUnits)
            {
                foreach (GameObject navUnit in nav.Units)
                {
                    if (navUnit == unit)
                    {
                        return nav;
                    }
                }
            }

            //The unit was not found
            return null;
        }

        //Find unit given id
        public Unit FindUnit(Identification.UnitClass unitClass)
        {
            foreach(Unit inf in infantryUnits)
            {
                if (inf.unitClass == unitClass)
                {
                    return inf;
                }
            }

            foreach (Unit veh in vehicleUnits)
            {
                if (veh.unitClass == unitClass)
                {
                    return veh;
                }
            }

            foreach (Unit avi in aviationUnits)
            {
                if (avi.unitClass == unitClass)
                {
                    return avi;
                }
            }

            foreach (Unit nav in navalUnits)
            {
                if (nav.unitClass == unitClass)
                {
                    return nav;
                }
            }
            Debug.LogWarning(unitClass+" --> This unit class was not found in infantry, vehicle, aviation or naval units1");
            return null;
        }

        //Find active units
        public List<Unit> FindActiveUnits()
        {
            List<Unit> unitsToReturn = new List<Unit>();

            foreach(Unit inf in infantryUnits)
            {
                if (inf.Units.Count>0 && unitsToReturn.Contains(inf) == false)
                {
                    unitsToReturn.Add(inf);
                }
                if (unitsToReturn.Contains(inf) && inf.Units.Count == 0)
                {
                    unitsToReturn.Remove(inf);
                }
            }

            foreach (Unit veh in vehicleUnits)
            {
                if (veh.Units.Count > 0 && unitsToReturn.Contains(veh) == false)
                {
                    unitsToReturn.Add(veh);
                }
                if (unitsToReturn.Contains(veh) && veh.Units.Count == 0)
                {
                    unitsToReturn.Remove(veh);
                }
            }

            foreach (Unit avi in aviationUnits)
            {
                if (avi.Units.Count > 0 && unitsToReturn.Contains(avi) == false)
                {
                    unitsToReturn.Add(avi);
                }
                if (unitsToReturn.Contains(avi) && avi.Units.Count == 0)
                {
                    unitsToReturn.Remove(avi);
                }
            }

            foreach (Unit nav in navalUnits)
            {
                if (nav.Units.Count > 0 && unitsToReturn.Contains(nav) == false)
                {
                    unitsToReturn.Add(nav);
                }
                if (unitsToReturn.Contains(nav) && nav.Units.Count == 0)
                {
                    unitsToReturn.Remove(nav);
                }
            }



            return unitsToReturn;
        }

        //Find active units ignoring transports
        public List<Unit> SmartFindActiveUnits()
        {
            List<Unit> unitsToReturn = new List<Unit>();

            foreach (Unit inf in infantryUnits)
            {
                if (inf.Units.Count > 0 && unitsToReturn.Contains(inf) == false)
                {
                    unitsToReturn.Add(inf);
                }
                if (unitsToReturn.Contains(inf) && inf.Units.Count == 0)
                {
                    unitsToReturn.Remove(inf);
                }
            }

            foreach (Unit veh in vehicleUnits)
            {
                if (veh.Units.Count > 0 && unitsToReturn.Contains(veh) == false)
                {
                    unitsToReturn.Add(veh);
                }
                if (unitsToReturn.Contains(veh) && veh.Units.Count == 0)
                {
                    unitsToReturn.Remove(veh);
                }
            }

            foreach (Unit avi in aviationUnits)
            {
                if (avi.Units.Count > 0 && unitsToReturn.Contains(avi) == false && avi.unitClass!= Identification.UnitClass.AirTransport)
                {
                    unitsToReturn.Add(avi);
                }
                if (unitsToReturn.Contains(avi) && avi.Units.Count == 0)
                {
                    unitsToReturn.Remove(avi);
                }
            }

            foreach (Unit nav in navalUnits)
            {
                if (nav.Units.Count > 0 && unitsToReturn.Contains(nav) == false && nav.unitClass!= Identification.UnitClass.NavalTransport)
                {
                    unitsToReturn.Add(nav);
                }
                if (unitsToReturn.Contains(nav) && nav.Units.Count == 0)
                {
                    unitsToReturn.Remove(nav);
                }
            }



            return unitsToReturn;
        }


        //Unit Created
        public void UnitCreated(Identification id, GameObject go)
        {
            if (id.unitType == Identification.UnitType.Infantry)
            {
                for (int i = 0; i < infantryUnits.Count; i++)
                {
                    if (infantryUnits[i].unitClass == id.unitClass)
                    {
                        infantryUnits[i].Units.Add(go);
                    }
                }
            }
            else if (id.unitType == Identification.UnitType.Aviation || id.unitType == Identification.UnitType.Helicopter)
            {
                for (int i = 0; i < aviationUnits.Count; i++)
                {
                    if (aviationUnits[i].unitClass == id.unitClass)
                    {
                        aviationUnits[i].Units.Add(go);
                    }
                }
            }
            else if (id.unitType == Identification.UnitType.LightVehicle || id.unitType == Identification.UnitType.HeavyVehicle)
            {
                for (int i = 0; i < vehicleUnits.Count; i++)
                {
                    if (vehicleUnits[i].unitClass == id.unitClass)
                    {
                        vehicleUnits[i].Units.Add(go);
                    }
                }
            }
            else if (id.unitType == Identification.UnitType.LightShip || id.unitType == Identification.UnitType.HeavyShip)
            {
                for (int i = 0; i < navalUnits.Count; i++)
                {
                    if (navalUnits[i].unitClass == id.unitClass)
                    {
                        navalUnits[i].Units.Add(go);
                    }
                }
            }
        }

        //Unit Destroyed
        public void UnitDestroyed(Identification id, GameObject go)
        {
            if (id.unitType == Identification.UnitType.Infantry)
            {
                for (int i = 0; i < infantryUnits.Count; i++)
                {
                    if (infantryUnits[i].unitClass == id.unitClass)
                    {
                        infantryUnits[i].Units.Remove(go);
                    }
                }
            }
            else if (id.unitType == Identification.UnitType.Aviation || id.unitType == Identification.UnitType.Helicopter)
            {
                for (int i = 0; i < aviationUnits.Count; i++)
                {
                    if (aviationUnits[i].unitClass == id.unitClass)
                    {
                        aviationUnits[i].Units.Remove(go);
                    }
                }
            }
            else if (id.unitType == Identification.UnitType.LightVehicle || id.unitType == Identification.UnitType.HeavyVehicle)
            {
                for (int i = 0; i < vehicleUnits.Count; i++)
                {
                    if (vehicleUnits[i].unitClass == id.unitClass)
                    {
                        vehicleUnits[i].Units.Remove(go);
                    }
                }
            }
            else if (id.unitType == Identification.UnitType.LightShip || id.unitType == Identification.UnitType.HeavyShip)
            {
                for (int i = 0; i < navalUnits.Count; i++)
                {
                    if (navalUnits[i].unitClass == id.unitClass)
                    {
                        navalUnits[i].Units.Remove(go);
                    }
                }
            }
        }
    }

    [System.Serializable]
    public class Unit
    {
        public string UnitName;
        public Texture unitIcon;
        public List<GameObject> Units = new List<GameObject>();
        public GameObject UnitPrefab;
        public float MaxNumberOfUnits;
        public Identification.UnitClass unitClass;
        public enum UnitStatus {Waiting, Following, Attacking, Mixed};

        public Unit(string unitName, Identification.UnitClass UnitClass)
        {
            UnitName = unitName;
            unitClass = UnitClass;
        }
    }
}
