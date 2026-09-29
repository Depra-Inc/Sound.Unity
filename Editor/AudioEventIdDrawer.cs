using System.Collections.Generic;
using Depra.Sound.Configuration;
using UnityEditor;
using UnityEngine;

namespace Depra.Sound.Editor
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

			var selected = options.FindIndex(option => option.Id == value.ulongValue);
			if (selected < 0)
			{
				options.Insert(0, new EventOption(value.ulongValue, $"Unknown ID ({value.ulongValue})"));
				selected = 0;
			}

			var labels = new string[options.Count];
			for (var index = 0; index < options.Count; index++)
			{
				labels[index] = options[index].Label;
			}

			EditorGUI.BeginProperty(position, label, property);
			EditorGUI.BeginChangeCheck();
			var popupRect = EditorGUI.PrefixLabel(position, label);
			var next = EditorGUI.Popup(popupRect, selected, labels);
			if (EditorGUI.EndChangeCheck())
			{
				value.ulongValue = options[next].Id;
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
					options.Add(new EventOption(id, name));
				}
			}

			return options;
		}

		private readonly struct EventOption
		{
			public readonly ulong Id;
			public readonly string Label;

			public EventOption(ulong id, string label)
			{
				Id = id;
				Label = label;
			}
		}
	}
}