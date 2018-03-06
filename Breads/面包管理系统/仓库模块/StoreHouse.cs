using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace 面包管理系统 {
	/// <summary>
	/// 仓库模块
	/// </summary>
	public class StoreHouse {
		public static bool CreateStoreHouse(int capacity) {
			if (storeHouseInstance != null)
				return false;

			storeHouseInstance = new StoreHouse(capacity);
			RefreshData();

			/*// just for test
			storeHouseInstance.inventory = 1;
			storeHouseInstance.breads.Add(new Croissant());
			storeHouseInstance.breads[0].Id = 1;
			//*/

			return true;
		}

		private static void RefreshData() {
			//从数据库中读取数据，更新capacity和库存
			storeHouseInstance.breads.Clear();
			storeHouseInstance.breads.AddRange(DataBaseWrapper.GetBreadTable());
			
			storeHouseInstance.inventory = storeHouseInstance.breads.Count;
		}

		public static StoreHouse GetStoreHouse() {
			return storeHouseInstance;
		}

		public List<Bread> GetBreads() {
			return breads;
		}

		public bool Add(Bread bread) {
			if (inventory + 1 > capacity)
				return false;

			if (DataBaseWrapper.InsertBread(bread)) {
				breads.Add(bread);
				inventory++;
				RefreshData();
				return true;
			} else {
				RefreshData();
				return false;
			}

		}

		public bool Add(List<Bread> breadList) {
			bool result = true;
			if (breadList.Count + inventory > capacity)
				return false;

			foreach (var c in breadList) {
				result &= Add(c);
			}

			return result;
		}

		public void UpdateBread(int id, string name, float price, DateTime manufactureDate, DateTime expirationDate) {
			if (DataBaseWrapper.UpdateBreadData(id, name, price, manufactureDate, expirationDate)) {
				RefreshData();
			}
		}

		public bool Remove(Bread bread) {
			if (DataBaseWrapper.RemoveBread(bread)) {
				breads.Remove(bread);
				inventory--;
				return true;
			} else {
				RefreshData();
				return false;
			}
		}

		public bool Remove(List<Bread> breadList) {
			bool result = true;
			foreach (Bread b in breadList)
				result &= Remove(b);

			return result;
		}

		public bool IsCapable(int num) {
			if (num + inventory > capacity)
				return false;
			return true;
		}

		private StoreHouse() { }
		private StoreHouse(int capacity) {
			this.capacity = capacity;	
		}
			
		private static StoreHouse storeHouseInstance;
		private int capacity;
		private int inventory;
		public int Capacity {
			get {
				return capacity;
			}
		}
		private List<Bread> breads = new List<Bread>();
	}
}
