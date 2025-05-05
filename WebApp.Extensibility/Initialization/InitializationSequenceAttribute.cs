namespace WebApp.Extensibility.Initialization
{
    using System;

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class InitializationSequenceAttribute : Attribute
    {
        public InitializationSequenceAttribute(int sequence)
        {
            Sequence = sequence;
        }
        public int Sequence { get; }
    }
}
