using System.Collections.Generic;

namespace Runner
{
    /// <summary>
    /// Author: Mikkel Emil Nielsen-Man
    /// 
    /// This interface must be implemented by repositories that provide save and load fuctionality for GameObjects in the game.
    /// </summary>
    public interface IGameRepository
    {
        void Open();
        void Close();
        void ClearData();
        void AddGameObjects(List<GameObject> gameObjects);
        List<GameObject> GetGameObjects();
    }
}
