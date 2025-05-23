// SPDX-License-Identifier: Apache-2.0
// © 2024-2025 Depra <n.melnikov@depra.org>

using System;
using System.Collections;
using System.Collections.Generic;
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
		IEnumerable<Type> IAudioSource.SupportedClips => _source.SupportedClips;

		public void Stop() => _source.Stop();

		public void Play(IAudioClip clip, IList<IAudioSourceParameter> parameters)
		{
			TryStopSelfDestroy();
			_source.Play(clip, parameters);
			var threshold = clip.Duration + _threshold;
			_selfDestroyCoroutine = StartCoroutine(SelfDestroy(threshold));
		}

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

		bool IAudioSource.Write(IAudioSourceParameter parameter) => _source.Write(parameter);
		IAudioSourceParameter IAudioSource.Read(Type parameterType) => _source.Read(parameterType);
		IEnumerable<IAudioSourceParameter> IAudioSource.EnumerateParameters() => _source.EnumerateParameters();
	}
}