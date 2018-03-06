using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Windows;
using System.Data;
using System.IO;

namespace 面包管理系统 {
	/// <summary>
	/// 数据库封装
	/// </summary>
	public static class DataBaseWrapper {
		public static bool ConnectDataBase() {
			string connStr = "server=192.168.233.130;user id=yy; password=123; database=mysql; pooling=false; Charset=utf8";
			try {
				conn = new MySqlConnection(connStr);
				conn.Open();
				conn.ChangeDatabase(dataBaseName);
				return true;

			} catch (MySqlException ex) {
				MessageBox.Show("Error connecting to the server: " + ex.Message);
				return false;
			}
		}

		public static void CloseDataBase() {
			conn.Close();
		}

		public static BreadType[] GetBreadTypeTable() {
			MySqlDataAdapter da;

			da = new MySqlDataAdapter("SELECT * FROM " + breadTypeTblName, conn);
			DataTable data = new DataTable();

			da.Fill(data);
			BreadType[] breadTypes = new BreadType[data.Rows.Count + 1];
			var reader = data.CreateDataReader();
			int i = 1;
			foreach (DataRow c in data.Rows) {
				int type = (int)c.Field<uint>("type");
				string name = c.Field<string>("name");
				float price = c.Field<float>("price");
				int expirationDays = (int)c.Field<uint>("expirationDays");
				byte[] img = c.Field<byte[]>("img");
				breadTypes[i++]= new BreadType(type, name, price, expirationDays, img);
			}

			return breadTypes;
		}

		public static List<Bread> GetBreadTable() {
			MySqlDataAdapter da;

			da = new MySqlDataAdapter("SELECT * FROM " + breadTblName, conn);
			DataTable data = new DataTable();

			da.Fill(data);
			List<Bread> breads = new List<Bread>();
			foreach (DataRow c in data.Rows) {
				int id = (int)c.Field<uint>("id");
				int type = (int)c.Field<uint>("type");
				string name = c.Field<string>("name");
				float price = c.Field<float>("price");
				DateTime dt1 = c.Field<DateTime>("manufactureDate");
				DateTime dt2 = c.Field<DateTime>("expirationDate");
				breads.Add(Producer.ProduceBread(id, type, name, price, dt1, dt2));
			}
			return breads;
		}

		public static bool UpdateBreadData(int id, string name, float price, DateTime manufactureDate, DateTime expirationDate) {
			string updateStr = string.Format(
				"UPDATE {0} SET " +
				"name=\"{1}\", price={2}, manufactureDate=\"{3}\", expirationDate=\"{4}\" " +
				"WHERE id={5} "
				,breadTblName, name, price
				,manufactureDate, expirationDate, id);
			MySqlCommand cmd = new MySqlCommand(updateStr, conn);
			try {
				cmd.ExecuteNonQuery();
				return true;
			} catch (MySqlException ex) {
				MessageBox.Show("Failed to insert bread:" + ex.Message);
				return false;
			}
		}

		public static bool InsertBread(Bread bread) {
			string insertStr = string.Format(
				"INSERT INTO {0}" +
				"(type, name, price, manufactureDate, expirationDate) " +
				"VALUES({1}, \"{2}\", {3}, \"{4}\",\"{5}\")",
				breadTblName, bread.Type, bread.Name, bread.Price,
				bread.ManufactureDate, bread.ExpirationDate);
			MySqlCommand cmd = new MySqlCommand(insertStr, conn);
			try {
				cmd.ExecuteNonQuery();
				return true;
			} catch (MySqlException ex) {
				MessageBox.Show("Failed to insert bread:" + ex.Message);
				return false;
			}
		}

		public static bool RemoveBread(Bread bread) {
			string delStr = string.Format(
				"DELETE FROM {0} " +
				"WHERE id={1}",
				breadTblName, bread.Id);
			MySqlCommand cmd = new MySqlCommand(delStr, conn);
			try {
				cmd.ExecuteNonQuery();
				return true;
			} catch (MySqlException ex) {
				MessageBox.Show("Failed to delete bread:" + ex.Message);
				return false;
			}
		}

		public static bool AddNewTypeBread(string name, float price, int expirationDays, string imgFile) {
			byte[] byImg = null;
			if (imgFile != "") {
				FileStream fs = new FileStream(imgFile, FileMode.Open, FileAccess.Read);
				byImg = new byte[fs.Length];
				fs.Read(byImg, 0, byImg.Length);
				fs.Close();
			}

			string insertStr = string.Format(
				"INSERT INTO {0}" +
				"(name, price, expirationDays, img) " +
				"VALUES(\"{1}\", {2}, \"{3}\",@byImg)",
				breadTypeTblName, name, price, expirationDays);
			MySqlCommand cmd = new MySqlCommand(insertStr, conn);
			cmd.Parameters.Add("@byImg", MySqlDbType.Blob);
			cmd.Parameters[0].Value = byImg;
			try {
				cmd.ExecuteNonQuery();
				return true;
			} catch (MySqlException ex) {
				MessageBox.Show("Failed to insert bread type:" + ex.Message);
				return false;
			}
		}

		public static MySqlConnection conn;


		private const string dataBaseName = "breadDB";
		private const string breadTblName = "bread_tbl";
		private const string breadTypeTblName = "bread_type_tbl";
	}
}
