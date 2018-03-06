using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
	/// StoreHouseWin.xaml 的交互逻辑
	/// </summary>
	public partial class StoreHouseWin : Window {
		public StoreHouseWin() {
			InitializeComponent();
		}

		public StoreHouse store;
		private List<Bread> breads;

		// 设置显示数据
		public void SetData(StoreHouse store) {
			this.store = store;
			breads = store.GetBreads();
			listView.ItemsSource = breads;
		}

		private void RefreshData() {
			breads = store.GetBreads();
			int index = listView.SelectedIndex;
			listView.ItemsSource = null;
			listView.SelectedIndex = -1;
			listView.ItemsSource = breads;
			listView.SelectedIndex = index;
		}

		private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			Bread bread = listView.SelectedItem as Bread;
			if (bread == null)
				return;

			tName.Text = bread.Name;
			tPrice.Text = "$"+ bread.Price.ToString();
			tDt1.Text = string.Format("{0:yyyy-MM-dd}", bread.ManufactureDate);
			tDt2.Text = string.Format("{0:yyyy-MM-dd}", bread.ExpirationDate);
		}

		private void Button_Change_Click(object sender, RoutedEventArgs e) {
			Bread bread = listView.SelectedItem as Bread;
			if (bread == null)
				return;
			var result = MessageBox.Show("确认修改数据？", "确认修改", MessageBoxButton.YesNo);
			if (result == MessageBoxResult.Yes) {
				try {
					store.UpdateBread(bread.Id, tName.Text, float.Parse(tPrice.Text.Substring(1))
						, DateTime.Parse(tDt1.Text), DateTime.Parse(tDt2.Text));
					RefreshData();
                } catch (Exception ex) {
					MessageBox.Show("输入数据有误：" + ex.Message);
				}
			}
		}

		private void Button_Del_Click(object sender, RoutedEventArgs e) {
			Bread bread = listView.SelectedItem as Bread;
			if (bread == null)
				return;
			var result = MessageBox.Show("确认删除面包？", "确认删除", MessageBoxButton.YesNo);
			if (result == MessageBoxResult.Yes) {
				if (store.Remove(bread)) {
					RefreshData();
                }
            }
		}
	}
}
