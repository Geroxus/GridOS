namespace IngameScript
{
    public abstract class OsBridge<T> where T : OsBridge<T>, new()
    {
        private static readonly T _instance;
        public static T Instance { get; } = _instance ?? (_instance = new T());
    }
}