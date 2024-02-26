using System.Collections.Generic;
using UnityEngine;

namespace General
{
    public class MusicPlayer : MonoBehaviour
    {
        [SerializeField] private List<Song> _songs = new List<Song>();
        [SerializeField] private AudioSource _audioSource;

        public void Construct(List<Song> songs)
        {
            _songs = songs;
        }
        
        public void ChooseRandomAudioSource()
        {
            var song = _songs[Random.Range(0, _songs.Count)];
            _audioSource.clip = song.Clip;
            _audioSource.pitch = song.Pitch;
            _audioSource.priority = song.Priority;
            _audioSource.volume = song.Volume;
            _audioSource.Play();
        }
    }
}