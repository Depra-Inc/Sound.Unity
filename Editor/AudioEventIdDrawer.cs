using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Depra.Sound.Unity.Editor
{
	[CustomPropertyDrawer(typeof(AudioEventId))]
	internal sealed class AudioEventIdDrawer : PropertyDrawer
	{
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
				ShowHierarchicalMenu(options, value);
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

			return string.IsNullOrEmpty(option.Label) ? "None" : option.Label;
		}

		private static void ShowHierarchicalMenu(List<EventOption> options, SerializedProperty value)
		{
			var menu = new GenericMenu();
			menu.AddItem(new GUIContent("None"), value.ulongValue == 0, () =>
			{
				value.ulongValue = 0;
				value.serializedObject.ApplyModifiedProperties();
			});

			menu.AddSeparator("");
			var groupedByBank = new Dictionary<string, List<EventOption>>();
			foreach (var option in options)
			{
				if (!groupedByBank.ContainsKey(option.BankName))
				{
					groupedByBank[option.BankName] = new List<EventOption>();
				}

				groupedByBank[option.BankName].Add(option);
			}

			foreach (var bankName in groupedByBank.Keys)
			{
				var bankEvents = groupedByBank[bankName];
				foreach (var option in bankEvents)
				{
					var menuPath = $"{bankName}/{option.Label}";
					var optionId = option.Id;
					var isSelected = value.ulongValue == optionId;
					menu.AddItem(new GUIContent(menuPath), isSelected, () =>
					{
						value.ulongValue = optionId;
						value.serializedObject.ApplyModifiedProperties();
					});
				}
			}

			menu.ShowAsContext();
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
	}
}