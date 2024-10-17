using UnityEngine;

namespace ScriptableVariables
{
	[CreateAssetMenu(fileName = "ConstantInt", menuName = "Scriptables/Constant/ConstantInt", order = 0)]
	public class ConstantIntSo : IntVariableSo
	{
		public override bool ConstantValue() => true;
	}
}