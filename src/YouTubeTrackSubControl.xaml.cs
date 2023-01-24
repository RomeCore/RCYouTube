using RCAudioPlayer.WPF;
using RCAudioPlayer.WPF.Playables.Sub;

namespace RCYouTube
{
	[PlayableSubControl(typeof(YouTubeTrack))]
	public partial class YouTubeTrackSubControl
	{
		public YouTubeTrack Track { get; }

		public YouTubeTrackSubControl(YouTubeTrack track) : base(track)
		{
			InitializeComponent();
			Track = track;

			authorText.Text = Track.Author;
			titleText.Text = Track.Title;

			thumbnail.Source = Utils.GetBitmapFromUri(Track.ThumbnailUrl);
		}
	}
}