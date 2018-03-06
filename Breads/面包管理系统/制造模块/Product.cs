using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 面包管理系统 {
	/// <summary>
	/// 面包制作器
	/// </summary>
	static class Producer {
		public static Bread ProduceBread(int type) {
			return new Bread(type);
		}

		public static Bread ProduceBread(int id, int type, string name, float price, DateTime manufactureDate, DateTime expirationDate) {
			return new Bread(id, type, name, price, manufactureDate, expirationDate);
		}

		public static List<Bread> ProduceBreads(int type, int num) {
			List<Bread> breads = new List<Bread>();
			for (int i = 0; i < num; i++) {
				breads.Add(new Bread(type));
			}

			return breads;
		}
	}
}
