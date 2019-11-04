namespace CocodriloDog.App {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Holds a list of AudioClip assets and their corresponding GUIDs.
	/// </summary>
	[CreateAssetMenu(menuName = "Cocodrilo Dog/App/Audio Clips GUID")]
	public class AudioClipsGUID : AssetsGUID<AudioClip, AudioClipGUIDReference> { }

	/// <summary>
	/// A class that pairs an AudioClip and its GUID.
	/// </summary>
	[Serializable]
	public class AudioClipGUIDReference : AssetGUIDReference<AudioClip> { }

}