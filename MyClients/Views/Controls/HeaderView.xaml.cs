namespace MyClients.Views.Controls;

public partial class HeaderView : ContentView
{
	public static readonly BindableProperty HeaderTextProperty =
		BindableProperty.Create(
			nameof(HeaderText),
			typeof(string),
			typeof(HeaderView),
			defaultValue: string.Empty,
			propertyChanged: OnHeaderTextChanged);

	public string HeaderText
	{
		get => (string)GetValue(HeaderTextProperty);
		set => SetValue(HeaderTextProperty, value);
	}

	private static void OnHeaderTextChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var control = (HeaderView)bindable;
		control.CenterLabel.Text = (string)newValue;
	}

	public HeaderView()
	{
		InitializeComponent();
	}
}