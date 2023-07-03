using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using Random = UnityEngine.Random;
#if UNITY_EDITOR
using System.Linq;
using System.IO;
using UnityEditor;
#endif

namespace Modules.LevelSpawner
{
    public enum OverfillStrategy
    {
        Loop,
        PureRandom,
        Shuffled
    }

    [CreateAssetMenu(menuName = "Modules/LevelSpawner/LevelsCollection")]
    public class LevelsCollection : ScriptableObject
    {
        [SerializeField] private OverfillStrategy _overfillStrategy;
        [SerializeField] private int _shuffleStartId;
        [SerializeField] public Level[] Levels;
        private Data.RandomIDShuffle randomIDShuffle;


        public void Init()
        {
            if(Levels.Length - _shuffleStartId <= 0)
            {
                _shuffleStartId = 0;
            }

            randomIDShuffle = new Data.RandomIDShuffle("levels_collection_a", Levels.Length-_shuffleStartId);
            
        }

        public string Get(int id, out bool mirrored) 
        {
            switch (_overfillStrategy)
            {
                case OverfillStrategy.Shuffled:
                    return GetWithShuffle(id, out mirrored);
                case OverfillStrategy.PureRandom:
                    return GetWithRandomOverFill(id, out mirrored);
                case OverfillStrategy.Loop:
                    mirrored = Levels[id % Levels.Length].Mirrored;
                    return Levels[id % Levels.Length].Id;
                default:
                    mirrored = Levels[id % Levels.Length].Mirrored;
                    return Levels[id % Levels.Length].Id;
            }
        }

        // if id > levels return with id else return random
        public string GetWithRandomOverFill(int id, out bool mirrored)
        {
            if(id < Levels.Length)
            {
                mirrored = Levels[id % Levels.Length].Mirrored;
                return Levels[id % Levels.Length].Id;
            }
            else
            {
                mirrored = Levels[Random.Range(0, Levels.Length)].Mirrored;
                return Levels[Random.Range(0, Levels.Length)].Id;
            }
        }

        public string GetWithShuffle(int id, out bool mirrored)
        {
            if(id < Levels.Length)
            {
                mirrored = Levels[id % Levels.Length].Mirrored;
                return Levels[id%Levels.Length].Id;
            }
            else
            {
                if(id >= _shuffleStartId)
                {
                    id -= _shuffleStartId;
                }

                int lvlIdx = (randomIDShuffle.Get(id) + _shuffleStartId) % Levels.Length;
                mirrored = Levels[lvlIdx].Mirrored;
                return Levels[lvlIdx].Id;
            }
        }

        public string GetRandom() 
        {
            return Levels[Random.Range(0, Levels.Length)].Id;
        }

#if UNITY_EDITOR
        
        [ContextMenu("Collect")]
        public void Collect() 
        {
            // get directory with config
            string targetDir = Path.GetDirectoryName(AssetDatabase.GetAssetPath(this));

            // collect assets at directory
            List<GameObject> levels = new List<GameObject>();
            foreach (var guid in AssetDatabase.FindAssets("", new[] { targetDir }))
            {
                GameObject go = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(GameObject)) as GameObject;
                if (go != null)
                {
                    levels.Add(go);
                }

            }

            // sort by name
            // add length at start of name to correctly sort strings with different lengths
            levels.OrderBy(level => level.name.Length.ToString() + level.name);
            Levels = new Level[levels.Count];
            for (int i = 0; i < levels.Count; i++)
            {
                Levels[i].Id = levels[i].name;
            }
            UnityEditor.EditorUtility.DisplayDialog("Levels collection", $"Collected and sorted by name {levels.Count} levels", "OK");
        }

#endif
    }
    
    [Serializable]
    public struct Level
    {
        public string Id;
        public bool Mirrored;
    }
}
