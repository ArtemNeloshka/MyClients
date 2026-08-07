namespace MyClients.Views.Controls;

public partial class RegistrationTextFieldView : ContentView
{
	public RegistrationTextFieldView()
	{
		InitializeComponent();
	}

	public static readonly BindableProperty TextProperty =
		BindableProperty.Create(nameof(Text),
			typeof(string),
			typeof(RegistrationTextFieldView),
			string.Empty,
			BindingMode.TwoWay);

	public string Text
	{
		get => (string)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
	}

	public static readonly BindableProperty PlaceholderProperty =
		BindableProperty.Create(nameof(Placeholder),
			typeof(string),
			typeof(RegistrationTextFieldView),
			string.Empty);

	public string Placeholder
	{
		get => (string)GetValue(PlaceholderProperty);
		set => SetValue(PlaceholderProperty, value);
	}

	public static readonly BindableProperty PlaceholderColorProperty =
		BindableProperty.Create(nameof(PlaceholderColor),
			typeof(Color),
			typeof(RegistrationTextFieldView),
			Colors.Gray);

	public Color PlaceholderColor
	{
		get => (Color)GetValue(PlaceholderColorProperty);
		set => SetValue(PlaceholderColorProperty, value);
	}

	public static readonly BindableProperty IsPasswordProperty =
		BindableProperty.Create(nameof(IsPassword),
			typeof(bool),
			typeof(RegistrationTextFieldView),
			false);

	public bool IsPassword
	{
		get => (bool)GetValue(IsPasswordProperty);
		set => SetValue(IsPasswordProperty, value);
	}

	public static readonly BindableProperty KeyboardProperty =
		BindableProperty.Create(nameof(Keyboard),
			typeof(Keyboard),
			typeof(RegistrationTextFieldView),
			Keyboard.Default);

	public Keyboard Keyboard
	{
		get => (Keyboard)GetValue(KeyboardProperty);
		set => SetValue(KeyboardProperty, value);
	}
}
