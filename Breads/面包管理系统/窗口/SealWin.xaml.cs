using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace 面包管理系统 {
	/// <summary>
	/// 销售窗口
	/// </summary>
	public partial class SealWin : Window {
		public SealWin() {
			InitializeComponent();
		}

		public StoreHouse store;
		private List<string> typeNames = new List<string>();

		private void RefreshData() {
			shoppingCart.Items.Clear();
			typeNames.Clear();
			List<Bread> storeBreads = store.GetBreads();
			breads.Clear();
			for (int i = 1; i < Bread.BreadTypes.Length; i++) {
				var q = from r in storeBreads
						where r.Type == i 
						orderby r.Id ascending
						select r;
				List<Bread> tBreads = new List<Bread>();
				tBreads.AddRange(q);
				breads.Add(tBreads);
				typeNames.Add(Bread.BreadTypes[i].Name);
			}
			typeList.ItemsSource = null;
			typeList.ItemsSource = typeNames;
			typeList.SelectedIndex = -1;
			typeList.SelectedIndex = 0;
		}
		private void Window_Loaded(object sender, RoutedEventArgs e) {
			RefreshData();
        }

		List<List<Bread>> breads = new List<List<Bread>>();

		private void typeList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			if (typeList.SelectedIndex < 0)
				return;
			breadList.ItemsSource = breads[typeList.SelectedIndex];
			if (Bread.BreadTypes[typeList.SelectedIndex + 1].Img != null) {
				image.Source = Bread.BreadTypes[typeList.SelectedIndex + 1].Img;
				image.Visibility = Visibility.Visible;
            } else {
				image.Visibility = Visibility.Hidden;
			}
		}

		private void CalTotalPrice() {
			float total = 0;
			foreach (var c in shoppingCart.Items) {
				total += ((Bread)c).Price;
			}

			totalPrice.Content = "$" + total;
		}
	
		private void breadList_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
			Bread bread = breadList.SelectedItem as Bread;
			if (bread == null)
				return;

			shoppingCart.Items.Add(bread);
			CalTotalPrice();
			breads[typeList.SelectedIndex].Remove(bread);
			breadList.ItemsSource = null;
			breadList.ItemsSource = breads[typeList.SelectedIndex];
		}

		private void shoppingCart_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
			Bread bread = shoppingCart.SelectedItem as Bread;
			if (bread == null)
				return;

			shoppingCart.Items.Remove(bread);
			CalTotalPrice();
        }

		private void Button_Click(object sender, RoutedEventArgs e) {
			var result = MessageBox.Show("共需支付" + totalPrice.Content + ", 请付款...", "确认付款？", MessageBoxButton.OKCancel);
			if (result == MessageBoxResult.OK) {
				foreach (var c in shoppingCart.Items) {
					store.Remove((Bread)c);
				}
				RefreshData();
            }
		}

		private void Button_Click_1(object sender, RoutedEventArgs e) {
			this.Close();
		}
	}
}
