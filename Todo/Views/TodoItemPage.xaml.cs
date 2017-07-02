using System;
using System.Diagnostics;
using Plugin.TextToSpeech;
using Xamarin.Forms;
using Plugin.TextToSpeech.Abstractions;

namespace Todo
{
	public partial class TodoItemPage : ContentPage
	{
		IBingSpeechService bingSpeechService;
		ITextTranslationService textTranslationService;
		bool isRecording = false;

		public static readonly BindableProperty TodoItemProperty =
			BindableProperty.Create("TodoItem", typeof(TodoItem), typeof(TodoItemPage), null);

		public TodoItem TodoItem
		{
			get { return (TodoItem)GetValue(TodoItemProperty); }
			set { SetValue(TodoItemProperty, value); }
		}

		public static readonly BindableProperty IsProcessingProperty =
			BindableProperty.Create("IsProcessing", typeof(bool), typeof(TodoItemPage), false);

		public bool IsProcessing
		{
			get { return (bool)GetValue(IsProcessingProperty); }
			set { SetValue(IsProcessingProperty, value); }
		}

		public TodoItemPage()
		{
			InitializeComponent();

			bingSpeechService = new BingSpeechService(new AuthenticationService(Constants.BingSpeechApiKey), Device.OS.ToString());
			textTranslationService = new TextTranslationService(new AuthenticationService(Constants.TextTranslatorApiKey));
		}

		async void OnRecognizeSpeechButtonClicked(object sender, EventArgs e)
		{
			try
			{
				var audioRecordingService = DependencyService.Get<IAudioRecorderService>();
				if (!isRecording)
				{
					audioRecordingService.StartRecording();
                    
					((Button)sender).Image = "recording.png";
                    ((Button)sender).Text = "Terminar Gravação";
                    IsProcessing = true;
				}
				else
				{
					audioRecordingService.StopRecording();
                    ((Button)sender).Image = "sandclock.png";
                    ((Button)sender).Text = "Aguarde...";
                }

				isRecording = !isRecording;
				if (!isRecording)
				{
					var speechResult = await bingSpeechService.RecognizeSpeechAsync(Constants.AudioFilename);
					Debug.WriteLine("Name: " + speechResult.Name);
					Debug.WriteLine("Confidence: " + speechResult.Confidence);

					if (!string.IsNullOrWhiteSpace(speechResult.Name))
					{
                        String response =  char.ToUpper(speechResult.Name[0]) + speechResult.Name.Substring(1);
                        TodoItem.beforeWords = response + "?";
                        String translateResponse = await textTranslationService.TranslateTextAsync(response);
                        CrossLocale brasiLocale = new CrossLocale();
                        

                        foreach (CrossLocale crossLocaleb in CrossTextToSpeech.Current.GetInstalledLanguages()) {
                            if (crossLocaleb.DisplayName.Contains("Portuguese(Brazil)")){
                                brasiLocale = crossLocaleb;
                            }
                        }

                        CrossTextToSpeech.Current.Speak(translateResponse, false, brasiLocale, 0.8f, 0.8f);
                        TodoItem.afterWords = translateResponse + "?";
                        OnPropertyChanged("TodoItem");
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
			finally
			{
				if (!isRecording)
				{
					((Button)sender).Image = "microphone64.png";
                    ((Button)sender).Text = "Gravar";
                    IsProcessing = false;
                    repeatButtom.IsVisible = true;
                }
			}
		}


        async void OnRepeatTextButtonClicked(object sender, EventArgs e)
        {
            IsProcessing = true;
            String translateResponse = await textTranslationService.TranslateTextAsync(TodoItem.beforeWords);
            TodoItem.afterWords = translateResponse;
            OnPropertyChanged("TodoItem");
            CrossTextToSpeech.Current.Speak(translateResponse);
            IsProcessing = false;
        }

            

        async void OnTranslateButtonClicked(object sender, EventArgs e)
		{
			try
			{
				if (!string.IsNullOrWhiteSpace(TodoItem.afterWords))
				{
					IsProcessing = true;

					TodoItem.afterWords = await textTranslationService.TranslateTextAsync(TodoItem.afterWords);
					OnPropertyChanged("TodoItem");

					IsProcessing = false;
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}

		async void OnSaveClicked(object sender, EventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(TodoItem.afterWords))
			{
				await App.TodoManager.SaveItemAsync(TodoItem);
			}
			await Navigation.PopAsync();
		}

		async void OnDeleteClicked(object sender, EventArgs e)
		{
			await App.TodoManager.DeleteItemAsync(TodoItem);
			await Navigation.PopAsync();
		}

		async void OnCancelClicked(object sender, EventArgs e)
		{
			await Navigation.PopAsync();
		}

        private void OnMostCommomSpeechClick(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MostCommomPage());
        }
    }
}
