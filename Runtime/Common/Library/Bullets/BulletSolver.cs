using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Laio.Library.Bullets;
using System.Diagnostics;

/// <summary>
/// TSolver being required feels wierd and is defined as WHERE T : SOLVER
/// </summary>
namespace Laio.Library.Bullets
{
    /// <summary>
    /// Handles and calculates the paths of bullets.
    /// </summary>
    /// <typeparam name="TSolver">Solver type</typeparam>
    /// <typeparam name="TBullet">Bullet type</typeparam>
    public abstract class BulletSolver<TSolver, TBullet> : MonoBehaviour
        where TBullet : Bullet
    {
        /// <summary>
        /// Singleton instance of the solver
        /// </summary>
        public static BulletSolver<TSolver, TBullet> Instance;

        protected HashSet<TBullet> BulletHashSet { get; private set; }

        public bool CalculateCost { get; set; }

        private long _lastTimeToSolve;

        public long TimeToSolveMS
        {
            get
            {
                if (!CalculateCost)
                    return -1;
                return _lastTimeToSolve;
            }
        }

        public int BulletCount
        {
            get
            {
                if (BulletHashSet == null)
                    return 0;
                else
                    return BulletHashSet.Count;
            }
        }

        /// <summary>
        /// Setup instance
        /// </summary>
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                BulletHashSet = new HashSet<TBullet>();
            }
            else
            {
                // Reference already set, so destroy
                Destroy(this);
            }
        }

        /// <summary>
        /// Register a bullet with this solver.
        /// </summary>
        /// <param name="bullet"></param>
        public void RegisterBullet(TBullet bullet)
        {
            BulletHashSet.Add(bullet);
            bullet.transform.parent = transform;
        }

        /// <summary>
        /// Dispose of a bullet in this solver.
        /// </summary>
        /// <param name="bullet"></param>
        public void DisposeBullet(TBullet bullet)
        {
            BulletHashSet.Remove(bullet);
        }

        /// <summary>
        /// Called every frame, handles solving the bullets.
        /// </summary>
        private void Update()
        {
            if (CalculateCost)
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                DoSolve();
                stopWatch.Stop();
                _lastTimeToSolve = stopWatch.ElapsedMilliseconds;
            }
            else
                DoSolve();
        }

        /// <summary>
        /// Solves the entire hashset
        /// </summary>
        private void DoSolve()
        {
            foreach (TBullet bullet in BulletHashSet)
                if (bullet.IsSetup)
                    Solve(bullet);
        }

        /// <summary>
        /// Solve a given bullet
        /// </summary>
        /// <param name="bullet">Bullet to solve</param>
        public abstract void Solve(TBullet bullet);

    }
}