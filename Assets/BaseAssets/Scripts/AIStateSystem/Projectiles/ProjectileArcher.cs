using UnityEngine;

namespace BaseAssets.AI.Projectile
{
    public class ProjectileArcher : ProjectileBase
    {
        public bool moveParticle = false;

        private ParticleSystem particleChild = null;

        private void Awake()
        {
            if (moveParticle)
            {
                particleChild = GetComponentInChildren<ParticleSystem>();
            }
        }

        private void Update()
        {
            if (homingProjectile && targetTrajectory == null) return;
            MoveArrowAlongTheCurve();
            CheckIfCanDoDamage();
            CheckIfProjectileHasReachedItsEndPosition();
        }

        private void MoveArrowAlongTheCurve()
        {
            timer += Time.deltaTime * speedMultiplier;

            if (homingProjectile)
            {
                transform.position = Vector3.Lerp(transform.position, Curve(startPosition, new Vector3(targetTrajectory.transform.position.x, targetTrajectory.transform.position.y + projectileTargetOffsetY, targetTrajectory.transform.position.z), timer), timer / projectileSpeedInSeconds);
                transform.LookAt(Curve(startPosition, new Vector3(targetTrajectory.transform.position.x, targetTrajectory.transform.position.y + projectileTargetOffsetY, targetTrajectory.transform.position.z), timer));
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, Curve(startPosition, new Vector3(targetPosition.x, targetPosition.y + projectileTargetOffsetY, targetPosition.z), timer), timer / projectileSpeedInSeconds);
                transform.LookAt(Curve(startPosition, new Vector3(targetPosition.x, targetPosition.y + projectileTargetOffsetY, targetPosition.z), timer));
            }

            if (moveParticle)
            {
                if (particleChild)
                {
                    ParticleSystem.Particle[] particles = new ParticleSystem.Particle[10];
                    int count = particleChild.GetParticles(particles);

                    if(homingProjectile)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            particles[i].position = Vector3.Lerp(transform.position, Curve(startPosition, new Vector3(targetTrajectory.transform.position.x, targetTrajectory.transform.position.y + projectileTargetOffsetY, targetTrajectory.transform.position.z), timer), timer / projectileSpeedInSeconds);
                            particles[i].rotation3D = transform.rotation.eulerAngles;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < count; i++)
                        {
                            particles[i].position = Vector3.Lerp(transform.position, Curve(startPosition, new Vector3(targetPosition.x, targetPosition.y + projectileTargetOffsetY, targetPosition.z), timer), timer / projectileSpeedInSeconds);
                            particles[i].rotation3D = transform.rotation.eulerAngles;
                        }
                    }

                    particleChild.SetParticles(particles);
                }
            }
        }
    }
}