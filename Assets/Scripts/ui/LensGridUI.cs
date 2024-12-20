using System;
using influence;
using lens;
using map;
using show;
using UnityEngine;
using Zenject;

namespace ui
{
    public class LensGridUI : MonoBehaviour
    {
        private static readonly int Values = Shader.PropertyToID("Values");
        private static readonly int Result = Shader.PropertyToID("Result");
        private static readonly int Width = Shader.PropertyToID("Width");
        private static readonly int Height = Shader.PropertyToID("Height");
        private static readonly int Alpha = Shader.PropertyToID("Alpha");
        private static readonly int MinValue = Shader.PropertyToID("MinValue");
        private static readonly int MaxValue = Shader.PropertyToID("MaxValue");
        private static readonly int IsExponential = Shader.PropertyToID("IsExponential");

        [SerializeField] private Renderer quad;
        [SerializeField] private ComputeShader visualizeShader;

        private RenderTexture _renderTexture;
        private ComputeBuffer _valuesBuffer;

        private Lens _currentLens = null;

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
            _showStatusEvents.OnShowLens += OnShowLens;

            _width = _mapController.width;
            _height = _mapController.height;

            _valuesBuffer = new ComputeBuffer(_width * _height, sizeof(double));
        }

        private void OnDisable()
        {
            _gridEvents.OnGridUpdate -= OnUpdate;
            _showStatusEvents.OnShowLens -= OnShowLens;

            _valuesBuffer.Dispose();
            _valuesBuffer = null;
        }

        private void OnShowLens(Lens lens)
        {
            _currentLens = lens;
            quad.gameObject.SetActive(lens != null);
            _mapController.Darken(lens != null);
            if (lens != null)
            {
                UpdateLens(lens);
            }
        }

        private void OnUpdate()
        {
            if (_currentLens == null)
            {
                return;
            }

            UpdateLens(_currentLens);
        }

        private void UpdateLens(Lens lens)
        {
            var values = lens.GetValues(_influenceController);

            _valuesBuffer.SetData(values);

            visualizeShader.SetBuffer(0, Values, _valuesBuffer);
            visualizeShader.SetTexture(0, Result, _renderTexture);
            visualizeShader.SetInt(Width, _width);
            visualizeShader.SetInt(Height, _height);
            visualizeShader.SetFloat(MinValue, lens.MinValue);
            visualizeShader.SetFloat(MaxValue, lens.MaxValue);
            visualizeShader.SetFloat(Alpha, lens.Alpha);
            visualizeShader.SetBool(IsExponential, lens.IsExponential);

            visualizeShader.Dispatch(visualizeShader.FindKernel("CSMain"), Mathf.CeilToInt(_width / 8f),
                Mathf.CeilToInt(_height / 8f), 1);
        }
    }
}