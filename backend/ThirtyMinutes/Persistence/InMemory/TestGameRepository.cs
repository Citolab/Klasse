using System.Collections.Generic;
using System.Linq;

namespace ThirtyMinutes.Persistence.InMemory
{
    public class TestGameRepository : IGameRepository
    {
        private readonly List<Game> _games;

        public TestGameRepository()
        {
            _games = new List<Game>
                {new Game(1, "IDENTITEIT", 5, 30), new Game(2, "PASSWORD", 4, 20)};
        }

        public Game GetById(int id)
        {
            return _games.FirstOrDefault(g => g.Id == id);
        }
    }
}