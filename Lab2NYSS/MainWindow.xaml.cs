using Microsoft.Win32;
using Npoi.Mapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab2NYSS
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	/// 
	public partial class MainWindow : Window
	{
		private string FILE_LINK = "https://bdu.fstec.ru/files/documents/thrlist.xlsx";
		private int currentPage;
		public MainWindow()
		{

			InitializeComponent();
			currentPage = 1;
			if (DBThreatsService.GetPagesCount() > 0)
			{
				CreateButton.IsEnabled = false;
				ThreatTable.ItemsSource = DBThreatsService.GetThreatsPage(currentPage);
				if (DBThreatsService.GetPagesCount() < 2)
				{
					NextButton.IsEnabled = false;
				}
			}
			else
			{
				UpdateButton.IsEnabled = false;
				NextButton.IsEnabled = false;
				SaveButton.IsEnabled = false;
			}
				PreviousButton.IsEnabled = false;
			
		}

		private void ThreatTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
		{
			Style elementStyle = new Style(typeof(TextBlock));
			elementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.WrapWithOverflow));
			elementStyle.Setters.Add(new Setter(TextBlock.PaddingProperty, new Thickness(5)));
			switch (e.Column.Header.ToString()) {
				case "Description":
				case "ThreatSource":
				case "Victim":
				case "Integrity":
				case "Availability":
				case "Confidentiality":
				case "Id":
					e.Column.Visibility = Visibility.Hidden;
					break;
				case "StringID":
					e.Column.Header = "Идентификатор угрозы";
					e.Column.Width = 100;
					((DataGridBoundColumn)e.Column).Binding = new Binding(e.PropertyName);
					((DataGridBoundColumn)e.Column).ElementStyle = elementStyle;
					break;
				case "Name":
					e.Column.Header = "Наименование угрозы";
					e.Column.Width = 342;
					((DataGridBoundColumn)e.Column).Binding = new Binding(e.PropertyName);
					((DataGridBoundColumn)e.Column).ElementStyle = elementStyle;
					break;
				default:
					break;
			}
		}

		private void ThreatTable_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
		{
			if (e.AddedCells.Count > 0)
			{
				var threat = (Threat)e.AddedCells[0].Item;
				DisplayDetailedInfo(threat);
				SourceOfAttack.Visibility = Visibility.Visible;
				Object.Visibility = Visibility.Visible;
				Availability.Visibility = Visibility.Visible;
				IntegrityBlock.Visibility = Visibility.Visible;
				ConfidentialityBlock.Visibility = Visibility.Visible;

			}

		}

		private void DisplayDetailedInfo(Threat threat) {
			IDBlock.Text = threat.StringID;
			TitleBlock.Text = threat.Name;
			DescriptionBlock.Text = threat.Description;
			SourceBlock.Text = threat.ThreatSource;
			VictimBlock.Text = threat.Victim;
			Color colorGreen = (Color)ColorConverter.ConvertFromString("#FF8FEA8B");
			Color colorRed = (Color)ColorConverter.ConvertFromString("#FFEA8B8B");


			if (threat.Integrity)
			{
				IntegrityBlock.Foreground = Brushes.Red;
				IntegrityBlock.Background = new RadialGradientBrush(new GradientStopCollection() { new GradientStop(colorRed, 0.384), new GradientStop(Colors.White, 1) });

			}
			else
			{
				IntegrityBlock.Foreground = Brushes.Green;
				IntegrityBlock.Background = new RadialGradientBrush(new GradientStopCollection() { new GradientStop(colorGreen, 0.384), new GradientStop(Colors.White, 1) });

			}
			if (threat.Confidentiality)
			{
				ConfidentialityBlock.Foreground = Brushes.Red;
				ConfidentialityBlock.Background = new RadialGradientBrush(new GradientStopCollection() { new GradientStop(colorRed, 0.384), new GradientStop(Colors.White, 1) });

			}
			else
			{
				ConfidentialityBlock.Foreground = Brushes.Green;
				ConfidentialityBlock.Background = new RadialGradientBrush(new GradientStopCollection() { new GradientStop(colorGreen, 0.384), new GradientStop(Colors.White, 1) });
			}
			if (threat.Availability)
			{
				Availability.Foreground = Brushes.Red;
				Availability.Background = new RadialGradientBrush(new GradientStopCollection() { new GradientStop(colorRed, 0.384), new GradientStop(Colors.White, 1) });

			}
			else 
			{
			Availability.Foreground = Brushes.Green;
			Availability.Background = new RadialGradientBrush(new GradientStopCollection() { new GradientStop(colorGreen, 0.384), new GradientStop(Colors.White, 1) });

			}

		}

		private void CreateButton_Click(object sender, RoutedEventArgs e)
		{
			string path = DownloadFile(FILE_LINK);
			List<Threat> lt = GetThreatsFromFile(path);
			DBThreatsService.SaveThreats(lt);
			lt = DBThreatsService.GetThreatsPage(currentPage);
			ThreatTable.ItemsSource = lt;
			CreateButton.IsEnabled = false;
			UpdateButton.IsEnabled = true;
			PreviousButton.IsEnabled = false;
			SaveButton.IsEnabled = true;
			if (DBThreatsService.GetPagesCount() > 1) NextButton.IsEnabled = true;
		}

		private string DownloadFile(string link)
		{
			string path = "thrlist.xlsx";
			using (var client = new WebClient())
			{
				client.DownloadFile(link, path);
			}
			return path;
		}

		private void PreviousButton_Click(object sender, RoutedEventArgs e)
		{
			if (--currentPage == 1)
			{
				PreviousButton.IsEnabled = false;
			}
			ThreatTable.ItemsSource = DBThreatsService.GetThreatsPage(currentPage);
			NextButton.IsEnabled = true;
		}

		private void NextButton_Click(object sender, RoutedEventArgs e)
		{
			if (++currentPage == DBThreatsService.GetPagesCount())
			{
				NextButton.IsEnabled = false;
			}
			ThreatTable.ItemsSource = DBThreatsService.GetThreatsPage(currentPage);
			PreviousButton.IsEnabled = true;

		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			SaveFileDialog fd = new SaveFileDialog();
			fd.Filter = "Excel file (*.xlsx)|*.xlsx";
			if (fd.ShowDialog() == true && fd.FileName != ""){
				string path = fd.FileName;
				MessageBox.Show(path);
				var threatlist = DBThreatsService.GetAllThreats();
				Mapper mapper = new Mapper();
				mapper.Save(path, threatlist);
			}

		}

		private List<Threat> GetThreatsFromFile(string path) {
			Mapper mapper = new Mapper(path);
			mapper.FirstRowIndex = 1;
			var obj = mapper.Take<Threat>("Sheet");
			List<Threat> lt = new List<Threat>();
			foreach (var item in obj)
			{
				lt.Add(item.Value);
			}
			File.Delete(path);
			return lt;
		}

		private void UpdateButton_Click(object sender, RoutedEventArgs e)
		{
			var answer = UpdateThreats();
			if (answer.status == UpdateStatuses.Failed)
			{
				MessageBox.Show(answer.error, "Возникла ошибка при обновлении базы данных");
			}
			else
			{
				DisplayUpdateReport(answer.changes);
			}
			ThreatTable.ItemsSource = DBThreatsService.GetThreatsPage(1);
			if (DBThreatsService.GetPagesCount() > 1) NextButton.IsEnabled = true;
			else NextButton.IsEnabled = false;
		}


		private void DisplayUpdateReport(List<Tuple<Threat, Threat>> changes) {
			var addedList = new List<Threat>();
			var deletedList = new List<Threat>();
			var changedList = new List<ThreatChanges>();

			foreach (var item in changes)
			{
				if (item.Item1 == null) {
					addedList.Add(item.Item2);
					continue;
				}
				if (item.Item2 == null) {
					deletedList.Add(item.Item1);
					continue;
				}
				changedList.Add(new ThreatChanges(item.Item1.Id, GetChangedFields(item)));
			}
			new UpdateReport(addedList, deletedList, changedList).Show();
		}

		private Dictionary<string, Tuple<string, string>> GetChangedFields(Tuple<Threat, Threat> threats) {
			var dictionary = new Dictionary<string, Tuple<string, string>>();
			if (threats.Item1.Name != threats.Item2.Name) {
				dictionary.Add("Наименование угрозы", new Tuple<string, string>(threats.Item1.Name, threats.Item2.Name));
			}
			if (threats.Item1.Description != threats.Item2.Description)
			{
				dictionary.Add("Описание угрозы", new Tuple<string, string>(threats.Item1.Description, threats.Item2.Description));
			}
			if (threats.Item1.ThreatSource != threats.Item2.ThreatSource)
			{
				dictionary.Add("Источник угрозы", new Tuple<string, string>(threats.Item1.ThreatSource, threats.Item2.ThreatSource));
			}
			if (threats.Item1.Victim != threats.Item2.Victim)
			{
				dictionary.Add("Объект воздействия", new Tuple<string, string>(threats.Item1.Victim, threats.Item2.Victim));
			}
			if (threats.Item1.Confidentiality != threats.Item2.Confidentiality)
			{
				dictionary.Add("Нарушение конфидециальности", new Tuple<string, string>(threats.Item1.Confidentiality.ToString(), threats.Item2.Confidentiality.ToString()));
			}
			if (threats.Item1.Availability != threats.Item2.Availability)
			{
				dictionary.Add("Нарушение доступности", new Tuple<string, string>(threats.Item1.Availability.ToString(), threats.Item2.Availability.ToString()));
			}
			if (threats.Item1.Integrity != threats.Item2.Integrity)
			{
				dictionary.Add("Нарушение целостности", new Tuple<string, string>(threats.Item1.Integrity.ToString(), threats.Item2.Integrity.ToString()));
			}
			return dictionary;
		}

		private UpdateAnswer UpdateThreats() {
			try
			{
				var savedThreats = DBThreatsService.GetAllThreats();
				var newThreats = GetThreatsFromFile(DownloadFile(FILE_LINK));
				DBThreatsService.ClearSavedThreats();
				DBThreatsService.SaveThreats(newThreats);
				var compareResult = CompareThreatLists(savedThreats, newThreats);
				return new UpdateAnswer(compareResult);
			} catch(Exception ex) {
				return new UpdateAnswer(ex.Message);
			}
		}

		private List<Tuple<Threat, Threat>> CompareThreatLists(List<Threat> savedThreats, List<Threat> newThreats)
		{
			int currentId, newCurrentId;
			var compareResult = new List<Tuple<Threat, Threat>>();
			int currentThreat = 0;
			int newCurrentThreat = 0;

			while(currentThreat < savedThreats.Count && newCurrentThreat < newThreats.Count) {

				currentId = savedThreats[currentThreat].Id;
				newCurrentId = newThreats[newCurrentThreat].Id;

				if (savedThreats[currentThreat].Equals(newThreats[newCurrentThreat])) {
					currentThreat++;
					newCurrentThreat++;
					continue;
				}
				
				if(currentId == newCurrentId && !savedThreats[currentThreat].Equals(newThreats[newCurrentThreat])) {
					compareResult.Add(new Tuple<Threat, Threat>(savedThreats[currentThreat], newThreats[newCurrentThreat]));
					currentThreat++;
					newCurrentThreat++;
					continue;
				}

				if(currentId < newCurrentId) {
					compareResult.Add(new Tuple<Threat, Threat>(savedThreats[currentThreat], null));
					currentThreat++;
					continue;
				}

				if (currentId > newCurrentId)
				{
					compareResult.Add(new Tuple<Threat, Threat>(null, newThreats[newCurrentThreat]));
					newCurrentThreat++;
					continue;
				}
			}

			if (newThreats.Count - newCurrentThreat > 0) {
				for (int i = newCurrentThreat; i < newThreats.Count; i++)
				{
					compareResult.Add(new Tuple<Threat, Threat>(null, newThreats[i]));
				}
			}
			return compareResult;
		}
	}
}
