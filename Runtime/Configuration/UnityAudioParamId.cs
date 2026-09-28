namespace Depra.Sound.Configuration
{
	public readonly struct UnityAudioParamId
	{
		public static readonly AudioParamId Position = new(101);
		public static readonly AudioParamId Transform = new(102);
		public static readonly AudioParamId LabeledInt = new(103);
		public static readonly AudioParamId LabeledFloat = new(104);
		public static readonly AudioParamId LabeledString = new(105);
	}
}