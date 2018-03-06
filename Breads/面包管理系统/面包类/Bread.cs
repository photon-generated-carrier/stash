using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 面包管理系统 {
	public class Bread {
		// 构造新面包
		public Bread(int type) {
			if (BreadTypes == null || BreadTypes.Length <= type)
				throw new IndexOutOfRangeException("未初始化面包类型数据！");
			this.type = type;
			this.name = BreadTypes[type].Name; // 默认数据
			price = BreadTypes[type].Price;
			manuFactureDate = DateTime.Now;
			expirationDate = manuFactureDate.AddDays(BreadTypes[type].ExpirationDays);
		}

		// 由数据构造面包
		public Bread(int id, int type, string name, float price, DateTime manufactureDate, DateTime expirationDate) {
			if (BreadTypes == null || BreadTypes.Length <= type)
				throw new IndexOutOfRangeException("未初始化面包类型数据！");
			this.id = id;
			this.type = type;
			this.name = name;
			this.price = price;
			this.manuFactureDate = manufactureDate;
			this.expirationDate = expirationDate;
		}

		protected int id;
		public int Id {
			get {
				return id;
			}
			set {
				id = value;
			}
		}
		protected int type;
		public int Type {
			get {
				return type;
			}
		}
		// 考虑有手动修改名称、数据等功能，因此单独保存Name等数据
		protected string name;
		public string Name {
			get {
				return name;
			}
		}
		protected DateTime manuFactureDate;
		public DateTime ManufactureDate {
			get {
				return manuFactureDate;
			}
		}
		protected DateTime expirationDate;
		public DateTime ExpirationDate {
			get {
				return expirationDate;
			}
		}
		protected float price = 0;
		public float Price {
			get {
				return price;
			}
		}

		public string TypeName {
			get {
				return BreadTypes[type].Name;
			}
		}
		public static BreadType[] BreadTypes;
	}
}
