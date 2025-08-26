namespace CardFramework.RandomProvider
{
    public struct RandomStateData
    {
        public ulong State;
        public ulong Inc;
    }

    internal sealed class PermuteCongruentialGenerator : IRandomProvider
    {
        // state
        private ulong state;
        private readonly ulong inc;
        
        public PermuteCongruentialGenerator() : this((ulong)System.Environment.TickCount)
        {
        }

        public PermuteCongruentialGenerator(ulong state, ulong streamId = 0)
        {
            this.state = 0ul;
            inc = (streamId << 1) | 1ul;
            Iterate();
            this.state += state;
            Iterate();
        }

        public RandomStateData State => new RandomStateData { Inc = inc, State = state };

        public PermuteCongruentialGenerator(RandomStateData state)
        {
            this.state = state.State;
            inc = state.Inc;
        }

        private uint Iterate()
        {
            ulong oldState = state;
            // Advance internal state
            state = unchecked(state * 6364136223846793005ul + inc);
            // Calculate output function (XSH RR), uses old state for max ILP
            uint xorshifted = (uint)(((oldState >> 18) ^ oldState) >> 27);
            int rot = (int)(oldState >> 59);
            return (xorshifted >> rot) | (xorshifted << ((-rot) & 31));
        }
        
        private uint Iterate(uint bound)
        {
            if (bound == 0u)
                return 0u;

            uint threshold = ((uint)-bound) % bound;

            while (true)
            {
                uint r = Iterate();
                if (r >= threshold)
                    return r % bound;
            }
        }
        
        public int Next()
        {
            return Next(int.MaxValue);
        }
        
        public int Next(int maxValue)
        {
            if (maxValue < 0)
                maxValue = 0;

            return (int)Iterate((uint)maxValue);
        }

        public int Range(int minValue, int maxValue)
        {
            if (maxValue < minValue)
                maxValue = minValue;

            uint delta = (uint)((long)maxValue - minValue);
            if (delta == 0u)
                return minValue;

            return minValue + (int)Iterate(delta);
        }

        public float NextFloat()
        {
            return Iterate() / (float)((ulong)uint.MaxValue + 1);
        }

        public float NextFloat(float minValue, float maxValue)
        {
            if (maxValue < minValue)
                maxValue = minValue;

            return minValue + (maxValue - minValue) * NextFloat();
        }
        
        public bool NextBool()
        {
            return NextFloat() <= 0.5f;
        }
    }
}