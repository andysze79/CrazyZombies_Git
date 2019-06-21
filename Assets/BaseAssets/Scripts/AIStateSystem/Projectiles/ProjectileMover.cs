using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace BaseAssets.AI.Projectile
{
    public class ProjectileMover : MonoBehaviour
    {
        public GameObject hit;

        public void PlayOnHit(RaycastHit _hit)
        {
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, _hit.normal);
            Vector3 pos = _hit.point + _hit.normal;

            if (hit != null)
            {
                var hitInstance = Instantiate(hit, pos, rot);
                hitInstance.transform.LookAt(_hit.point + _hit.normal);
                var hitPs = hitInstance.GetComponent<ParticleSystem>();

                var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(hitInstance, hitPsParts.main.duration);
            }
        }
    }
}