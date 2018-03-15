using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace MazeGame {
	// model
	class MazeGameModel {
		public MazeGameModel(int gameSizeH, int gameSizeW) {
			_gameSizeH = gameSizeH;
			_gameSizeW = gameSizeW;
		}

		public void CreateMaze(MazeFactory factory, MazeType mazeType = MazeType.Prime) {
			_maze = factory.MakeMaze(_gameSizeH, _gameSizeW, mazeType);
			
			SetGameParameters();
		}

		private Man _man = new Man();
		public Man Man {
			get {
				return _man;
			}
		}

		// 玩家移动
		public bool Move(Direction direction) {
			Room currentRoom = _maze.GetRoom(_man.GetLocation());
			IMapSite dstSite = currentRoom.GetSite(direction);
			if (dstSite != null && dstSite.EnterAble) {
				Room dstRoom = dstSite.Enter(currentRoom);
				var dstLocation = dstRoom.GetLocation();
				if (dstLocation - _man.GetLocation() == 1) {
					// neighbor
					_man.Move(direction);
				} else {
					_man.SetLocation(dstLocation);
				}
				return true;
			}

			return false;
		}

		public bool IsSuccess() {
			return (_man.GetLocation() == _exitLocation);
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
			// Maze.SetViewMan(view, _currentLocation.X, _currentLocation.Y);
		}

		private void SetGameParameters() {
			_entryLocation = new Location(0, 0);
			_man.SetLocation(_entryLocation);
			_man.SetGameSize(_gameSizeH, _gameSizeW);
			_exitLocation = new Location(_gameSizeH - 1, _gameSizeW - 1);
		}

		private Maze _maze;
		private int _gameSizeH; // height
		private int _gameSizeW; // width
		private Location _entryLocation;
		private Location _exitLocation;
	}
}
