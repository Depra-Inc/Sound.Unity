// SPDX-License-Identifier: Apache-2.0
// © 2024-2025 Depra <n.melnikov@depra.org>

using System;
using Depra.SerializeReference.Extensions;
using Depra.Sound.Exceptions;
using JetBrains.Annotations;
using UnityEngine;
using static Depra.Sound.Module;

namespace Depra.Sound.Source
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

		private IAudioSource _source;

		private void Awake()
		{
			Guard.AgainstNull(_clip, nameof(_clip));
			_source = GetComponent<IAudioSource>();
			Guard.AgainstNull(_source, nameof(_source));
			_parameters ??= Array.Empty<IAudioSourceParameter>();
		}

		[UsedImplicitly]
		public void Play() => _source.Play(_clip, _parameters);
	}
}