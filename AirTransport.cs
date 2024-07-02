using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(Identification))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(AudioSource))]


public class AirTransport : MonoBehaviour
{
    [Tooltip("These systems will be played on take off and landing")]
    [SerializeField] private List<ParticleSystem> particleSystems;

    public enum UnitStatus {Waiting, WarmingUp, TakingOff, Flying, Landing, Unloading};
    [ReadOnly]
    public UnitStatus unitStatus;

    [Header("Rotor")]
    [SerializeField] private List<GameObject> Rotors;
    [SerializeField] private float maxRotorSpeed=150f;
    [SerializeField] private float RotorSpeedChangeRate = 10f;
    [SerializeField] private bool playRotorsSoundEffect = true;

    [Header("Animations")]
    [SerializeField] private string OpenDoorTrigger = "Open Door";
    [SerializeField] private string CloseDoorTrigger = "Close Door";
    [SerializeField] private string LandingTrigger = "Landing";
    [SerializeField] private string TakeOffTrigger = "Take Off";

    [Header("Movement")]
    [SerializeField] private float TotalFlightTime=10;
    [SerializeField] private float TotalTakeOffTime=5;
    [SerializeField] private float TotalLandingTime=5;
    [SerializeField] private float RotationSpeed = 1f;
    [ReadOnly]
    [SerializeField] private float CurrentSpeed;

    [Header("Unloading")]
    [SerializeField] private float UnloadingTime=10f;
    [SerializeField] private List<GameObject> tanks;
    [SerializeField] private List<GameObject> infantry;
    [SerializeField] private List<Transform> TanksPos;
    [SerializeField] private List<Transform> InfantryPos;

    private List<TransportPassenger> tankPassengers = new List<TransportPassenger>();
    private GameObject startPos;
    private AudioSource audioSource;
    private Animator animator;
    private RotateContinuously[] rcs = new RotateContinuously[0];

    public event EventHandler<EventArgs> UnloadingComplete;



    // Start is called before the first frame update
    void Start()
    {
        rcs = new RotateContinuously[Rotors.Count];
        startPos = new GameObject();
        startPos.transform.position = gameObject.transform.position;
        startPos.name = gameObject.name + " (StartPosition)";
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        //Check the rotors for the component and set their speed to 0
        for (int i = 0; i < Rotors.Count; i++)
        {
            if (Rotors[i].GetComponent<RotateContinuously>() == false)
            {
                RotateContinuously rc = Rotors[i].AddComponent<RotateContinuously>();
                rc.RotationsPerMinute = 0;
                Debug.LogWarning(Rotors[i] + "--> This rotor does not have a rotate continuously component. One has been added. You may need to add one yourself if you want to change the default direction of rotation.");
            }
            else
            {
                Rotors[i].GetComponent<RotateContinuously>().RotationsPerMinute = 0;
            }
        }

        if (infantry.Count>InfantryPos.Count)
        {
            Debug.LogWarning("There are more infantry than infantry positions in "+gameObject.name+". Some infantry may not be spawned!");
        }
        if (tanks.Count>TanksPos.Count)
        {
            Debug.LogWarning("There are more tanks than tank positions in "+gameObject.name+". Some tanks may not be spawned!"); ;
        }
        for (var i = 0; i < Rotors.Count; i++)
        {
            rcs[i] = Rotors[i].GetComponent<RotateContinuously>();
        }
    }



    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < rcs.Length; i++)
        {
            Mathf.Clamp(rcs[i].RotationsPerMinute, 0, maxRotorSpeed);
        }
    }

    

    public void TransferToHelipadAndReturn(Helipad helipad)
    {
        StartCoroutine(DispatchToHelipadAndReturn(helipad));
    }

    private IEnumerator SimpleMoveToPosition(Vector3 targetPos)
    {
        do
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPos, CurrentSpeed * Time.deltaTime);

            Vector3 targetPosition = targetPos - gameObject.transform.position;
            targetPosition.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(targetPosition);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * RotationSpeed);

            yield return null;
        } while (Vector3.Distance(gameObject.transform.position,targetPos)>0.01f);
    }

    private IEnumerator SimpleMoveToPosition(Vector3 targetPos, Vector3 lookAtPos)
    {
        do
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPos, CurrentSpeed * Time.deltaTime);

            Vector3 targetPosition = lookAtPos - gameObject.transform.position;
            targetPosition.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(targetPosition);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * RotationSpeed);

            yield return null;
        } while (Vector3.Distance(gameObject.transform.position, targetPos) > 0.01f);
    }

    private void TryPlayAnimation(string triggerName)
    {
        if (triggerName!="")
        {
            animator.SetTrigger(triggerName);
        }
    }


    private IEnumerator DispatchToHelipadAndReturn(Helipad helipad)
    {
        //Play rotors sound if bool is true
        if (playRotorsSoundEffect == true)
        {
            audioSource.loop = true;
            audioSource.Play();
            audioSource.volume = 1;
        }

        //Play TakeOff and start rotors
        TryPlayAnimation(TakeOffTrigger);
        for (int i = 0; i < rcs.Length; i++)
        {
            rcs[i].RotationsPerMinute = maxRotorSpeed;
        }

        //Set flight speed & move to the FlyToPos
        CurrentSpeed = ((Vector3.Distance(gameObject.transform.position,helipad.FlyToPos.transform.position))/TotalFlightTime);
        yield return StartCoroutine(SimpleMoveToPosition(helipad.FlyToPos.transform.position));

        //Play Landing Trigger, Particle Systems and move to landing position
        TryPlayAnimation(LandingTrigger);
        CurrentSpeed = ((Vector3.Distance(gameObject.transform.position, helipad.LandingPos.transform.position)) / TotalLandingTime);
        StartParticleSystems();

        //See if you can create an alternate COROUTINE to deccelerate
        yield return StartCoroutine(SimpleMoveToPosition(helipad.LandingPos.transform.position, helipad.LastLookAtObject.transform.position));

        //Stop Particle System and slow down rotors
        StopParticleSystems();
        do
        {
            for (int i = 0; i < rcs.Length; i++)
            {
                rcs[i].RotationsPerMinute -= RotorSpeedChangeRate * Time.deltaTime;
            }
            yield return null;
        } while (Rotors[Rotors.Count - 1].GetComponent<RotateContinuously>().RotationsPerMinute > 0);

        if (playRotorsSoundEffect == true)
        {
            audioSource.Stop();
        }

        //Unload Tanks
        for (var i = 0; i < tanks.Count; i++)
        {
            GameObject t = Instantiate(tanks[i], TanksPos[i].transform.position, TanksPos[i].transform.rotation);
            TransportPassenger tp = t.AddComponent<TransportPassenger>();
            tankPassengers.Add(tp);
        }


        //Open Door & Wait
        TryPlayAnimation(OpenDoorTrigger);
        do
        {
            yield return null;
        } while (animator.GetCurrentAnimatorStateInfo(0).IsName("Close Door Idle") || animator.GetCurrentAnimatorStateInfo(0).IsName("Open Door"));

        for (var i=0;i<tankPassengers.Count;i++)
        {
            //tankPassengers[i].ExitTransport(helipad.tankMoveToPos[i]);
            //This is the updated command, this moves straight and then to the side. Maybe the old one will work for infantry?
            tankPassengers[i].ExiTransport(helipad.tankInitialMoveToPos, helipad.tankMoveToPos[i]);
        }

        //Wait 10 seoncds then wait for door to close
        yield return new WaitForSeconds(UnloadingTime);

        UnloadingComplete?.Invoke(this, EventArgs.Empty);

        //Close Door
        TryPlayAnimation(CloseDoorTrigger);
        do
        {
            yield return null;
        } while (animator.GetCurrentAnimatorStateInfo(0).IsName("Close Door") || animator.GetCurrentAnimatorStateInfo(0).IsName("Open Door Idle"));

        //Warm Up Rotors
        do
        {
            for (int i = 0; i < rcs.Length; i++)
            {
                rcs[i].RotationsPerMinute += RotorSpeedChangeRate * Time.deltaTime;
            }
            yield return null;
        } while (Rotors[Rotors.Count - 1].GetComponent<RotateContinuously>().RotationsPerMinute < maxRotorSpeed);

        if(playRotorsSoundEffect == true)
        {
            audioSource.Play();
        }

        //Play Take Off, Particle Systems and move to FlyToPos
        TryPlayAnimation(TakeOffTrigger);
        StartParticleSystems();
        CurrentSpeed = ((Vector3.Distance(gameObject.transform.position, helipad.FlyToPos.transform.position)) / TotalTakeOffTime);
        yield return StartCoroutine(SimpleMoveToPosition(helipad.FlyToPos.transform.position,startPos.transform.position));
        StopParticleSystems();


        //Move to start Pos and then destory this
        CurrentSpeed = ((Vector3.Distance(gameObject.transform.position, startPos.transform.position) / TotalFlightTime));
        yield return StartCoroutine(SimpleMoveToPosition(startPos.transform.position));
        Destroy(startPos);
        Destroy(gameObject);
    }



    private void StartParticleSystems()
    {
        if (particleSystems.Count>0)
        {
            for (int i = 0; i < particleSystems.Count; i++)
            {
                if (particleSystems[i].isPlaying == false)
                {
                    particleSystems[i].Play();
                }
            }
        }
    }

    private void StopParticleSystems()
    {
        if (particleSystems.Count > 0)
        {
            for (int i = 0; i < particleSystems.Count; i++)
            {
                if (particleSystems[i].isPlaying == true)
                {
                    particleSystems[i].Stop();
                }
            }
        }
    }
}
