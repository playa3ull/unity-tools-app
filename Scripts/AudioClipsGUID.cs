namespace CocodriloDog.App {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[CreateAssetMenu(menuName = "Cocodrilo Dog/App/Audio Clips GUID")]
	public class AudioClipsGUID : AssetsGUID<AudioClip, AudioClipGUIDReference> { }

	[Serializable]
	public class AudioClipGUIDReference : AssetGUIDReference<AudioClip> { }

}