// SPDX-License-Identifier: Apache-2.0
// © 2024-2025 Depra <n.melnikov@depra.org>

using System;
using System.Collections;
using UnityEngine;
using static Depra.Sound.Module;

namespace Depra.Sound.Source
{
	[AddComponentMenu(MENU_PATH + nameof(OneTimeAudioSource), DEFAULT_ORDER)]
	public sealed class OneTimeAudioSource : MonoBehaviour, IAudioSource
	{
		[SerializeField] private SceneAudioSource _target;
		[SerializeField] private float _threshold = 0.1f;

		private IAudioSource _source;
		private Coroutine _selfDestroyCoroutine;

		event Action IAudioSource.Started
		{
			add => _source.Started += value;
			remove => _source.Started -= value;
		}

		event Action<AudioStopReason> IAudioSource.Stopped
		{
			add => _source.Stopped += value;
			remove => _source.Stopped -= value;
		}

		private void Awake() => _source = _target.GetComponent<IAudioSource>();

		private void OnDestroy() => TryStopSelfDestroy();

		bool IAudioSource.IsPlaying => _source.IsPlaying;
		IAudioClip IAudioSource.Current => _source.Current;

		public void Play(IAudioClip clip)
		{
			TryStopSelfDestroy();
			_source.Play(clip);
			var threshold = clip.Duration + _threshold;
			_selfDestroyCoroutine = StartCoroutine(SelfDestroy(threshold));
		}

		public void Play(IAudioClip clip, ReadOnlySpan<AudioParameter> staticParams, ReadOnlySpan<AudioParameter> dynamicParams)
		{
			TryStopSelfDestroy();
			_source.Play(clip, staticParams, dynamicParams);
			var threshold = clip.Duration + _threshold;
			_selfDestroyCoroutine = StartCoroutine(SelfDestroy(threshold));
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