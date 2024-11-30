using System;
using influence;
using influence.buildings;
using time;
using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour
{
    [Inject] private InfluenceController _influenceController;
    [Inject] private BuildingController _buildingController;
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
        _buildingController.Step();
        _gridEvents.GridUpdateEvent();
    }
}