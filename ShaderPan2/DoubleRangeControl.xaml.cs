using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShaderPan2
{
	/// <summary>
	/// DoubleRangeControl.xaml 的交互逻辑
	/// </summary>
	public partial class DoubleRangeControl : UserControl
	{
		public DoubleRangeControl()
		{
			InitializeComponent();
		}



		public string ValueName
		{
			get { return (string)GetValue(ValueNameProperty); }
			set { SetValue(ValueNameProperty, value); }
		}

		// Using a DependencyProperty as the backing store for ValueName.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ValueNameProperty =
			DependencyProperty.Register("ValueName", typeof(string), typeof(DoubleRangeControl), new PropertyMetadata(""));



		public double CurValue
		{
			get { return (double)GetValue(CurValueProperty); }
			set { SetValue(CurValueProperty, value); }
		}

		// Using a DependencyProperty as the backing store for CurValue.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty CurValueProperty =
			DependencyProperty.Register("CurValue", typeof(double), typeof(DoubleRangeControl), new PropertyMetadata(0.0));




	}
}
