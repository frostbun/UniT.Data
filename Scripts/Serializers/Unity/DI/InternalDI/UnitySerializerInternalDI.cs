#nullable enable
namespace UniT.Data.Serializers.Unity.DI
{
    using UniT.DI;

    public static class UnitySerializerInternalDI
    {
        public static void AddUnitySerializer(this DependencyContainer container)
        {
            container.AddInterfaces<UnitySerializer>();
        }
    }
}