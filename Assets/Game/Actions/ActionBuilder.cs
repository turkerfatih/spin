using System.Collections.Generic;

namespace Game.Actions
{
    public class ActionBuilder
    {
        private readonly List<IGameAction> actions = new();

        public ActionBuilder Do(IGameAction action)
        {
            actions.Add(action);
            return this;
        }

        public ActionBuilder Wait(float seconds)
        {
            actions.Add(new WaitAction(seconds));
            return this;
        }

        public ActionBuilder Parallel(params IGameAction[] pActions)
        {
            this.actions.Add(new ParallelAction(pActions));
            return this;
        }

        public ActionBuilder Sequence(params IGameAction[] pActions)
        {
            this.actions.Add(new SequenceAction(pActions));
            return this;
        }

        public IGameAction Build() => new SequenceAction(actions);
    }
}