using UnityEngine;

[RequireComponent(typeof(TargetingSystem))]
public class AutoGun : MonoBehaviour
{
    private TargetingSystem ts;
    [SerializeField] private GameObject barrel;
    private Quaternion startRotation;
    [SerializeField] private Quaternion offset;

    [SerializeField] private float ReloadTime;
    [SerializeField] private GameObject Projectile;
    [SerializeField] private GameObject FiringPosition;
    private bool canShoot = false;

    [SerializeField] private GameObject gameObjectForProjectileToIgnore;
    // Start is called before the first frame update
    void Start()
    {
        startRotation = gameObject.transform.rotation;
        ts = gameObject.GetComponent<TargetingSystem>();
        if (ts.TargetingMethod!= TargetingSystem.TargetingMethods.PhysicsOverlapBox)
        {
            Debug.LogWarning("It is recommended to use the box targeting method for autoguns!");
        }

        InvokeRepeating("LaunchProjectile", 0, ReloadTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (ts.CurrentTarget!=null)
        {
            barrel.transform.LookAt(ts.CurrentTarget.transform.position);
            canShoot = true;
        }
        else
        {
            canShoot = false;
            barrel.transform.rotation = startRotation;
        }
    }


    private void LaunchProjectile()
    {
        if (canShoot == true)
        {
            GameObject proj = Instantiate(Projectile, FiringPosition.transform.position, FiringPosition.transform.rotation);
            if (gameObject == gameObjectForProjectileToIgnore)
            {
                proj.GetComponent<Projectile>().Shooter = gameObject;
            }
            else if (gameObject!=gameObjectForProjectileToIgnore)
            {
                proj.GetComponent<Projectile>().Shooter = gameObjectForProjectileToIgnore;
            }

        }
    }
}
