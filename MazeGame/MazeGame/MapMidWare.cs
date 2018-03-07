using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeGame {
	// Room 和 Room之间的东西，比如墙，门，传送门
	class MapMidWare :IMapSite {
		public MapMidWare(Room room1, Room room2) {
			_room1 = room1;
			_room2 = room2;
			if (_room2 == null)
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
			room1.SetSite(direction1, this);
			room2.SetSite(direction2, this);
		}

		public virtual Room Enter(IMapSite srcSite) {
			Room srcRoom = srcSite as Room;
			if (srcRoom == null || srcRoom != _room1 || srcRoom != _room2)
				return null;

			// 进入另一侧
			return OtherSideRoom(srcRoom).Enter(this);
		}

		// 获取另一侧的房间
		public Room OtherSideRoom(Room room) {
			if (room == _room1)
				return _room2;
			else if (room == _room2)
				return _room1;
			else
				return null; // 非法情况
		}

		private Room _room1, _room2;

		virtual public bool EnterAble {
			get {
				return false;
			}
		}
	}
}
