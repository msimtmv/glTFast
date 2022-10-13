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
    /// Parsing gltf requires the use of dynamic object to be able to support any extensions. That is, custom properties
    /// type at runtime. CustomPropertyRegistry enables ExtensibleObject subclasses to do so. 
    ///
    /// Ex)
    ///     var data = @"{
    ///         "asset":{
    ///             "version":"2.0",
    ///             "generator":"Unity 2021.3.4f1 glTFast 4.8.5"
    ///          },
    ///          "extensions":{
    ///             "UNITY_my_custom_rootExtension":{
    ///                 "data": 1
    ///             }
    ///          }
    ///     }"
    /// 
    ///     [Serializable]
    ///     class MyCustomRootExtension
    ///     {
    ///         public int data;
    ///     }
    ///
    ///     // done once at app init
    ///     CustomPropertyRegistry.Register<RootExtension, MyCustomRootExtension>("UNITY_my_custom_rootExtension")
    ///     
    ///     using var gltf = new GltfImport();
    ///     await gltf.Load(data, settings);
    ///     var customData = (MyCustomRootExtension) gltf.GetSourceRoot().extensions.genericProperties["UNITY_my_custom_rootExtension"];
    ///     // customData.data == 1
    /// </summary>
    public static class CustomPropertyRegistry
    {
        static Dictionary<(Type extensible, string propertyName), Type /*propertyType*/> s_CustomProperties;
        
        public static bool Register<T1, T2>(string propertyName) where T1: ExtensibleObject
        {
            if (string.IsNullOrEmpty(propertyName))
                return false;

            s_CustomProperties ??= new Dictionary<(Type, string), Type>();
            return s_CustomProperties.TryAdd((typeof(T1), propertyName), typeof(T2));
        }

        public static bool Unregister<T>(string propertyName) where T: ExtensibleObject
        {
            return !string.IsNullOrEmpty(propertyName)
                && s_CustomProperties != null 
                && s_CustomProperties.Remove((typeof(T), propertyName));
        }

        public static bool IsRegistered<T>(string propertyName) where T: ExtensibleObject
        {
            return !string.IsNullOrEmpty(propertyName)
                && s_CustomProperties != null
                && s_CustomProperties.ContainsKey((typeof(T), propertyName));
        }

        internal static Type GetPropertyType(Type extensible, string propertyName)
        {
            if (s_CustomProperties == null)
                return null;

            if (s_CustomProperties.TryGetValue((extensible, propertyName), out var propertyType))
                return propertyType;

            return null;
        }
    }
}