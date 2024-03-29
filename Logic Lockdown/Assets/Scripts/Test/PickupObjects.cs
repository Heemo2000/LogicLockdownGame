using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

namespace Game.Test
{
    public class PickupObjects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private Material changedMaterial;
        private MeshRenderer _graphic;

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("Pointer In");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("Pointer Out");
        }

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
