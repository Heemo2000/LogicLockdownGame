using System.Collections;
using System.Collections.Generic;
using Game.Core;
using UnityEngine;
using UnityEngine.Events;

namespace Game.PlayerManagement
{
    public class LevelCompleteDetector : MonoBehaviour
    {
        [SerializeField]private LayerMask playerMask;
        Collider _collider;
        private void Awake() {
            _collider = GetComponent<Collider>();
        }

        private void Start() {
            _collider.isTrigger = true;
        }
        private void OnTriggerEnter(Collider other) 
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if(player != null)
            {
                Debug.Log("Level Completed");
                player.enabled = false;
                GameManager.Instance.OnGameEnd?.Invoke();
            }
        }


    }

}

