using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MazeGame {
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();
			_timer.Interval = new TimeSpan(600000);
			_timer.Tick += _timer_Tick;
		}

		private int _gameSizeH = 20;
		private int _gameSizeW = 30;
		private MazeType _mazeType = MazeType.Default;
		private void Menu_NewGame_Click(object sender, RoutedEventArgs e) {
			NewGameWin newWin = new NewGameWin();
			if (newWin.ShowDialog() == true) {
				_gameSizeH = newWin.GameHeight;
				_gameSizeW = newWin.GameWidth;
				_mazeType = newWin.MazeType;
				_mazeGame = new MazeGameModel(_gameSizeH, _gameSizeW);
				_mazeGame.CreateMaze(_factory, _mazeType);
				_mazeGame.Man.SetParam(_roomHeight, _roomWidth);
				SiteType[][] view = _mazeGame.GetView();
				//for (int i = 0; i < view.Count(); i++)
				//	Console.WriteLine(view[i].ToIntString());

				DrawingMaze();
			}
        }

		private int _roomWidth = 22;
		private int _roomHeight = 22;

		private void DrawExit(DrawingContext dc, int x, int y, Direction d) {
			const int radius = 4;
			Point point = new Point();
			switch (d) {
				case Direction.East:
					point.X = _roomWidth * (y + 1);
					point.Y = _roomHeight * x + _roomHeight / 2;
					break;
				case Direction.South:
					point.X = _roomWidth * y + _roomWidth / 2;
					point.Y = _roomHeight * (x + 1);
					break;
				case Direction.West:
					point.X = _roomWidth * (x + 1) + _roomWidth / 2;
					point.Y = _roomHeight * y;
					break;
				case Direction.North:
					point.X = _roomWidth * y + _roomWidth / 2;
					point.Y = _roomHeight * x;
					break;
				default:
					return;
			}
			dc.DrawEllipse(Brushes.Green, null, point, radius, radius);
		}

		private void DrawEntry(DrawingContext dc, int x, int y, Direction d) {
			const int radius = 4;
			Point point = new Point();
			switch (d) {
				case Direction.East:
					point.X = _roomWidth * (y + 1);
					point.Y = _roomHeight * x  + _roomHeight / 2;
					break;
				case Direction.South:
					point.X = _roomWidth * (x + 1) + _roomWidth / 2;
					point.Y = _roomHeight * y;
					break;
				case Direction.West:
					point.X = _roomWidth * y;
					point.Y = _roomHeight * x + _roomHeight / 2;
					break;
				case Direction.North:
					point.X = _roomWidth * (x + 1) + _roomWidth / 2;
					point.Y = _roomHeight * y;
					break;
				default:
					return;
			}
			dc.DrawEllipse(Brushes.Red, null, point, radius, radius);
		}

		private void DrawDoor(DrawingContext dc, int x, int y, Direction d) {
			Rect rect = new Rect();
			const int wallWidth = 1;
			switch (d) {
				case Direction.East:
					rect.Location = new Point(_roomWidth * (y + 1) - wallWidth / 2, _roomHeight * x - wallWidth / 2);
					rect.Size = new Size(wallWidth, _roomHeight + wallWidth);
					break;
				case Direction.South:
					rect.Location = new Point(_roomWidth * y - wallWidth / 2, _roomHeight * (x + 1) - wallWidth / 2);
					rect.Size = new Size(_roomWidth + wallWidth, wallWidth);
					break;
				case Direction.West: // 注意边界呦
					rect.Location = new Point(_roomHeight * y - wallWidth / 2, _roomWidth * x);
					rect.Size = new Size(wallWidth, _roomHeight);
					break;
				case Direction.North:
					rect.Location = new Point(_roomHeight * y, _roomWidth * x - wallWidth / 2);
					rect.Size = new Size(_roomHeight, wallWidth);
					break;
				default:
					return; ;
			}
			dc.DrawRectangle(Brushes.Orange, null, rect);
		}

		private void DrawWall(DrawingContext dc, int x, int y, Direction d) {
			Rect rect = new Rect();
			const int wallWidth = 2;
			switch(d) {
				case Direction.East:
					rect.Location = new Point(_roomWidth * (y + 1) - wallWidth / 2, _roomHeight * x - wallWidth / 2);
					rect.Size = new Size(wallWidth, _roomHeight + wallWidth);
					break;
				case Direction.South:
					rect.Location = new Point(_roomWidth * y - wallWidth / 2, _roomHeight * (x + 1) - wallWidth / 2);
					rect.Size = new Size(_roomWidth + wallWidth, wallWidth);
					break;
				case Direction.West: // 注意边界呦
					rect.Location = new Point(_roomHeight * y - wallWidth / 2, _roomWidth * x);
					rect.Size = new Size(wallWidth, _roomHeight);
					break;
				case Direction.North:
					rect.Location = new Point(_roomHeight * y, _roomWidth * x - wallWidth / 2);
					rect.Size = new Size(_roomHeight, wallWidth);
					break;
				default:
					return;
			}
			dc.DrawRectangle(Brushes.DarkCyan, null, rect);
		}

		private void DrawSite(DrawingContext dc, int x, int y, Direction d, SiteType type) {
			switch(type) {
				case SiteType.Door:
					DrawDoor(dc, x, y, d);
					break;
				case SiteType.Wall:
					DrawWall(dc, x, y, d);
					break;
				case SiteType.Entry:
					DrawEntry(dc, x, y, d);
					break;
				case SiteType.Exit:
					DrawExit(dc, x, y, d);
					break;
				default:
					return;
			}
		}

		private void DrawingMaze() {
			SiteType[][] view = _mazeGame.GetView();
			
			// Create a DrawingGroup
			DrawingGroup dGroup = new DrawingGroup();
			dGroup.ClipGeometry = new RectangleGeometry(new Rect(0, 0, _roomWidth * _gameSizeW, _roomHeight * _gameSizeH));

			// Obtain a DrawingContext from 
			// the DrawingGroup.
			using (DrawingContext dc = dGroup.Open()) {
				// Pen shapeOutlinePen = new Pen(Brushes.Black, 2);
				//shapeOutlinePen.Freeze();

				// Push an opacity change of 0.5. 
				// The opacity of each subsequent drawing will
				// will be multiplied by 0.5.
				//dc.PushOpacity(0.5);

				// This rectangle is drawn at 50% opacity.
				//dc.DrawRectangle(Brushes.Blue, shapeOutlinePen, new Rect(5, 5, 5, 5));

				// Blurs subsquent drawings. 
				//dc.PushEffect(new BlurBitmapEffect(), null);

				dc.DrawRectangle(Brushes.White, null, new Rect(0, 0, _roomWidth * _gameSizeW, _roomHeight * _gameSizeH));

				for (int i = 0; i < _gameSizeH; i++) {
					for (int j = 0; j < _gameSizeW; j++) {
						DrawSite(dc, i, j, Direction.East, view[i * 2 + 1][j + 1]);
						DrawSite(dc, i, j, Direction.South, view[i * 2 + 2][j + 1]);
					}
				}
				// draw left-top border
				for (int i = 0; i < _gameSizeW; i++) {
					DrawSite(dc, 0, i, Direction.North, view[0][i + 1]);
				}
				for (int i = 0; i < _gameSizeH; i++) {
					DrawSite(dc, i, 0, Direction.West, view[i * 2 + 1][0]);
				}

				DrawMan(dc);
            }

			// Display the drawing using an image control.
			DrawingImage dImageSource = new DrawingImage(dGroup);
			canvas.Source = dImageSource;
		}

		private void DrawMan(DrawingContext dc) {
			Man man = _mazeGame.Man;
			Location p = man.GetPosition();
			// head
			dc.DrawEllipse(Brushes.Black, null, new Point(p.Y + _roomWidth / 2, p.X + 0.25 * _roomHeight), 0.20 * _roomWidth, 0.20 * _roomHeight);
			dc.DrawLine(new Pen(Brushes.Black, 0.05 * _roomWidth), new Point(p.Y + _roomWidth / 2, p.X + 0.4 * _roomHeight), new Point(p.Y + _roomWidth / 2, p.X + 0.65 * _roomHeight));
			dc.DrawLine(new Pen(Brushes.Black, 0.05 * _roomWidth), new Point(p.Y + 0.2 * _roomWidth, p.X + 0.5 * _roomHeight), new Point(p.Y + 0.8 * _roomWidth, p.X + 0.5 * _roomHeight));
			dc.DrawLine(new Pen(Brushes.Black, 0.05 * _roomWidth), new Point(p.Y + _roomWidth / 2, p.X + 0.65 * _roomHeight), new Point(p.Y + 0.2 * _roomWidth, p.X + 0.9 * _roomHeight));
			dc.DrawLine(new Pen(Brushes.Black, 0.05 * _roomWidth), new Point(p.Y + _roomWidth / 2, p.X + 0.65 * _roomHeight), new Point(p.Y + 0.8 * _roomWidth, p.X + 0.9 * _roomHeight));
		}

		private MazeGameModel _mazeGame;
		private MazeFactory _factory = MazeFactory.GetFactroy();

		private DispatcherTimer _timer = new DispatcherTimer();
		private void Window_KeyUp(object sender, KeyEventArgs e) {
			_timer.Stop();
		}

		private void Window_KeyDown(object sender, KeyEventArgs e) {
			if (e.Key == Key.Down || e.Key == Key.Up || e.Key == Key.Left || e.Key == Key.Right) {
				_timer.Tag = e.Key;
				_timer.Start();
            } else {
				_timer.Stop();
			}
		}

		private void _timer_Tick(object sender, EventArgs e) {
			Key key = (Key)_timer.Tag;
			if (key == Key.Down) {
				_mazeGame.Move(Direction.South);
			} else if (key == Key.Up) {
				_mazeGame.Move(Direction.North);
			} else if (key == Key.Left) {
				_mazeGame.Move(Direction.West);
			} else if (key == Key.Right) {
				_mazeGame.Move(Direction.East);
			}
			DrawingMaze();
			if (_mazeGame.IsSuccess()) {
				_timer.Stop();
				MessageBox.Show("You Win!");
			}
		}

		private void Menu_Exit_Click(object sender, RoutedEventArgs e) {
			Close();
		}
	}
}
