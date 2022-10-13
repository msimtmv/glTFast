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

using System.Collections.Generic;
using System.Threading.Tasks;
using GLTFast.Schema;
using UnityEngine;

namespace GLTFast.Export {
    
    /// <summary>
    /// glTF format
    /// </summary>
    public enum GltfFormat {
        /// <summary>
        /// JSON-based glTF (.gltf file extension)
        /// </summary>
        Json,
        /// <summary>
        /// glTF-binary (.glb file extension)
        /// </summary>
        Binary
    }
    
    /// <summary>
    /// Destination for image files
    /// </summary>
    public enum ImageDestination {
        /// <summary>
        /// Automatic decision. Main buffer for glTF-binary, separate files for JSON-based glTFs.
        /// </summary>
        Automatic,
        /// <summary>
        /// Embeds images in main buffer
        /// </summary>
        MainBuffer,
        /// <summary>
        /// Saves images as separate files relative to glTF file
        /// </summary>
        SeparateFile
    }

    /// <summary>
    /// Resolutions to existing file conflicts
    /// </summary>
    public enum FileConflictResolution {
        /// <summary>
        /// Abort and keep existing files
        /// </summary>
        Abort,
        /// <summary>
        /// Replace existing files with newly created ones
        /// </summary>
        Overwrite
    }
    
    /// <summary>
    /// glTF export settings
    /// </summary>
    public class ExportSettings {
        /// <summary>
        /// Export to JSON-based or binary format glTF files
        /// </summary>
        public GltfFormat format = GltfFormat.Json;
        
        /// <inheritdoc cref="ImageDestination"/>
        public ImageDestination imageDestination = ImageDestination.Automatic;
        
        /// <inheritdoc cref="FileConflictResolution"/>
        public FileConflictResolution fileConflictResolution = FileConflictResolution.Abort;
        
        /// <summary>
        /// Light intensity values are multiplied by this factor.
        /// </summary>
        [Tooltip("Light intensity values are multiplied by this factor")]
        public float lightIntensityFactor = 1.0f;
    }

    /// <summary>
    /// Encapsulate delegates invoked by GameObjectExport and IGltfWritable sub classes.
    /// </summary>
    public static class ExportDelegates {
        /// <summary>
        /// Invoked at the start of the export process.
        /// <param name="gltf">Gltf object to store/retrieve info from</param> 
        /// </summary>
        public delegate void ExportInstanceCreated(IGltfWritable gltf);
        
        /// <summary>
        /// Invoked when a game object is processed. Invokee are responsible to collect
        /// relevant information to be later processed and added to a GLTFast.Schema.Root
        /// <param name="gltf">Gltf object to store/retrieve info from</param>
        /// <param name="gameObject">Game object to process</param>
        /// <param name="nodeId">gltf node to which this game object is tied to</param> 
        /// </summary>
        public delegate void GameObjectAdded(IGltfWritable gltf, GameObject gameObject, int nodeId);
        
        /// <summary>
        /// Invoked when a scene is processed. Invokee are responsible to collect
        /// relevant information to be later processed and added to a GLTFast.Schema.Root
        /// <param name="gltf">Gltf object to store/retrieve info from</param>
        /// <param name="name">Name of the added scene</param> 
        /// </summary>
        public delegate void SceneAdded(IGltfWritable gltf, string name);
        
        /// <summary>
        /// Invoked when gathered information needs long standing processing. For example, compressing data
        /// <param name="gltf">Gltf object to store/retrieve info from</param>
        /// <param name="directory"></param>
        /// <param name="tasks">Add created tasks to this list so export process can be synchronized</param>
        /// </summary>
        public delegate void Bake(IGltfWritable gltfWritable, string directory, List<Task<bool>> tasks);

        /// <summary>
        /// Invoked just before gltf is written to disk. Typical use case is to update relevant members
        /// of GLTFast.Schema.Root 
        /// <param name="gltf">Gltf object to store/retrieve info from</param>
        /// </summary>
        public delegate void Update(IGltfWritable gltfWritable);
        
        /// <summary>
        /// Invoked when export process is done.
        /// <param name="gltf">Gltf object to store/retrieve info from</param>
        /// </summary>
        public delegate void Disposing(IGltfWritable gltfWritable);

        public static ExportInstanceCreated exportInstanceCreated;
        public static GameObjectAdded gameObjectAdded;
        public static SceneAdded sceneAdded;
        public static Bake bake;
        public static Update update;
        public static Disposing disposing;
    }
}
