using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeGame {
	// model
	class MazeGameModel {
		public MazeGameModel(int gameSize) {
			_gameSize = gameSize;
		}

		public void CreateMaze(MazeFactory factory) {
			_maze = factory.MakeMaze(_gameSize);
			_maze.InitialRooms(factory);
			_maze.GenWalls(factory);
			SetGameParameters();
		}

		// 玩家移动
		public bool Move(Direction direction) {
			Room currentRoom = _maze.GetRoom(_currentLocation);
			IMapSite dstSite = currentRoom.GetSite(direction);
			if (dstSite.EnterAble) {
				Room dstRoom = dstSite.Enter(currentRoom);
				_currentLocation = dstRoom.GetLocation();
				return true;
			}

			return false;
		}

		public SiteType[][] GetView() {
			SiteType[][] view = _maze.GetCharView();
			SetViewParms(view);

			return view;
		}

		// 设置出口、入口、人物位置
		private void SetViewParms(SiteType[][] view) {
			Maze.SetViewEntry(view, _entryLocation.X, _entryLocation.Y, SiteType.Entry);
			Maze.SetViewEntry(view, _exitLocation.X, _exitLocation.Y, SiteType.Exit);
			Maze.SetViewMan(view, _currentLocation.X, _currentLocation.Y);
		}

		private void SetGameParameters() {
			_entryLocation = new Location(0, 0);
			_currentLocation = _entryLocation;
			_exitLocation = new Location(_gameSize - 1, _gameSize - 1);
		}

		private Maze _maze;
		private int _gameSize;
		private Location _entryLocation;
		private Location _exitLocation;
		private Location _currentLocation; // 当前位置
	}
}
