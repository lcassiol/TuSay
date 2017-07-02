using System;
using Xamarin.Forms;
using Plugin.TextToSpeech;

namespace Todo
{
	public partial class MostCommomPage : ContentPage
	{
		public MostCommomPage()
		{
			InitializeComponent();
		}

        private void translationLunchClick(object sender, EventArgs e)
        {
            CrossTextToSpeech.Current.Speak("where can I have lunch?");
        }
        private void translationShoppingClick(object sender, EventArgs e)
        {
            CrossTextToSpeech.Current.Speak("Hi, where's the mall?");
        }
        private void translationBeachClick(object sender, EventArgs e)
        {
            CrossTextToSpeech.Current.Speak("Hello, where is the beach ? ");
        }
        private void translationGoodClick(object sender, EventArgs e)
        {
            CrossTextToSpeech.Current.Speak("How are you?");
        }
    }
}
