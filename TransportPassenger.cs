using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class TransportPassenger : MonoBehaviour
{
    public GameObject target;
    private NavMeshAgent agent;
    public bool movedToTarget = false;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Identification>().currentStatus = Identification.Status.AIControlled;
        agent = gameObject.GetComponent<NavMeshAgent>();
        gameObject.GetComponent<Collider>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (target!=null)
        {
            Debug.Log("Step 2");
            agent.SetDestination(target.transform.position);
            
        }

        if (agent.remainingDistance<=0.1f && target!=null && movedToTarget == true && Vector3.Distance(gameObject.transform.position,target.transform.position)<=1)
        {
            Debug.Log("Done!");
            agent.isStopped = true;
            gameObject.GetComponent<Collider>().enabled = true;
            gameObject.GetComponent<Identification>().currentStatus = Identification.Status.PlayerOrdered;
            Destroy(this);
        }
    }


    public void ExitTransport(GameObject targetPos)
    {
        Debug.Log("Step 1");
        target = targetPos;
        movedToTarget = true;
        agent.isStopped = false;
        agent.SetDestination(target.transform.position);

    }

    public void ExiTransport(GameObject Pos1, GameObject Pos2)
    {
        StartCoroutine(AdvancedLeaveTransport(Pos1, Pos2));
    }

    private IEnumerator AdvancedLeaveTransport(GameObject p1, GameObject p2)
    {
        float stopDistance = agent.stoppingDistance;
        agent.stoppingDistance = 3;
        agent.isStopped = false;
        yield return null;
        agent.SetDestination(p1.transform.position);
        yield return new WaitForSeconds(1);
        while (agent.remainingDistance>0.5f)
        {
            agent.SetDestination(p1.transform.position);
            yield return null;
        }
        while (Vector3.Distance(p2.transform.position, gameObject.transform.position) > 0.5f)
        {
            agent.SetDestination(p2.transform.position);
            yield return null;
        }
        agent.stoppingDistance = stopDistance;
        agent.isStopped = true;
        gameObject.GetComponent<Collider>().enabled = true;
        gameObject.GetComponent<Identification>().currentStatus = Identification.Status.PlayerOrdered;
        Destroy(this);
    }
}
