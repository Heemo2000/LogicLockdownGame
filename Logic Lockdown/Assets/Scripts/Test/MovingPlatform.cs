using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Test
{
    public class MovingPlatform : MonoBehaviour
    {
        [SerializeField]private Transform startPoint;
        [SerializeField]private Transform endPoint;

        [SerializeField]private float moveSpeed = 20.0f;

        [SerializeField]private float waitTime = 2.0f;

        private Rigidbody _platformRB;
        private Collider _platformCollider;

        private Vector3 _previousPos = Vector3.zero;
        private Vector3 _velocity = Vector3.zero;
        private IEnumerator MovePlatform()
        {
            Vector3 startPos = startPoint.position;
            Vector3 endPos = endPoint.position;

            
            while(this.enabled)
            {
                Vector3 direction = (endPos - transform.position).normalized;
                _platformRB.velocity = direction * moveSpeed;

                float squareDistance = (endPos - transform.position).sqrMagnitude;

                if(squareDistance <= 0.01f)
                {
                    _platformRB.velocity = Vector3.zero;
                    yield return new WaitForSeconds(waitTime);
                    Vector3 temp = startPos;
                    startPos = endPos;
                    endPos = temp;
                    
                }
                
                yield return null;
            }
        }


        private void Awake() {
            _platformRB = GetComponent<Rigidbody>();
            _platformCollider = GetComponent<Collider>();
        }

        private void Start() {
            _platformRB.isKinematic = false;
            _platformCollider.isTrigger = false;
            StartCoroutine(MovePlatform());
        }

        private void OnCollisionEnter(Collision other) 
        {
            Debug.Log("Detected something.");
            CharacterController controller = other.transform.GetComponent<CharacterController>();
            if(controller != null)
            {
                Debug.Log("Detected character controller.");
                controller.Move(_platformRB.velocity * Time.fixedDeltaTime);
            }
        }

        private void OnCollisionStay(Collision other) {
            Debug.Log("Detected something.");
            CharacterController controller = other.transform.GetComponent<CharacterController>();
            if(controller != null)
            {
                Debug.Log("Detected character controller.");
                controller.Move(_platformRB.velocity * Time.fixedDeltaTime);
            }
            
        }
    }
}
