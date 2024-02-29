using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Game.InteractionManagement;

namespace Game.PlayerManagement
{
    public class PlayerInteract : MonoBehaviour
    {
        [Header("Interaction Settings: ")]
        [Min(1.0f)]
        [SerializeField]private float interactionRadius = 5.0f;

        [SerializeField]private Vector3 offset;

        [SerializeField]private LayerMask interactionMask;

        public void OnInteractionPressed(InputAction.CallbackContext context)
        {
            if(context.phase != InputActionPhase.Performed)
            {
                return;
            } 
            Debug.Log("Interaction Pressed");
            Collider closest = GetClosest();
            
            if(closest != null)
            {
                Debug.Log("Closest name: " + closest.transform.name);
                var interactable = closest.gameObject.GetComponent<IInteractable>();
                if(interactable != null)
                {
                    Debug.Log("Trying to interact!");
                    interactable.Interact();
                }
                else
                {
                    Debug.Log("Does not have Interactable component");
                }
            }
            else
            {
                Debug.Log("Closest is null");
            }
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
        


        private void OnDrawGizmosSelected() 
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position + offset, interactionRadius);    
        }
    }
}
