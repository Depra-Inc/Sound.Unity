using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Depra.Sound.Unity.Editor
{
	internal sealed class AudioLibraryEditorGUI
	{
		private const float WIDE_LAYOUT_WIDTH = 900f;
		private AudioProjectSettings _table;
		private int _bankIndex;

		private AudioProjectSettings _boundTable;
		private SerializedObject _tableObject;
		private ReorderableList _banks;
		private UnityEditor.Editor _bankEditor;
		private Vector2 _bankScroll;
		private float _bankPaneWidth = 230f;
		private float _availableWidth;
		private string _eventSearch = string.Empty;

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
			_eventSearch = EditorGUILayout.TextField(" Search in Global Lookup", _eventSearch, EditorStyles.textField);

			if (width >= WIDE_LAYOUT_WIDTH)
			{
				using (new EditorGUILayout.HorizontalScope())
				{
					DrawBankPane(true);
					DrawPaneSplitter();
					DrawEventPane(banks);
				}
			}
			else
			{
				DrawBankPane(false);
				EditorGUILayout.Space(6f);
				DrawEventPane(banks);
			}

			if (_tableObject.ApplyModifiedProperties())
			{
				EditorUtility.SetDirty(_table);
			}
		}

		private void DrawBankPane(bool wideLayout)
		{
			using (new EditorGUILayout.VerticalScope(wideLayout
				       ? GUILayout.Width(_bankPaneWidth)
				       : GUILayout.ExpandWidth(true)))
			{
				DrawBanks();
				DrawImportButtons();
			}
		}

		private void DrawBanks()
		{
			_banks.index = _bankIndex;
			var height = _banks.GetHeight();
			_bankScroll = EditorGUILayout.BeginScrollView(_bankScroll, GUILayout.Height(height));
			_banks.DoList(GUILayoutUtility.GetRect(0f, height, GUILayout.ExpandWidth(true)));
			EditorGUILayout.EndScrollView();
		}

		private void DrawImportButtons()
		{
			var allImportersInAssembly = TypeCache.GetTypesDerivedFrom<IAudioBankImporter>();
			foreach (var importerType in allImportersInAssembly)
			{
				var importer = (IAudioBankImporter)Activator.CreateInstance(importerType);
				if (GUILayout.Button($"Import with {importer.Name}"))
				{
					if (!EditorApplication.ExecuteMenuItem(importer.MenuPath))
					{
						Debug.LogWarning($"{importer.Name} is unavailable.");
					}
				}
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

			UnityEditor.Editor.CreateCachedEditor(bank, null, ref _bankEditor);
			using (new EditorGUILayout.VerticalScope(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
			{
				DrawBank(bank);
				DrawBankMembers();
			}
		}

		private void DrawBankMembers()
		{
			if (_bankEditor == null)
			{
				EditorGUILayout.HelpBox("Cannot create editor for selected bank.", MessageType.Error);
				return;
			}

			if (_bankEditor is IAudioBankEmbeddedEditor embeddedEditor)
			{
				embeddedEditor.DrawEmbedded(_table);
				return;
			}

			_bankEditor.OnInspectorGUI();
		}

		private void DrawBank(AudioBankAsset bank)
		{
			using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
			{
				EditorGUILayout.LabelField("Bank", EditorStyles.boldLabel, GUILayout.Width(42f));
				var path = AssetDatabase.GetAssetPath(bank);
				var bankName = EditorGUILayout.DelayedTextField(bank.name);
				if (bankName == bank.name || string.IsNullOrWhiteSpace(bankName))
				{
					return;
				}

				var error = AssetDatabase.RenameAsset(path, bankName);
				if (!string.IsNullOrEmpty(error))
				{
					Debug.LogError(error, bank);
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
				elementHeightCallback = index =>
				{
					if (index >= 0 && index < banks.arraySize)
					{
						return MatchesBankSearch(banks.GetArrayElementAtIndex(index))
							? EditorGUIUtility.singleLineHeight + 2f
							: 0f;
					}

					return 0f;
				},
				drawHeaderCallback = rect =>
				{
					var found = 0;
					var total = banks.arraySize;
					for (var index = 0; index < total; index++)
					{
						if (MatchesBankSearch(banks.GetArrayElementAtIndex(index)))
						{
							found++;
						}
					}

					EditorGUI.LabelField(rect, $"Banks ({found}/{total})");
				},
				drawElementCallback = (rect, index, _, _) =>
				{
					if (index >= banks.arraySize)
					{
						return;
					}

					var bankProperty = banks.GetArrayElementAtIndex(index);
					if (!MatchesBankSearch(bankProperty))
					{
						return;
					}

					rect.y += 1f;
					rect.height = EditorGUIUtility.singleLineHeight;
					if (Event.current.type == EventType.MouseDown && Event.current.button == 0 &&
					    rect.Contains(Event.current.mousePosition))
					{
						SelectBank(index);
						_banks.index = index;
					}

					var bankInstance = bankProperty.objectReferenceValue as AudioBankAsset;
					if (bankInstance != null && !string.IsNullOrEmpty(bankInstance.IconPath))
					{
						var bankIcon = EditorGUIUtility.Load(bankInstance.IconPath) as Texture2D;
						if (bankIcon != null)
						{
							var iconRect = new Rect(rect.x, rect.y, rect.height, rect.height);
							GUI.DrawTexture(iconRect, bankIcon, ScaleMode.ScaleToFit);
							rect.x += rect.height + 2f;
							rect.width -= rect.height + 2f;
						}
					}

					EditorGUI.PropertyField(rect, bankProperty, GUIContent.none);
				},
				onSelectCallback = list =>
				{
					if (list.index >= 0 && list.index < banks.arraySize &&
					    MatchesBankSearch(banks.GetArrayElementAtIndex(list.index)))
					{
						SelectBank(list.index);
					}
				},
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
					DeleteArrayElement(banks, list.index);
					_bankIndex = Mathf.Clamp(_bankIndex, 0, banks.arraySize - 1);
				}
			};
		}

		private void ResetBinding()
		{
			_boundTable = null;
			_banks = null;
			if (_bankEditor != null)
			{
				Object.DestroyImmediate(_bankEditor);
				_bankEditor = null;
			}

			_bankIndex = 0;
		}

		private void RepairDuplicateIds()
		{
			var usedIds = new HashSet<ulong>();
			var processedBanks = new HashSet<AudioBankAsset>();
			foreach (var bank in _table.Banks)
			{
				if (bank == null || !processedBanks.Add(bank))
				{
					continue;
				}

				var bankObject = new SerializedObject(bank);
				bankObject.Update();
				var entries = bankObject.FindProperty("_events");
				var changed = false;
				for (var index = 0; index < entries.arraySize; index++)
				{
					var id = entries.GetArrayElementAtIndex(index)
						.FindPropertyRelative("Id")
						.FindPropertyRelative("Value");

					if (usedIds.Add(id.ulongValue))
					{
						continue;
					}

					if (!changed)
					{
						Undo.RecordObject(bank, "Regenerate duplicate audio event IDs");
					}

					if (!changed)
					{
						Undo.RecordObject(_table, "Advance audio event ID counter");
					}

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
			if (_bankIndex == index)
			{
				return;
			}

			if (_bankEditor != null)
			{
				Object.DestroyImmediate(_bankEditor);
				_bankEditor = null;
			}

			_bankIndex = index;
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

		private bool MatchesBankSearch(SerializedProperty bankProperty)
		{
			if (string.IsNullOrWhiteSpace(_eventSearch))
			{
				return true;
			}

			var bank = bankProperty.objectReferenceValue as AudioBankAsset;
			if (bank == null)
			{
				return false;
			}

			var search = _eventSearch.Trim();
			return bank.name.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
			       bank.GetAllEventNames()
				       .Any(pair => pair.label.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0);
		}
	}
}