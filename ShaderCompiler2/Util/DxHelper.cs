using System;
using System.Runtime.InteropServices;

public class DxHelper
{
	[DllImport("D3DX9_40.dll", CharSet = CharSet.Auto)]
	public static extern int D3DXCompileShader([MarshalAs(UnmanagedType.LPStr)] string pSrcData, int dataLen, IntPtr pDefines, IntPtr includes, [MarshalAs(UnmanagedType.LPStr)] string pFunctionName, [MarshalAs(UnmanagedType.LPStr)] string pTarget, int flags, out ID3DXBuffer ppShader, out ID3DXBuffer ppErrorMsgs, out IntPtr ppConstantTable);

	[DllImport("D3DX9_40_64bit.dll", CharSet = CharSet.Auto, EntryPoint = "D3DXCompileShader")]
	public static extern int D3DXCompileShader64Bit([MarshalAs(UnmanagedType.LPStr)] string pSrcData, int dataLen, IntPtr pDefines, IntPtr includes, [MarshalAs(UnmanagedType.LPStr)] string pFunctionName, [MarshalAs(UnmanagedType.LPStr)] string pTarget, int flags, out ID3DXBuffer ppShader, out ID3DXBuffer ppErrorMsgs, out IntPtr ppConstantTable);

	[DllImport("d3dx10_43.dll", CharSet = CharSet.Auto)]
	public static extern int D3DX10CompileFromMemory([MarshalAs(UnmanagedType.LPStr)] string pSrcData, int dataLen, [MarshalAs(UnmanagedType.LPStr)] string pFilename, IntPtr pDefines, IntPtr pInclude, [MarshalAs(UnmanagedType.LPStr)] string pFunctionName, [MarshalAs(UnmanagedType.LPStr)] string pProfile, int flags1, int flags2, IntPtr pPump, out ID3DXBuffer ppShader, out ID3DXBuffer ppErrorMsgs, ref int pHresult);
}
