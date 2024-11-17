using System;
using influence;
using input;
using map;
using UnityEngine;
using Zenject;

namespace ui
{
    public class InfluenceGridUI : MonoBehaviour
    {
        [SerializeField] private Renderer quad;
        private Texture2D _texture2D;
        private bool _show = false;

        [Inject] private InfluenceController _influenceController;
        [Inject] private IColorChooser _colorChooser;
        [Inject] private GridEvents _gridEvents;
        [Inject] private InputEvents _inputEvents;
        [Inject] private MapController _mapController;

        private void Awake()
        {
            quad.gameObject.SetActive(_show);
        }

        private void Start()
        {
            var grid = _influenceController.GetGrid();

            quad.transform.position = new Vector3(grid.Width / 2f, grid.Height / 2f, -1);
            quad.transform.localScale = new Vector3(grid.Width, grid.Height, grid.Width);

            _texture2D = new Texture2D(grid.Width, grid.Height);
            _texture2D.filterMode = FilterMode.Point;
            OnUpdate();
            quad.material.mainTexture = _texture2D;
        }

        private void OnEnable()
        {
            _gridEvents.OnGridUpdate += OnUpdate;
            _inputEvents.OnToggleShowInfluence += OnToggleShow;
        }

        private void OnDisable()
        {
            _gridEvents.OnGridUpdate -= OnUpdate;
            _inputEvents.OnToggleShowInfluence -= OnToggleShow;
        }

        private void OnToggleShow()
        {
            _show = !_show;
            quad.gameObject.SetActive(_show);
            _mapController.Darken(_show);
        }

        private void OnUpdate()
        {
            InfluenceGrid grid = _influenceController.GetGrid();

            int index = 0;
            var values = grid.GetValues();
            var colors = new Color[values.Length];

            while (index < values.Length)
            {
                colors[index] = _colorChooser.GetColor(values[index]);
                index++;
            }

            _texture2D.SetPixels(colors);
            _texture2D.Apply();
        }
    }
}