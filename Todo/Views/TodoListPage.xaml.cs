using System;
using Xamarin.Forms;

namespace Todo
{
	public partial class TodoListPage : ContentPage
	{
		public TodoListPage()
		{
			InitializeComponent();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
		}

        async void OnItemAdded(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TodoItemPage
            {
                TodoItem = new TodoItem()
            });
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await Navigation.PushAsync(new TodoItemPage
            {
                TodoItem = e.SelectedItem as TodoItem
            });
        }

        async void OnAboutClick(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new AboutPage());
		}
	}
}
