using YoutubeExplode;

namespace RCYouTube
{
	internal static class Client
	{
		public readonly static YoutubeClient Instance;

		static Client()
		{
			Instance = new YoutubeClient();
		}
	}
}