using System;
using System.Windows.Controls;

public partial class ShaderModelConstantRegister
{
	public bool IsDdxUvDdyUv
	{
		get
		{
			return RegisterName == "DdxUvDdyUv";
		}
	}
}
public partial class ShaderModelConstantRegister
{

	public ShaderModelConstantRegister(string registerName, Type registerType, int registerNumber,
		string description, object minValue, object maxValue, object defaultValue)
	{
		this.RegisterName = registerName;
		this.RegisterType = registerType;
		this.RegisterNumber = registerNumber;
		this.Description = description;
		this.MinValue = minValue;
		this.MaxValue = maxValue;
		this.DefaultValue = defaultValue;
	}

	#region Properties
	/// <summary>
	/// The name of this register variable.
	/// </summary>
	public string RegisterName { get; private set; }

	/// <summary>
	///  The .NET type of this register variable.
	/// </summary>
	public Type RegisterType { get; private set; }

	/// <summary>
	/// The register number of this register variable.
	/// </summary>
	public int RegisterNumber { get; private set; }

	/// <summary>
	/// The description of this register variable.
	/// </summary>
	public string Description { get; private set; }

	/// <summary>
	/// The minimum value for this register variable.
	/// </summary>
	public object MinValue { get; private set; }

	/// <summary>
	/// The maximum value for this register variable.
	/// </summary>
	public object MaxValue { get; private set; }

	/// <summary>
	/// The default value of this register variable.
	/// </summary>
	public object DefaultValue { get; private set; }

	/// <summary>
	/// The user interface control associated with this register variable.
	/// </summary>
	public Control AffiliatedControl { get; set; }
	#endregion
}
