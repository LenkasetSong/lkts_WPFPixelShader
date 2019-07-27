using System;
using System.Runtime.InteropServices;

[Guid("8BA5FB08-5195-40e2-AC58-0D989C3A0102")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface ID3DXBuffer
{
	[PreserveSig]
	IntPtr GetBufferPointer();

	[PreserveSig]
	int GetBufferSize();
}
