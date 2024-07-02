using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PlayerUnitIcon : MonoBehaviour
{
    [HideInInspector]
    public FactionTypes.Unit currentUnit;
    [HideInInspector]
    public List<GameObject> Units;

    public bool isAllUnitIcon = false;
    [Header("Local Variables")]
    public RawImage unitIcon;
    public Text unitName;
    public Text numberOfUnits;
    public RawImage unitStatus;

    [Header("Unit Status Icons")]
    public Texture Waiting;
    public Texture Attacking;
    public Texture Following;
    public Texture Mixed;

    [Range(0,1)]
    public float Transparency=1;
    public Image imageBackground;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        AssignTransparency();
    }

    public void AssignIcon(Texture UnitIcon, List<GameObject> units, string Name)
    {
        unitName.text = Name;
        unitIcon.texture = UnitIcon;
        numberOfUnits.text = units.Count.ToString();
        Units = units;
        AssignStatus(units);
    }

    public void AssignIcon(Texture UnitIcon, List<GameObject> units, string Name, FactionTypes.Unit unit)
    {
        Units = units;
        currentUnit = unit;
        unitName.text = Name;
        unitIcon.texture = UnitIcon;
        numberOfUnits.text = units.Count.ToString();

        if (unit.Units.Contains(PlayerManager.CurrentPlayer))
        {
            numberOfUnits.text = (units.Count - 1).ToString();
        }

        AssignStatus(units);
    }

    private void AssignStatus(List<GameObject> units)
    {
        if (units.All(i => i.GetComponent<Identification>().unitStatus == FactionTypes.Unit.UnitStatus.Waiting))
        {
            unitStatus.texture = Waiting;
        }
        else if (units.All(i => i.GetComponent<Identification>().unitStatus == FactionTypes.Unit.UnitStatus.Following))
        {
            unitStatus.texture = Following;
        }
        else if (units.All(i => i.GetComponent<Identification>().unitStatus == FactionTypes.Unit.UnitStatus.Attacking))
        {
            unitStatus.texture = Attacking;
        }
        else
        {
            unitStatus.texture = Mixed;
        }
    }

    public void SwitchPlayer()
    {
        if (Units.Count>0 && PlayerManager.CurrentPlayer != currentUnit.Units[0] && isAllUnitIcon == false)
        {
            PlayerManager.CurrentPlayer = currentUnit.Units[0];
        }
    }

    public void AssignTransparency()
    {
        imageBackground.ChangeAlpha(Transparency);
        unitIcon.ChangeAlpha(Transparency);
        unitStatus.ChangeAlpha(Transparency);
        unitName.ChangeAlpha(Transparency);
        numberOfUnits.ChangeAlpha(Transparency);
    }
}
