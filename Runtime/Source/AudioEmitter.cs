// SPDX-License-Identifier: Apache-2.0
// © 2024-2025 Depra <n.melnikov@depra.org>

using System;
using System.Runtime.CompilerServices;
using Depra.SerializeReference.Extensions;
using Depra.Sound.Exceptions;
using JetBrains.Annotations;
using UnityEngine;
using static Depra.Sound.Module;

namespace Depra.Sound
{
	[RequireComponent(typeof(IAudioSource))]
	[AddComponentMenu(MENU_PATH + nameof(AudioEmitter), DEFAULT_ORDER)]
	public sealed class AudioEmitter : MonoBehaviour
	{
		[SerializeReferenceDropdown]
		[UnityEngine.SerializeReference]
		private IAudioClip _clip;

		[SerializeReferenceDropdown]
		[UnityEngine.SerializeReference]
		private IAudioSourceParameter[] _parameters;

		[SerializeField] private EmitterEvent _playEvent = EmitterEvent.OBJECT_START;
		[SerializeField] private EmitterEvent _stopEvent = EmitterEvent.NONE;
		[SerializeField] private bool _triggerOnce;

		private bool _isQuitting;
		private bool _hasTriggered;
		private IAudioSource _source;

		private void Awake()
		{
			Guard.AgainstNull(_clip, nameof(_clip));
			_source = GetComponent<IAudioSource>();
			Guard.AgainstNull(_source, nameof(_source));
			_parameters ??= Array.Empty<IAudioSourceParameter>();
		}

		private void Start() => HandleEvent(EmitterEvent.OBJECT_START);

		private void OnEnable() => HandleEvent(EmitterEvent.OBJECT_ENABLE);

		private void OnDisable() => HandleEvent(EmitterEvent.OBJECT_DISABLE);

		private void OnApplicationQuit() => _isQuitting = true;

		private void OnDestroy()
		{
			if (!_isQuitting)
			{
				HandleEvent(EmitterEvent.OBJECT_DESTROY);
			}
		}

		[UsedImplicitly] public bool IsPlaying => _source is { IsPlaying: true };

		[UsedImplicitly]
		public void Play()
		{
			if (!_triggerOnce || !_hasTriggered)
			{
				_source.Play(_clip, _parameters);
			}
		}

		[UsedImplicitly]
		public void Stop() => _source?.Stop();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void HandleEvent(EmitterEvent gameEvent)
		{
			if (_playEvent == gameEvent)
			{
				Play();
			}

			if (_stopEvent == gameEvent)
			{
				Stop();
			}
		}

		private enum EmitterEvent
		{
			[InspectorName("None")] NONE,
			[InspectorName("Object Start")] OBJECT_START,
			[InspectorName("Object Destroy")] OBJECT_DESTROY,
			[InspectorName("Object Enable")] OBJECT_ENABLE,
			[InspectorName("Object Disable")] OBJECT_DISABLE,
		}
	}
}