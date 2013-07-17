using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

// TODO Add severity (warning, error) to ValidationMessage
namespace Alba.Framework.Windows.Mvvm
{
    public struct ValidationMessage : IEquatable<ValidationMessage>
    {
        internal static readonly ReadOnlyCollection<ValidationMessage> EmptyCollection = new ReadOnlyCollection<ValidationMessage>(new List<ValidationMessage>(0));

        public string Message { get; set; }
        public int Code { get; set; }
        //public Severity Severity { get; set; }

        public bool Equals (ValidationMessage other)
        {
            return string.Equals(Message, other.Message) && Code == other.Code;
        }

        public override bool Equals (object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is ValidationMessage && Equals((ValidationMessage)obj);
        }

        public override int GetHashCode ()
        {
            return unchecked ((Message.GetHashCode() * 397) ^ Code);
        }
    }
}