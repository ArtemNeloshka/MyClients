using CommunityToolkit.Mvvm.ComponentModel;
using MyClients.Domain.Constants;

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

	protected void ClearSession()
	{
		Session.CurrentUserEmail = string.Empty;
		Session.CurrentUserId = null;
	}
}
