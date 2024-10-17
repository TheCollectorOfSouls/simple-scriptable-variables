using UnityEngine;

namespace ScriptableVariables
{
	[CreateAssetMenu(fileName = "ConstantFloat", menuName = "Scriptables/Constant/ConstantFloat", order = 0)]
	public class ConstantFloatSo :  FloatVariableSo
	{
		public override bool ConstantValue() => true;
	}
}