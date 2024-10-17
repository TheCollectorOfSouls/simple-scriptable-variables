using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ScriptableVariables.Editor
{
	//[CustomPropertyDrawer(typeof(VariableReference))]
	public abstract class VariableReferenceEditor : PropertyDrawer
	{
		private SerializedProperty _property;
		private PropertyField _variableSoProp;
		private PropertyField _rawValueProp;
		private SerializedProperty _useRawValue;
		private SerializedProperty _rawValue;
		protected SerializedProperty VariableSo;
		private VisualElement _container;
		
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			_property = property;
			// Create property container element.
			_container = new VisualElement();
			
			// Find properties.
			_useRawValue = property.FindPropertyRelative("useRawValue");
			_rawValue = property.FindPropertyRelative("rawValue");
			VariableSo = property.FindPropertyRelative("variableSo");
			
			// Create property fields.
			_variableSoProp = new PropertyField(VariableSo, property.displayName);
			_rawValueProp = new PropertyField(_rawValue, property.displayName);
			
			_variableSoProp.bindingPath = VariableSo.propertyPath;
			_rawValueProp.bindingPath = _rawValue.propertyPath;
			// Add fields to the container.
			_container.Add(_variableSoProp);
			_container.Add(_rawValueProp);

			AddANewContextMenu(_rawValueProp);
			AddANewContextMenu(_variableSoProp);
			
			ToggleRawValue(_useRawValue.boolValue);
			
			return _container;
		}
		
		void AddANewContextMenu(VisualElement element)
		{
			// The manipulator handles the right click and sends a ContextualMenuPopulateEvent to the target element.
			// The callback argument passed to the constructor is automatically registered on the target element.
			element.AddManipulator(new ContextualMenuManipulator((evt) =>
			{
				evt.menu.AppendSeparator();
				
				evt.menu.AppendAction("Scriptable Variable", action: menuAction => 
					ToggleRawValue(false), actionStatusCallback: dropdownAction  => 
					!_useRawValue.boolValue ? 
						DropdownMenuAction.Status.Checked : DropdownMenuAction.Status.Normal);
				
				evt.menu.AppendAction("Raw Value", action: menuAction => 
					ToggleRawValue(true),actionStatusCallback: dropdownAction => 
					_useRawValue.boolValue ? 
						DropdownMenuAction.Status.Checked : DropdownMenuAction.Status.Normal);
				
				evt.menu.AppendSeparator();
				
				evt.menu.AppendAction("Invoke Event", action: menuAction => 
					InvokeEvent(), actionStatusCallback: dropdownAction => 
					VariableSo.objectReferenceValue && !_useRawValue.boolValue ? 
						DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Hidden);
			}));
		}

		private void ToggleRawValue(bool enable)
		{
			_property.FindPropertyRelative("useRawValue").boolValue = enable;
			if (enable)
			{
				_variableSoProp.style.display = DisplayStyle.None;
				_rawValueProp.style.display = DisplayStyle.Flex;
			}
			else
			{
				_variableSoProp.style.display = DisplayStyle.Flex;
				_rawValueProp.style.display = DisplayStyle.None;
			}
			_property.serializedObject.ApplyModifiedProperties();
		}
		
		protected abstract void InvokeEvent();
	}

	[CustomPropertyDrawer(typeof(FloatReference))]
	public class FloatReferenceEditor : VariableReferenceEditor
	{
		protected override void InvokeEvent()
		{
			var so = VariableSo.objectReferenceValue as FloatVariableSo;
			if (so != null) so.RaiseValueUpdated();
		}
	}
	[CustomPropertyDrawer(typeof(IntReference))]
	public class IntReferenceEditor : VariableReferenceEditor
	{
		protected override void InvokeEvent()
		{
			var so = VariableSo.objectReferenceValue as IntVariableSo;
			if (so != null) so.RaiseValueUpdated();
		}
	}
	[CustomPropertyDrawer(typeof(Vector3Reference))]
	public class Vector3ReferenceEditor : VariableReferenceEditor
	{
		protected override void InvokeEvent()
		{
			var so = VariableSo.objectReferenceValue as Vector3VariableSo;
			if (so != null) so.RaiseValueUpdated();
		}
	}
}