using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using YoutubeExplode.Playlists;
using YoutubeExplode.Search;
using YoutubeExplode.Videos;

namespace RCYouTube
{
	public partial class YouTubeBrowser
	{
		private Dictionary<YouTubeVideoResultControl, IVideo> videos = new Dictionary<YouTubeVideoResultControl, IVideo>();
		private Dictionary<YouTubePlaylistResultControl, IPlaylist> playlists = new Dictionary<YouTubePlaylistResultControl, IPlaylist>();

		public IEnumerable<IVideo> Videos => videos.Values;

		public YouTubeBrowser()
		{
			InitializeComponent();
		}

		private async void SearchButton_Click(object sender, RoutedEventArgs e)
		{
			var input = queryTextBox.Text;

			searchResultsPanel.Visibility = Visibility.Visible;
			searchNoResultsLabel.Visibility = Visibility.Hidden;

			searchResultsList.Items.Clear();

			var videoId = VideoId.TryParse(input);
			if (videoId != null)
			{
				await AddVideo(videoId.Value);
				return;
			}

			var playlistId = PlaylistId.TryParse(input);
			if (playlistId != null)
			{
				await AddPlaylist(playlistId.Value);
				return;
			}

			var results = Client.Instance.Search.GetResultsAsync(input);
			int resultsNum = 0;
			await foreach(var result in results)
			{
				resultsNum++;
				if (result is VideoSearchResult videoResult)
					AddVideo(videoResult);
				else if (result is PlaylistSearchResult playlistResult)
					AddPlaylist(playlistResult);

				if (resultsNum > 200)
					break;
			}

			if (resultsNum == 0)
			{
				searchResultsPanel.Visibility = Visibility.Hidden;
				searchNoResultsLabel.Visibility = Visibility.Visible;
			}
		}

		private void OkButton_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = videos.Count != 0;
			Close();
		}
		
		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
			Close();
		}

		private async Task AddVideo(VideoId id)
		{
			var video = await Client.Instance.Videos.GetAsync(id.Value);
			AddVideo(video);
		}
		
		private void AddVideo(IVideo video)
		{
			var control = new YouTubeVideoResultControl(video);
			control.checkBox.IsChecked = videos.ContainsKey(control);
			control.checkBox.Checked += (s, e) => videos.Add(control, video);
			control.checkBox.Unchecked += (s, e) => videos.Remove(control);
			searchResultsList.Items.Add(control);
		}

		private async Task AddPlaylist(PlaylistId id)
		{
			var playlist = await Client.Instance.Playlists.GetAsync(id.Value);
			AddPlaylist(playlist);
		}

		private void AddPlaylist(IPlaylist playlist)
		{
			var control = new YouTubePlaylistResultControl(playlist);
			control.checkBox.IsChecked = playlists.ContainsKey(control);
			control.checkBox.Checked += async (s, e) =>
			{
				playlists.Add(control, playlist);
				await control.LoadVideos();
				foreach (var video in control.LoadedVideos)
					video.checkBox.IsChecked = true;
			};
			control.checkBox.Unchecked += (s, e) =>
			{
				playlists.Remove(control);
				foreach (var video in control.LoadedVideos)
					video.checkBox.IsChecked = false;
			};
			control.VideoLoaded += (s, e) =>
			{
				var video = e.Video;
				var control = e.ResultControl;
				control.checkBox.IsChecked = videos.ContainsKey(control);
				control.checkBox.Checked += (s, e) => videos.Add(control, video);
				control.checkBox.Unchecked += (s, e) => videos.Remove(control);
			};

			searchResultsList.Items.Add(control);
		}
	}
}