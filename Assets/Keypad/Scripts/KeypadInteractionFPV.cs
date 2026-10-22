using UnityEngine;
using UnityEngine.InputSystem;

namespace NavKeypad
{
    public class KeypadInteractionFPV : MonoBehaviour
    {
        [SerializeField] private Camera cam;

        private void Awake()
        {
            if (cam == null)
                cam = Camera.main;
        }

        private void Update()
        {
            if (cam == null)
                return;

            // Ray durch die Bildschirmmitte
            Ray ray = cam.ViewportPointToRay(
                new Vector3(0.5f, 0.5f, 0f)
            );

            // Ray während des Spielens im Editor anzeigen
            Debug.DrawRay(
                ray.origin,
                ray.direction * 10f,
                Color.red,
                0f,
                false
            );

            // Linksklick über das neue Input System
            var mouse = Mouse.current;

            if (mouse == null || !mouse.leftButton.wasPressedThisFrame)
                return;

            if (Physics.Raycast(ray, out var hit))
            {
                if (hit.collider.TryGetComponent(out KeypadButton keypadButton))
                {
                    keypadButton.PressButton();
                }
            }
        }
    }
}