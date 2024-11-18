using influence;
using input;
using map;
using tool;
using ui;
using ui.tool;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public GameController gameController;
    public MapController mapController;
    public InfluenceController influenceController;

    public GradientColorChooser colorChooser;

    public ToolbarUI toolbarUI;

    public override void InstallBindings()
    {
        Container.Bind<MapController>().FromInstance(mapController).AsSingle();
        Container.Bind<GameController>().FromInstance(gameController).AsSingle();
        Container.Bind<InfluenceController>().FromInstance(influenceController).AsSingle();

        Container.Bind<IColorChooser>().FromInstance(colorChooser).AsSingle();

        Container.Bind<InputEvents>().FromNew().AsSingle();
        Container.Bind<ToolEvents>().FromNew().AsSingle();
        Container.Bind<GridEvents>().FromNew().AsSingle();

        toolbarUI.toolSelection.ForEach(tool => { Container.QueueForInject(tool); });
    }
}