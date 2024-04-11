using System;
using UnityEngine;

namespace Game.UI
{
    public class ScreenUIManager : MonoBehaviour
    {
        [SerializeField]private Canvas[] interfaces;
    
        [SerializeField]private Canvas startingInterface;

        private void Awake()
        {
            Open(startingInterface);
        }
        
        public void Open(Canvas instance)
        {
            for(int i = 0; i < interfaces.Length; i++)
            {
                GameObject currentUI = interfaces[i].gameObject;
                currentUI.gameObject.SetActive(false);
            }
            instance.gameObject.SetActive(true);  
        }
    }

}

