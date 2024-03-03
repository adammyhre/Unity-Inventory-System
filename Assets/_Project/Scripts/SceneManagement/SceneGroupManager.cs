using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Systems.SceneManagement {
    public class SceneGroupManager {
        public event Action<string> OnSceneLoaded = delegate { };
        public event Action<string> OnSceneUnloaded = delegate { };
        public event Action OnSceneGroupLoaded = delegate { };
        
        readonly AsyncOperationHandleGroup handleGroup = new AsyncOperationHandleGroup(10);
        
        SceneGroup ActiveSceneGroup;
        
        public async Task LoadScenes(SceneGroup group, IProgress<float> progress, bool reloadDupScenes = false) { 
            ActiveSceneGroup = group;
            var loadedScenes = new List<string>();

            await UnloadScenes();

            int sceneCount = SceneManager.sceneCount;
            
            for (var i = 0; i < sceneCount; i++) {
                loadedScenes.Add(SceneManager.GetSceneAt(i).name);
            }

            var totalScenesToLoad = ActiveSceneGroup.Scenes.Count;

            var operationGroup = new AsyncOperationGroup(totalScenesToLoad);

            for (var i = 0; i < totalScenesToLoad; i++) {
                var sceneData = group.Scenes[i];
                if (reloadDupScenes == false && loadedScenes.Contains(sceneData.Name)) continue;
                
                if (sceneData.Reference.State == SceneReferenceState.Regular)
                {
                    var operation = SceneManager.LoadSceneAsync(sceneData.Reference.Path, LoadSceneMode.Additive);
                    operationGroup.Operations.Add(operation);
                }
                else if (sceneData.Reference.State == SceneReferenceState.Addressable)
                {
                    var sceneHandle = Addressables.LoadSceneAsync(sceneData.Reference.Path, LoadSceneMode.Additive);
                    handleGroup.Handles.Add(sceneHandle);
                }
                
                OnSceneLoaded.Invoke(sceneData.Name);
            }
            
            // Wait until all AsyncOperations in the group are done
            while (!operationGroup.IsDone || !handleGroup.IsDone) {
                progress?.Report((operationGroup.Progress + handleGroup.Progress) / 2);
                await Task.Delay(100);
            }

            Scene activeScene = SceneManager.GetSceneByName(ActiveSceneGroup.FindSceneNameByType(SceneType.ActiveScene));

            if (activeScene.IsValid()) {
                SceneManager.SetActiveScene(activeScene);
            }

            OnSceneGroupLoaded.Invoke();
        }

        public async Task UnloadScenes() { 
            var scenes = new List<string>();
            var activeScene = SceneManager.GetActiveScene().name;
            
            int sceneCount = SceneManager.sceneCount;

            for (var i = sceneCount - 1; i > 0; i--) {
                var sceneAt = SceneManager.GetSceneAt(i);
                if (!sceneAt.isLoaded) continue;
                
                var sceneName = sceneAt.name;
                if (sceneName.Equals(activeScene) || sceneName == "Bootstrapper") continue;
                if (handleGroup.Handles.Any(h => h.IsValid() && h.Result.Scene.name == sceneName)) continue;
                
                scenes.Add(sceneName);
            }
            
            // Create an AsyncOperationGroup
            var operationGroup = new AsyncOperationGroup(scenes.Count);
            
            foreach (var scene in scenes) { 
                var operation = SceneManager.UnloadSceneAsync(scene);
                if (operation == null) continue;
                
                operationGroup.Operations.Add(operation);

                OnSceneUnloaded.Invoke(scene);
            }
            
            foreach (var handle in handleGroup.Handles) {
                if (handle.IsValid()) {
                    Addressables.UnloadSceneAsync(handle);
                }
            }
            handleGroup.Handles.Clear();

            // Wait until all AsyncOperations in the group are done
            while (!operationGroup.IsDone) {
                await Task.Delay(100); // delay to avoid tight loop
            }
            
            // Optional: UnloadUnusedAssets - unloads all unused assets from memory
            await Resources.UnloadUnusedAssets();
        }
    }
    
    public readonly struct AsyncOperationGroup { 
        public readonly List<AsyncOperation> Operations;

        public float Progress => Operations.Count == 0 ? 0 : Operations.Average(o => o.progress);
        public bool IsDone => Operations.All(o => o.isDone);

        public AsyncOperationGroup(int initialCapacity) {
            Operations = new List<AsyncOperation>(initialCapacity);
        }
    }

    public readonly struct AsyncOperationHandleGroup {
        public readonly List<AsyncOperationHandle<SceneInstance>> Handles;
        
        public float Progress => Handles.Count == 0 ? 0 : Handles.Average(h => h.PercentComplete);
        public bool IsDone => Handles.Count == 0 || Handles.All(o => o.IsDone);

        public AsyncOperationHandleGroup(int initialCapacity) {
            Handles = new List<AsyncOperationHandle<SceneInstance>>(initialCapacity);
        }
    }
}