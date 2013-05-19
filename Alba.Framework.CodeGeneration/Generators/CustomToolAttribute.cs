using System;

namespace Alba.Framework.CodeGeneration.Generators
{
    [AttributeUsage (AttributeTargets.Class, AllowMultiple = false)]
    public class CustomToolAttribute : Attribute
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        public CustomToolAttribute (string name, string description = "")
        {
            Name = name;
            Description = description;
        }
    }
}