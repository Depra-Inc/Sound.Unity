using System;
using UnityEngine;

namespace Depra.Sound.Parameter
{
	[Serializable]
	public struct PositionParameter : IAudioClipParameter
	{
		public Vector3 Value;

		public PositionParameter(Vector3 value) => Value = value;
	}
}