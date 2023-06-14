using System;
using MergeIdle.Scripts.Configs.Audio.Enums;
using UnityEngine;

namespace MergeIdle.Scripts.Configs.Audio.Data
{
    [Serializable]
    public class AudioItem
    {
        public EAudio AudioType;
        public AudioClip AudioClip;
    }
}