using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Game.CameraManagement
{
    public class CameraSwitcher : MonoBehaviour
    {
        private const int MinPriority = 10;
        private const int MaxPriority = 20;
        [SerializeField]private CinemachineVirtualCameraBase[] cameras;
        
        [SerializeField]private CinemachineVirtualCameraBase startingCamera;

        public void SwitchCamera(CinemachineVirtualCameraBase toCamera)
        {
            //Debug.Log("Switching Cameras");
            foreach(var camera in cameras)
            {
                if(camera.Equals(toCamera))
                {
                    camera.Priority = MaxPriority;
                }
                else
                {
                    camera.Priority = MinPriority;
                }
            }
        }

        private void Start() 
        {
            SwitchCamera(startingCamera);    
        }
    }
}
