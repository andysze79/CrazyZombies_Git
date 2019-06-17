using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ProjectileMover : MonoBehaviour
{
    public float hitOffset = 0f;
    public GameObject hit;
    public GameObject flash;

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

    public void PlayOnHit(RaycastHit _hit)
    {
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, _hit.normal);
        Vector3 pos = _hit.point + _hit.normal * hitOffset;

        if (hit != null)
        {
            var hitInstance = Instantiate(hit, pos, rot);
            hitInstance.transform.LookAt(_hit.point + _hit.normal);
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
