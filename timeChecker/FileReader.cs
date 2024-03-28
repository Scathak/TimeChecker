using System.IO;

namespace timeChecker
{
	public interface IReader
	{
		public string ReadAll();
	}
	public class FileReader(string fileName) : IReader
	{
		private readonly string _fileName = fileName;

		public string ReadAll()
		{
			try
			{
				using var r = new StreamReader(
									Path.Combine(
										AppDomain.CurrentDomain.BaseDirectory
										, "Texts"
										, _fileName)
									);
				return r.ReadToEnd();
			}
			catch (FileNotFoundException e)
			{
				Console.WriteLine($"The affirmations file was not found: '{e}'");
			}
			catch (DirectoryNotFoundException e)
			{
				Console.WriteLine($"The directory was not found: '{e}'");
			}
			catch (IOException e)
			{
				Console.WriteLine($"The affirmations file could not be opened: '{e}'");
			}
			return "";
		}
	}
}
