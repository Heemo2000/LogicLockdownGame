using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.Events;
using Game.CameraManagement;


namespace Game.InteractionManagement
{
    public class PuzzleInteractable : MonoBehaviour, IInteractable
    {
        
        public UnityEvent OnSuccessfulInteraction;
        
       
        public void Interact()
        {
            Debug.Log("Interact!");
            OnSuccessfulInteraction?.Invoke();
            //cameraSwitcher.SwitchCamera(associatedCamera);            
        }
    }
}
