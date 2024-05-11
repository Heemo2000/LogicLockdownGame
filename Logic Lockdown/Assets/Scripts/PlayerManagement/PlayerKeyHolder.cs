using System;
using System.Collections.Generic;
using UnityEngine;
using Game.PuzzleManagement;

namespace Game.PlayerManagement
{
    public class PlayerKeyHolder : MonoBehaviour
    {
        [SerializeField]private ParticleSystem unlockedParticleEffect;
        [SerializeField]private ParticleSystem pickupParticleEffect;
        public Action<List<KeyType>> OnKeysChanged;

        private List<KeyType> _keyList;

        private void Awake() {
            _keyList = new List<KeyType>();
        }

        public List<KeyType> GetKeyList() {
            return _keyList;
        }

        public void AddKey(KeyType keyType) {
            Debug.Log("Added Key: " + keyType);
            _keyList.Add(keyType);
            OnKeysChanged?.Invoke(_keyList);
        }

        public void RemoveKey(KeyType keyType) {
            Debug.Log("Removed key: " + keyType);
            _keyList.Remove(keyType);
            OnKeysChanged?.Invoke(_keyList);
        }

        public bool ContainsKey(KeyType keyType) {
            return _keyList.Contains(keyType);
        }


        private void OnTriggerEnter(Collider collider) {
            Key key = collider.GetComponent<Key>();
            if (key != null) {
                AddKey(key.GetKeyType());
                Instantiate(pickupParticleEffect, key.transform.position, Quaternion.identity);
                Destroy(key.gameObject);
            }

            Lock keyLock = collider.GetComponent<Lock>();
            if (keyLock != null) {
                if (ContainsKey(keyLock.GetKeyType())) {
                    // Currently holding Key to open this lock
                    Instantiate(unlockedParticleEffect, keyLock.transform.position, Quaternion.identity);
                    RemoveKey(keyLock.GetKeyType());
                    keyLock.Open();
                }
            }
        }
    }
}
