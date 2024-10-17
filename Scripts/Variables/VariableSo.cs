using System;
using UnityEngine;

namespace ScriptableVariables
{
	public abstract class BaseVariable<T> : ScriptableObject
	{
		[SerializeField] protected T value;
		[NonSerialized] protected T CurrentValue;
		[NonSerialized] protected bool Initialized = false;
		public event Action<T> OnValueUpdated;
		public virtual bool CreateReference() => false;
		public virtual bool ConstantValue() => false;

		public virtual void RaiseValueUpdated()
		{
			OnValueUpdated?.Invoke(CurrentValue);
		}
		
		private void OnDisable()
		{
			OnValueUpdated = null;
		}
	}
	
	public class VariableSo<T> : BaseVariable<T>
	{
		public virtual T Value
		{
			get
			{
				if (Initialized) return CurrentValue;
				
				CurrentValue = value;
				Initialized = true;
				return CurrentValue;
			}
			set
			{
				if(ConstantValue()) return;
				if (!Initialized)
				{
					CurrentValue = this.value;
					Initialized = true;
				}
					
				CurrentValue = value;
				RaiseValueUpdated();
			}
		}
		
		public override void RaiseValueUpdated()
		{
			if (!Initialized)
			{
				CurrentValue = value;
				Initialized = true;
			}
			base.RaiseValueUpdated();
		}
	}
	
	// public class ConstantVariableSo<T> : VariableSo<T>
	// {
	// 	public override bool ConstantValue() => true;
	// }
	//
	// public class LocalVariableSo<T> : VariableSo<T>
	// {
	// 	public override bool CreateReference() => true;
	// }

	public class IntVariableSo : VariableSo<int> { }
	
	public class FloatVariableSo : VariableSo<float> { }
	
	public class Vector3VariableSo : VariableSo<Vector3> { }
}