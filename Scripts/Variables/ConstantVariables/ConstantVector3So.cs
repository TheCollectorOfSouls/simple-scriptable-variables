using UnityEngine;

namespace ScriptableVariables
{
	[CreateAssetMenu(fileName = "ConstantVector3", menuName = "Scriptables/Constant/ConstantVector3", order = 0)]
	public class ConstantVector3So :  Vector3VariableSo
	{
		public override bool ConstantValue() => true;
	}
}