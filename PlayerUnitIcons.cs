using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class PlayerUnitIcons : MonoBehaviour
{
    [Header("Current Player")]
    public RawImage CurrentPlayerIcon;
    public Text CurrentPlayerName;
    public ProgressBarCircle CurrentPlayerHealthCircle;
    public PlayerUnitIcon allUnitsIcon;
    public Texture factionIcon;

    private float CurrentPlayerHealthPercentage = 100;

    public List<PlayerUnitIcon> unitIcons;
    [SerializeField]
    private List<PlayerUnitIcon> activeUnitIcons = new List<PlayerUnitIcon>();

    [SerializeField]
    private List<FactionTypes.Unit> activeUnits;

    [ReadOnly]
    public PlayerUnitIcon SelectedIcon;

    [Range(0, 1)]
    public float unselectedIconTransparency = 0.75f;

    // Start is called before the first frame update
    void Start()
    {
        activeUnits = Finder.PlayerManager.playerUnits.SmartFindActiveUnits();
        activeUnitIcons = unitIcons.Where(i => i.gameObject.activeSelf == true).ToList();
        SelectedIcon = allUnitsIcon;
        allUnitsIcon.Transparency = 1;
        foreach (PlayerUnitIcon pui in activeUnitIcons)
        {
            pui.Transparency = unselectedIconTransparency;
        }
    }

    // Update is called once per frame
    void Update()
    {

        activeUnits = Finder.PlayerManager.playerUnits.SmartFindActiveUnits();

        activeUnitIcons = unitIcons.Where(i => i.gameObject.activeSelf == true).ToList();

        AssignCurrentPlayerIcon(PlayerManager.CurrentPlayer);
        AssignTotalPlayersIcon();

        if (activeUnits.Count > unitIcons.Count)
        {
            Debug.LogWarning("There are more active units than UI unit bars! Either add more unit icons or prevne too many unit types from being produced. ");
        }

        //This loop manages the unit Icons active state
        for (var i = 0; i < unitIcons.Count; i++)
        {
            if ((activeUnits.Count - 1) >= i)
            {
                unitIcons[i].gameObject.SetActive(true);
            }
            else
            {
                unitIcons[i].gameObject.SetActive(false);
            }
        }

        //This loop assign the icons for the first time
        for (var i = 0; i < activeUnits.Count; i++)
        {
            if (activeUnitIcons.Any(j => j.currentUnit == activeUnits[i]) == false)
            {
                //The first time
                AssignUnitIcon(activeUnits[i], unitIcons[i]);
            }




            //This used to be in another loop!
            FactionTypes.Unit targetUnit = Finder.PlayerManager.playerUnits.FindUnit(unitIcons[i].currentUnit.unitClass);
            AssignUnitIcon(targetUnit, unitIcons[i]);
        }

        ManageUnitIcons();
        SelectedIcon.Transparency = 1;

        if (Input.GetKeyDown(KeyCode.F))
        {
            ChangeUnitStatus();
        }
    }




    private void ChangeUnitStatus()
    {
        //If a lockedObject exists
        if (Finder.CursorManager.LockedTarget != null && Finder.CursorManager.TargetLocked == true)
        {
            if (Finder.PlayerManager.enemyFactions.Contains(Finder.CursorManager.LockedTarget.GetComponent<Identification>().faction))
            {
                Debug.Log("Locked --> Attack!");
                SetUnitListStatus(SelectedIcon.Units, FactionTypes.Unit.UnitStatus.Attacking);
                foreach (GameObject go in SelectedIcon.Units)
                {
                    go.GetComponent<TargetingSystem>().forcedTarget = Finder.CursorManager.LockedTarget;
                }
            }
        }

        //If all waiting
        else if (SelectedIcon.Units.All(i => i.GetComponent<Identification>().unitStatus == FactionTypes.Unit.UnitStatus.Waiting))
        {
            Debug.Log("Waiting --> Following");
            SetUnitListStatus(SelectedIcon.Units, FactionTypes.Unit.UnitStatus.Following);
            foreach (GameObject go in SelectedIcon.Units)
            {
                go.GetComponent<TargetingSystem>().forcedTarget = null;
            }
        }

        //If all following
        else if (SelectedIcon.Units.All(i => i.GetComponent<Identification>().unitStatus == FactionTypes.Unit.UnitStatus.Following))
        {
            Debug.Log("Following --> Waiting");
            SetUnitListStatus(SelectedIcon.Units, FactionTypes.Unit.UnitStatus.Waiting);
            foreach (GameObject go in SelectedIcon.Units)
            {
                go.GetComponent<TargetingSystem>().forcedTarget = null;
            }
        }

        //If it is a mixed status, then make them all wait
        else
        {
            Debug.Log("Mixed --> Waiting");
            SetUnitListStatus(SelectedIcon.Units, FactionTypes.Unit.UnitStatus.Waiting);
            foreach (GameObject go in SelectedIcon.Units)
            {
                go.GetComponent<TargetingSystem>().forcedTarget = null;
            }
        }

    }

    private void SetUnitListStatus(List<GameObject> units, FactionTypes.Unit.UnitStatus newStatus)
    {
        foreach (GameObject go in units)
        {
            go.GetComponent<Identification>().unitStatus = newStatus;
        }
    }

    private void ManageUnitIcons()
    {

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //If the selected icon is the all unit Icon
            if (SelectedIcon == allUnitsIcon && activeUnitIcons.Count > 0)
            {
                SelectedIcon = activeUnitIcons[0];
                allUnitsIcon.Transparency = unselectedIconTransparency;
            }
            //If it is one of the normal icons
            else if (activeUnitIcons.Contains(SelectedIcon))
            {
                int index = activeUnitIcons.FindIndex(x => x.Equals(SelectedIcon));
                if (index < activeUnitIcons.Count - 1)
                {
                    SelectedIcon = activeUnitIcons[index + 1];
                    activeUnitIcons[index].Transparency = unselectedIconTransparency;
                }

                else if (index == activeUnitIcons.Count - 1)
                {
                    SelectedIcon = allUnitsIcon;
                    activeUnitIcons[index].Transparency = unselectedIconTransparency;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //If the selected icon is the all unit icon
            if (SelectedIcon == allUnitsIcon && activeUnitIcons.Count > 0)
            {
                SelectedIcon = activeUnitIcons[activeUnitIcons.Count - 1];
                allUnitsIcon.Transparency = unselectedIconTransparency;
            }

            //If it is one of the normal icons
            else if (activeUnitIcons.Contains(SelectedIcon))
            {
                int index = activeUnitIcons.FindIndex(x => x.Equals(SelectedIcon));

                if (index > 0)
                {
                    SelectedIcon = activeUnitIcons[index - 1];
                    activeUnitIcons[index].Transparency = unselectedIconTransparency;
                }

                if (index == 0)
                {
                    SelectedIcon = allUnitsIcon;
                    activeUnitIcons[index].Transparency = unselectedIconTransparency;
                }

            }
        }
    }

    private void AssignUnitIcon(FactionTypes.Unit unit, PlayerUnitIcon icon)
    {
        icon.AssignIcon(unit.unitIcon, unit.Units, unit.UnitName, unit);
    }

    private void AssignTotalPlayersIcon()
    {
        allUnitsIcon.AssignIcon(factionIcon, PlayerManager.playerTeam, "All");
    }

    private void AssignCurrentPlayerIcon(GameObject currentPlayer)
    {
        FactionTypes.Unit currentUnit = Finder.PlayerManager.playerUnits.FindUnit(currentPlayer);
        if (currentUnit!=null)
        {
            if (currentUnit.unitIcon != null)
            {
                CurrentPlayerIcon.texture = currentUnit.unitIcon;
            }
            else
            {
                Debug.LogWarning(currentUnit.UnitName + " is the current player's unit. It does not have a unit icon, please assign one.");
            }
            CurrentPlayerName.text = currentPlayer.GetComponent<Identification>().Name;
            Health currentPlayerHealth = PlayerManager.CurrentPlayer.GetComponent<Health>();
            CurrentPlayerHealthPercentage = (currentPlayerHealth.CurrentHealth / currentPlayerHealth.MaxHealth) * 100;

            CurrentPlayerHealthCircle.UpdateValue(CurrentPlayerHealthPercentage);
        }
        else
        {
            //Current Unit was null
        }
    }
}
