using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Face;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace cogs
{
    public class App : Application
    {
        public App()
        {
            var btnCap = new Button();
            btnCap.Text = "Capturar Foto";
            // The root page of your application
            MainPage = new ContentPage
            {
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Children = {
                        new Label {
                            XAlign = TextAlignment.Center,
                            Text = "Welcome to Cognitive Services!"
                        },
                        btnCap
                    }
                }
            };

            btnCap.Clicked += BtnCap_Clicked;
        }

        private async void BtnCap_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                //DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "test.jpg"
            });

            if (file == null)
                return;

            var faceClient = new FaceServiceClient("yourkey");
            var Faces = await faceClient.DetectAsync(file.GetStream(), false, true, new FaceAttributeType[] { FaceAttributeType.Gender, FaceAttributeType.Age, FaceAttributeType.Smile, FaceAttributeType.Glasses });

            System.Diagnostics.Debug.WriteLine(Faces.Length + " rosto(s)");

            foreach (var item in Faces)
            {
                System.Diagnostics.Debug.WriteLine("Face Id: " + item.FaceId);
                System.Diagnostics.Debug.WriteLine("Gender: " + item.FaceAttributes.Gender);
                System.Diagnostics.Debug.WriteLine("Age: " + item.FaceAttributes.Age);
                System.Diagnostics.Debug.WriteLine("And your Smile? " + item.FaceAttributes.Smile);
                System.Diagnostics.Debug.WriteLine("With Glass? " + item.FaceAttributes.Glasses);
            }
            try
            {

                var emotionClient = new EmotionServiceClient("yourkey");
                var emotions = await emotionClient.RecognizeAsync(file.GetStream());

                System.Diagnostics.Debug.WriteLine(emotions);

                foreach (var item2 in emotions)
                {
                    System.Diagnostics.Debug.WriteLine(item2.Scores);
                }

            }
            catch (Exception ex)
            {

                throw;
            }

        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
