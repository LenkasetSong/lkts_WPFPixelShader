using System;
using System.IO;
using System.Text;

namespace ShaderCompiler
{
	class Program
	{
		static void Main(string[] args)
		{
			string path = "";
			if (args.Length > 0)
			{
				path = args[0];//@"C:\Users\wenyunchun\Desktop\WpfTPL\shader\ToonShader.fx";
			}
			while (string.IsNullOrWhiteSpace(path))
			{
				Console.WriteLine("no .fx path selected,please input a .fx path~");
				Console.Write("input:");
				path = Console.ReadLine();
			}
			try
			{
				using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
				{
					using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
					{
						Compiler cpl = new Compiler();
						FileInfo fi = new FileInfo(path);
						string dpath = fi.DirectoryName + "\\";
						string fname = fi.Name.Split('.')[0] + ".ps";
						cpl.Compile(sr.ReadToEnd(), dpath, fname);

						Console.WriteLine("output:" + dpath + fname);
					}
				}
			}
			catch (Exception exp)
			{
				Console.WriteLine(exp.Message);
			}

			Console.ReadLine();
		}
	}
}
