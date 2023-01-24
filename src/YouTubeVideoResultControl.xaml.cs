using RCAudioPlayer.Core;
using YoutubeExplode.Videos;

namespace RCYouTube
{
	public partial class YouTubeVideoResultControl
	{
		public IVideo Video { get; }

		public YouTubeVideoResultControl(IVideo video)
		{
			InitializeComponent();
			Video = video;

			if (video.Duration != null)
				authorText.Text = $"({video.Duration.ToStr()}) " + video.Author.ChannelTitle;
			else
				authorText.Text = $"[Live] " + video.Author.ChannelTitle;
			titleText.Text = video.Title;

			thumbnail.Source = video.Thumbnails.GetRecommendBitmap();
		}
	}
}