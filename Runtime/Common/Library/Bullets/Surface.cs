using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laio.Library.Bullets
{

    /// <summary>
    /// Custom surface for interacting with bullets. Requires sufface data.
    /// </summary>
    public abstract class Surface<T> : MonoBehaviour where T : Bullet
    {

        /// <summary>
        /// Impact check for bullet hitting surface
        /// </summary>
        /// <param name="bullet">Bullet data passed in to modify based on surface params</param>
        /// <param name="hit">Hit from raycast</param>
        /// <returns>Should the bullet be stopped on impact?</returns>
        public abstract bool OnImpact(T bullet, RaycastHit hit);

    }


}