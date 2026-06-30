#nullable enable
namespace UniT.Data.Serializers.Default.DI
{
    using Zenject;

    public static class DefaultSerializerZenject
    {
        public static void BindDefaultSerializer(this DiContainer container)
        {
            container.BindInterfacesAndSelfTo<DefaultSerializer>().AsSingle();
        }
    }
}