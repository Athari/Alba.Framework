﻿using System;
using System.Runtime.Serialization;
using Alba.Framework.Text;

namespace Alba.Framework.Serialization
{
    public static class SerializationBinderExts
    {
        public static string BindToName (this SerializationBinder @this, Type serializedType)
        {
            string assemblyName;
            string typeName;
            @this.BindToName(serializedType, out assemblyName, out typeName);
            return assemblyName.IsNullOrEmpty() ? typeName : string.Format("{0}, {1}", typeName, assemblyName);
        }

        public static Type BindToType (this SerializationBinder @this, string typeName)
        {
            return @this.BindToType("", typeName);
        }
    }
}