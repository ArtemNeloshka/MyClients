using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Shapes;

namespace MyClients.Views;

public partial class TrainPage : ContentPage
{
	public TrainPage()
	{
		InitializeComponent();
	}

	private void OnPauseTrainingClicked(object? sender, TappedEventArgs e)
	{
		throw new NotImplementedException();
	}

	private void OnFinishTrainingClicked(object? sender, TappedEventArgs e)
	{
		throw new NotImplementedException();
	}

	private void OnAddAttemptClicked(object? sender, EventArgs e)
	{
		var border = new Border
		{
			StrokeShape = new RoundRectangle { CornerRadius = 14 },
			BackgroundColor = Color.FromArgb("#80D9D9D9"),
			Padding = 5,
		};
		
		var grid = new Grid
		{
			RowDefinitions =
			{
				new RowDefinition { Height = GridLength.Star },
				new RowDefinition { Height = GridLength.Star }
			},
			ColumnDefinitions =
			{
				new ColumnDefinition { Width = GridLength.Star },
				new ColumnDefinition { Width = GridLength.Star }
			},
			RowSpacing = 5,
			ColumnSpacing = 10,
		};

		var disciplinePickerBorder = new Border
		{
			StrokeShape = new RoundRectangle { CornerRadius = 14 },
			BackgroundColor = Color.FromArgb("#D9D9D9"),
			Padding = 0,
		};

		var disciplinePicker = new Picker
		{
			Title = "Discipline",
			HorizontalTextAlignment = TextAlignment.Center,
			VerticalTextAlignment = TextAlignment.Center,
			FontAttributes = FontAttributes.Bold | FontAttributes.Italic,
			FontSize = 12,
			ItemsSource = new List<string> { "Bouldering", "Top Rope", "Lead", "Speed" },
		};

		var gradePickerBorder = new Border
		{
			StrokeShape = new RoundRectangle { CornerRadius = 14 },
			BackgroundColor = Color.FromArgb("#D9D9D9"),
			Padding = 0,
		};
		
		var gradePicker = new Picker
		{
			Title = "Grade",
			HorizontalTextAlignment = TextAlignment.Center,
			VerticalTextAlignment = TextAlignment.Center,
			FontAttributes = FontAttributes.Bold | FontAttributes.Italic,
			FontSize = 12,
			ItemsSource = new List<string> { "5", "6", "7", "8" },
		};

		var climbResultPickerBorder = new Border
		{
			StrokeShape = new RoundRectangle { CornerRadius = 14 },
			BackgroundColor = Color.FromArgb("#D9D9D9"),
			Padding = 0,
		};
		
		var climbResultPicker = new Picker
		{
			Title = "Result",
			HorizontalTextAlignment = TextAlignment.Center,
			VerticalTextAlignment = TextAlignment.Center,
			FontAttributes = FontAttributes.Bold | FontAttributes.Italic,
			FontSize = 12,
			ItemsSource = new List<string> { "Flash", "Top", "Zone", "Failed" },
		};
		
		Grid.SetRow(disciplinePickerBorder, 0);
		Grid.SetColumn(disciplinePickerBorder, 0);
		Grid.SetColumnSpan(disciplinePickerBorder, 2);
		
		Grid.SetRow(gradePickerBorder, 1);
		Grid.SetColumn(gradePickerBorder, 0);
		
		Grid.SetRow(climbResultPickerBorder, 1);
		Grid.SetColumn(climbResultPickerBorder, 1);

		disciplinePickerBorder.Content = disciplinePicker;
		gradePickerBorder.Content = gradePicker;
		climbResultPickerBorder.Content = climbResultPicker;
		
		grid.Children.Add(disciplinePickerBorder);
		grid.Children.Add(gradePickerBorder);
		grid.Children.Add(climbResultPickerBorder);
		
		border.Content = grid;
		
		TrainingAttemptsContainer.Children.Add(border);
	}
}