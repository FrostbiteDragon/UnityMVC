using Prevail.Data;
using Prevail.Inventory;
using UnityEngine;
using Zenject;

public class TestInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<InventoryController>().AsSingle();
        Container.Bind<CraftingController>().AsSingle();
        Container.Bind<InventoryData>().AsSingle();
        Container.Bind<ContainerCache>().AsSingle();
    }
}
