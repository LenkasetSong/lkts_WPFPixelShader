using Microsoft.Win32;
using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Media.Effects;

namespace ShaderPan
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

		}

		private void ChoosBtn_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "HLSL|*.fx|All|*.*";
			if (ofd.ShowDialog() == true)
			{
				pathText.Text = ofd.FileName;
			}
		}

		private void ApplyBtn_Click(object sender, RoutedEventArgs e)
		{
			string psPath = "";
			ShaderModel _shaderModel = null;
			string _csText = "";

			string path = pathText.Text;
			compile(path);

			string[] args = { Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path), "ShaderPan" };
			generate(args, ref psPath, ref _shaderModel, ref _csText);

			apply(psPath, _shaderModel, _csText);
		}

		void compile(string path)
		{
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

						logText.Items.Insert(0, "output:" + dpath + fname);
					}
				}
			}
			catch (Exception exp)
			{
				logText.Items.Insert(0, exp.Message);
			}
		}

		void generate(string[] args, ref string psPath, ref ShaderModel _shaderModel, ref string _csText)
		{
			string fxPath = args[0] + "\\" + args[1] + ".fx";//@"C:\Users\wenyunchun\Desktop\WpfTPL\shader\ToonShader.fx";
			psPath = args[0] + "\\" + args[1] + ".ps";
			string GeneratedNamespace = args[2];

			using (FileStream fs = new FileStream(fxPath, FileMode.Open, FileAccess.Read))
			{
				using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
				{
					CodeParser.GeneratedNamespace = !string.IsNullOrWhiteSpace(GeneratedNamespace) ? GeneratedNamespace : "Shaders"; // || 动态命名空间 || //
					_shaderModel = CodeParser.ParseShader(psPath, sr.ReadToEnd());

					CreatePixelShaderClass.shaderPath = psPath;
					_csText = CreatePixelShaderClass.GetSourceText(CodeDomProvider.CreateProvider("CSharp"), _shaderModel, false);

					string topath = args[0] + "\\" + args[1] + "Effect.cs";
					using (FileStream fs2 = new FileStream(topath, FileMode.OpenOrCreate, FileAccess.Write))
					{
						using (StreamWriter sw = new StreamWriter(fs2, Encoding.UTF8))
						{
							sw.Write(_csText);

							logText.Items.Insert(0, topath);
						}
					}
				}
			}
		}

		void apply(string psPath, ShaderModel _shaderModel, string _csText)
		{
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
				eftImg.Effect = se;
				logText.Items.Insert(0, se);
			}
		}
	}
}
