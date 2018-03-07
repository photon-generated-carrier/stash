using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeGame {
	class Maze {
		public Maze(int gameSize) {
			_gameSize = gameSize;
			_rooms = new Room[_gameSize * _gameSize];
		}

		// 使用工厂来初始化room和外墙
		public void InitialRooms(MazeFactory factory) {
			for (int x = 0; x < _gameSize; x++)
				for (int y = 0; y < _gameSize; y++) {
					Room newRoom = factory.MakeRoom(new Location(x, y));

					if (x == 0) {
						new Wall(newRoom, Direction.North);
					} else {
						Room westRoom = GetRoom(x - 1, y);
						westRoom.SetSite(Direction.East, newRoom);
						newRoom.SetSite(Direction.West, westRoom);
						if (x == _gameSize - 1)
							new Wall(newRoom, Direction.South);
					}

					if (y == 0) {
						new Wall(newRoom, Direction.West);
					} else {
						Room northRoom = GetRoom(x, y - 1);
						northRoom.SetSite(Direction.South, newRoom);
						newRoom.SetSite(Direction.North, northRoom);
						if (y == _gameSize - 1) {
							new Wall(newRoom, Direction.East);
						}
					}

					_rooms[x * _gameSize + y] = newRoom;
				}
		}

		// 生成墙
		public void GenWalls(MazeFactory factory) {
			GenSimpleTestWalls(factory);
			GenCharView();
		}

		// 简单测试墙
		//|------------------|
	//in  *  |               |
		//|  |               |
		//|  |               |
		//|  |               |
		//|  |               |
		//|  |               |
		//|  |               |
		//|  |_______________|
		//|  -> -> -> -> ->  * out
		//|------------------|
		private void GenSimpleTestWalls(MazeFactory factory) {
			int x = 0, y = 0;
			for (x = 0; x < _gameSize - 1; x++) {
				new Wall(GetRoom(x, y), GetRoom(x, y + 1));
			}
			for (y = 1; y < _gameSize; y++) {
				new Wall(GetRoom(x, y), GetRoom(x - 1, y));
			}
		}

		// 获取地图的字符表示
		//2222222222222222222
		//ba21010101010101012
		//2000000000000000002
		//2121010101010101012
		//2000000000000000002
		//2121010101010101012
		//2000000000000000002
		//2121010101010101012
		//2000000000000000002
		//2121010101010101012
		//2000000000000000002
		//2121010101010101012
		//2000000000000000002
		//2121010101010101012
		//2000000000000000002
		//2121010101010101012
		//2000000000000000002
		//211101010101010101c
		//2222222222222222222
		private void GenCharView() {
			_charView = new SiteType[_gameSize * 2 + 1][];

			for (int x = 0; x < _gameSize * 2 + 1; x++) {
				_charView[x] = new SiteType[_gameSize * 2 + 1];
			}

			for (int x = 0; x < _gameSize; x++) {
				for (int y = 0; y < _gameSize; y++) {
					Room currentRoom = GetRoom(x, y);
					if (currentRoom.GetSite(Direction.East) is Wall)
						SetViewSite(x, y, Direction.East, SiteType.Wall);
					if (currentRoom.GetSite(Direction.South) is Wall)
						SetViewSite(x, y, Direction.South, SiteType.Wall);
				}
            }

			SetViewRoomAndOuterWall();
		}

		public SiteType[][] GetCharView() {
			return _charView;
		}

		private void SetViewRoomAndOuterWall() {
			// rooms
			for (int x = 0; x < _gameSize; x++) {
				for (int y = 0; y < _gameSize; y++) {
					_charView[1 + x * 2][1 + y * 2] = SiteType.Room;
				}
			}

			// outer wall
			for (int x = 0; x < _gameSize * 2 + 1; x++) {
				_charView[x][0] = SiteType.Wall;
				_charView[x][_gameSize * 2] = SiteType.Wall;
			}
			for (int y = 0; y < _gameSize*2 + 1; y++) {
				_charView[0][y] = SiteType.Wall;
				_charView[_gameSize*2][y] = SiteType.Wall;
			}
		}

		private void SetViewSite(int x, int y, Direction direction, SiteType siteType) {
			if (direction == Direction.South) {
				if (x == _gameSize - 1)
					return;

				_charView[1 + x * 2 + 1][1 + y * 2] = SiteType.Wall;
			}

			if (direction == Direction.East) {
				if (y == _gameSize - 1)
					return;

				_charView[1 + x * 2][1 + y * 2 + 1] = SiteType.Wall;
			}
		}

		// 设置入口和出口位置
		// 没写参数检查哦
		static public void SetViewEntry(SiteType[][] view, int x, int y, SiteType entryType) {
			int corX = 1 + x * 2;
			int corY = 1 + y * 2;
			Direction direction;
			if (x == 0)
				direction = Direction.West;
			else if (y == 0)
				direction = Direction.North;
			else if (x > y)
				direction = Direction.South;
			else
				direction = Direction.East;

			if (direction == Direction.North)
				view[corX - 1][corY] = entryType;
			else if (direction == Direction.South)
				view[corX + 1][corY] = entryType;
			else if (direction == Direction.East)
				view[corX][corY + 1] = entryType;
			else if (direction == Direction.West)
				view[corX][corY - 1] = entryType;
		}

		// 设置人的位置
		static public void SetViewMan(SiteType[][] view, int x, int y) {
			view[1 + x * 2][1 + y * 2] = SiteType.Man;
        }

		public Room GetRoom(int x, int y) {
			if (!(x < _gameSize && y < _gameSize))
				return null;
			return _rooms[x * _gameSize + y];
		}

		public Room GetRoom(Location location) {
			return GetRoom(location.X, location.Y);
		}

		private Room[] _rooms;
		private SiteType[][] _charView;
		private int _gameSize;
	}
}
