using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeGame {
	// 用来标志一条永远畅通路，以后也可以用来复杂化迷宫
	class Door : MapMidWare {
		public Door(Room room1, Room room2):base(room1, room2){
		}

		override public SiteType Iam() {
			return SiteType.Door;
		}

		override public bool EnterAble {
			get {
				return true;
			}
		}
	}
}
