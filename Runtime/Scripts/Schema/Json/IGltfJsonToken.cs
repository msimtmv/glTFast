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

using System;

namespace GLTFast.Schema
{
    /// <summary>
    /// Generic object access and parsing. It is expected that
    /// custom extensions register their own type via CustomExtensionRegistry
    /// </summary>
    public interface IGltfJsonToken
    {
        /// <summary>
        /// Deserialisation
        /// </summary>
        /// <typeparam name="T">Type of object to deserialize</typeparam>
        /// <returns></returns>
        T ToObject<T>();
        
        /// <summary>
        /// Generic value accessor
        ///  
        /// string json = @"{
        ///     'propName1': {
        ///         'propName2': 'propName2 value'
        ///     }
        /// }
        ///
        /// assuming token is initialized from the content above
        /// token["propName1"]["propName2"].ToObject<string>()
        /// </summary>
        IGltfJsonToken this[object key] { get; }

        string ToString();
    }
}
