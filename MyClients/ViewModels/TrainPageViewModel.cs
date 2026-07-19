using System.ComponentModel;
using System.Xml;

namespace MyClients.ViewModels;

public class TrainPageViewModel : INotifyPropertyChanged, IDisposable
{
	public event PropertyChangedEventHandler? PropertyChanged;
	
	private string _trainingDateString = string.Empty;

	public string TrainingDateString
	{
		get => _trainingDateString;

		set
		{
			_trainingDateString = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TrainingDateString)));
		}
	}
	
	private string _timerString = String.Empty;

	public string TimerString
	{
		get => _timerString;
		set
		{
			_timerString = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TimerString)));
		}
	}

	private IDispatcherTimer? _timer;
	private TimeSpan _elapsed = TimeSpan.Zero;
	
	public void LoadTrainingInfoAsync()
	{
		var trainingDate = DateOnly.FromDateTime(DateTime.Today);

		TrainingDateString = trainingDate.ToString("dd.MM.yy");
		StartTimer();
	}
	
	private void StartTimer()
    {
     	_timer = Application.Current?.Dispatcher.CreateTimer();
     	_timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += (s, e) =>
        {
	        _elapsed = _elapsed.Add(_timer.Interval);
	        TimerString = _elapsed.ToString(@"hh\:mm\:ss");
        };
        
        _timer.Start();
    }

	public void StopTimer()
	{
		_timer?.Stop();
	}

	public void ResumeTimer()
	{
		_timer?.Start();
	}

	public void Dispose()
	{
		_timer?.Stop();
	}
}