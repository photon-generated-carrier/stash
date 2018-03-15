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

		public virtual Maze MakeMaze(int gameSizeH, int gameSizeW, MazeType mazeType = MazeType.Default) {
			Maze maze = null;
			switch(mazeType) {
				case MazeType.Default:
					maze = new Maze(gameSizeH, gameSizeW);
					break;
				case MazeType.Prime:
					maze = new MazePrime(gameSizeH, gameSizeW);
					break;
				default:
					break;
			}
			maze.InitialRooms(this);
			maze.GenWalls(this);
			maze.GenBorderCharView();
			return maze; 
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
