using influence;
using input;
using map;
using show;
using time;
using tool;
using ui.bar;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public GameController gameController;
    public MapController mapController;
    public InfluenceController influenceController;
    public ShowStatusController showStatusController;
    public TimeController timeController;
    public BuildingController buildingController;

    public ToolbarUI toolbarUI;

    public override void InstallBindings()
    {
        Container.Bind<MapController>().FromInstance(mapController).AsSingle();
        Container.Bind<GameController>().FromInstance(gameController).AsSingle();
        Container.Bind<InfluenceController>().FromInstance(influenceController).AsSingle();
        Container.Bind<ShowStatusController>().FromInstance(showStatusController).AsSingle();
        Container.Bind<TimeController>().FromInstance(timeController).AsSingle();
        Container.Bind<BuildingController>().FromInstance(buildingController).AsSingle();

        Container.Bind<InputEvents>().FromNew().AsSingle();
        Container.Bind<ToolEvents>().FromNew().AsSingle();
        Container.Bind<GridEvents>().FromNew().AsSingle();
        Container.Bind<ShowStatusEvents>().FromNew().AsSingle();

        toolbarUI.toolSelection.ForEach(tool => { Container.QueueForInject(tool); });
    }
}