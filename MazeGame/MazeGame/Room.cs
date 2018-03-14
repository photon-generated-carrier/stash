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
	}
}
