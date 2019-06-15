using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ProjectileMover : MonoBehaviour
{
    public float speed = 15f;
    public float hitOffset = 0f;
    public bool UseFirePointRotation;
    public GameObject hit;
    public GameObject flash;
    public bool canMove = true;

    private void Start ()
    {
        if (flash != null)
        {
            var flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
            flashInstance.transform.forward = gameObject.transform.forward;
            var flashPs = flashInstance.GetComponent<ParticleSystem>();
            if (flashPs == null)
            {
                Destroy(flashInstance, flashPs.main.duration);
            }
            else
            {
                var flashPsParts = flashInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(flashInstance, flashPsParts.main.duration);
            }
        }
	}

    private void FixedUpdate ()
    {
		if (canMove)
        {
            transform.position += transform.forward * (speed * Time.deltaTime);         
        }
	}

    public void PlayOnHit(RaycastHit _hit)
    {
        canMove = false;
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, _hit.normal);
        Vector3 pos = _hit.point + _hit.normal * hitOffset;

        if (hit != null)
        {
            var hitInstance = Instantiate(hit, pos, rot);
            if (UseFirePointRotation)
            { 
                hitInstance.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, 180f, 0); 
            }
            else
            { 
                hitInstance.transform.LookAt(_hit.point + _hit.normal);
            }

            var hitPs = hitInstance.GetComponent<ParticleSystem>();
            if (hitPs == null)
            {
                Destroy(hitInstance, hitPs.main.duration);
            }
            else
            {
                var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(hitInstance, hitPsParts.main.duration);
            }
        }
        
        Destroy(gameObject);
    }
}
