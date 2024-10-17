using UnityEngine;

namespace ScriptableVariables.SO
{
	[CreateAssetMenu(fileName = "LocalVector3", menuName = "Scriptables/Local/LocalVector3", order = 0)]
	public class LocalVector3So : Vector3VariableSo
	{
		public override bool CreateReference() => true;
	}
}