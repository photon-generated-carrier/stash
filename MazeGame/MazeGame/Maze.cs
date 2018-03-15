using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeGame {
	class Maze {
		public Maze(int gameSizeH, int gameSizeW) {
			_gameSizeH = gameSizeH;
			_gameSizeW = gameSizeW;
			_rooms = new Room[_gameSizeH * _gameSizeW];
		}

		// 使用工厂来初始化room和外墙
		public void InitialRooms(MazeFactory factory) {
			for (int x = 0; x < _gameSizeH; x++)
				for (int y = 0; y < _gameSizeW; y++) {
					Room newRoom = factory.MakeRoom(new Location(x, y));

					if (x == 0) {
						new Wall(newRoom, Direction.North);
					} else {
						Room northRoom = GetRoom(x - 1, y);
						northRoom.SetSite(Direction.South, newRoom);
						newRoom.SetSite(Direction.North, northRoom);
						if (x == _gameSizeH - 1)
							new Wall(newRoom, Direction.South);
					}

					if (y == 0) {
						new Wall(newRoom, Direction.West);
					} else {
						Room westRoom = GetRoom(x, y - 1);
						westRoom.SetSite(Direction.East, newRoom);
						newRoom.SetSite(Direction.West, westRoom);
						if (y == _gameSizeW - 1) {
							new Wall(newRoom, Direction.East);
						}
					}

					_rooms[x * _gameSizeW + y] = newRoom;
				}
		}

		// 生成墙
		public virtual void GenWalls(MazeFactory factory) {
			GenSimpleTestWalls();
		}

		// 简单测试墙
		//|------------------|
	//in  *  |               |
		//|  D     D         |
		//|  |               |
		//|  |               |
		//|  |               |
		//|  |               |
		//|  |               |
		//|  |_________D_____|
		//|  -> -> -> -> ->  * out
		//|------------------|
		private void GenSimpleTestWalls() {
			int x = 0, y = 0;
			for (x = 0; x < _gameSizeH - 1; x++) {
				new Wall(GetRoom(x, y), GetRoom(x, y + 1));
			}
			for (y = 1; y < _gameSizeW; y++) {
				new Wall(GetRoom(x, y), GetRoom(x - 1, y));
			}
			new Door(GetRoom(1, 0), GetRoom(1, 1));
			new Door(GetRoom(1, 1), GetRoom(1, 2));
			new Door(GetRoom(18, 29), GetRoom(19, 29));
		}

		// 获取地图的边缘字符表示
		//22222222222
		//82000000002
		//22000000002
		//22000000002
		//22000000002
		//22000000002
		//22000000002
		//22000000002
		//22222222222
		//22222222229
		//222222222222
		public void GenBorderCharView() {
			_charView = new SiteType[_gameSizeH * 2 + 1][];

			for (int x = 0; x < _gameSizeH * 2 + 1; x++) {
				_charView[x] = new SiteType[_gameSizeW + 1];
			}

			for (int x = 0; x < _gameSizeH; x++) {
				for (int y = 0; y < _gameSizeW; y++) {
					Room currentRoom = GetRoom(x, y);
					if (currentRoom.GetSite(Direction.East) != null)
						SetViewSite(x, y, Direction.East, currentRoom.GetSite(Direction.East).Iam());
					if (currentRoom.GetSite(Direction.South) != null)
						SetViewSite(x, y, Direction.South, currentRoom.GetSite(Direction.South).Iam());
				}
            }

			SetOuterWall();
        }

		public SiteType[][] GetCharView() {
			return _charView;
		}

		private void SetOuterWall() {
			// outer wall
			for (int x = 1; x < _gameSizeH * 2 + 1; x+=2) {
				_charView[x][0] = SiteType.Wall;
				_charView[x][_gameSizeW] = SiteType.Wall;
			}
			for (int y = 0; y < _gameSizeW + 1; y++) {
				_charView[0][y] = SiteType.Wall;
				_charView[_gameSizeH * 2][y] = SiteType.Wall;
			}
		}

		private void SetViewSite(int x, int y, Direction direction, SiteType siteType) {
			if (siteType == SiteType.Room)
				return;

			if (direction == Direction.South) {
				if (x == _gameSizeH - 1)
					return;

				_charView[1 + x * 2 + 1][1 + y]= siteType;
			}

			if (direction == Direction.East) {
				if (y == _gameSizeW - 1)
					return;

				_charView[1 + x * 2][1 + y] = siteType;
			}
		}

		// 设置入口和出口位置
		// 没写参数检查哦
		static public void SetViewEntry(SiteType[][] view, int x, int y, SiteType entryType) {
			int corX = 1 + x * 2;
			int corY = 1 + y;
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
				view[corX][corY] = entryType;
			else if (direction == Direction.West)
				view[corX][corY - 1] = entryType;
		}

		// 设置人的位置
		static public void SetViewMan(SiteType[][] view, int x, int y) {
			view[1 + x * 2][1 + y * 2] = SiteType.Man;
        }

		public Room GetRoom(int x, int y) {
			if (!(x < _gameSizeH && y < _gameSizeW))
				return null;
			return _rooms[x * _gameSizeW + y];
		}

		public Room GetRoom(Location location) {
			return GetRoom(location.X, location.Y);
		}

		protected Room[] _rooms;
		private SiteType[][] _charView;
		protected int _gameSizeH;
		protected int _gameSizeW;
	}
}
