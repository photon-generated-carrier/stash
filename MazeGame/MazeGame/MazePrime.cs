using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeGame {
	/// <summary>
	/// generate walls by prime-algorithm
	/// </summary>
	class MazePrime : Maze{
		public MazePrime(int gameSizeH, int gameSizeW):base(gameSizeH, gameSizeW) { }

		public override void GenWalls(MazeFactory factory) {
			// generate all walls
			for (int x = 0; x < _gameSizeH; x++)
				for (int y = 0; y < _gameSizeW; y++) {
					if (x < _gameSizeH - 1)
						new Wall(GetRoom(x, y), GetRoom(x + 1, y));
					if (y < _gameSizeW - 1)
						new Wall(GetRoom(x, y), GetRoom(x, y + 1));
				}

			Random rand = new Random();
			int[] visited = new int[_gameSizeW * _gameSizeH]; // 0:no visted, 1:visited, 2:joined
			List<Location> path = new List<Location>();
			path.Add(new Location(0, 0));

			while (path.Count != 0) {
				Location c = path[rand.Next(path.Count)];
				visited[c.X * _gameSizeW + c.Y] = 1;
				path.Remove(c);
				List<Direction> check = new List<Direction>();

				if (c.Y > 0) {
					if (visited[c.X * _gameSizeW + c.Y - 1] == 1)
						check.Add(Direction.West);
					else if(visited[c.X * _gameSizeW + c.Y - 1] == 0) {
						path.Add(new Location(c.X, c.Y - 1));
						visited[c.X * _gameSizeW + c.Y - 1] = 2;
                    }
				}
				if (c.X > 0) {
					if (visited[(c.X - 1) * _gameSizeW + c.Y] == 1)
						check.Add(Direction.North);
					else if (visited[(c.X - 1) * _gameSizeW + c.Y] == 0) {
						path.Add(new Location(c.X - 1, c.Y));
						visited[(c.X - 1) * _gameSizeW + c.Y] = 2;
					}
				}
				if (c.Y < _gameSizeW - 1) {
					if (visited[c.X * _gameSizeW + c.Y + 1] == 1)
						check.Add(Direction.East);
					else if (visited[c.X * _gameSizeW + c.Y + 1] == 0) {
						path.Add(new Location(c.X, c.Y + 1));
						visited[c.X * _gameSizeW + c.Y + 1] = 2;
					}
				}
				if (c.X < _gameSizeH - 1) {
					if (visited[(c.X + 1) * _gameSizeW + c.Y] == 1)
						check.Add(Direction.South);
					else if (visited[(c.X + 1) * _gameSizeW + c.Y] == 0) {
						path.Add(new Location(c.X + 1, c.Y));
						visited[(c.X + 1) * _gameSizeW + c.Y] = 2;
					}
				}

				if (check.Count > 0) {
					Direction d = check[rand.Next(check.Count)];
					switch (d) {
						case Direction.North:
							Room.Join(GetRoom(c), GetRoom(c.X - 1, c.Y));
							break;
						case Direction.South:
							Room.Join(GetRoom(c), GetRoom(c.X + 1, c.Y));
							break;
						case Direction.East:
							Room.Join(GetRoom(c), GetRoom(c.X, c.Y + 1));
							break;
						case Direction.West:
							Room.Join(GetRoom(c), GetRoom(c.X, c.Y - 1));
							break;
						default:
							break;
					}
				}
			}
		}
	}
}
