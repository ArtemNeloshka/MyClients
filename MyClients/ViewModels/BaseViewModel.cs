using CommunityToolkit.Mvvm.ComponentModel;
using MyClients.Domain.Constants;
using MyClients.Views;

namespace MyClients.ViewModels;

public abstract partial class BaseViewModel : ObservableObject
{
	protected async Task GoBackWithAlertAsync(string message, string pagePath = "..", bool isError = false)
	{
		var parameters = new Dictionary<string, object>
		{
			{ Navigation.AlertMessageKey, message },
			{ Navigation.IsErrorKey, isError },
		};

		if (pagePath.Contains(AppRoutes.LoginPage))
		{
			ClearSession();
		}

		await Shell.Current.GoToAsync(pagePath, parameters);
	}

	protected void RedirectToLoginPage(string? message, bool isError = true)
	{
		MainThread.BeginInvokeOnMainThread(() =>
		{
			ClearSession();
			var loginPage = Application.Current.Handler?.MauiContext?.Services.GetService<LoginPage>();

			if (loginPage?.BindingContext is LoginViewModel loginViewModel && !string.IsNullOrEmpty(message))
			{
				loginViewModel.IsErrorAlert = isError;
				loginViewModel.AlertMessage = message;
			}

			Application.Current.MainPage = loginPage;
		});
	}
	
	protected void ClearSession()
	{
		Session.CurrentUserEmail = string.Empty;
		Session.CurrentUserId = null;
	}
}
