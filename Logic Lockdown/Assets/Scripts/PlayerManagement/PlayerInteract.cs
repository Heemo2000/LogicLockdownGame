using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using Game.InteractionManagement;
using Game.Core;

namespace Game.PlayerManagement
{
    public class PlayerInteract : MonoBehaviour
    {
        [Header("Interaction Settings: ")]
        [Min(1.0f)]
        [SerializeField]private float interactionRadius = 5.0f;

        [SerializeField]private Vector3 offset;

        [SerializeField]private LayerMask interactionMask;

        [Min(0.0f)]
        [SerializeField]private float interactionCheckTime = 0.5f;

        [Header("Interaction UI Settings: ")]
        [SerializeField]private Canvas interactDetectCanvas;
        [SerializeField]private Button backButton;
        [SerializeField]private Canvas interactCanvas;

        private bool _interactionLocked = false;
        private PlayerMovement _playerMovement;
        private IInteractable _currentInteractable;

        public UnityEvent OnSuccessfulInteraction;
        public UnityEvent OnLeavingInteraction;

        public Canvas InteractDetectCanvas { get => interactDetectCanvas; set => interactDetectCanvas = value; }
        public Button BackButton { get => backButton; set => backButton = value; }
        public Canvas InteractCanvas { get => interactCanvas; set => interactCanvas = value; }

        public void OnInteractionPressed(InputAction.CallbackContext context)
        {
            if(context.phase != InputActionPhase.Performed)
            {
                return;
            }

            TryToInteract();
        }


        public Collider GetClosest()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position + offset, interactionRadius, interactionMask.value);
            Collider result = null;
            for(int i = 0 ; i < colliders.Length; i++)
            {
                Collider collider = colliders[i];
                if(result == null)
                {
                    result = collider;
                }

                else if(Vector3.SqrMagnitude(transform.position - collider.transform.position) < 
                   Vector3.SqrMagnitude(transform.position - result.transform.position))
                {
                    result = collider;
                }
            }

            return result;
        }
        
        private void StopInteracting()
        {
            _interactionLocked = false;
        }

        private void TryToInteract()
        {
            Debug.Log("Interaction Pressed");
            Collider closest = GetClosest();
            
            if(closest != null)
            {
                Debug.Log("Closest name: " + closest.transform.name);
                var interactable = closest.gameObject.GetComponent<IInteractable>();
                if(interactable != null)
                {
                    Debug.Log("Trying to interact!");
                    _currentInteractable = interactable;
                    //interactable.Interact();
                    OnSuccessfulInteraction?.Invoke();
                    _interactionLocked = true;
                }
                else
                {
                    Debug.Log("Does not have Interactable component");
                    _interactionLocked = false;
                }
            }
            else
            {
                Debug.Log("Closest is null");
                _currentInteractable = null;
                _interactionLocked = true;

            }
        }
        private IEnumerator CheckInteractions()
        {
            
            while(GameManager.Instance.GameplayStatus == GameplayStatus.OnGoing)
            {
                if(GameManager.Instance.GamePauseStatus != GamePauseStatus.Paused)
                {
                    var closest = GetClosest();
                
                    if(closest != null && !_interactionLocked)
                    {
                        Show();
                    }
                    else
                    {
                        Hide();
                    }
                    if(_interactionLocked)
                    {
                        _playerMovement.enabled = false;
                    }
                    else
                    {
                        _playerMovement.enabled = true;
                    }    
                    yield return new WaitForSeconds(interactionCheckTime);
                }
                else
                {
                    yield return null;
                }
            }
        }

        private void Show()
        {
            interactDetectCanvas.gameObject.SetActive(true);
        }

        private void Hide()
        {
            interactDetectCanvas.gameObject.SetActive(false);
        }

        private void GoBack()
        {
            OnLeavingInteraction?.Invoke();
        }

        private void SetInteractDetectCanvas(bool value)
        {
            interactDetectCanvas.gameObject.SetActive(value);
        }
        private void SetInteractCanvas(bool value)
        {
            interactCanvas.gameObject.SetActive(value);
        }

        private void InteractWithInteractable()
        {
            _currentInteractable.Interact();
        }

        private void Awake() 
        {
            _playerMovement = GetComponent<PlayerMovement>();    
        }
        private void Start() 
        {
            StartCoroutine(CheckInteractions());
            backButton.onClick.AddListener(()=> GoBack());

            OnSuccessfulInteraction.AddListener(InteractWithInteractable);
            OnSuccessfulInteraction.AddListener(()=> SetInteractDetectCanvas(false));
            OnSuccessfulInteraction.AddListener(()=> SetInteractCanvas(true));

            OnLeavingInteraction.AddListener(StopInteracting);
            OnLeavingInteraction.AddListener(()=> SetInteractDetectCanvas(true));
            OnLeavingInteraction.AddListener(()=> SetInteractCanvas(false));

        }

        private void OnDestroy() 
        {
            backButton.onClick.RemoveAllListeners();    
            OnSuccessfulInteraction.RemoveAllListeners();
            OnLeavingInteraction.RemoveAllListeners();
        }
        private void OnDrawGizmosSelected() 
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position + offset, interactionRadius);    
        }
    }
}
