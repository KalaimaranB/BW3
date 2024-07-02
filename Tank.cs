using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof (TargetingSystem))]
public class Tank : BasicUnit
{
    [Header("Tank")]
    [SerializeField] private GameObject Projectile;
    [SerializeField] private Vector3 tankRotationOffset;

    [SerializeField] private bool canShoot = false;
    [SerializeField] private float reloadTime;
    [SerializeField] private float stoppingDistance;

    [Header("Main Turret")]
    [SerializeField] private GameObject MainTurret;
    [Tooltip("The speed the Turret rotates at")]
    [SerializeField] private float TurretRotationSpeed=1;


    [Header("Barrel")]
    [SerializeField] private List<Armament> tankArmaments;

    [Tooltip("The minimum angle of the barrel")]
    [SerializeField] private float MinAngle = -10;

    [Tooltip("The maximum angle of the barrel")]
    [SerializeField] private float MaxAngle = 10;

    private float eulerX;

    private float barrelAngle=0;

    [Header("Movement")]

    [Tooltip("The speed the tank moves at")]
    [SerializeField] private float TankMoveSpeed = 3;

    [Tooltip("The speed the tank rotates at")]
    [SerializeField] private float TankRotationSpeed = 20;

    [Tooltip("This should be half the Tank Rotation Speed")]
    [SerializeField] private float CursorRotationSpeed = 10;


    private RaycastHit RayHit;
    private Vector3 HitPoint;
    private NavMeshAgent agent;
    private TargetingSystem TS;
    // Start is called before the first frame update
    void Start()
    {
        GetStartValues();
        TS = GetComponent<TargetingSystem>();
        agent = GetComponent<NavMeshAgent>();

        InvokeRepeating("FireProjectile", 0,reloadTime);

        if (reloadTime ==0)
        {
            Debug.LogWarning("The reload time is 0. Unless you increase it, projectiles won't fire! Assign it to "+gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        HitPoint = CastBasicRay();
        if (gameObject == PlayerManager.CurrentPlayer)
        {
            if (Input.GetMouseButton(0))
            {
                canShoot = true;
            }
            else
            {
                canShoot = false;
            }

            MoveTank();

            if (Finder.CursorManager.LockedTarget == null)
            {
                RotateTankBasedOnCursor();
                RotateMainTurretBasedOnCursor();
                CursorBasedBarrelAiming();
            }
            else if (Finder.CursorManager.LockedTarget !=null)
            {
                MakeMainTurretLookAt(Finder.CursorManager.LockedTarget.transform.position, 1);
                ForceBarrelAimAtPos(Finder.CursorManager.LockedTarget.transform.position);
            }

            if (TS.forcedTarget==null && identification.unitStatus == FactionTypes.Unit.UnitStatus.Attacking)
            {
                identification.unitStatus = FactionTypes.Unit.UnitStatus.Waiting;
            }
            agent.enabled = false;
        }
        else if (identification.currentStatus != Identification.Status.AIControlled && identification.currentStatus!= Identification.Status.Captured)
        {
            agent.enabled = true;
            switch (identification.unitStatus)
            {
                case FactionTypes.Unit.UnitStatus.Waiting:
                    Wait();
                    break;

                case FactionTypes.Unit.UnitStatus.Following:
                    agent.isStopped = false;
                    FollowCurrentPlayer();
                    break;

                case FactionTypes.Unit.UnitStatus.Attacking:
                    agent.isStopped = false;

                    if (TS.forcedTarget!=null) {AttackTarget(TS.forcedTarget, true);}
                    else {identification.unitStatus = FactionTypes.Unit.UnitStatus.Waiting;}

                    break;
                default:
                    break;
            }
        }

        for (int i = 0; i < tankArmaments.Count; i++)
        {
            barrelAngle = tankArmaments[i].Barrel.transform.eulerAngles.x;
        }
    }


    private void Wait()
    {
        agent.isStopped = true;
        if (TS.CurrentTarget!=null)
        {
            AttackTarget(TS.CurrentTarget,false);
        }
        if (TS.CurrentTarget == null && TS.forcedTarget == null)
        {
            canShoot = false;
        }
    }

    private void AttackTarget(GameObject target, bool moveTo)
    {
        
        MakeMainTurretLookAt(target.transform.position, TurretRotationSpeed);
        ForceBarrelAimAtPos(target.transform.position);

        if (Vector3.Distance(gameObject.transform.position, target.transform.position)>stoppingDistance && moveTo == true)
        {
            agent.SetDestination(target.transform.position);
        }

        if (IsFacingObject(tankArmaments[0].FiringPoint,target, TS.Range))
        {
            canShoot = true;
        }
        else
        {
            canShoot = false;
        }
    }

    //This method will fire the projectile at the spawnLocation using spawnRotation
    public void FireProjectile()
    {
        if (canShoot == true)
        {
            foreach (Armament TA in tankArmaments)
            {
                GameObject Tproj = Instantiate(Projectile, TA.FiringPoint.transform.position, TA.FiringPoint.transform.rotation);
                Tproj.GetComponent<Projectile>().Shooter = gameObject;
                Tproj.transform.rotation *= Quaternion.Euler(TA.ProjectileSpawnOffset);
            }
        }
    }

    //This method will aim the barrel at the cursor
    private void CursorBasedBarrelAiming()
    {
        ForceBarrelAimAtPos(HitPoint);
    }

    //This method will make the barrel aim at a point
    private void ForceBarrelAimAtPos(Vector3 pos)
    {
        for (var i=0;i<tankArmaments.Count;i++)
        {
            tankArmaments[i].Barrel.transform.LookAt(pos);
            Vector3 eulerAngles = tankArmaments[i].Barrel.transform.rotation.eulerAngles;
            eulerX = eulerAngles.x * -1;

            eulerX = ClampAngle(eulerX, MinAngle, MaxAngle);

            eulerAngles = new Vector3(eulerX, MainTurret.transform.rotation.eulerAngles.y, 0);
            tankArmaments[i].Barrel.transform.rotation = Quaternion.Euler(eulerAngles);
            tankArmaments[i].Barrel.transform.rotation *= Quaternion.Euler(tankArmaments[i].BarrelRotationOffset);
        }

    }

    //Rotate the tank based on the cursor
    private void RotateTankBasedOnCursor()
    {
        float rotateTank = Finder.CursorManager.X;
        transform.Rotate(Vector3.up * CursorRotationSpeed * rotateTank * Time.deltaTime);
    }


    //Rotate the main turret based on the cursor
    private void RotateMainTurretBasedOnCursor()
    {
        GameObjectLookAtAnotherOverTime(MainTurret, HitPoint, TurretRotationSpeed, true, false, true);
    }

    private void MakeMainTurretLookAt(Vector3 targetPos, float speed)
    {
        GameObjectLookAtAnotherOverTime(MainTurret, targetPos, speed, true, false, true);
    }

    //Moves the tank
    private void MoveTank()
    {
        float rotateTank = Input.GetAxis("Horizontal");
        float moveTank = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.Space) == false)
        {
            transform.Translate(Vector3.forward * TankMoveSpeed / 10 * moveTank);
            transform.Rotate(Vector3.up * TankRotationSpeed * rotateTank * Time.deltaTime);
        }
    }

    //Follow the Current Player
    private void FollowCurrentPlayer()
    {
        agent.SetDestination(PlayerManager.CurrentPlayer.transform.position);

        if (TS.CurrentTarget ==null && TS.forcedTarget == null)
        {
            canShoot = false;
        }

        if (TS.CurrentTarget!=null)
        {
            AttackTarget(TS.CurrentTarget, false);
        }

        else if (TS.Targets.Count==0)
        {
            canShoot = false;
            MakeMainTurretLookAt(PlayerManager.CurrentPlayer.transform.position, TurretRotationSpeed);
        }
    }

}
