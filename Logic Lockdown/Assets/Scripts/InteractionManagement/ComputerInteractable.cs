using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Game.CameraManagement;


namespace Game.InteractionManagement
{
    public class ComputerInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField]private CameraSwitcher cameraSwitcher;
        [SerializeField]private CinemachineVirtualCameraBase associatedCamera;
        [SerializeField]private FadeScreen fadeScreen;

       

        public void Interact()
        {
            Debug.Log("Interact!");

            cameraSwitcher.SwitchCamera(associatedCamera);
            //StartCoroutine(fadeScreen.Fade());
        }
    }
}
