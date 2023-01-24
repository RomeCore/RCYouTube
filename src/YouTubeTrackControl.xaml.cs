using RCAudioPlayer.Core.Players;
using RCAudioPlayer.WPF;
using RCAudioPlayer.WPF.Playables;

namespace RCYouTube
{
	[PlayableControl(typeof(YouTubeTrack))]
	public partial class YouTubeTrackControl
	{
		public YouTubeTrack Track { get; }
		public Player Player { get; }

		public YouTubeTrackControl(YouTubeTrack track, Player player) : base(track)
		{
			InitializeComponent();
			Track = track;
			Player = player;

			authorText.Text = Track.Author;
			titleText.Text = Track.Title;

			if (!string.IsNullOrEmpty(Track.ThumbnailUrl))
				thumbnail.Source = Utils.GetBitmapFromUri(Track.ThumbnailUrl);
			else
				thumbnail.Visibility = System.Windows.Visibility.Collapsed;
		}
	}
}