using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeGame {
	interface IMapSite {
		// 返回最终停留的房间, 输入进入的节点
		Room Enter(IMapSite srcSite);
		bool EnterAble { get; }
		SiteType Iam();
	}

	enum SiteType {
		None = 0,
		Room,
		Wall,
		Door,
		Man = 7,
		Entry,
		Exit
	}

	internal static class SiteTypeExtensions {
		public static string ToIntString(this SiteType[] siteTypes) {
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < siteTypes.Count(); i++)
				sb.Append((int)siteTypes[i]);
			return sb.ToString();
		}
	}
}
