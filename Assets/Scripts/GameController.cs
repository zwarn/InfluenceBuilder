using influence;
using map;
using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour
{
    [SerializeField] private Transform quad;
    private bool _automatic = false;

    [Inject] private MapController _mapController;
    [Inject] private InfluenceController _influenceController;

    private void Start()
    {
        var width = _mapController.Width;
        var height = _mapController.Height;

        var camera = Camera.main;
        camera.transform.position = new Vector3(width / 2, height / 2, -10);
        camera.orthographicSize = (width / 2) + 5;

        quad.position = new Vector3(width / 2, height / 2, 0);
        quad.localScale = new Vector3(width, height, width);
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