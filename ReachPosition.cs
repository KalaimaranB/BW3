using UnityEngine;

public class ReachPosition : MonoBehaviour
{
    public bool reached = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == PlayerManager.CurrentPlayer)
        {
            reached = true;
        }
    }
}
