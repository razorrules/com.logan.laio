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

        public abstract bool OnImpact(T b, RaycastHit hit);

    }


}