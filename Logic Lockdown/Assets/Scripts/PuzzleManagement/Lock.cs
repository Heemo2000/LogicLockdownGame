using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.PuzzleManagement
{
    public class Lock : SolutionAcceptor
    {
        [SerializeField]private KeyType keyType;
        [SerializeField]private bool isOpened;

        public Action OnUnlocked;

        public KeyType GetKeyType()
        {
            return keyType;
        }

        public void Open()
        {
            isOpened = true;
            OnUnlocked?.Invoke();
        }

        public override bool IsSolutionCorrect()
        {
            return isOpened == true;
        }

        

        private void PlayUnlockedSound()
        {

        }

        private void Start() 
        {
            OnUnlocked += PlayUnlockedSound;
        }

        private void OnDestroy() 
        {
            OnUnlocked -= PlayUnlockedSound;    
        }
    }
}
