using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Required components
[RequireComponent(typeof(Identification))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CameraInfo))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(UnitSFX))]
public class BasicUnit : MonoBehaviour
{
    //Variables for all units
    [HideInInspector] public Identification identification;
    [HideInInspector] public Health health;
    [HideInInspector] public UnitSFX SFX;
    [HideInInspector] public AudioSource ad;
    
    public void GetStartValues()
    {
        identification = GetComponent<Identification>();
        health = GetComponent<Health>();
        SFX = GetComponent<UnitSFX>();
        ad = GetComponent<AudioSource>();

        if (gameObject.GetComponent<TargetingSystem>())
        {
            gameObject.GetComponent<TargetingSystem>().enemyFactions = Finder.PlayerManager.enemyFactions;
        }
    }

    //This method will cast a basic ray from the mouse Postion
    public Vector3 CastBasicRay()
    {
        RaycastHit RayHit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RayHit))
        {
            GameObject ObjectHit = RayHit.transform.gameObject;
            return RayHit.point;
        }
        return Vector3.zero;
    }

    //This method will have a gameObject look at another over time
    public void GameObjectLookAtAnotherOverTime(GameObject original, Vector3 targetPoint, float speed, bool lockX, bool lockY, bool lockZ)
    {
        Quaternion newRotation = Quaternion.LookRotation(original.transform.position - targetPoint);

        //Lock Rotation
        if (lockX == true) { newRotation.x = 0; }
        if (lockY == true) { newRotation.y = 0; }
        if (lockZ == true) { newRotation.z = 0; }

        original.transform.rotation = Quaternion.Slerp(original.transform.rotation, newRotation, Time.deltaTime * speed);
    }


    //This method will clamp a given angle
    public float ClampAngle(float angle, float from, float to)
    {
        if (angle < 0f)
        {
            angle = 360 + angle;
        }
        if (angle > 180f)
        {
            return Mathf.Max(angle, 360 + from);
        }
        return Mathf.Min(angle, to);
    }

    //This method will check if a gameObject looks at another object
    public bool IsFacingObject(GameObject original, GameObject target, float Range)
    {
        RaycastHit objectHit;
        Vector3 fwd = original.transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(original.transform.position, fwd * Range, Color.green);
        if (Physics.Raycast(original.transform.position, fwd, out objectHit, Range))
        {
            if (objectHit.collider.gameObject == target)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }


    //This class contains the damage values
    [System.Serializable]
    public class Damage
    {
        public float InfantryDamage, HelicopterDamage, AviationDamage, LightVehicleDamage, HeavyVehicleDamage, LightShipDamage, HeavyShipDamage, BuildingDamage, ObstacleDamage;
    }
    //This class contains armament details
    [System.Serializable]
    public class Armament
    {
        public GameObject Barrel;
        public GameObject FiringPoint;
        public Vector3 ProjectileSpawnOffset;
        public Vector3 BarrelRotationOffset = new Vector3(0,180,0);
    }
}