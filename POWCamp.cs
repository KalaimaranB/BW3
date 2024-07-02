using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POWCamp : MonoBehaviour
{
    public List<Health> ammoDumps;
    public List<Identification> prisoners;
    public GameObject BarbedWire;
    public GameObject POWSmoke;
    public GameObject POWSmokeLocation;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Health ad in ammoDumps)
        {
            ad.UnitDied += FreePrisoners;
        }
        if (prisoners.Count>0)
        {
            foreach (Identification prisoner in prisoners)
            {
                prisoner.currentStatus = Identification.Status.Captured;
            }
        }
        else
        {
            Debug.LogWarning("This POW Camp, "+gameObject.name+", does not have any prisoners to guard. Add some POWs!");
        }
    }

    public void FreePrisoners(object sender, EventArgs e)
    {
        foreach(Health he in ammoDumps)
        {
            if (he!=null)
            {
                he.UnitDied -= FreePrisoners;
                he.ForceDeath(true);
            }
        }
        Destroy(BarbedWire);
        Instantiate(POWSmoke, POWSmokeLocation.transform.position, POWSmokeLocation.transform.rotation);
        if (prisoners.Count>0)
        {
            foreach (Identification prisoner in prisoners)
            {
                prisoner.currentStatus = Identification.Status.PlayerOrdered;
            }
        }
        else
        {
            Debug.LogWarning("There are no prisoners to free, but the FreePrisoners method was callled!");
        }
    }
}
