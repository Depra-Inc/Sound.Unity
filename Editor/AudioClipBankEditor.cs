using Depra.Sound.Configuration;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Depra.Sound.Editor
{
	[CustomEditor(typeof(AudioClipBank))]
	internal sealed class AudioClipBankEditor : UnityEditor.Editor, IAudioBankEmbeddedEditor
	{
		private ReorderableList _events;
		private ReorderableList _containers;
		private AudioProjectSettings _settings;

		public override void OnInspectorGUI() =>
			DrawEmbedded(AudioProjectSettingsProvider.LoadTable());

		public void DrawEmbedded(AudioProjectSettings settings)
		{
			_settings = settings;
			serializedObject.Update();
			DrawEventList();
			DrawContainerList();
			_events.DoList(GUILayoutUtility.GetRect(0f, _events.GetHeight(), GUILayout.ExpandWidth(true)));
			EditorGUILayout.Space(6f);
			_containers.DoList(GUILayoutUtility.GetRect(0f, _containers.GetHeight(), GUILayout.ExpandWidth(true)));
			serializedObject.ApplyModifiedProperties();
		}

		private void DrawEventList()
		{
			var entries = serializedObject.FindProperty("_events");
			_events = new ReorderableList(serializedObject, entries, false, true, true, true)
			{
				index = entries.arraySize > 0 ? 0 : -1,
				elementHeightCallback = index =>
					index >= entries.arraySize
						? EditorGUIUtility.singleLineHeight
						: GetEventHeight(entries.GetArrayElementAtIndex(index)),
				drawHeaderCallback = rect => EditorGUI.LabelField(rect, $"Events ({entries.arraySize})"),
				drawElementCallback = (rect, index, _, _) => DrawEventRow(rect, entries, index),
				onAddCallback = list => AddEvent(entries, list),
				onRemoveCallback = list =>
				{
					if (EditorUtility.DisplayDialog("Remove Event", "Remove the selected event?", "Remove", "Cancel"))
					{
						DeleteArrayElement(entries, list.index);
						list.index = Mathf.Min(list.index, entries.arraySize - 1);
					}
				}
			};
		}

		private void DrawContainerList()
		{
			var entries = serializedObject.FindProperty("_containers");
			_containers = new ReorderableList(serializedObject, entries, true, true, true, true)
			{
				index = entries.arraySize > 0 ? 0 : -1,
				elementHeightCallback = index => index >= entries.arraySize
					? EditorGUIUtility.singleLineHeight
					: GetContainerHeight(entries.GetArrayElementAtIndex(index)),
				drawHeaderCallback = rect => EditorGUI.LabelField(rect, $"Containers ({entries.arraySize})"),
				drawElementCallback = (rect, index, _, _) => DrawContainerRow(rect, entries, index),
				onAddCallback = list => AddContainer(entries, list),
				onRemoveCallback = list =>
				{
					if (EditorUtility.DisplayDialog("Remove Container", "Remove the selected container?", "Remove",
						    "Cancel"))
					{
						DeleteArrayElement(entries, list.index);
						list.index = Mathf.Min(list.index, entries.arraySize - 1);
					}
				}
			};
		}

		private void AddEvent(SerializedProperty entries, ReorderableList list)
		{
			if (!TryGetSettings("Cannot add event without project Audio Table."))
			{
				return;
			}

			var index = entries.arraySize;
			entries.InsertArrayElementAtIndex(index);
			var entry = entries.GetArrayElementAtIndex(index);
			entry.FindPropertyRelative(nameof(AudioClipBank.EventEntry.Name)).stringValue = $"Event {index + 1}";
			Undo.RecordObject(_settings, "Allocate audio event ID");
			var eventId = _settings.AllocateEventId();
			EditorUtility.SetDirty(_settings);
			SetId(entry.FindPropertyRelative(nameof(AudioClipBank.EventEntry.Id)), eventId);
			entry.FindPropertyRelative(nameof(AudioClipBank.EventEntry.Description)).managedReferenceValue = new AudioEventDescription();
			list.index = index;
		}

		private void AddContainer(SerializedProperty entries, ReorderableList list)
		{
			if (!TryGetSettings("Cannot add container without project Audio Table."))
			{
				return;
			}

			var index = entries.arraySize;
			entries.InsertArrayElementAtIndex(index);
			var entry = entries.GetArrayElementAtIndex(index);
			entry.FindPropertyRelative(nameof(AudioContainerEntry.Name)).stringValue = $"Container {index + 1}";
			Undo.RecordObject(_settings, "Allocate audio container ID");
			var containerId = _settings.AllocateEventId();
			EditorUtility.SetDirty(_settings);
			SetId(entry.FindPropertyRelative(nameof(AudioContainerEntry.Id)), containerId);
			entry.FindPropertyRelative(nameof(AudioContainerEntry.Container)).objectReferenceValue = null;
			list.index = index;
		}

		private bool TryGetSettings(string error)
		{
			if (_settings)
			{
				return true;
			}

			Debug.LogError(error);
			return false;
		}

		private static float GetEventHeight(SerializedProperty entry)
		{
			var line = EditorGUIUtility.singleLineHeight;
			if (!entry.isExpanded)
			{
				return line + 6f;
			}

			var propertyName = nameof(AudioClipBank.EventEntry.Description);
			var description = entry.FindPropertyRelative(propertyName);
			var descriptionHeight = EditorGUI.GetPropertyHeight(description, new GUIContent(propertyName), true);
			return line + 2f + line + 2f + descriptionHeight + 8f;
		}

		private static float GetContainerHeight(SerializedProperty entry)
		{
			var line = EditorGUIUtility.singleLineHeight;
			if (!entry.isExpanded)
			{
				return line + 6f;
			}

			return line + 2f + line + 2f + line + 8f;
		}

		private static void DrawEventRow(Rect rect, SerializedProperty entries, int index)
		{
			if (index >= entries.arraySize)
			{
				return;
			}

			var entry = entries.GetArrayElementAtIndex(index);
			var name = entry.FindPropertyRelative(nameof(AudioClipBank.EventEntry.Name));
			var id = entry.FindPropertyRelative(nameof(AudioClipBank.EventEntry.Id));
			var header = new Rect(rect.x, rect.y + 2f, rect.width, EditorGUIUtility.singleLineHeight);
			var idValue = id.FindPropertyRelative("Value");
			entry.isExpanded = EditorGUI.Foldout(header, entry.isExpanded,
				$"{(string.IsNullOrWhiteSpace(name.stringValue) ? $"Event {index + 1}" : name.stringValue)}   (ID {idValue.ulongValue})",
				true);
			if (!entry.isExpanded)
			{
				return;
			}

			var y = header.yMax + 2f;
			var indent = rect.x + 14f;
			var width = rect.width - 14f;
			var nameRect = new Rect(indent, y, width, EditorGUIUtility.singleLineHeight);
			EditorGUI.PropertyField(nameRect, name);
			y = nameRect.yMax + 2f;
			var description = entry.FindPropertyRelative("Description");
			var descriptionHeight = EditorGUI.GetPropertyHeight(description, new GUIContent("Description"), true);
			EditorGUI.PropertyField(new Rect(indent, y, width, descriptionHeight),
				description, new GUIContent("Description"), true);
		}

		private static void DrawContainerRow(Rect rect, SerializedProperty entries, int index)
		{
			if (index >= entries.arraySize)
			{
				return;
			}

			var entry = entries.GetArrayElementAtIndex(index);
			var name = entry.FindPropertyRelative(nameof(AudioContainerEntry.Name));
			var id = entry.FindPropertyRelative(nameof(AudioContainerEntry.Id));
			var header = new Rect(rect.x, rect.y + 2f, rect.width, EditorGUIUtility.singleLineHeight);
			var idValue = id.FindPropertyRelative(nameof(AudioContainerEntry.Id.Value));
			entry.isExpanded = EditorGUI.Foldout(header, entry.isExpanded,
				$"{(string.IsNullOrWhiteSpace(name.stringValue) ? $"Container {index + 1}" : name.stringValue)}   (ID {idValue.ulongValue})",
				true);
			if (!entry.isExpanded)
			{
				return;
			}

			var y = header.yMax + 2f;
			var indent = rect.x + 14f;
			var width = rect.width - 14f;
			var nameRect = new Rect(indent, y, width, EditorGUIUtility.singleLineHeight);
			EditorGUI.PropertyField(nameRect, name);
			y = nameRect.yMax + 2f;
			var description = entry.FindPropertyRelative(nameof(AudioContainerEntry.Container));
			EditorGUI.ObjectField(new Rect(indent, y, width, EditorGUIUtility.singleLineHeight + 2f),
				description,
				typeof(AudioEventContainer), GUIContent.none);
		}

		private static void DeleteArrayElement(SerializedProperty array, int index)
		{
			var size = array.arraySize;
			array.DeleteArrayElementAtIndex(index);
			if (array.arraySize == size)
			{
				array.DeleteArrayElementAtIndex(index);
			}
		}

		private static void SetId(SerializedProperty id, ulong value) =>
			id.FindPropertyRelative("Value").ulongValue = value;
	}
}


