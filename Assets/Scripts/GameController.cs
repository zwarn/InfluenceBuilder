using System;
using influence;
using time;
using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour
{
    [Inject] private InfluenceController _influenceController;
    [Inject] private TimeController _timeController;
    [Inject] private GridEvents _gridEvents;

    private void Update()
    {
        if (_timeController.ShouldStep())
        {
            Step();
        }
    }

    public void Step()
    {
        _influenceController.Step();
        _gridEvents.GridUpdateEvent();
    }
}