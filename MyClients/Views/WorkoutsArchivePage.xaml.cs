using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Shapes;
using MyClients.DAL.Entities;
using MyClients.ViewModels;

namespace MyClients.Views;

public partial class WorkoutsArchivePage : ContentPage
{
	public WorkoutsArchivePage()
	{
		InitializeComponent();
		Shell.SetNavBarIsVisible(this, false);
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		var viewModel = IPlatformApplication.Current.Services.GetService<WorkoutsArchiveViewModel>();
		BindingContext = viewModel;
		await viewModel.LoadTrainingsAsync();
		
		foreach (var training in viewModel.Trainings)
		{
			var trainingCard = GenerateTrainingCardBorder(training);
			WorkoutsContainerVerticalStackLayout.Children.Add(trainingCard);
		}
	}
	
	private Border GenerateTrainingCardBorder(Training training)
	{
		var border = new Border
		{
			StrokeShape = new RoundRectangle { CornerRadius = 15 },
			StrokeThickness = 0,
			BackgroundColor = Color.FromArgb("#D9D9D9"),
		};

		var backgroundGrid = new Grid();

		var cardBackgroundPicturePath = "workouts_archive_background.png";

		var trainingMainInfoGrid = new Grid
		{
			RowDefinitions =
			{
				new RowDefinition { Height = GridLength.Star },
				new RowDefinition { Height = GridLength.Star },
				new RowDefinition { Height = GridLength.Star },
				new RowDefinition { Height = GridLength.Star },
			},
			ColumnDefinitions = 
			{
				new ColumnDefinition { Width = GridLength.Star },
				new ColumnDefinition { Width = GridLength.Star },
			},
			RowSpacing = 10,
			ColumnSpacing = 10,
			VerticalOptions = LayoutOptions.Fill,
			HorizontalOptions = LayoutOptions.Fill,
			Padding = 10,
		};
		
		var trainingDateLabel = GetTrainingDateLabel(training, 0, 0, 32);
		var trainingTimeLabel = GetTrainingTimeLabel(training, 0, 1, 32);
		var disciplinesTrainedTextLabel = CreateTrainingCardLabel(
			content: "Disciplines trained:",
			fontsize: 20,
			horizontalPositioning: LayoutOptions.Start,
			verticalPositioning: LayoutOptions.Center,
			gridRow: 1,
			gridColumn: 0);
		var bestGradesTextLabel = CreateTrainingCardLabel(
			content: "Best grades\nclimbed:",
			fontsize: 20,
			horizontalPositioning: LayoutOptions.End,
			verticalPositioning: LayoutOptions.Center,
			gridRow: 1,
			gridColumn: 1);
		var disciplinesTrainedListLabel = GetTrainedDisciplinesLabel(training, 2, 0, 20);
		var bestGradesListLabel = GetBestTrainingGradesLabel(training, 2, 1, 20);
		var climbingGymLabel = CreateTrainingCardLabel(
			content: "Climbing Gym",
			fontsize: 20,
			horizontalPositioning: LayoutOptions.Start,
			verticalPositioning: LayoutOptions.End,
			gridRow: 3,
			gridColumn: 0);
		var viewMoreButtonBorder = new Border
		{
			StrokeShape = new RoundRectangle { CornerRadius = 15 },
			StrokeThickness = 0,
			Padding = 5,
			BackgroundColor = Colors.Brown,
			HorizontalOptions = LayoutOptions.End,
			VerticalOptions = LayoutOptions.End,
		};
		var viewMoreButtonTextLabel = new Label
		{
			Text = "View more",
			TextColor = Colors.Black,
			FontSize = 20,
			FontAttributes = FontAttributes.Italic | FontAttributes.Bold,
		};


		var tapGesture = new TapGestureRecognizer();
		tapGesture.Tapped += async (s, e) =>
		{
			var trainingDetailsPage = Handler.MauiContext.Services.GetService<TrainingDetailsPage>();
			trainingDetailsPage.TrainingId = training.Id;
			await Navigation.PushAsync(trainingDetailsPage);
		};
		
		viewMoreButtonBorder.Content = viewMoreButtonTextLabel;
		viewMoreButtonBorder.GestureRecognizers.Add(tapGesture);
		
		Grid.SetRow(viewMoreButtonBorder, 3);
		Grid.SetColumn(viewMoreButtonBorder, 1);

		trainingMainInfoGrid.Children.Add(trainingDateLabel);
		trainingMainInfoGrid.Children.Add(trainingTimeLabel);
		trainingMainInfoGrid.Children.Add(disciplinesTrainedTextLabel);
		trainingMainInfoGrid.Children.Add(bestGradesTextLabel);
		trainingMainInfoGrid.Children.Add(disciplinesTrainedListLabel);
		trainingMainInfoGrid.Children.Add(bestGradesListLabel);
		trainingMainInfoGrid.Children.Add(climbingGymLabel);
		trainingMainInfoGrid.Children.Add(viewMoreButtonBorder);

		backgroundGrid.Children.Add(new Image
		{
			Source = cardBackgroundPicturePath,
			Aspect = Aspect.AspectFit,
			Opacity = 0.5,
		});
		backgroundGrid.Children.Add(trainingMainInfoGrid);

		border.Content = backgroundGrid;

		return border;
	}
	
	private Label GetTrainingDateLabel(Training training, int gridRow, int gridColumn, int fontsize)
	{
		var rawDate = training.TrainingDate;
		var dateString = rawDate.ToString("dd.MM.yy");

		return CreateTrainingCardLabel(content: dateString,
			fontsize: fontsize,
			horizontalPositioning: LayoutOptions.Start,
			verticalPositioning: LayoutOptions.Start,
			gridRow: gridRow,
			gridColumn: gridColumn);
	}
	
	private Label GetTrainingTimeLabel(Training training, int gridRow, int gridColumn, int fontsize)
	{
		return CreateTrainingCardLabel(content: "Test:time",
			fontsize: fontsize,
			horizontalPositioning: LayoutOptions.End,
			verticalPositioning: LayoutOptions.Start,
			gridRow: gridRow,
			gridColumn: gridColumn);
	}

	private Label GetTrainedDisciplinesLabel(Training training, int gridRow, int gridColumn, int fontsize)
	{
		return CreateTrainingCardLabel(content: "Test\nDiscipline",
			fontsize: fontsize,
			horizontalPositioning: LayoutOptions.Start,
			verticalPositioning: LayoutOptions.Start,
			gridRow: gridRow,
			gridColumn: gridColumn);
	}
	
	private Label GetBestTrainingGradesLabel(Training training, int gridRow, int gridColumn, int fontsize)
	{
		return CreateTrainingCardLabel(content: "Test\nGrades",
			fontsize: fontsize,
			horizontalPositioning: LayoutOptions.End,
			verticalPositioning: LayoutOptions.Start,
			gridRow: gridRow,
			gridColumn: gridColumn);
	}

	private Label CreateTrainingCardLabel(string content, int fontsize, LayoutOptions horizontalPositioning,
		LayoutOptions verticalPositioning, int gridRow, int gridColumn)
	{
		var label = new Label
		{
			Text = content,
			
			FontAttributes = FontAttributes.Bold | FontAttributes.Italic,
			FontSize = fontsize,
			HorizontalOptions = horizontalPositioning,
			VerticalOptions = verticalPositioning
		};
		
		Grid.SetRow(label, gridRow);
		Grid.SetColumn(label, gridColumn);

		return label;
	}
}