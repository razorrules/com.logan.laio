using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laio.Library.Bullets
{

    /// <summary>
    /// This class can probably be turned into something else.
    /// The point of this class currently, is for bullets to 
    /// quickly check if they hit something that was damageable, 
    /// though this inheriting MonoBehaviour might not make the most sense
    /// </summary>
    public interface IDamageable
    {
        public abstract void TakeDamage(float damage);

        public abstract void TakeDamage(float damage, GameObject Instigator);

    }
}