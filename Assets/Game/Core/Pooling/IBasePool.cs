namespace Game.Pooling
{
    public interface IBasePool<T> where T : IPoolable<T>
    {
        T Get();
        void Return(T item);
    }
}