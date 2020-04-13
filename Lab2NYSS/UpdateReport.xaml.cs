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

namespace Lab2NYSS
{
	/// <summary>
	/// Логика взаимодействия для UpdateReport.xaml
	/// </summary>
	public partial class UpdateReport : Window
	{
		public UpdateReport(List<Threat> addedThreats, List<Threat> deletedThreats, List<ThreatChanges> changedThreats)
		{
			InitializeComponent();
			ListBoxOfAddedThreats.ItemsSource = addedThreats;
			ListBoxOfChanges.ItemsSource = changedThreats;
			ListBoxOfDeletedThreats.ItemsSource = deletedThreats;
			addedCounter.Text = addedThreats.Count.ToString();
			changedCounter.Text = changedThreats.Count.ToString();
			deletedCounter.Text = deletedThreats.Count.ToString();

		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
