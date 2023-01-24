using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;
using YoutubeExplode.Playlists;
using YoutubeExplode.Videos;

namespace RCYouTube
{
	public partial class YouTubePlaylistResultControl : UserControl
	{
		public class VideoLoadedEventArgs
		{
			public IVideo Video { get; }
			public YouTubeVideoResultControl ResultControl { get; }

			public VideoLoadedEventArgs(IVideo video, YouTubeVideoResultControl resultControl)
			{
				Video = video;
				ResultControl = resultControl;
			}
		}

		public IPlaylist Playlist { get; }
		public List<YouTubeVideoResultControl> LoadedVideos = new List<YouTubeVideoResultControl>();
		public event EventHandler<VideoLoadedEventArgs> VideoLoaded = (s, e) => { };

		public YouTubePlaylistResultControl(IPlaylist playlist)
		{
			InitializeComponent();
			Playlist = playlist;

			if (playlist.Author != null)
				authorText.Text = playlist.Author.ChannelTitle;
			titleText.Text = playlist.Title;

			thumbnail.Source = playlist.Thumbnails.GetRecommendBitmap();
		}

		private bool videosLoaded = false;

		public async Task LoadVideos()
		{
			if (videosLoaded)
				return;
			videosLoaded = true;

			await foreach (var video in Client.Instance.Playlists.GetVideosAsync(Playlist.Id))
			{
				var control = new YouTubeVideoResultControl(video);
				resultsList.Items.Add(control);
				LoadedVideos.Add(control);
				VideoLoaded(this, new VideoLoadedEventArgs(video, control));
			}
		}

		private async void Expander_Expanded(object sender, System.Windows.RoutedEventArgs e)
		{
			await LoadVideos();
		}
	}
}