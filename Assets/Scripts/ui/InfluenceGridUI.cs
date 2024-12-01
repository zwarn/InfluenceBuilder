using System;
using influence;
using map;
using show;
using UnityEngine;
using Zenject;

namespace ui
{
    public class InfluenceGridUI : MonoBehaviour
    {
        private static readonly int Values = Shader.PropertyToID("Values");
        private static readonly int Result = Shader.PropertyToID("Result");
        private static readonly int Width = Shader.PropertyToID("Width");
        private static readonly int Height = Shader.PropertyToID("Height");
        private static readonly int Alpha = Shader.PropertyToID("Alpha");
        private static readonly int MinValue = Shader.PropertyToID("MinValue");
        private static readonly int MaxValue = Shader.PropertyToID("MaxValue");

        [SerializeField] private Renderer quad;
        [SerializeField] private ComputeShader visualizeShader;
        [SerializeField] float alpha = 0.15f;
        [SerializeField] float minValue = 0.001f;
        [SerializeField] float maxValue = 5000;

        private RenderTexture _renderTexture;
        private ComputeBuffer _valuesBuffer;
        private Layer? _currentLayer = null;
        private int _width;
        private int _height;

        [Inject] private InfluenceController _influenceController;
        [Inject] private GridEvents _gridEvents;
        [Inject] private ShowStatusEvents _showStatusEvents;
        [Inject] private MapController _mapController;


        private void Start()
        {
            _width = _mapController.width;
            _height = _mapController.height;

            quad.transform.position = new Vector3(_width / 2f, _height / 2f, -1);
            quad.transform.localScale = new Vector3(_width, _height, _width);

            _renderTexture = new RenderTexture(_width, _height, 0, RenderTextureFormat.ARGB32)
            {
                enableRandomWrite = true,
                filterMode = FilterMode.Point
            };
            _renderTexture.Create();

            quad.material.mainTexture = _renderTexture;
            OnUpdate();
        }

        private void OnEnable()
        {
            _gridEvents.OnGridUpdate += OnUpdate;
            _showStatusEvents.OnShowLayer += OnShowLayer;

            _width = _mapController.width;
            _height = _mapController.height;

            _valuesBuffer = new ComputeBuffer(_width * _height, sizeof(double));
        }

        private void OnDisable()
        {
            _gridEvents.OnGridUpdate -= OnUpdate;
            _showStatusEvents.OnShowLayer -= OnShowLayer;

            _valuesBuffer.Dispose();
            _valuesBuffer = null;
        }

        private void OnShowLayer(Layer? layer)
        {
            _currentLayer = layer;
            quad.gameObject.SetActive(layer != null);
            _mapController.Darken(layer != null);
            if (layer != null)
            {
                UpdateLayer(layer.Value);
            }
        }

        private void OnUpdate()
        {
            if (_currentLayer == null)
            {
                return;
            }

            UpdateLayer(_currentLayer.Value);
        }

        private void UpdateLayer(Layer layer)
        {
            var values = _influenceController.GetValues(layer);

            _valuesBuffer.SetData(values);

            visualizeShader.SetBuffer(0, Values, _valuesBuffer);
            visualizeShader.SetTexture(0, Result, _renderTexture);
            visualizeShader.SetInt(Width, _width);
            visualizeShader.SetInt(Height, _height);
            visualizeShader.SetFloat(MinValue, minValue);
            visualizeShader.SetFloat(MaxValue, maxValue);
            visualizeShader.SetFloat(Alpha, alpha);

            visualizeShader.Dispatch(visualizeShader.FindKernel("CSMain"), Mathf.CeilToInt(_width / 8f),
                Mathf.CeilToInt(_height / 8f), 1);
        }
    }
}