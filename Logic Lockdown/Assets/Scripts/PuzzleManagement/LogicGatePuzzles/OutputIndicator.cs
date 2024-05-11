using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Game.PuzzleManagement.LogicGatePuzzles;

namespace Game.PuzzleManagement
{
    public class OutputIndicator : MonoBehaviour
    {
        [SerializeField]private Transform[] points;
        [SerializeField]private TMP_Text statusText;

        [SerializeField]private LogicGate logicGate;
        [SerializeField]private Color lowVoltColor;
        [SerializeField]private Color highVoltColor;
        [SerializeField]private Material wireMatPrefab;
        private LineRenderer _wireRenderer;
        private Material _wireMaterial;

        private void Awake() {
            _wireRenderer = GetComponent<LineRenderer>();
        }
        // Start is called before the first frame update
        void Start()
        {
            
            _wireMaterial = Material.Instantiate(wireMatPrefab);
            _wireRenderer.material = _wireMaterial;
        }

        // Update is called once per frame
        void Update()
        {
            _wireRenderer.positionCount = points.Length;
            for(int i = 0; i < points.Length; i++)
            {
                _wireRenderer.SetPosition(i, points[i].position);
            }
            
            if(logicGate.GetOutput() == Voltage.Low)
            {
                _wireMaterial.color = lowVoltColor;
                statusText.text = "0";
            }
            else
            {
                _wireMaterial.color = highVoltColor;
                statusText.text = "1";
            }
        }
    }
}
