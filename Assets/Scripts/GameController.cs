using influence;
using input;
using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour
{
    [SerializeField] private Transform quad;
    private bool _automatic = false;

    [Inject] private InfluenceController _influenceController;
    [Inject] private InputEvents _inputEvents;

    private void OnEnable()
    {
        _inputEvents.OnPerformStepCommand += Tick;
        _inputEvents.OnToggleAutomaticCommand += ToggleAutomatic;
    }

    private void OnDisable()
    {
        _inputEvents.OnPerformStepCommand -= Tick;
        _inputEvents.OnToggleAutomaticCommand -= ToggleAutomatic;
    }

    private void Update()
    {
        if (_automatic)
        {
            Tick();
        }
    }

    public void Tick()
    {
        _influenceController.Tick();
    }

    public void ToggleAutomatic()
    {
        _automatic = !_automatic;
    }
}