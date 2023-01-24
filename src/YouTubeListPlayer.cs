using System.Collections.Generic;
using RCAudioPlayer.Core;
using RCAudioPlayer.Core.Players;
using YoutubeExplode.Videos;
using YoutubeExplode.Playlists;
using System.Threading.Tasks;
using RCAudioPlayer.Core.Playables;

namespace RCYouTube
{
	[Player("youtube_list")]
	public class YouTubeListPlayer : ListPlayer<YouTubeTrack>
	{
		public YouTubeListPlayer(PlayerMaster master, string name) : base(master, name)
		{
		}

		public override async Task<IPlayable> Create(string str)
		{
			return await YouTubeTrack.Create(str);
		}

		public async Task Add(IVideo video)
		{
			await Task.Run(() => Add(new YouTubeTrack(video)));
		}
		
		public async Task Add(IAsyncEnumerable<IVideo> videos)
		{
			await foreach (IVideo video in videos)
				await Add(video);
		}
		
		public async Task Add(IEnumerable<IVideo> videos)
		{
			foreach (IVideo video in videos)
				await Add(video);
		}

		public async Task AddPlaylist(string urlOrId)
		{
			var id = PlaylistId.Parse(urlOrId);
			var videos = Client.Instance.Playlists.GetVideosAsync(id);
			await Add(videos);
		}
	}
}