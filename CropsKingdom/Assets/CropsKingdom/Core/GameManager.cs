using System.Collections.Generic;

namespace CropsKingdom.Core
{
    public class GameManager
    {
        private static List<Game> _games = new List<Game>();

        public static Game CreateGame()
        {
            var game = new Game()
            {
                Id = _games.Count
            };
            _games.Add(game);
            return game;
        }

        public static Game GetGameById(int id)
        {
            return _games[id];
        }

        public static void RemoveGame(int id)
        {
            _games.RemoveAt(id);
        }
    }
}