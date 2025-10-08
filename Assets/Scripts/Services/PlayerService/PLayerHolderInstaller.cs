using UnityEngine;
using VContainer;

[CreateAssetMenu(menuName = "SO/Installers/PlayerHolder", fileName = "PlayerHolder")]
public class PLayerHolderInstaller : ScriptableInstaller
{
    [SerializeField] private PlayerView _pfb;
    public override void Install(IContainerBuilder container)
    {
        container.Register<PlayerHolderService>(Lifetime.Singleton)
            .AsImplementedInterfaces()
            .WithParameter("playerPfb", _pfb);
    }
}
