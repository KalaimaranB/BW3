using System;
using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float MaxHealth;
    public float CurrentHealth;
    public enum deathStyle {DestroyOnDeath, CreateObjectThenDestroy, CreateObjectOnly, Ignore};

    [Tooltip("What this unit should do when it's health falls or equal 0")]
    public deathStyle DeathStyle;

    public enum ArmorType { Infantry, Helicopter, Aviation, LightVehicle, HeavyVehicle, LightShip, HeavyShip, Building, Obstacle};
    public ArmorType armorType;
    public GameObject explosion;
    public Vector3 explosionOffset = new Vector3(0, 0, 0);

    public event EventHandler UnitDied;
    public void Update()
    {
        if (CurrentHealth<=0)
        {
            UnitDied?.Invoke(this, EventArgs.Empty);
            switch (DeathStyle)
            {
                case deathStyle.DestroyOnDeath:
                    Destroy(gameObject);
                    break;

                case deathStyle.CreateObjectThenDestroy:
                    GameObject t = Instantiate(explosion, gameObject.transform.position + explosionOffset, explosion.transform.rotation);
                    if (t!=null)
                    {
                        Destroy(gameObject);
                    }
                    break;

                case deathStyle.CreateObjectOnly:
                    Instantiate(explosion, gameObject.transform.position + explosionOffset, explosion.transform.rotation);
                    break;

                case deathStyle.Ignore:
                    break;

                default:
                    Debug.LogWarning(DeathStyle+" --> This is not a valid death style. Please select a valid method.");
                    break;
            }
        }
    }

    public void ForceDeath(bool createExplosion)
    {
        StartCoroutine(Die(createExplosion));
    }

    public IEnumerator Die(bool createExplosion)
    {
        if (createExplosion == false)
        {
            UnitDied?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
        }

        else if (createExplosion == true)
        {
            if (explosion != null)
            {
                GameObject exp = Instantiate(explosion, gameObject.transform.position + explosionOffset, explosion.transform.rotation);
                do
                {
                    yield return null;
                } while (exp == null);
                UnitDied?.Invoke(this, EventArgs.Empty);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("A die command was issued to " + gameObject.name + ", but this gameObject has no explosion. The order was ignored.");
            }
        }
    }

    //This method will allow the unit to take damage
    public void TakeDamage(BasicUnit.Damage damage)
    {
        //This switch command ensures that the unit receives the correct amount of damage based on it's armor type
        switch (armorType)
        {
            case ArmorType.Aviation:
                CurrentHealth -= damage.AviationDamage;
                break;

            case ArmorType.Building:
                CurrentHealth -= damage.BuildingDamage;
                break;

            case ArmorType.HeavyShip:
                CurrentHealth -= damage.HeavyShipDamage;
                break;

            case ArmorType.Helicopter:
                CurrentHealth -= damage.HelicopterDamage;
                break;

            case ArmorType.Infantry:
                CurrentHealth -= damage.InfantryDamage;
                break;

            case ArmorType.LightShip:
                CurrentHealth -= damage.LightShipDamage;
                break;

            case ArmorType.LightVehicle:
                CurrentHealth -= damage.LightVehicleDamage;
                break;

            case ArmorType.HeavyVehicle:
                CurrentHealth -= damage.HeavyVehicleDamage;
                break;

            case ArmorType.Obstacle:
                CurrentHealth -= damage.ObstacleDamage;
                break;


            default:
                Debug.LogError("Damage has an invalid armor type! Did you forget to add the armor type? The invalid damage type is "+ damage+". The armor type is " + armorType);
                break;
        }
    }

    //This method will increase health 
    public void HealUnit(float Health)
    {
        if (CurrentHealth < MaxHealth)
        {
            CurrentHealth += Health;
        }
    }

}
