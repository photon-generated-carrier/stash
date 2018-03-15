using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeGame {
	class Room : IMapSite {
		public Room(Location location) {
			_location = location;
		}

		public virtual SiteType Iam() {
			return SiteType.Room;
		}

		public Room Enter(IMapSite srcMapSite) {
			return this;
		}

		public void SetSite(Direction direction, IMapSite site) {
			_sides[(int)direction] = site; 
		}

		public IMapSite GetSite(Direction direction) {
			return _sides[(int)direction];
		}

		public Direction FindSite(IMapSite site) {
			for (int i = 0; i < (int)Direction._MAXNUM; i++)
				if (_sides[i] == site) {
					return (Direction)i;
				}

			return Direction.None;
		}

		public bool EnterAble {
			get {
				return true;
			}
		}

		public Location GetLocation() {
			return _location;
		}

		private IMapSite[] _sides = new IMapSite[(int)Direction._MAXNUM];
		private Location _location;

		/// <summary>
		/// join r1 with r2
		/// </summary>
		public static void Join(Room room1, Room room2) {
			if (room1 == null || room2 == null)
				return;
			Location l1 = room1.GetLocation();
			Location l2 = room2.GetLocation();
			if (l1 - l2 != 1)
				return;
			Direction direction1, direction2;
			if (l1.X - l2.X > 0) {
				direction1 = Direction.North;
				direction2 = Direction.South;
			} else if (l1.X - l2.X < 0) {
				direction1 = Direction.South;
				direction2 = Direction.North;
			} else if (l1.Y - l2.Y > 0) {
				direction1 = Direction.West;
				direction2 = Direction.East;
			} else {
				direction1 = Direction.East;
				direction2 = Direction.West;
			}
			room1.SetSite(direction1, room2);
			room2.SetSite(direction2, room1);
		}
	}
}
