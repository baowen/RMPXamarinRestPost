using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Json;
using System.Text;
using System.Threading.Tasks;

namespace RestJSONRMP
{
	[Activity (Label = "Rest JSON RMP", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{

		String username = "ben.a.owen@gmail.com";
		String password = "k1lkenny";
		String restUrl = "https://live.runmyprocess.com/live/118261460635893523/host/141905/service/225679?P_mode=TEST";

	//	String username = "";
	//	String password = "";
	//	String restUrl = "http://";

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.Main);

			EditText name = FindViewById<EditText> (Resource.Id.nameText);
			TextView email = FindViewById<TextView> (Resource.Id.emailValue);

			Button button = FindViewById<Button> (Resource.Id.myButton);

			button.Click += async (sender, e) =>  {
				email.Text = await FetchEmailAsync(name.Text);
			};
		}

		private async Task<String> FetchEmailAsync (string name)
		{

			var authData = string.Format("{0}:{1}", username, password);
			var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));

			HttpClient client = new HttpClient();
			client.MaxResponseContentBufferSize = 256000;
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);

			var uri = new Uri(string.Format(restUrl));

			StringContent stringContent = new StringContent (
				"{ \"name\":\""+name+"\"   }",
				UnicodeEncoding.UTF8,
				"application/json");

			var content = "";

			try
			{
				var response = await client.PostAsync(uri, stringContent);
				if (response.IsSuccessStatusCode)
				{
					var stringValue = await response.Content.ReadAsStringAsync();
					var data = JsonObject.Parse(stringValue);
					content = data["email"];

				}
			}
			catch (Exception ex)
			{
				Console.Out.WriteLine ("ERROR {0}", ex.Message);
			}

			return content;

		}
	}


		
}


