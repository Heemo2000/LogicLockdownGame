using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Test
{
    public class PickupObjects : MonoBehaviour
    {
        [SerializeField]private Camera lookCamera;
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private Material changedMaterial;
        [SerializeField] private LayerMask detectMask;

        private MeshRenderer _graphic;

        private void Awake() {
            _graphic = GetComponent<MeshRenderer>();
        }
        private void OnMouseEnter() 
        {
            _graphic.material = changedMaterial;
        }

        private void OnMouseExit() 
        {
            _graphic.material = defaultMaterial;    
        }
    }
}
