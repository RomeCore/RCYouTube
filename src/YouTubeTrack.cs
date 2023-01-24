using System;
using System.Linq;
using System.Threading.Tasks;
using YoutubeExplode.Videos;
using RCAudioPlayer.Core.Data;
using RCAudioPlayer.Core.Streams;
using RCAudioPlayer.Core.Playables;

namespace RCYouTube
{
	public class YouTubeTrack : IPlayable
	{
		public string Url { get; }
		public string Id { get; }
		public string StreamUrl { get; private set; }

		public string Title { get; }
		public string Author { get; }
		public TimeSpan? Duration { get; }
		public string FullTitle => Author + " - " + Title;
		public string ThumbnailUrl { get; }

		public YouTubeTrack(IVideo video)
		{
			Id = video.Id;
			Url = video.Url;
			Title = video.Title;
			Author = video.Author.ChannelTitle;
			Duration = video.Duration;
			ThumbnailUrl = video.Thumbnails.GetRecommendUrl();
		}

		public async Task UpdateStreamUrl()
		{
			if (Duration != null && Duration.Value != TimeSpan.Zero)
			{
				var manifest = await Client.Instance.Videos.Streams.GetManifestAsync(Url);
				var stream = manifest.GetAudioOnlyStreams().ElementAt(1);
				StreamUrl = stream.Url;
			}
		}

		public static async Task<YouTubeTrack> Create(IVideo video)
		{
			var track = new YouTubeTrack(video);
			await track.UpdateStreamUrl();
			return track;
		}
		
		public static async Task<YouTubeTrack> Create(string str)
		{
			if (string.IsNullOrEmpty(str))
				throw new Exception("Line is null!");
			var video = await Client.Instance.Videos.GetAsync(str);
			return await Create(video);
		}

		public string Save()
		{
			return Id;
		}

		public bool SkipBackward()
		{
			return true;
		}

		public bool SkipForward()
		{
			return true;
		}

		public async Task<AudioInput> GetInput()
		{
			return await Task.Run(() => new MediaFoundationAudioData(StreamUrl).GetInput());
		}
	}
}