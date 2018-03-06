using Microsoft.Win32;
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
	/// ProduceNewTypeWin.xaml 的交互逻辑
	/// </summary>
	public partial class ProduceNewTypeWin : Window {
		public ProduceNewTypeWin() {
			InitializeComponent();
		}

		private void Button_Select_Click(object sender, RoutedEventArgs e) {
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "图像文件(*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png";
			bool? result = dlg.ShowDialog(this);
			if (result == true) {
				string fileName = dlg.FileName;
				tImg.Text = fileName;
				BitmapImage bmp = new BitmapImage(new Uri(fileName));
				img.Source = bmp;
				img.Visibility = Visibility.Visible;
			}
		}

		private void Button_OK_Click(object sender, RoutedEventArgs e) {
			var result = MessageBox.Show("确认添加新面包类型？（一旦添加便不可删除）", "确认添加", MessageBoxButton.YesNo);
			if (result == MessageBoxResult.Yes) {
				if (DataBaseWrapper.AddNewTypeBread(tName.Text
					, float.Parse(tPrice.Text), int.Parse(tDays.Text), tImg.Text)) {
					MessageBox.Show("添加成功！");
				} else {
					MessageBox.Show("添加失败！");
				}
            }
		}

		private void Button_Cancel_Click(object sender, RoutedEventArgs e) {
			Close();
		}
	}
}
