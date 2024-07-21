namespace Depra.Sound.Configuration
{
	public readonly struct SegmentedAudioTrack : IAudioTrack
	{
		private readonly AudioTrackSegment[] _segments;

		public SegmentedAudioTrack(params AudioTrackSegment[] segments) => _segments = segments;

		void IAudioTrack.Play(IAudioSource source)
		{
			foreach (var segment in _segments)
			{
				source.Play(segment.Clip, segment.Parameters);
			}
		}

		void IAudioTrack.Deconstruct(out AudioTrackSegment[] segments) => segments = _segments;
	}
}