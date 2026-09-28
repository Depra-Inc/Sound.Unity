using System.Collections.Generic;
using Depra.Sound.Configuration;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Depra.Sound.Editor
{
	internal sealed class AudioLibraryEditorGUI
	{
		private AudioProjectSettings _table;
		private int _bankIndex;

		private AudioProjectSettings _boundTable;
		private SerializedObject _tableObject;
		private ReorderableList _banks;
		private AudioBankAsset _bank;
		private SerializedObject _bankObject;
		private ReorderableList _events;
		private Vector2 _bankScroll;
		private float _bankPaneWidth = 230f;
		private float _availableWidth;
		private const float WIDE_LAYOUT_WIDTH = 900f;

		internal void Draw(AudioProjectSettings table)
		{
			if (_table != table)
			{
				_table = table;
				ResetBinding();
			}

			if (_table)
			{
				DrawLibraryContents(EditorGUIUtility.currentViewWidth);
			}
		}

		private void DrawLibraryContents(float width)
		{
			_availableWidth = width;
			EnsureTableState();
			RepairDuplicateIds();
			_tableObject.Update();
			var banks = _tableObject.FindProperty("_banks");

			if (width >= WIDE_LAYOUT_WIDTH)
			{
				using (new EditorGUILayout.HorizontalScope())
				{
					DrawBankPane(banks, 250f, true);
					DrawPaneSplitter();
					DrawEventPane(banks);
				}
			}
			else
			{
				DrawBankPane(banks, 170f, false);
				EditorGUILayout.Space(6f);
				DrawEventPane(banks);
			}

			if (_bankObject != null && _bankObject.ApplyModifiedProperties())
			{
				EditorUtility.SetDirty(_bank);
			}

			if (_tableObject.ApplyModifiedProperties())
			{
				EditorUtility.SetDirty(_table);
			}
		}

		private void DrawBanks(SerializedProperty banks, float maxHeight)
		{
			_banks.index = _bankIndex;
			var height = Mathf.Min(_banks.GetHeight(), maxHeight);
			_bankScroll = EditorGUILayout.BeginScrollView(_bankScroll, GUILayout.Height(height));
			_banks.DoList(GUILayoutUtility.GetRect(0f, _banks.GetHeight(), GUILayout.ExpandWidth(true)));
			EditorGUILayout.EndScrollView();
		}

		private void DrawBankPane(SerializedProperty banks, float maxHeight, bool wideLayout)
		{
			using (new EditorGUILayout.VerticalScope(wideLayout
				       ? GUILayout.Width(_bankPaneWidth)
				       : GUILayout.ExpandWidth(true)))
			{
				DrawBanks(banks, maxHeight);
			}
		}

		private void DrawEventPane(SerializedProperty banks)
		{
			var bank = SelectedBank(banks);
			if (!bank)
			{
				EditorGUILayout.HelpBox("Select a bank to edit its events.", MessageType.Info);
				return;
			}

			EnsureEventList();
			_bankObject.Update();
			using (new EditorGUILayout.VerticalScope(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
			{
				DrawBank(bank);
				_events.DoList(GUILayoutUtility.GetRect(0f, _events.GetHeight(), GUILayout.ExpandWidth(true)));
			}
		}

		private void DrawBank(AudioBankAsset bank)
		{
			using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
			{
				EditorGUILayout.LabelField("Bank", EditorStyles.boldLabel, GUILayout.Width(42f));
				var path = AssetDatabase.GetAssetPath(bank);
				var bankName = EditorGUILayout.DelayedTextField(bank.name);
				if (bankName != bank.name && !string.IsNullOrWhiteSpace(bankName))
				{
					var error = AssetDatabase.RenameAsset(path, bankName);
					if (!string.IsNullOrEmpty(error)) Debug.LogError(error, bank);
				}
			}
		}

		private void DrawPaneSplitter()
		{
			var rect = GUILayoutUtility.GetRect(4f, 0f, GUILayout.Width(4f), GUILayout.ExpandHeight(true));
			var controlId = GUIUtility.GetControlID(FocusType.Passive);
			EditorGUIUtility.AddCursorRect(rect, MouseCursor.ResizeHorizontal);
			if (GUIUtility.hotControl != 0 && GUIUtility.hotControl != controlId)
			{
				return;
			}

			if (Event.current.type == EventType.MouseDown && Event.current.button == 0 &&
			    rect.Contains(Event.current.mousePosition))
			{
				GUIUtility.hotControl = controlId;
				Event.current.Use();
			}

			if (GUIUtility.hotControl == controlId && Event.current.type == EventType.MouseDrag)
			{
				_bankPaneWidth = Mathf.Clamp(_bankPaneWidth + Event.current.delta.x, 160f, _availableWidth - 360f);
				Event.current.Use();
				EditorWindow.focusedWindow?.Repaint();
			}

			if (GUIUtility.hotControl == controlId && Event.current.type == EventType.MouseUp)
			{
				GUIUtility.hotControl = 0;
				Event.current.Use();
			}

			EditorGUI.DrawRect(rect, EditorGUIUtility.isProSkin
				? new Color(1f, 1f, 1f, 0.12f)
				: new Color(0f, 0f, 0f, 0.16f));
		}

		private void EnsureEventList()
		{
			var bank = SelectedBank(_tableObject.FindProperty("_banks"));
			if (bank == _bank && _events != null)
			{
				return;
			}

			_bank = bank;
			_bankObject = new SerializedObject(bank);
			var entries = _bankObject.FindProperty("_events");
			var canAddEvents = _bank is AudioClipBank;
			_events = new ReorderableList(_bankObject, entries, false, true, canAddEvents, true)
			{
				index = entries.arraySize > 0 ? 0 : -1,
				elementHeightCallback = index =>
				{
					if (index >= entries.arraySize)
					{
						return EditorGUIUtility.singleLineHeight;
					}

					var entry = entries.GetArrayElementAtIndex(index);
					return GetEventHeight(entry);
				},
				drawHeaderCallback = rect => EditorGUI.LabelField(rect, $"Events ({entries.arraySize})"),
				drawElementCallback = (rect, index, _, __) => DrawEventRow(rect, entries, index),
				onAddCallback = list =>
				{
					var index = entries.arraySize;
					entries.InsertArrayElementAtIndex(index);
					var entry = entries.GetArrayElementAtIndex(index);
					entry.FindPropertyRelative("Name").stringValue = $"Event {index + 1}";
					Undo.RecordObject(_table, "Allocate audio event ID");
					var eventId = _table.AllocateEventId();
					EditorUtility.SetDirty(_table);
					SetId(entry.FindPropertyRelative("Id"), eventId);
					entry.FindPropertyRelative("Description").managedReferenceValue = new AudioEventDescription();
					list.index = index;
				},
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

		private static float GetEventHeight(SerializedProperty entry)
		{
			var line = EditorGUIUtility.singleLineHeight;
			if (!entry.isExpanded)
			{
				return line + 6f;
			}

			var description = entry.FindPropertyRelative("Description");
			var descriptionHeight = EditorGUI.GetPropertyHeight(
				description, new GUIContent("Description"), true);
			return line + 2f // foldout header
			       + line + 2f // name
			       + descriptionHeight + 8f;
		}

		private void DrawEventRow(Rect rect, SerializedProperty entries, int index)
		{
			if (index >= entries.arraySize)
			{
				return;
			}

			var entry = entries.GetArrayElementAtIndex(index);
			var name = entry.FindPropertyRelative("Name");
			var id = entry.FindPropertyRelative("Id");
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

		private void EnsureTableState()
		{
			if (_boundTable == _table && _banks != null)
			{
				return;
			}

			_boundTable = _table;
			_tableObject = new SerializedObject(_table);
			var banks = _tableObject.FindProperty("_banks");
			_banks = new ReorderableList(_tableObject, banks, true, true, true, true)
			{
				index = _bankIndex,
				elementHeight = EditorGUIUtility.singleLineHeight + 2f,
				drawHeaderCallback = rect => EditorGUI.LabelField(rect, $"Banks ({banks.arraySize})"),
				drawElementCallback = (rect, index, _, __) =>
				{
					if (index >= banks.arraySize) return;
					var bankProperty = banks.GetArrayElementAtIndex(index);
					rect.y += 1f;
					rect.height = EditorGUIUtility.singleLineHeight;
					if (Event.current.type == EventType.MouseDown && Event.current.button == 0 &&
					    rect.Contains(Event.current.mousePosition))
					{
						SelectBank(index);
						_banks.index = index;
					}

					EditorGUI.PropertyField(rect, bankProperty, GUIContent.none);
				},
				onSelectCallback = list => SelectBank(list.index),
				onAddCallback = list =>
				{
					banks.InsertArrayElementAtIndex(banks.arraySize);
					list.index = banks.arraySize - 1;
					banks.GetArrayElementAtIndex(list.index).objectReferenceValue = null;
					SelectBank(list.index);
				},
				onReorderCallback = list => SelectBank(list.index),
				onRemoveCallback = list =>
				{
					if (!EditorUtility.DisplayDialog("Remove Bank", "Remove this bank from the table?",
						    "Remove", "Cancel")) return;
					DeleteArrayElement(banks, list.index);
					_bankIndex = Mathf.Clamp(_bankIndex, 0, banks.arraySize - 1);
					_bank = null;
				}
			};
		}

		private void ResetBinding()
		{
			_boundTable = null;
			_banks = null;
			_bank = null;
			_bankObject = null;
			_events = null;
			_bankIndex = 0;
		}

		private void RepairDuplicateIds()
		{
			var usedIds = new HashSet<ulong>();
			var processedBanks = new HashSet<AudioBankAsset>();
			foreach (var bank in _table.Banks)
			{
				if (bank == null || !processedBanks.Add(bank)) continue;
				var bankObject = new SerializedObject(bank);
				bankObject.Update();
				var entries = bankObject.FindProperty("_events");
				var changed = false;
				for (var index = 0; index < entries.arraySize; index++)
				{
					var id = entries.GetArrayElementAtIndex(index).FindPropertyRelative("Id")
						.FindPropertyRelative("Value");
					if (usedIds.Add(id.ulongValue)) continue;
					if (!changed) Undo.RecordObject(bank, "Regenerate duplicate audio event IDs");
					if (!changed) Undo.RecordObject(_table, "Advance audio event ID counter");
					id.ulongValue = _table.AllocateEventId(usedIds, bank);
					usedIds.Add(id.ulongValue);
					changed = true;
				}

				if (changed)
				{
					bankObject.ApplyModifiedProperties();
					EditorUtility.SetDirty(bank);
					EditorUtility.SetDirty(_table);
				}
			}
		}

		private void SelectBank(int index)
		{
			if (_bankIndex == index) return;
			_bankIndex = index;
			_bank = null;
			EditorWindow.focusedWindow?.Repaint();
		}

		private AudioBankAsset SelectedBank(SerializedProperty banks) =>
			_bankIndex >= 0 && _bankIndex < banks.arraySize
				? banks.GetArrayElementAtIndex(_bankIndex).objectReferenceValue as AudioBankAsset
				: null;

		private static void DeleteArrayElement(SerializedProperty array, int index)
		{
			var size = array.arraySize;
			array.DeleteArrayElementAtIndex(index);
			if (array.arraySize == size)
			{
				array.DeleteArrayElementAtIndex(index);
			}
		}

		private static void SetId(SerializedProperty id, ulong value)
		{
			id.FindPropertyRelative("Value").ulongValue = value;
		}
	}
}