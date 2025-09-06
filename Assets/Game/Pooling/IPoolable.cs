namespace Game.Pooling
{
    public interface IPoolable
    {
        void OnSpawnFromPool();
        void OnReturnToPool();
        void SetPool<T>(IBasePool<T> pool) where T : IPoolable;
    }
}