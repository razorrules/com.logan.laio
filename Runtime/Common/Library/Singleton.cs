using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SingletonParams : byte
{
    DontDestroyOnLoad,
}

/// <summary>
/// Simple singleton template
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : class
{
    //Instance of singleton
    public static T Instance { get; private set; }

    //Params
    private SingletonParams[] args;

    /// <summary>
    /// Set the singleton instance
    /// </summary>
    /// <param name="value">Value to set instance to. 'this'</param>
    /// <param name="args">Arguments for the singleton</param>
    protected virtual void SetInstance(T value, params SingletonParams[] args)
    {
        Instance = value;
        this.args = args;

        if (HasArg(SingletonParams.DontDestroyOnLoad))
            DontDestroyOnLoad(this);
    }

    /// <summary>
    /// Dispose of the singleton
    /// </summary>
    protected void DisposeSingleton() =>
        Instance = null;

    /// <summary>
    /// Does the singleton have a given argument.
    /// </summary>
    private bool HasArg(SingletonParams arg)
    {
        int pos = Array.IndexOf(args, arg);
        if (pos > -1)
            return true;
        return false;
    }

}
