using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.InteractionManagement;

namespace Game.PlayerManagement
{
    public class PlayerInteractUI : MonoBehaviour
    {
        [Min(0.0f)]
        [SerializeField]private float interactionCheckTime = 0.5f;
        [SerializeField]private Canvas interactCanvas;
        private PlayerInteract _playerInteract;
        
        private IEnumerator CheckInteractions()
        {
            
            while(this.enabled)
            {
                
                var closest = _playerInteract.GetClosest();
                
                if(closest != null)
                {
                    Show();
                }
                else
                {
                    Hide();
                }
                
                yield return new WaitForSeconds(interactionCheckTime);
            }
        }

        private void Show()
        {
            interactCanvas.gameObject.SetActive(true);
        }

        private void Hide()
        {
            interactCanvas.gameObject.SetActive(false);
        }

        private void Awake() {
            _playerInteract = GetComponent<PlayerInteract>();

        }
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(CheckInteractions());
        }
        
    }
}
