using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Windows;
using System.Windows.Media.Effects;

namespace CodeGenerator
{
	class Program
	{
		static void Main(string[] args)//fx_ps_DirectoryPath ShaderName [GeneratedNamespace]
		{
			if (args.Length < 3) return;

			string fxPath = args[0] + "\\" + args[1] + ".fx";//@"C:\Users\wenyunchun\Desktop\WpfTPL\shader\ToonShader.fx";
			string psPath = args[0] + "\\" + args[1] + ".ps";
			string GeneratedNamespace = args[2];

			using (FileStream fs = new FileStream(fxPath, FileMode.Open, FileAccess.Read))
			{
				using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
				{
					//string psPath = @"C:\Users\wenyunchun\Desktop\WpfTPL\shader\ToonShader.ps";
					CodeParser.GeneratedNamespace = !string.IsNullOrWhiteSpace(GeneratedNamespace) ? GeneratedNamespace : "Shaders"; // || 动态命名空间 || //
					ShaderModel _shaderModel = CodeParser.ParseShader(psPath, sr.ReadToEnd());

					CreatePixelShaderClass.shaderPath = psPath;
					string _csText = CreatePixelShaderClass.GetSourceText(CodeDomProvider.CreateProvider("CSharp"), _shaderModel, false);
					//string _vbText = CreatePixelShaderClass.GetSourceText(CodeDomProvider.CreateProvider("VisualBasic"), _shaderModel, false);

					string topath = args[0] + "\\" + args[1] + "Effect.cs";
					using (FileStream fs2 = new FileStream(topath, FileMode.OpenOrCreate, FileAccess.Write))
					{
						using (StreamWriter sw = new StreamWriter(fs2, Encoding.UTF8))
						{
							sw.Write(_csText);
						}
					}

					// || 添加动态资源文件 || //

					var ps = new PixelShader { UriSource = new Uri(psPath) };
					Assembly autoAssembly = CreatePixelShaderClass.CompileInMemory(_csText);
					if (autoAssembly == null)
					{
						MessageBox.Show("Cannot compile the generated C# code.", "Compile error", MessageBoxButton.OK, MessageBoxImage.Error);
						//return;
					}
					else
					{
						Type type = autoAssembly.GetType(String.Format("{0}.{1}", _shaderModel.GeneratedNamespace, _shaderModel.GeneratedClassName));
						//ShaderEffect se = (ShaderEffect)Activator.CreateInstance(type, new object[] { ps });
						ShaderEffect se = (ShaderEffect)Activator.CreateInstance(type);
						Console.WriteLine(se);
					}
				}
			}
		}
	}
}
