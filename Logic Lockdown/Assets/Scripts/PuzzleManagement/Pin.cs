using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.PuzzleManagement
{
    [RequireComponent(typeof(BoxCollider))]
    public class Pin : MonoBehaviour
    {
        //Field for the current
        [SerializeField]private Voltage voltage;
        [SerializeField]private Color lowVoltColor;
        [SerializeField]private Color highVoltColor;
        [SerializeField]private MeshRenderer pinGraphic;
        [SerializeField]private Material pinMaterialPrefab;
        [SerializeField]private bool allowUserChanges = false;

        public UnityEvent<Voltage> OnSetVoltage;


        //Collider for detecting wire
        private BoxCollider _pinCollider;

        private Material _pinMaterial;

        public Voltage PinVoltage { get => voltage; }
    
        public void SetVoltage(Voltage voltage)
        {
            this.voltage = voltage;
            OnSetVoltage?.Invoke(voltage);
        }

        private void SetPinColor(Voltage voltage)
        {
            if(voltage == Voltage.Low)
            {
                _pinMaterial.color = lowVoltColor;
            }
            else
            {
                _pinMaterial.color = highVoltColor;
            }
        }
        
        private void Awake() {
            _pinCollider = GetComponent<BoxCollider>();
        }

        private void Start() 
        {
            _pinMaterial = Material.Instantiate(pinMaterialPrefab);
            pinGraphic.material = _pinMaterial;
            OnSetVoltage.AddListener(SetPinColor);
            SetVoltage(voltage);
        }

        private void OnValidate() 
        {
            if(allowUserChanges)
            {
                SetVoltage(voltage);
            }
        }


        private void OnDestroy() 
        {
            OnSetVoltage.RemoveListener(SetPinColor);
        }
        
        
    }
}
