using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement; // Required for scene detection

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSounds, sfxSounds, uiSounds;
    public AudioSource musicSource, sfxSource, uiSource;

    private string[] currentPlaylist; // The active playlist
    private int currentMusicIndex = 0;
    private bool musicStopped = false;

    // Define different playlists for different maps
    private Dictionary<string, string[]> mapPlaylists = new Dictionary<string, string[]>()
    {
        { "Tutorial", new string[] { "tutorial", "eli" } },
        { "Alec Assasination", new string[] { "swamp", "boss" } },
        { "Alec Elimination", new string[] { "swamp", "eli" } },
        { "Forest_Assassination", new string[] { "mushroom", "boss" } },
        { "Forest_Hunt", new string[] { "mushroom", "hunt" } },
        { "JackyEli", new string[] { "ruins", "eli" } },
        { "JackyHunt", new string[] { "ruins", "hunt" } },
        { "LizMap_KillAll", new string[] { "village", "eli" } },
        { "LizMap_FindObject", new string[] { "village", "hunt" } },
        { "Hub", new string[] { "hub" } },
        { "MainMenu - UI", new string[] { "main1" } },
        { "Map - UI", new string[] { "main2" } },
        { "Options", new string[] { "hub" } }
    };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SetPlaylistForScene(SceneManager.GetActiveScene().name); // Set initial music
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if (!musicSource.isPlaying && !musicStopped && currentPlaylist.Length > 0)
        {
            PlayNextMusic();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene Loaded: {scene.name}");
        SetPlaylistForScene(scene.name);
    }

    private void SetPlaylistForScene(string sceneName)
    {
        if (mapPlaylists.ContainsKey(sceneName))
        {
            currentPlaylist = mapPlaylists[sceneName];
            currentMusicIndex = 0;
            Debug.Log($"🎵 Playlist set for scene: {sceneName}. Now playing: {currentPlaylist[currentMusicIndex]}");
            PlayMusic(currentPlaylist[currentMusicIndex]); // Play first song
        }
        else
        {
            Debug.Log($"❌ No specific playlist for scene: {sceneName}. Stopping music.");
            StopMusic();
        }
    }

    public void PlayNextMusic()
    {
        if (currentPlaylist.Length == 0) return;

        currentMusicIndex = (currentMusicIndex + 1) % currentPlaylist.Length;
        Debug.Log($"🎵 Switching to next track: {currentPlaylist[currentMusicIndex]}");
        PlayMusic(currentPlaylist[currentMusicIndex]);
    }

    public void PlayMusic(string name)
    {
        musicStopped = false;
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log($"❌ Sound Not Found: {name}");
        }
        else
        {
            if (musicSource.clip == s.clip && musicSource.isPlaying)
            {
                Debug.Log($"⚠️ Music '{name}' is already playing.");
                return; // Prevent restarting the same track
            }

            musicSource.clip = s.clip;
            musicSource.Play();
            Debug.Log($"🎵 Now Playing: {name}");
        }
    }

    public void StopMusic()
    {
        if (musicSource.isPlaying && !musicStopped)
        {
            musicStopped = true;
            musicSource.Stop();
            Debug.Log("🛑 Music Stopped.");
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log($"❌ SFX Sound Not Found: {name}");
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
            Debug.Log($"🔊 Playing SFX: {name}");
        }
    }

    public void PlayUI(string name)
    {
        Sound s = Array.Find(uiSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log($"❌ UI Sound Not Found: {name}");
        }
        else
        {
            uiSource.PlayOneShot(s.clip);
            Debug.Log($"🖱️ Playing UI Sound: {name}");
        }
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
        Debug.Log($"🎵 Music Muted: {musicSource.mute}");
    }

    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
        uiSource.mute = !uiSource.mute;
        Debug.Log($"🔊 SFX Muted: {sfxSource.mute}");
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
        Debug.Log($"🎚️ Music Volume Set To: {volume}");
    }

    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
        uiSource.volume = volume;
        Debug.Log($"🎚️ SFX Volume Set To: {volume}");
    }
}
