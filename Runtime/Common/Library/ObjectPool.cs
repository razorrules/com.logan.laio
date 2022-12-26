using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laio
{
    /// <summary>
    /// Simple object pool class.
    /// 
    /// Has a GetObject and ReturnObject. Does not keep track or validate that the object being
    /// returned is the correct type. 
    /// </summary>
    public class ObjectPool
    {

        public string name;
        public GameObject prefab { get; protected set; }

        protected List<GameObject> Pool;
        protected GameObject Parent;
        protected bool _useParent = true;

        /// <summary>
        /// Setup object pool.
        /// </summary>
        /// <param name="prefab">Prefab the pool will use.</param>
        public ObjectPool(GameObject prefab)
        {
            this.name = prefab.name;
            this.prefab = prefab;
            Pool = new List<GameObject>();
            _useParent = true;
            if (_useParent)
            {
                Parent = new GameObject(name);
            }
        }

        /// <summary>
        /// Setup object pool.
        /// </summary>
        /// <param name="prefab">Prefab the pool will use.</param>
        /// <param name="name">Name of the pool</param>
        public ObjectPool(GameObject prefab, string name)
        {
            this.name = name;
            this.prefab = prefab;
            Pool = new List<GameObject>();
            _useParent = true;
            if (_useParent)
            {
                Parent = new GameObject(name);
            }
        }

        /// <summary>
        /// Setup object pool.
        /// </summary>
        /// <param name="prefab">Prefab the pool will use.</param>
        /// <param name="name">Name of the pool</param>
        /// <param name="prepopulated">How many objects should be prepopulated</param>
        public ObjectPool(GameObject prefab, string name, int prepopulated)
        {
            this.name = name;
            this.prefab = prefab;
            Pool = new List<GameObject>();
            _useParent = true;
            if (_useParent)
            {
                Parent = new GameObject(name);
            }
            for (int i = 0; i < prepopulated; i++)
            {
                Pool.Add(AddObject(false));
            }
        }

        /// <summary>
        /// Setup object pool.
        /// </summary>
        /// <param name="prefab">Prefab the pool will use.</param>
        /// <param name="name">Name of the pool</param>
        /// <param name="prepopulated">How many objects should be prepopulated</param>
        /// <param name="useParent">Use parent to store unused objects</param>
        public ObjectPool(GameObject prefab, string name, int prepopulated, bool useParent = true)
        {
            this.name = name;
            this.prefab = prefab;
            Pool = new List<GameObject>();
            this._useParent = useParent;
            if (useParent)
            {
                Parent = new GameObject(name);
            }
            for (int i = 0; i < prepopulated; i++)
            {
                Pool.Add(AddObject(false));
            }
        }

        /// <summary>
        /// Get an object from the pool
        /// </summary>
        public virtual GameObject GetObject()
        {
            if (Pool.Count > 0)
            {
                GameObject returnValue = Pool[0];
                Pool.Remove(returnValue);
                return returnValue;
            }
            return AddObject(true);
        }

        /// <summary>
        /// Get an object from the pool
        /// </summary>
        /// <returns></returns>
        protected virtual GameObject AddObject(bool isRequestingNew)
        {
            GameObject gameObject = GameObject.Instantiate(prefab);
            if (_useParent)
            {
                gameObject.transform.SetParent(Parent.transform);
                gameObject.SetActive(isRequestingNew);
            }
            return gameObject;
        }

        /// <summary>
        /// Return a single object back into the pool
        /// </summary>
        /// <param name="gameObject">Object to add back into the pool</param>
        public virtual void ReturnObject(GameObject gameObject)
        {
            Pool.Add(gameObject);
            if (_useParent)
            {
                gameObject.transform.SetParent(Parent.transform);
                gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Return a list of objects
        /// </summary>
        /// <param name="gameObjects"></param>
        public virtual void ReturnObjects(IList<GameObject> gameObjects)
        {
            foreach (GameObject gameObject in gameObjects)
            {
                Pool.Add(gameObject);
                if (_useParent)
                {
                    gameObject.transform.SetParent(Parent.transform);
                }
            }
        }

    }
}