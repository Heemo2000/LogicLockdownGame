using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Core;
using Game.CameraManagement;
using Cinemachine;

namespace Game.PlayerManagement
{
    public class PlayerSetup: MonoBehaviour
    {
        [SerializeField]private PlayerMovement playerPrefab;
        [SerializeField]private Transform spawnPoint;
        [SerializeField]private Canvas interactDetectCanavas;
        [SerializeField]private Canvas interactCanvas;
        [SerializeField]private Button backButton;
        [SerializeField]private CinemachineFreeLook freeLookCamera;
        [SerializeField]private Camera mainCamera;
        [SerializeField]private CameraSwitcher cameraSwitcher;
        private void InitializePlayer()
        {
            var player = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
            player.FollowCamera = mainCamera.transform;
            var playerInteract = player.GetComponent<PlayerInteract>();
            playerInteract.InteractDetectCanvas = interactDetectCanavas;
            playerInteract.InteractCanvas = interactCanvas;
            playerInteract.BackButton = backButton;

            freeLookCamera.Follow = player.transform;
            freeLookCamera.LookAt = player.transform;

            playerInteract.OnLeavingInteraction.AddListener(cameraSwitcher.SwitchBackToPlayerCamera);
        }

        private void Start() 
        {
            GameManager.Instance.OnGameplayStart.AddListener(InitializePlayer);
            GameManager.Instance.OnGameplayStart?.Invoke();
        }
    }
}
