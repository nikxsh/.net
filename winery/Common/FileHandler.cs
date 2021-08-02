using Newtonsoft.Json;
using System.IO;

namespace Common
{
	public class FileHandler
	{
		public T ReadJsonData<T>(string path)
		{
			using var streamReader = new StreamReader(path);
			string jsonData = streamReader.ReadToEnd();
			return JsonConvert.DeserializeObject<T>(jsonData);
		}

		public void WriteJsonData<T>(string path, T Content)
		{
			var fileContent = JsonConvert.SerializeObject(Content);
			WriteToFile(path, fileContent);
		}

		public void WriteToFile(string path, string content)
		{
			File.WriteAllText(path, content);
		}
	}
}