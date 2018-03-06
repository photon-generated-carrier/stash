using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace 面包管理系统 {
	/// <summary>
	/// 对应数据库的面包类型
	/// </summary>
	public class BreadType {
		public BreadType() {
			type = 0;
			name = "迷之面包";
			price = 0;
			expirationDays = 0;
		}

		public BreadType(int type, string name, float price, int expirationDays, byte[] img = null) {
			this.type = type;
			this.name = name;
			this.price = price;
			this.expirationDays = expirationDays;
			if (img != null) {
				this.img = new BitmapImage();
				this.img.BeginInit();
				this.img.StreamSource = new MemoryStream(img);
				this.img.EndInit();
			}
		}

		private int type;
		public int Type {
			get {
				return type;
			}
		}
		private string name;
		public string Name {
			get {
				return name;
			}
		}
		private float price;
		public float Price {
			get {
				return price;
			}
		}
		private int expirationDays;
		public int ExpirationDays {
			get {
				return expirationDays;
			}
		}
		private BitmapImage img = null;
		public BitmapImage Img {
			get {
				return img;
			}
		}
	}
}
