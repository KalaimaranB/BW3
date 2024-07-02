using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class TargetingSystem : MonoBehaviour
{
    public List<Identification.UnitType> unitsItCanAttack;
    public List<Identification.Faction> enemyFactions;
    public enum TargetingMethods {PhysicsOverlapSphere, PhysicsOverlapBox};
    public TargetingMethods TargetingMethod = TargetingMethods.PhysicsOverlapSphere;
    [ShowIf("TargetingMethod",TargetingMethods.PhysicsOverlapSphere)]
    public float Range;

    [ShowIf("TargetingMethod", TargetingMethods.PhysicsOverlapBox)]
    public Vector3 BoxRange;
    [ShowIf("TargetingMethod", TargetingMethods.PhysicsOverlapBox)]
    public Transform BoxCenter;


    private Collider[] colliders;
    [Header("Targets")]
    public GameObject CurrentTarget;

    [ReadOnly]
    public GameObject forcedTarget;
    public List<GameObject> Targets;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ManageTargetingSystem();
    }

    private void OnDrawGizmos()
    {
        if (gameObject.GetComponent<AutoGun>())
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(BoxCenter.position,BoxRange);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(gameObject.transform.position, Range);
        }
    }

    private void ManageTargetingSystem()
    {
        switch (TargetingMethod)
        {
            case TargetingMethods.PhysicsOverlapSphere:
                colliders = Physics.OverlapSphere(gameObject.transform.position, Range);
                break;

            case TargetingMethods.PhysicsOverlapBox:
                colliders = Physics.OverlapBox(BoxCenter.position, BoxRange);
                break;
            default:
                Debug.LogWarning("Invalid Targeting Method");
                break;
        }

        colliders = colliders.Where(p => p).ToArray();
        Targets = Targets.Where(y => y).ToList();

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.GetComponent<Identification>() != null)
            {
                Identification id = collider.gameObject.GetComponent<Identification>();
                if (enemyFactions.Any(x=>x == id.faction) &&
                    unitsItCanAttack.Any(y=>y == id.unitType) &&
                    id.unitType != Identification.UnitType.Facility &&
                    id.currentStatus != Identification.Status.Captured &&
                    collider.gameObject.activeSelf == true &&
                    Targets.Contains(collider.gameObject) == false)
                {
                    Targets.Add(collider.gameObject);
                }
            }
        }

        foreach (GameObject target in Targets.ToArray())
        {
            if (colliders.Any(i=>i.gameObject == target) == false)
            {
                Targets.Remove(target);
            }

            if (target == null)
            {
                Targets.Remove(target);
            }
            else if (target!=null)
            {
                if (target.GetComponent<Health>().CurrentHealth <= 0 || target.activeSelf == false)
                {
                    Targets.Remove(target);
                }
            }
        }

        if (Targets.Count > 0)
        {
            CurrentTarget = Targets[0];
        }
        else if (Targets.Count == 0)
        {
            CurrentTarget = null;
        }
    }
}
