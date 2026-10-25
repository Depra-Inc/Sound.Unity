using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Depra.Sound.Unity.Editor
{
	[CustomPropertyDrawer(typeof(AudioEventId))]
	internal sealed class AudioEventIdDrawer : PropertyDrawer
	{
		private const float DROPDOWN_WIDTH = 420f;
		private const float DROPDOWN_HEIGHT = 420f;

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
			EditorGUIUtility.singleLineHeight;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var value = property.FindPropertyRelative(nameof(AudioEventId.Value));
			if (value == null)
			{
				EditorGUI.PropertyField(position, property, label, true);
				return;
			}

			var table = AudioProjectSettingsProvider.LoadTable();
			var options = GetOptions(table);
			if (options.Count == 0)
			{
				EditorGUI.PropertyField(position, value, label);
				return;
			}

			EditorGUI.BeginProperty(position, label, property);
			var prefixRect = EditorGUI.PrefixLabel(position, label);
			var currentLabel = GetCurrentLabel(options, value.ulongValue);
			if (EditorGUI.DropdownButton(prefixRect, new GUIContent(currentLabel), FocusType.Keyboard))
			{
				ShowSearchablePopup(prefixRect, options, value);
			}

			EditorGUI.EndProperty();
		}

		private static List<EventOption> GetOptions(AudioProjectSettings table)
		{
			var options = new List<EventOption>();
			if (!table)
			{
				return options;
			}

			foreach (var bank in table.Banks)
			{
				if (!bank)
				{
					continue;
				}

				foreach (var (id, name) in bank.GetAllEventNames())
				{
					options.Add(new EventOption(id, name, bank.name));
				}
			}

			return options;
		}

		private static string GetCurrentLabel(List<EventOption> options, ulong currentId)
		{
			var option = options.Find(opt => opt.Id == currentId);
			if (option.Id == 0 && currentId != 0)
			{
				return $"Unknown ID ({currentId})";
			}

			return string.IsNullOrEmpty(option.Label) ? "None" : $"{option.BankName}://{option.Label}";
		}

		private static void ShowSearchablePopup(Rect activatorRect, List<EventOption> options, SerializedProperty value)
		{
			var targetObjects = value.serializedObject.targetObjects;
			var dropdown = new EventAdvancedDropdown(new AdvancedDropdownState(), options,
				selectedId => ApplySelection(targetObjects, value.propertyPath, selectedId),
				new Vector2(DROPDOWN_WIDTH, DROPDOWN_HEIGHT));

			dropdown.Show(activatorRect);
		}

		private static void ApplySelection(Object[] targetObjects, string propertyPath, ulong selectedId)
		{
			if (targetObjects == null || targetObjects.Length == 0)
			{
				return;
			}

			foreach (var target in targetObjects)
			{
				if (!target)
				{
					continue;
				}

				var serializedObject = new SerializedObject(target);
				var property = serializedObject.FindProperty(propertyPath);
				if (property == null)
				{
					continue;
				}

				property.ulongValue = selectedId;
				serializedObject.ApplyModifiedProperties();
				EditorUtility.SetDirty(target);
			}
		}

		private readonly struct EventOption
		{
			public readonly ulong Id;
			public readonly string Label;
			public readonly string BankName;

			public EventOption(ulong id, string label, string bankName = "")
			{
				Id = id;
				Label = label;
				BankName = bankName;
			}
		}

		private sealed class EventAdvancedDropdown : AdvancedDropdown
		{
			private readonly List<EventOption> _options;
			private readonly Action<ulong> _onSelected;

			public EventAdvancedDropdown(AdvancedDropdownState state, List<EventOption> options,
				Action<ulong> onSelected, Vector2 minSize) : base(state)
			{
				_options = options ?? new List<EventOption>();
				_onSelected = onSelected;
				minimumSize = minSize;
			}

			protected override AdvancedDropdownItem BuildRoot()
			{
				var root = new AdvancedDropdownItem("Audio Events");
				root.AddChild(new EventDropdownItem("None", 0));

				foreach (var group in _options
					         .GroupBy(option => string.IsNullOrEmpty(option.BankName) ? "Other" : option.BankName)
					         .OrderBy(group => group.Key, StringComparer.OrdinalIgnoreCase))
				{
					var bankItem = new AdvancedDropdownItem(group.Key);
					foreach (var option in group.OrderBy(item => item.Label, StringComparer.OrdinalIgnoreCase))
					{
						bankItem.AddChild(new EventDropdownItem(option.Label, option.Id));
					}

					root.AddChild(bankItem);
				}

				return root;
			}

			protected override void ItemSelected(AdvancedDropdownItem item)
			{
				if (item is EventDropdownItem eventItem)
				{
					_onSelected?.Invoke(eventItem.EventId);
				}
			}
		}

		private sealed class EventDropdownItem : AdvancedDropdownItem
		{
			public readonly ulong EventId;
			public EventDropdownItem(string name, ulong eventId) : base(name) => EventId = eventId;
		}
	}
}