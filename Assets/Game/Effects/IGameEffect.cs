namespace Game.Effects
{
    public interface IGameEffect
    {
        /// <summary>Apply immediately when the card is played.</summary>
        void Apply();

        /// <summary>Called before a spin starts (optional).</summary>
        void OnBeforeSpin();

        /// <summary>Called after a spin ends (optional).</summary>
        void OnAfterSpin();

        /// <summary>Whether the effect has expired (durability, etc).</summary>
        bool IsExpired { get; }
    }
}