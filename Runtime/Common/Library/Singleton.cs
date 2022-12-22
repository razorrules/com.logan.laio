using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SingletonParams : byte
{
    DontDestroyOnLoad,
}

public abstract class Singleton<T> : MonoBehaviour where T : class
{
    private static T _instance;

    public static T Instance { get; private set; }

    //Params
    private SingletonParams[] args;

    protected virtual void SetInstance(T value, params SingletonParams[] args)
    {
        Instance = value;
        this.args = args;

        if (HasArg(SingletonParams.DontDestroyOnLoad))
            DontDestroyOnLoad(this);
    }

    protected void DisposeSingleton() =>
        Instance = null;

    private bool HasArg(SingletonParams arg)
    {
        int pos = Array.IndexOf(args, arg);
        if (pos > -1)
            return true;
        return false;
    }

}
