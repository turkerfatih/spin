namespace Game.Pooling
{
    public interface IBasePool<T> where T : IPoolable
    {
        T Get();
        void Return(T item);
    }
}