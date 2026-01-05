namespace Game.Core.Animation
{
    public interface INudgeable<in T>
    {
        public void Nudge(T t);
    }
}