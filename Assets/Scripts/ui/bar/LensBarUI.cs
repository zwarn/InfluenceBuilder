using System;
using System.Collections.Generic;
using input;
using lens;
using scriptableObjects.lens;
using show;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace ui.bar
{
    public class LensBarUI : MonoBehaviour
    {
        [SerializeField] private ButtonView prefab;

        private Dictionary<Lens, ButtonView> _buttonViews = new();
        private ButtonView _emptyButton;

        public List<LensType> lensTypes;
        public Sprite emptyIcon;

        [Inject] private ShowStatusEvents _showStatusEvents;
        [Inject] private InputEvents _inputEvents;

        private void Awake()
        {
            CreateButtonViews();
        }

        private void OnEnable()
        {
            _showStatusEvents.OnShowLens += LensSelection;
        }

        private void OnDisable()
        {
            _showStatusEvents.OnShowLens -= LensSelection;
        }

        private void CreateButtonViews()
        {
            _emptyButton = Instantiate(prefab, transform);
            _emptyButton.SetData(emptyIcon, () => _inputEvents.ToggleShowInfluenceEvent(null), null, null);
            _emptyButton.SetSelected(true);

            lensTypes.ForEach(lensType =>
            {
                var buttonView = Instantiate(prefab, transform);
                buttonView.SetData(lensType.GetIcon(), () => _inputEvents.ToggleShowInfluenceEvent(lensType.GetLens()),
                    () => _inputEvents.PreviewShowInfluenceEvent(lensType.GetLens()),
                    () => _inputEvents.PreviewShowInfluenceEvent(null));
                _buttonViews.Add(lensType.GetLens(), buttonView);
                buttonView.SetSelected(false);
            });
        }

        private void LensSelection(Lens lens)
        {
            _emptyButton.SetSelected(lens == null);

            foreach (var pair in _buttonViews)
            {
                pair.Value.SetSelected(pair.Key == lens);
            }
        }
    }
}