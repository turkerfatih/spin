namespace Game.Pooling
{
    public interface IPoolable<T> where T : IPoolable<T>
    {
        void OnSpawnFromPool();
        void OnReturnToPool();
        void SetPool(IBasePool<T> pool);
    }
}