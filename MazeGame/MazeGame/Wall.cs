using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeGame {
	class Wall : MapMidWare {
		public Wall(Room room1, Room room2):base(room1, room2){
		}

		override public SiteType Iam() {
			return SiteType.Wall;
		}
		// 单侧墙（外墙）
		public Wall(Room room, Direction direction):base(room, null) {
			room.SetSite(direction, this);
		}

		override public Room Enter(IMapSite srcSite) {
			return null;
		}
	}
}
