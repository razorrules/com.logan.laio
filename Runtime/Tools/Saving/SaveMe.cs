using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Laio;
using Laio.Tools;

namespace Laio.Saving
{
    public struct ObjectSave
    {

    }

    public class SaveMe : MonoBehaviour
    {
        [SerializeField, ReadOnly] private int _uniqueIdentifier;

        [Header("Transform")]
        [SerializeField] private bool _position;
        [SerializeField] private bool _rotation;
        [SerializeField] private bool _scale;

        private Component[] _toSave;


        public ObjectSave GetSaveData()
        {
            return new ObjectSave();
        }

        public void LoadSaveData()
        {

        }

    }

}