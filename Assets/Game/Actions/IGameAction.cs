using Cysharp.Threading.Tasks;

namespace Game.Actions
{
    public interface IGameAction
    {
         UniTask ExecuteAsync();
    }
}