using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SwitchCameraTest : MonoBehaviour
    {
        [SerializeField]private Camera firstCamera;
        [SerializeField] private Camera secondCamera;


        // Start is called before the first frame update
        void Start()
        {
            firstCamera.gameObject.SetActive(true);
            secondCamera.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                if(firstCamera.gameObject.activeInHierarchy)
                {
                    firstCamera.gameObject.SetActive(false);
                    secondCamera.gameObject.SetActive(true);
                }
                else
                {
                    firstCamera.gameObject.SetActive(true);
                    secondCamera.gameObject.SetActive(false);
                }
            }
        }
    }
}
