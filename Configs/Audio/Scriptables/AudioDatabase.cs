using System.Collections.Generic;
using System.Linq;
using MergeIdle.Scripts.Configs.Audio.Data;
using MergeIdle.Scripts.Configs.Audio.Enums;
using UnityEngine;

namespace MergeIdle.Scripts.Configs.Audio.Scriptables
{
    [CreateAssetMenu(fileName = "AudioDatabase", menuName = "ScriptableObjects/Database/AudioDatabase")]
    public class AudioDatabase : ScriptableObject
    {
    
#pragma warning disable 649
        [SerializeField] private List<AudioItem> _audioItems;
#pragma warning restore 649
    
        public AudioItem GetByType(EAudio audio)
        {
            return _audioItems.First(x => x.AudioType == audio);
        }
    }
}
