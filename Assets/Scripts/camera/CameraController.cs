using System;
using input;
using map;
using UnityEngine;
using Zenject;

namespace camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private new Camera camera;
        [SerializeField] private float zoomFactor = 1.1f;
        [SerializeField] private float zoomSmooth = 0.7f;
        [SerializeField] private float translationSpeed = 1f;

        private Vector3 _basePosition;
        private float _targetZoom;
        private Vector2 _targetTranslation = Vector2.zero;
        private float _maxTranslation = 5;
        private float _minZoom = 1;
        private float _maxZoom = 10;

        [Inject] private MapController _mapController;
        [Inject] private InputEvents _inputEvents;

        private void OnEnable()
        {
            _inputEvents.OnZoomIn += ZoomIn;
            _inputEvents.OnZoomOut += ZoomOut;
            _inputEvents.OnTranslate += Translate;
        }

        private void OnDisable()
        {
            _inputEvents.OnZoomIn -= ZoomIn;
            _inputEvents.OnZoomOut -= ZoomOut;
            _inputEvents.OnTranslate -= Translate;
        }

        private void Start()
        {
            _targetZoom = _mapController.Width / 2f;
            _basePosition = new Vector3(_mapController.Width / 2f, _mapController.Height / 2f, -10);
            _minZoom = 1;
            _maxZoom = _mapController.Width / 2f;
            CalculateMaxTranslation(_mapController.Width);
        }

        private void Update()
        {
            HandleZoom();
            HandleTranslate();
        }

        private void HandleTranslate()
        {
            var cameraTransform = camera.transform;
            var rotatedDelta = cameraTransform.rotation *
                               new Vector3(_targetTranslation.x, _targetTranslation.y, 0);
            var targetPosition = _basePosition + rotatedDelta;
            camera.transform.position = Vector3.MoveTowards(cameraTransform.position, targetPosition,
                camera.orthographicSize * translationSpeed * Time.deltaTime);
        }

        private void HandleZoom()
        {
            camera.orthographicSize = Mathf.SmoothStep(camera.orthographicSize, _targetZoom, zoomSmooth);
        }

        private void ZoomOut()
        {
            _targetZoom *= zoomFactor;
            _targetZoom = Mathf.Clamp(_targetZoom, _minZoom, _maxZoom);
        }

        private void ZoomIn()
        {
            _targetZoom *= 1 / zoomFactor;
            _targetZoom = Mathf.Clamp(_targetZoom, _minZoom, _maxZoom);
        }

        private void Translate(Vector2 delta)
        {
            _targetTranslation += delta * (camera.orthographicSize * Time.deltaTime);
            _targetTranslation.x = Mathf.Clamp(_targetTranslation.x, -_maxTranslation, _maxTranslation);
            _targetTranslation.y = Mathf.Clamp(_targetTranslation.y, -_maxTranslation, _maxTranslation);
        }


        private void CalculateMaxTranslation(int size)
        {
            _maxTranslation = size / 2f;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(_targetTranslation, 1);
        }
    }
}