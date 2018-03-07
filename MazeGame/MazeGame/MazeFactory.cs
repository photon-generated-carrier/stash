using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeGame {
	class MazeFactory {
		private MazeFactory() { }

		static public MazeFactory GetFactroy() {
			if (_mazeFactory == null)
				_mazeFactory = new MazeFactory();

			return _mazeFactory;
		}

		public virtual Maze MakeMaze(int gameSize) {
			return new Maze(gameSize); 
		}

		public virtual Wall MakeWall(Room room1, Room room2) {
			return new Wall(room1, room2);
		}

		public virtual Door MakeDoor(Room room1, Room room2) {
			return new Door(room1, room2);
		}

		public virtual Room MakeRoom(Location location) {
			return new Room(location);
		}

		static private MazeFactory _mazeFactory = null;
	}
}
