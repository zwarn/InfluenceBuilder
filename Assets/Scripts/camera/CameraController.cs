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
        [SerializeField] private float translationSmooth = 0.1f;

        private Vector3 _basePosition;
        private float _targetZoom;
        private Vector2 _targetTranslation;
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
            _targetZoom = _mapController.width / 2f;
            _basePosition = new Vector3(0, 0, -10);
            _targetTranslation = new Vector2(_mapController.width / 2f, _mapController.height / 2f);
            _minZoom = 1;
            _maxZoom = _mapController.width / 2f;
            _maxTranslation = _mapController.width;
        }

        private void Update()
        {
            HandleZoom();
            HandleTranslate();
        }

        private void HandleTranslate()
        {
            var cameraTransform = camera.transform;
            var delta = new Vector3(_targetTranslation.x, _targetTranslation.y, 0);
            var targetPosition = _basePosition + delta;
            camera.transform.position = Vector3.Lerp(cameraTransform.position, targetPosition, translationSmooth);
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
            _targetTranslation.x = Mathf.Clamp(_targetTranslation.x, 0, _maxTranslation);
            _targetTranslation.y = Mathf.Clamp(_targetTranslation.y, 0, _maxTranslation);
        }
    }
}