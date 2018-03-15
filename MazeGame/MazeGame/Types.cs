using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeGame {
	// 坐标，x是离上边的距离，y是离左边的距离
	struct Location {
		public int X;
		public int Y;

		public Location(int x, int y) {
			X = x;
			Y = y;
		}

		static public int operator-(Location l1, Location l2) {
			return Math.Abs(l1.X - l2.X) + Math.Abs(l1.Y - l2.Y);
		}

		static public bool operator==(Location l1, Location l2) {
			return (l1.X == l2.X && l1.Y == l2.Y);
		}

		static public bool operator!=(Location l1, Location l2) {
			return !(l1 == l2);
		}

		public override bool Equals(object obj) {
			return base.Equals(obj);
		}

		public override int GetHashCode() {
			return base.GetHashCode();
		}
	}

	enum Direction : int {
		North = 0,
		East,
		South,
		West,
		_MAXNUM,
		None
	}

	public enum MazeType : int {
		Default = 0,
		Prime
	}
}
