using System;
using UnityEngine;
using Logger = ScriptableVariables.Utils.Logger;
using Object = UnityEngine.Object;

namespace ScriptableVariables
{
	[Serializable]
	public class VariableReference <TBase, TVariable> : VariableReference where TVariable : VariableSo<TBase>
	{
		[SerializeField] private TVariable variableSo;
		[SerializeField] private TBase rawValue;
		[SerializeField] private bool useRawValue = false;
		private VariableSo<TBase> _variableSoRef;
		private event Action<TBase> OnValueUpdated;
		
		public VariableReference () {}
		public VariableReference(TBase baseValue)
		{
			useRawValue = true;
			rawValue = baseValue;
		}
		
		public virtual TBase Value
		{
			get => useRawValue ? rawValue : GetActiveSo().Value;
			set
			{
				if (useRawValue)
				{
					rawValue = value;
					RaiseValueUpdated();
					return;
				}
				if(variableSo.ConstantValue()) return;
				GetActiveSo().Value = value;
			}
		}

		public void RegisterListener(Action<TBase> action)
		{
			if (useRawValue)
			{
				OnValueUpdated += action;
				return;
			}
			
			if(variableSo.ConstantValue()) return;
			
			GetActiveSo().OnValueUpdated += action;
		}
		
		public void UnregisterListener(Action<TBase> action)
		{
			if (useRawValue)
			{
				OnValueUpdated -= action;
				return;
			}
			
			if(variableSo.ConstantValue()) return;
			
			GetActiveSo().OnValueUpdated -= action;
		}

		private void CheckReference()
		{
			if (!_variableSoRef)
			{
				_variableSoRef = Object.Instantiate(variableSo);
			}

			if (!_variableSoRef)
				Logger.LogError("Failed to create reference");
		}

		private VariableSo<TBase> GetActiveSo()
		{
			if (!variableSo.CreateReference()) return variableSo;
			CheckReference();
			
			return _variableSoRef;
		}

		private void RaiseValueUpdated()
		{
			if(useRawValue)
				OnValueUpdated?.Invoke(rawValue);
			else
				GetActiveSo().RaiseValueUpdated();
		}
	}
	
	[Serializable]
	public class FloatReference : VariableReference<float, FloatVariableSo>
	{
		public FloatReference () : base () {}
		public FloatReference(float value) : base(value) { }
	}
	[Serializable]
	public class IntReference : VariableReference<int, IntVariableSo>
	{
		public IntReference () : base () {}
		public IntReference (int value) : base (value) {}
	}
	[Serializable]
	public class Vector3Reference : VariableReference<Vector3, Vector3VariableSo>
	{
		public Vector3Reference () : base () {}
		public Vector3Reference (Vector3 value) : base (value) {}
	}
	
	public abstract class VariableReference
	{
	}
}