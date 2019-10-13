namespace CocodriloDog.App {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(AudioClipGUIDReference))]
	public class AudioClipGUIDReferencePropertyDrawer : AssetGUIDReferencePropertyDrawer { }

}