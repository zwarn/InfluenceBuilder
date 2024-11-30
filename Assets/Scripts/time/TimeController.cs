using System;
using input;
using UnityEngine;
using Zenject;

namespace time
{
    public class TimeController : MonoBehaviour
    {
        [Inject] private InputEvents _inputEvents;

        private bool _play = false;
        private bool _step = false;

        private void OnEnable()
        {
            _inputEvents.OnToggleAutomaticCommand += OnTogglePlay;
            _inputEvents.OnPerformStepCommand += Step;
        }

        private void OnDisable()
        {
            _inputEvents.OnToggleAutomaticCommand -= OnTogglePlay;
            _inputEvents.OnPerformStepCommand -= Step;
        }

        public bool ShouldStep()
        {
            var shouldStep = _play || _step;
            _step = false;

            return shouldStep;
        }

        public bool IsPlay()
        {
            return _play;
        }

        private void OnTogglePlay()
        {
            _play = !_play;
        }

        public void Stop()
        {
            _play = false;
        }

        public void Step()
        {
            _step = true;
        }

        public void Play()
        {
            _play = true;
        }
    }
}