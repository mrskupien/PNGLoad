using System.IO;
using UnityEngine;
using System.Threading.Tasks;

public static class Utility
{
	public static class Texture
    {
		public async static Task<Texture2D> LoadImageAsync(string filePath)
		{
			Texture2D tex = new Texture2D(1, 1, TextureFormat.ARGB32, false);
			try
			{
				if(File.Exists(filePath))
				{
					byte[] fileData;
					using(FileStream SourceStream = File.Open(filePath, FileMode.Open))
					{
						fileData = new byte[SourceStream.Length];
						await SourceStream.ReadAsync(fileData, 0, (int)SourceStream.Length);
					}
					tex.LoadImage(fileData);
				}
			}
			catch
			{
				Debug.LogWarning($"Can't load image: {filePath}!");
			}
			
			return tex;
		}
	}
	
}
