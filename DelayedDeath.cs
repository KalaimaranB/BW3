using System.Collections;
using UnityEngine;

public class DelayedDeath : MonoBehaviour
{
    public float TimeToWait;

    public void Start()
    {
        if (TimeToWait==0)
        {
            Debug.LogWarning("You seemed to have added a Delayed Death to "+gameObject.name+", but the time is 0 seconds. It was destroyed immediately!");
        }
        StartCoroutine(toWait());
    }

    private IEnumerator toWait()
    {
        yield return new WaitForSeconds(TimeToWait);
        Destroy(gameObject);
    }
}
