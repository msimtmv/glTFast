// Copyright 2020-2022 Andreas Atteneder
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using GLTFast.Schema;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GLTFast
{
    /// <summary>
    /// Interface between parsers and custom extension private data
    /// </summary>
    public interface ICustomExtensionParser
    {
        object Deserialize(IGltfJsonToken token) => null;
    }
    
    /// <summary>
    /// Convenience class to wrap objects that use Serializable and SerializeField
    /// for parsing 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SimpleExtensionParser<T> : ICustomExtensionParser
    {
        public object Deserialize(IGltfJsonToken token) => token.ToObject<T>();
    }
    
    /// <summary>
    /// Custom extension data registry. Any type extending ExtensibleObject can dynamically
    /// add custom data. Parsing will create proper instance in ExtensibleObject.genericProperties.
    ///
    /// Ex)
    /// Code:
    ///     [Serializable]
    ///     class MyCustomRootExtension
    ///     {
    ///         public int data;
    ///     }
    /// 
    ///     CustomExtensionRegistry.RegisterSimple<RootExtension, MyCustomRootExtension>("UNITY_my_custom_rootExtension")
    ///
    /// Data:
    ///     {
    ///         "asset":{
    ///             "version":"2.0",
    ///             "generator":"Unity 2021.3.4f1 glTFast 4.8.5"
    ///          },
    ///          "extensions":{
    ///             "UNITY_my_custom_rootExtension":{
    ///             "data": 1
    ///          }
    ///     }
    ///
    ///  GltfImport.GetSourceRoot().extensions.genericProperties["UNITY_my_custom_rootExtension"] is MyCustomRootExtension {data = 1} 
    /// </summary>
    public static class CustomExtensionRegistry
    {
        static Dictionary<(Type extensible, string extensionName), ICustomExtensionParser> s_CustomExtensions;
        
        public static bool RegisterParser<T1, T2>(string extensionName)
            where T1: ExtensibleObject
            where T2: ICustomExtensionParser, new()
        {
            if (string.IsNullOrEmpty(extensionName))
                return false;

            s_CustomExtensions ??= new Dictionary<(Type, string), ICustomExtensionParser>();
            return s_CustomExtensions.TryAdd((typeof(T1), extensionName), new T2());
        }

        public static bool RegisterSimpleParser<T1, T2>(string extensionName) where T1 : ExtensibleObject
        {
            return RegisterParser<T1, SimpleExtensionParser<T2>>(extensionName);
        }

        public static bool UnregisterParser<T>(string extensionName) where T: ExtensibleObject
        {
            return !string.IsNullOrEmpty(extensionName)
                && s_CustomExtensions != null 
                && s_CustomExtensions.Remove((typeof(T), extensionName));
        }

        public static bool IsRegistered<T>(string extensionName) where T: ExtensibleObject
        {
            return !string.IsNullOrEmpty(extensionName)
                && s_CustomExtensions != null
                && s_CustomExtensions.ContainsKey((typeof(T), extensionName));
        }

        internal static ICustomExtensionParser GetParser(Type extensible, string extensionName)
        {
            if (s_CustomExtensions == null)
                return null;

            if (s_CustomExtensions.TryGetValue((extensible, extensionName), out var valueParser))
                return valueParser;

            return null;
        }
    }
}