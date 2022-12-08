namespace CocodriloDog.App.Examples {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	using UnityEngine;
	using UnityEngine.UI;

	public class RuntimeScriptableStorage_Example : MonoBehaviour {


		[Header("References")]

		[SerializeField]
		public RuntimeScriptableStorage RuntimeScriptableStorage;


		#region Unity Methods

		private void OnEnable() {
			InputField.onEndEdit.AddListener(InputField_onEndEdit);
			AddObjectButton.onClick.AddListener(AddObjectButton_onClick);
			RemoveObjectButton.onClick.AddListener(RemoveObjectButton_onClick);
			SaveButton.onClick.AddListener(SaveButton_onClick);
			DeleteButton.onClick.AddListener(DeleteButton_onClick);
		}

		private void Start() {
			AddAllSubobjectTexts();
		}

		private void Update() {
			Text.text = string.Format("SomeField: {0}", PlayerScriptableObject.SomeField);
		}

		private void OnDisable() {
			InputField.onEndEdit.AddListener(InputField_onEndEdit);
			AddObjectButton.onClick.RemoveListener(AddObjectButton_onClick);
			RemoveObjectButton.onClick.RemoveListener(RemoveObjectButton_onClick);
			SaveButton.onClick.RemoveListener(SaveButton_onClick);
			DeleteButton.onClick.RemoveListener(DeleteButton_onClick);
		}

		#endregion


		#region Event Handlers
		
		private void InputField_onEndEdit(string arg0) {
			PlayerScriptableObject.SomeField = arg0;
		}

		private void AddObjectButton_onClick() {
			ExampleScriptableObject subobject = ScriptableObject.CreateInstance<ExampleScriptableObject>();
			subobject.SomeField = string.Format(
				"Subobject {0}", 
				PlayerScriptableObject.SubScriptableObjectsList.Count
			);
			PlayerScriptableObject.SubScriptableObjectsList.Add(subobject);
			AddSubobjectText(subobject);
		}

		private void RemoveObjectButton_onClick() {
			PlayerScriptableObject.SubScriptableObjectsList.RemoveAt(
				PlayerScriptableObject.SubScriptableObjectsList.Count - 1
			);
			RemoveSubobjectText();
		}

		private void SaveButton_onClick() {
			RuntimeScriptableStorage.Save();
		}

		private void DeleteButton_onClick() {
			RemoveAllSubobjectTexts();
			RuntimeScriptableStorage.Delete();
			StartCoroutine(AddAllSubobjectTextsAfterFrame());
		}

		#endregion


		#region Private Fields - Serialized

		[Header("Subcomponents")]

		[SerializeField]
		private InputField m_InputField;

		[SerializeField]
		private Text m_Text;

		[SerializeField]
		private Button m_AddObjectButton;

		[SerializeField]
		private Button m_RemoveObjectButton;

		[SerializeField]
		private RectTransform m_SubobjectTexts;

		[SerializeField]
		private Button m_SaveButton;

		[SerializeField]
		private Button m_DeleteButton;

		[SerializeField]
		private Button m_PlayAudioButton;

		#endregion


		#region Private Fields - Non Serialized

		[NonSerialized]
		private ExampleScriptableObject m_PlayerScriptableObject;

		[NonSerialized]
		private AudioSource m_AudioSource;

		#endregion


		#region Private Properties

		private InputField InputField { get { return m_InputField; } }

		private Text Text { get { return m_Text; } }

		private Button AddObjectButton { get { return m_AddObjectButton; } }

		private Button RemoveObjectButton { get { return m_RemoveObjectButton; } }

		private RectTransform SubobjectTexts { get { return m_SubobjectTexts; } }

		private Button SaveButton { get { return m_SaveButton; } }

		private Button DeleteButton { get { return m_DeleteButton; } }

		private Button PlayAudioButton { get { return m_PlayAudioButton; } }

		private ExampleScriptableObject PlayerScriptableObject {
			get {
				if (m_PlayerScriptableObject == null) {
					m_PlayerScriptableObject = RuntimeScriptableStorage.GetRuntimeScriptableObject<ExampleScriptableObject>();
				}
				return m_PlayerScriptableObject;
			}
		}

		private AudioSource AudioSource {
			get {
				if(m_AudioSource == null) {
					m_AudioSource = gameObject.AddComponent<AudioSource>();
				}
				return m_AudioSource;
			}
		}

		#endregion


		#region Private Methods

		private void AddAllSubobjectTexts() {
			foreach (ExampleScriptableObject subobject in PlayerScriptableObject.SubScriptableObjectsList) {
				AddSubobjectText(subobject);
			}
		}

		IEnumerator AddAllSubobjectTextsAfterFrame() {
			yield return null;
			AddAllSubobjectTexts();
		}

		private void RemoveAllSubobjectTexts() {
			foreach(Transform tr in SubobjectTexts) {
				Destroy(tr.gameObject);
			}
			SubobjectTexts.sizeDelta -= Vector2.up * SubobjectTexts.sizeDelta.y;
		}

		private void AddSubobjectText(ExampleScriptableObject subobject) {

			Text text = Instantiate(Text);
			text.text = subobject.SomeField;
			text.rectTransform.SetParent(SubobjectTexts);
			text.rectTransform.localScale = Vector3.one;

			SubobjectTexts.sizeDelta += Vector2.up * text.rectTransform.sizeDelta.y;

		}

		private void RemoveSubobjectText() {
			RectTransform text = (RectTransform)SubobjectTexts.GetChild(SubobjectTexts.childCount - 1);
			Destroy(text.gameObject);
			SubobjectTexts.sizeDelta -= Vector2.up * text.sizeDelta.y;
		}

		#endregion


	}
}