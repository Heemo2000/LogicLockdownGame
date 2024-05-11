using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.PuzzleManagement
{
    public enum KeyType
    {
        Red,
        Green,
        Blue,
        Black
    }
    public class Key : MonoBehaviour
    {
        [SerializeField]private KeyType keyType;

        [Min(0.0f)]
        [SerializeField]private float rotationSpeed = 10.0f;
        public Action OnPickup;

        public KeyType GetKeyType()
        {
            return keyType;
        }

        private void PlayPickupSound()
        {

        }

        private void Start() 
        {
            OnPickup += PlayPickupSound;
        }

        private void FixedUpdate() 
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.fixedDeltaTime);    
        }

        private void OnDestroy() 
        {
            OnPickup?.Invoke();
            OnPickup -= PlayPickupSound;    
        }
    }
}
