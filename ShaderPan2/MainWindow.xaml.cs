using Microsoft.Win32;
using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Effects;
using IPath = System.IO.Path;

namespace ShaderPan2
{
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
			ofd.FileName = IPath.GetFileName(pathText.Text);
			if (ofd.ShowDialog() == true)
			{
				pathText.Text = ofd.FileName;

				using (FileStream fs = new FileStream(pathText.Text, FileMode.Open, FileAccess.Read))
				{
					using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
					{
						codeText.Text = sr.ReadToEnd();
						logText.Items.Insert(0, "select:" + pathText.Text);
					}
				}
			}
		}

		private void ApplyBtn_Click(object sender, RoutedEventArgs e)
		{
			string psPath = "";
			ShaderModel _shaderModel = null;
			string _csText = "";

			string path = pathText.Text;
			logText.Items.Insert(0, "input:" + path);
			compile(path);

			string[] args = { IPath.GetDirectoryName(path), IPath.GetFileNameWithoutExtension(path), "ShaderPan" };
			generate(args, ref psPath, ref _shaderModel, ref _csText);

			codeText.Text = _csText;
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
				stk.Children.Clear();
				stk.DataContext = se;
				PropertyInfo[] pInfos = type.GetProperties();
				for (int i = 0; i < pInfos.Length; i++)
				{
					if (pInfos[i].PropertyType == typeof(double))
					{
						DoubleRangeControl sl = new DoubleRangeControl() { Margin = new Thickness(10) };
						sl.ValueName = pInfos[i].Name + " : ";
						Binding b = new Binding(pInfos[i].Name);
						b.Mode = BindingMode.TwoWay;
						b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
						sl.SetBinding(DoubleRangeControl.CurValueProperty, b);
						stk.Children.Add(sl);
					}
				}
				logText.Items.Insert(0, se);
			}
		}
	}
}
