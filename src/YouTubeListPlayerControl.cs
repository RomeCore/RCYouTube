using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using RCAudioPlayer.Core.Players;
using RCAudioPlayer.WPF.Players;
using RCAudioPlayer.WPF.Players.Main;

namespace RCYouTube
{
	[PlayerControl(typeof(YouTubeListPlayer))]
	public class YouTubeListPlayerControl : ListPlayerControl
	{
		public YouTubeListPlayerControl(ListPlayer listPlayer) : base(listPlayer)
		{
		}

		protected override async void AddButton_Click(object sender, RoutedEventArgs e)
		{
			var browser = new YouTubeBrowser();
			if (browser.ShowDialog() ?? false)
			{
				var list = new List<Task<YouTubeTrack>>();
				foreach (var video in browser.Videos)
					list.Add(YouTubeTrack.Create(video));
				foreach (var video in list)
					Add(await video);
			}
		}

		protected override void List_Drop(object sender, DragEventArgs e)
		{
		}
	}
}