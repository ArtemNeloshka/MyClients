namespace MyClients.Views.Controls;

public partial class RegistrationTextFieldView : ContentView
{
    // Placeholder
    public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(RegistrationTextFieldView),
            propertyChanged: (b, _, n) => ((RegistrationTextFieldView)b).InnerEntry.Placeholder = (string)n);

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    // IsPassword
    public static readonly BindableProperty IsPasswordProperty =
        BindableProperty.Create(nameof(IsPassword), typeof(bool), typeof(RegistrationTextFieldView), false,
            propertyChanged: (b, _, n) => ((RegistrationTextFieldView)b).InnerEntry.IsPassword = (bool)n);

    public bool IsPassword
    {
        get => (bool)GetValue(IsPasswordProperty);
        set => SetValue(IsPasswordProperty, value);
    }

    // Keyboard
    public static readonly BindableProperty KeyboardProperty =
        BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(RegistrationTextFieldView), Keyboard.Default,
            propertyChanged: (b, _, n) => ((RegistrationTextFieldView)b).InnerEntry.Keyboard = (Keyboard)n);

    public Keyboard Keyboard
    {
        get => (Keyboard)GetValue(KeyboardProperty);
        set => SetValue(KeyboardProperty, value);
    }

    // Text
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(RegistrationTextFieldView), string.Empty,
            BindingMode.TwoWay,
            propertyChanged: (b, _, n) => ((RegistrationTextFieldView)b).InnerEntry.Text = (string)n);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    // PlaceholderColor
    public static readonly BindableProperty PlaceholderColorProperty =
        BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(RegistrationTextFieldView), Colors.DarkGray,
            propertyChanged: (b, _, n) => ((RegistrationTextFieldView)b).InnerEntry.PlaceholderColor = (Color)n);

    public Color PlaceholderColor
    {
        get => (Color)GetValue(PlaceholderColorProperty);
        set => SetValue(PlaceholderColorProperty, value);
    }

    public RegistrationTextFieldView()
    {
        InitializeComponent();
        InnerEntry.TextChanged += (s, e) => Text = e.NewTextValue;
    }
}
