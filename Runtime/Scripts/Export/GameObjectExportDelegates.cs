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
using UnityEngine;

namespace GLTFast.Export {
    /// <summary>
    /// Encapsulate delegates invoked by GameObjectExport. Typical use case is to collect
    /// information that will be later processed by ExportDelegates
    /// </summary>
    public class GameObjectExportDelegates {
        /// <summary>
        /// Invoked at the start of the export process.
        /// </summary>
        public delegate void Reset();
        
        /// <summary>
        /// Invoked when a game object is processed. Invokee are responsible to collect
        /// relevant information to be later processed and added to a GLTFast.Schema.Root
        /// <param name="gameObject">Game object to process</param> 
        /// </summary>
        public delegate void GameObjectAdded(GameObject gameObject, int nodeId);
        
        /// <summary>
        /// Invoked when a scene is processed. Invokee are responsible to collect
        /// relevant information to be later processed and added to a GLTFast.Schema.Root
        /// <param name="name">Scene name added</param> 
        /// </summary>
        public delegate void SceneAdded(string name);

        public Reset reset;
        public GameObjectAdded gameObjectAdded;
        public SceneAdded sceneAdded;
    }
}
