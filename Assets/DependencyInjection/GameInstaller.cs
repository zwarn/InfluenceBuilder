using influence;
using map;
using ui;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public GameController gameController;
    public MapController mapController;
    public InfluenceController influenceController;

    public GradientColorChooser colorChooser;

    public override void InstallBindings()
    {
        Container.Bind<MapController>().FromInstance(mapController).AsSingle();
        Container.Bind<GameController>().FromInstance(gameController).AsSingle();
        Container.Bind<InfluenceController>().FromInstance(influenceController).AsSingle();

        Container.Bind<IColorChooser>().FromInstance(colorChooser).AsSingle();
    }
}