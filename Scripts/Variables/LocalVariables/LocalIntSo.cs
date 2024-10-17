using UnityEngine;

namespace ScriptableVariables.SO
{
	[CreateAssetMenu(fileName = "LocalInt", menuName = "Scriptables/Local/LocalInt", order = 0)]
	public class LocalIntSo : IntVariableSo
	{
		public override bool CreateReference() => true;
	}
}