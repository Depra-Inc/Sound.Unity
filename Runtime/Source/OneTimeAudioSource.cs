// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using System.Collections;
using Depra.Sound.Clip;
using UnityEngine;

namespace Depra.Sound.Source
{
	[RequireComponent(typeof(UnityAudioSource))]
	public sealed class OneTimeAudioSource : MonoBehaviour, IAudioSource
	{
		[SerializeField] private float _threshold = 0.1f;

		private UnityAudioSource _source;
		private Coroutine _selfDestroyCoroutine;

		private void Awake() => _source = GetComponent<UnityAudioSource>();

		private void OnDestroy() => TryStopSelfDestroy();

		public bool IsPlaying => _source.IsPlaying;

		public float Volume
		{
			get => _source.Volume;
			set => _source.Volume = value;
		}

		public void Play(IAudioClip clip)
		{
			TryStopSelfDestroy();
			_source.Play(clip);
			_selfDestroyCoroutine = StartCoroutine(SelfDestroy(clip.Duration + _threshold));
		}

		public void Stop() => _source.Stop();

		private IEnumerator SelfDestroy(float duration)
		{
			yield return new WaitForSeconds(duration);

			_source.Stop();
			Destroy(this);
		}

		private void TryStopSelfDestroy()
		{
			if (_selfDestroyCoroutine != null)
			{
				StopCoroutine(_selfDestroyCoroutine);
			}
		}
	}
}