using System;
using time;
using UnityEngine;
using Zenject;

namespace ui.bar
{
    public class TimeBarUI : MonoBehaviour
    {
        [SerializeField] private ButtonView prefab;
        [SerializeField] private Sprite stepImage;
        [SerializeField] private Sprite playImage;
        [SerializeField] private Sprite stopImage;

        private ButtonView _stepButton;
        private ButtonView _playButton;
        private ButtonView _stopButton;

        [Inject] private TimeController _timeController;

        private void Awake()
        {
            CreateButtonViews();
        }

        private void CreateButtonViews()
        {
            _stopButton = CreateButtonView(stopImage, Stop);
            _stepButton = CreateButtonView(stepImage, Step);
            _playButton = CreateButtonView(playImage, Play);

            UpdateHighlightState();
        }

        private void Update()
        {
            UpdateHighlightState();
        }

        private void UpdateHighlightState()
        {
            if (_timeController.IsPlay())
            {
                _playButton.SetSelected(true);
                _stepButton.SetSelected(false);
                _stopButton.SetSelected(false);
            }
            else
            {
                _playButton.SetSelected(false);
                _stepButton.SetSelected(false);
                _stopButton.SetSelected(true);
            }
        }

        private ButtonView CreateButtonView(Sprite icon, Action onClick)
        {
            var buttonView = Instantiate(prefab, transform);
            buttonView.SetData(icon, onClick, null, null);
            return buttonView;
        }

        private void Stop()
        {
            _timeController.Stop();
            UpdateHighlightState();
        }
        private void Step()
        {
            _timeController.Step();
            UpdateHighlightState();
        }
        private void Play()
        {
            _timeController.Play();
            UpdateHighlightState();
        }
    }
}