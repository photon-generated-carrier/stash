using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace MazeGame {
	/// <summary>
	/// The player
	/// </summary>
	class Man {
		public Man() { }

		public void SetParam(int roomHeight, int roomWidth) {
			_roomHeight = roomHeight;
			_roomWidth = roomWidth;
		}

		public void SetLocation(Location p) {
			if (p.X < 0 || p.X >= _gameSize || p.Y < 0 || p.Y >= _gameSize)
				return;

			_curLocation = p;
			// 动画
			_curPosition.X = p.X * _roomHeight;
			_curPosition.Y = p.Y * _roomWidth;
		}

		public Location GetLocation() {
			return _curLocation;
		}

		public Location GetPosition() {
			return _curPosition;
		}

		public void SetGameSize(int gameSize) {
			_gameSize = gameSize;
		}

		public void Move(Direction d) {
			switch (d) {
				case Direction.North:
					Up();
					break;
				case Direction.East:
					Right();
					break;
				case Direction.South:
					Down();
					break;
				case Direction.West:
					Left();
					break;
				default:
					break;
			}
		}

		public void Down() {
			if (_curLocation.X < _gameSize) {
				_curLocation.X++;
				_curPosition.X += _roomHeight;
			}
			// 动画
		}

		public void Up() {
			if (_curLocation.X > 0) {
				_curLocation.X--;
				_curPosition.X -= _roomHeight;
			}
		// 动画
	}

		public void Left() {
			if (_curLocation.Y > 0) {
				_curLocation.Y--;
				_curPosition.Y -= _roomWidth;
			}
			// 动画
		}

		public void Right() {
			if (_curLocation.Y < _gameSize) {
				_curLocation.Y++;
				_curPosition.Y += _roomWidth;
			}
		// 动画
	}

		private int _roomHeight;
		private int _roomWidth;
		private int _gameSize;
		private Location _curLocation = new Location(0, 0); // 虚拟坐标
		private Location _curPosition = new Location(0, 0); // 画板内的坐标(左少角）
	}
}
