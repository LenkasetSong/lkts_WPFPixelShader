using System;
using System.IO;
using System.Runtime.InteropServices;

public class Compiler : NotifyPropertyChanged
{
	private string _errorText;
	public string ErrorText
	{
		get
		{
			return _errorText;
		}
		set
		{
			_errorText = value;
			RaiseNotifyChanged("ErrorText");
		}
	}

	private bool _isCompiled;
	public bool IsCompiled
	{
		get
		{
			return _isCompiled;
		}
		set
		{
			_isCompiled = value;
			RaiseNotifyChanged("IsCompiled");
		}
	}

	public void Compile(string codeText, string output, string fxName, ShaderProfile shaderProfile = ShaderProfile.ps_2_0)
	{
		IsCompiled = false;
		string path = output;
		IntPtr defines = IntPtr.Zero;
		IntPtr includes = IntPtr.Zero;
		IntPtr ppConstantTable = IntPtr.Zero;
		string methodName = "main";
		string targetProfile2 = "ps_2_0";
		targetProfile2 = ((shaderProfile != ShaderProfile.ps_3_0) ? "ps_2_0" : "ps_3_0");
		bool useDx10 = false;
		int hr2 = 0;
		ID3DXBuffer ppShader2;
		ID3DXBuffer ppErrorMsgs2;
		if (!useDx10)
		{
			hr2 = ((IntPtr.Size != 8) ?
				DxHelper.D3DXCompileShader(codeText, codeText.Length, defines, includes, methodName, targetProfile2, 0, out ppShader2, out ppErrorMsgs2, out ppConstantTable)
				:
				DxHelper.D3DXCompileShader64Bit(codeText, codeText.Length, defines, includes, methodName, targetProfile2, 0, out ppShader2, out ppErrorMsgs2, out ppConstantTable));
		}
		else
		{
			int pHr = 0;
			hr2 = DxHelper.D3DX10CompileFromMemory(codeText, codeText.Length, string.Empty, IntPtr.Zero, IntPtr.Zero, methodName, targetProfile2, 0, 0, IntPtr.Zero, out ppShader2, out ppErrorMsgs2, ref pHr);
		}
		if (hr2 != 0)
		{
			IntPtr errors = ppErrorMsgs2.GetBufferPointer();
			ppErrorMsgs2.GetBufferSize();
			ErrorText = Marshal.PtrToStringAnsi(errors);
			IsCompiled = false;
		}
		else
		{
			ErrorText = "";
			IsCompiled = true;
			string psPath = path + fxName;
			IntPtr pCompiledPs = ppShader2.GetBufferPointer();
			int compiledPsSize = ppShader2.GetBufferSize();
			byte[] compiledPs = new byte[compiledPsSize];
			Marshal.Copy(pCompiledPs, compiledPs, 0, compiledPs.Length);
			using (FileStream psFile = File.Open(psPath, FileMode.Create, FileAccess.Write))
			{
				psFile.Write(compiledPs, 0, compiledPs.Length);
			}
		}
		if (ppShader2 != null)
		{
			Marshal.ReleaseComObject(ppShader2);
		}
		ppShader2 = null;
		if (ppErrorMsgs2 != null)
		{
			Marshal.ReleaseComObject(ppErrorMsgs2);
		}
		ppErrorMsgs2 = null;
		CompileFinished();
	}

	private void CompileFinished()
	{
	}

	public void Reset()
	{
		ErrorText = "not compiled";
	}
}
