using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laio.Library.Bullets
{
    /// <summary>
    /// Abstract bullet class to be used with the BulletSolver
    /// </summary>
    public abstract class Bullet : MonoBehaviour
    {
        /// <summary>
        /// Whether or not the bullet is setup and registered
        /// </summary>
        public bool IsSetup { get; private set; }

        private float _startTime;

        /// <summary>
        /// Lifetime of the bullet
        /// </summary>
        public float Lifetime
        {
            get
            {
                return Time.time - _startTime;
            }
        }

        /// <summary>
        /// Setup the bullet object.
        /// </summary>
        /// <param name="Instigator"></param>
        public virtual void Setup(object Instigator)
        {
            IsSetup = true;
            Register();
            _startTime = Time.time;
        }

        /// <summary>
        /// Dispose of the bullet on destroy
        /// </summary>
        public void OnDestroy()
        {
            Dispose();
        }

        /// <summary>
        /// Setup the bullet 
        /// </summary>
        /// <param name="instigator"></param>
        /// <param name="lifeTime"></param>
        public virtual void Setup(GameObject instigator, float lifeTime)
        {
            IsSetup = true;
        }

        /// <summary>
        /// Get the damage for the bullet
        /// </summary>
        /// <returns></returns>
        public abstract float GetDamage();

        /// <summary>
        /// Check collision of the bullet. Called from Solvers
        /// </summary>
        /// <param name="origin">Where the bullet started</param>
        /// <param name="direction">Where the bullet went</param>
        public abstract void CollisionCheck(Vector3 origin, Vector3 direction);

        /// <summary>
        /// Dispose of the bullet and remove it from the BulletSolver
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// Register the bullet with the BulletSolver
        /// </summary>
        public abstract void Register();

    }

}
