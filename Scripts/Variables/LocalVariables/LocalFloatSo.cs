using UnityEngine;

namespace ScriptableVariables.SO
{
	[CreateAssetMenu(fileName = "LocalFloat", menuName = "Scriptables/Local/LocalFloat", order = 0)]
	public class LocalFloatSo : FloatVariableSo
	{
		public override bool CreateReference() => true;
	}
}