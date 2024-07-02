using UnityEngine;

[RequireComponent(typeof(DelayedDeath))]
public class Projectile : MonoBehaviour
{
    public GameObject Shooter;
    public BasicUnit.Damage damage;
    public float Speed;

    public void Update()
    {
        gameObject.transform.Translate(Vector3.forward * Time.deltaTime * Speed);

        if (Shooter== null)
        {
            Debug.LogWarning("Shooter is null! Please assign the shooter.");
        }
    }

    //This is where the projectile actually does it's logic.
    public void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.GetComponent<Health>() == true && collider.gameObject!= Shooter)
        {
            collider.gameObject.GetComponent<Health>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (collider.gameObject!= Shooter)
        {
            Debug.Log(collider.gameObject);
            Destroy(gameObject);
        }

        if (collider.gameObject == Shooter)
        {
            Debug.Log(collider.gameObject + "is shooter");
            Physics.IgnoreCollision(collider.gameObject.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
        }
    }
}
