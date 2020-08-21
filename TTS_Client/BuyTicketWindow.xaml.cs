﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace TTS_Client
{
	/// <summary>
	/// BuyTicketWindow.xaml 的交互逻辑
	/// </summary>
	public partial class BuyTicketWindow : Window {
		public BuyTicketWindow() {
			InitializeComponent();
		}

		public BuyTicketWindow(ClientWindow.TicketQueryInfo ticketQueryInfo) {
			InitializeComponent();
			this.ticketQueryInfo = ticketQueryInfo;
			canBuyTicket = SubmitTicketQuery(ticketQueryInfo);
			listView.ItemsSource = canBuyTicket;
		}

		public struct BuyTicketExtend {
			public int EnterStationNumber { get; set; }
			public string EnterStationName { get; set; }
			public string EnterStationTime { get; set; }
			public string EnterStationTimeIn { get; set; }
			public string EnterStationTimeOut { get; set; }

			public int LeaveStationNumber { get; set; }
			public string LeaveStationName { get; set; }
			public string LeaveStationTime { get; set; }
			public string LeaveStationTimeIn { get; set; }
			public string LeaveStationTimeOut { get; set; }

			public double TicketPrice { get; set; }
			public int TicketLine { get; set; }
			public string LineName { get; set; } //所属线路名称
			public int TrainID { get; set; } //车次
			public int BuyNumber { get; set; } //车票购买数量
			public int TicketRemain { get; set; } //车票剩余数量
			public string TimeTake { get; set; } //花费时间
		}

		public class AllBuyTicketExtend : ObservableCollection<BuyTicketExtend> { } //定义集合
		public ClientWindow.TicketQueryInfo ticketQueryInfo;
		public AllBuyTicketExtend searchBuyTicket;
		public AllBuyTicketExtend canBuyTicket;

		private void Button2_Copy_Click(object sender, RoutedEventArgs e) {
			textBox_tic1.Clear();
			textBox_tic2.Clear();
			textBox_tic3.Clear();
			textBox_tic4.Clear();
			textBox_tic5.Clear();
			textBox_tic6.Clear();
			textBox_tic7.Clear();
			checkBox_remain.IsChecked = true;
			precision.IsChecked = true;
			textBox.Text = "1";
			listView.ItemsSource = canBuyTicket;
		} //清空

		private void button2_Click(object sender, RoutedEventArgs e) {
			searchBuyTicket = new AllBuyTicketExtend();
			for (int i = 0; i < canBuyTicket.Count; i++) {
				searchBuyTicket.Add(canBuyTicket[i]);
			} //Copy

			if (precision.IsChecked == true) {
				if (textBox_tic1.Text != string.Empty) {
					for (int i = 0; i < searchBuyTicket.Count; i++) {
						if (searchBuyTicket[i].TrainID.ToString() != textBox_tic1.Text) {
							searchBuyTicket.Remove(searchBuyTicket[i]);
							i--;
						}
					}
				} //TrainID
				if (textBox_tic2.Text != string.Empty) {
					for (int i = 0; i < searchBuyTicket.Count; i++) {
						if (searchBuyTicket[i].EnterStationTime.ToString() != textBox_tic2.Text) {
							searchBuyTicket.Remove(searchBuyTicket[i]);
							i--;
						}
					}
				} //EnterStationTime
				if (textBox_tic3.Text != string.Empty) {
					for (int i = 0; i < searchBuyTicket.Count; i++) {
						if (searchBuyTicket[i].LeaveStationTimeIn.ToString() != textBox_tic3.Text) {
							searchBuyTicket.Remove(searchBuyTicket[i]);
							i--;
						}
					}
				} //LeaveStationTimeIn
				if (textBox_tic4.Text != string.Empty) {
					for (int i = 0; i < searchBuyTicket.Count; i++) {
						if (searchBuyTicket[i].TimeTake.ToString() != textBox_tic4.Text) {
							searchBuyTicket.Remove(searchBuyTicket[i]);
							i--;
						}
					}
				} //TimeTake
			} //精确搜索

			else {
				if (textBox_tic1.Text != string.Empty) {
					for (int i = 0; i < searchBuyTicket.Count; i++) {
						if (searchBuyTicket[i].TrainID.ToString().IndexOf(textBox_tic1.Text) == -1) {
							searchBuyTicket.Remove(searchBuyTicket[i]);
							i--;
						}
					}
				} //TrainID
				if (textBox_tic2.Text != string.Empty) {
					for (int i = 0; i < searchBuyTicket.Count; i++) {
						if (searchBuyTicket[i].EnterStationTime.ToString().IndexOf(textBox_tic2.Text) == -1) {
							searchBuyTicket.Remove(searchBuyTicket[i]);
							i--;
						}
					}
				} //EnterStationTime
				if (textBox_tic3.Text != string.Empty) {
					for (int i = 0; i < searchBuyTicket.Count; i++) {
						if (searchBuyTicket[i].LeaveStationTimeIn.ToString().IndexOf(textBox_tic3.Text) == -1) {
							searchBuyTicket.Remove(searchBuyTicket[i]);
							i--;
						}
					}
				} //LeaveStationTimeIn
				if (textBox_tic4.Text != string.Empty) {
					for (int i = 0; i < searchBuyTicket.Count; i++) {
						if (searchBuyTicket[i].TimeTake.ToString().IndexOf(textBox_tic4.Text) == -1) {
							searchBuyTicket.Remove(searchBuyTicket[i]);
							i--;
						}
					}
				} //TimeTake
			} //模糊搜索

			if (textBox_tic5.Text != string.Empty) {
				int BuyNumber = -1;
				if (int.TryParse(textBox_tic5.Text, out BuyNumber) == false || BuyNumber < 0) {
					MessageBox.Show("请输入正确的余票筛选数量！");
					return;
				}
				for (int i = 0; i < searchBuyTicket.Count; i++) {
					if (searchBuyTicket[i].TicketRemain < BuyNumber) {
						searchBuyTicket.Remove(searchBuyTicket[i]);
						i--;
					}
				}
			} //TicketRemain
			if (textBox_tic6.Text != string.Empty) {
				int PriceUpper = -1;
				if (int.TryParse(textBox_tic6.Text, out PriceUpper) == false || PriceUpper <= 0) {
					MessageBox.Show("请输入正确的价格筛选数量！");
					return;
				}
				for (int i = 0; i < searchBuyTicket.Count; i++) {
					if (searchBuyTicket[i].TicketPrice > PriceUpper) {
						searchBuyTicket.Remove(searchBuyTicket[i]);
						i--;
					}
				}
			} //TicketPriceUpper
			if (textBox_tic7.Text != string.Empty) {
				int PriceLower = -1;
				if (int.TryParse(textBox_tic7.Text, out PriceLower) == false || PriceLower < 0) {
					MessageBox.Show("请输入正确的价格筛选数量！");
					return;
				}
				for (int i = 0; i < searchBuyTicket.Count; i++) {
					if (searchBuyTicket[i].TicketPrice < PriceLower) {
						searchBuyTicket.Remove(searchBuyTicket[i]);
						i--;
					}
				}
			} //TicketPriceLower
			if (checkBox_remain.IsChecked == true) {
				for (int i = 0; i < searchBuyTicket.Count; i++) {
					if (searchBuyTicket[i].TicketRemain <= 0) {
						searchBuyTicket.Remove(searchBuyTicket[i]);
						i--;
					}
				}
			} //仅看有余票

			listView.ItemsSource = searchBuyTicket;
		} //筛选

		private AllBuyTicketExtend SubmitTicketQuery(ClientWindow.TicketQueryInfo ticketQueryInfo) {
			AllBuyTicketExtend buyTickets = new AllBuyTicketExtend();
			BuyTicketExtend buyTicket = new BuyTicketExtend();

			buyTicket.TrainID = 3;
			buyTicket.EnterStationTimeIn = DateTime.Now.AddHours(1).ToString();
			buyTicket.EnterStationTimeOut = DateTime.Now.AddHours(1.1).ToString();
			buyTicket.EnterStationTime = buyTicket.EnterStationTimeIn + " - " + buyTicket.EnterStationTimeOut;
			buyTicket.LeaveStationTimeIn = DateTime.Now.AddHours(2.3).ToString();
			buyTicket.LeaveStationTimeOut = DateTime.Now.AddHours(2.4).ToString();
			buyTicket.LeaveStationTime = buyTicket.EnterStationTimeIn + " - " + buyTicket.EnterStationTimeOut;
			buyTicket.TimeTake = DateTime.Parse(buyTicket.LeaveStationTimeIn).AddTicks(DateTime.Parse(
				buyTicket.EnterStationTimeOut).Ticks).TimeOfDay.ToString();
			buyTicket.TicketRemain = 3;
			buyTicket.TicketPrice = 290;
			buyTickets.Add(buyTicket);

			buyTicket.TrainID = 4;
			buyTicket.EnterStationTimeIn = DateTime.Now.AddHours(2).ToString();
			buyTicket.EnterStationTimeOut = DateTime.Now.AddHours(2.1).ToString();
			buyTicket.EnterStationTime = buyTicket.EnterStationTimeIn + " - " + buyTicket.EnterStationTimeOut;
			buyTicket.LeaveStationTimeIn = DateTime.Now.AddHours(3.3).ToString();
			buyTicket.LeaveStationTimeOut = DateTime.Now.AddHours(3.4).ToString();
			buyTicket.LeaveStationTime = buyTicket.EnterStationTimeIn + " - " + buyTicket.EnterStationTimeOut;
			buyTicket.TimeTake = DateTime.Parse(buyTicket.LeaveStationTimeIn).AddTicks(DateTime.Parse(
				buyTicket.EnterStationTimeOut).Ticks).TimeOfDay.ToString();
			buyTicket.TicketRemain = 15;
			buyTicket.TicketPrice = 260;
			buyTickets.Add(buyTicket);

			return buyTickets;
		}

		private void button_Click(object sender, RoutedEventArgs e) {
			if (listView.SelectedItems.Count == 0) {
				MessageBox.Show("未选择地点！");
				return;
			}
			int BuyNumber = 0;
			if (int.TryParse(textBox.Text, out BuyNumber) == false || BuyNumber <= 0) {
				MessageBox.Show("请输入正确的购票数量！");
				return;
			}
			BuyTicketExtend buyTicket = new BuyTicketExtend();
			buyTicket = (BuyTicketExtend)listView.SelectedItem;
			buyTicket.BuyNumber = BuyNumber;
			canBuyTicket.Add(buyTicket);
			//Close();
		} //购买

	}
}
