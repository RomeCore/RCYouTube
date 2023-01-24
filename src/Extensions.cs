using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;
using YoutubeExplode.Common;
using RCAudioPlayer.WPF;

namespace RCYouTube
{
	internal static class Extensions
	{
		public static string GetRecommendUrl(this IEnumerable<Thumbnail> thumbnails)
		{
			return thumbnails.First().Url;
		}

		public static BitmapSource GetRecommendBitmap(this IEnumerable<Thumbnail> thumbnails)
		{
			return Utils.GetBitmapFromUri(thumbnails.First().Url);
		}
	}
}