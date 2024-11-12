using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

//Authors Matt Smith & Shaun Ferns

[RequireComponent(typeof(VideoPlayer))] //this calls the basic video instructions for handling video
[RequireComponent(typeof(AudioSource))] //this calls the basic instruction for handling audio

public class PlayVideo : MonoBehaviour
{
    public VideoClip videoClip; //create a public var of videoClip to handle video stream

    private VideoPlayer videoPlayer; //create a private Video Player Var that instantiates video player
    private AudioSource audioSource; //create a private AudioSource var that instantiates audio stream

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>(); //Upon start get the video Player component
        audioSource = GetComponent<AudioSource>(); //Upon start get the audio player component

        // disable Play on Awake for both vide and audio
        videoPlayer.playOnAwake = true;
        //audioSource.playOnAwake = false;

        // assign video clip
        videoPlayer.source = VideoSource.VideoClip;
        videoPlayer.clip = videoClip;

        // setup AudioSource 
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.SetTargetAudioSource(0, audioSource);

        // render video to main texture of assigned GameObject
        videoPlayer.renderMode = VideoRenderMode.MaterialOverride;
        videoPlayer.targetMaterialRenderer = GetComponent<Renderer>();
        videoPlayer.targetMaterialProperty = "_MainTex";

        //apparently methods running on events requires a += if you subscribe to them
        videoPlayer.loopPointReached += OnVideoFinished;

    }

    void Update()
    {
        // space bar to start / pause
        if (Input.GetButtonDown("Jump"))//looks for a press on the spacebar to either play or pause on the update
            OnVideoFinished(videoPlayer);
    }

    //this method looks to see if video is playing or paused and then starts the opposite
    //private void PlayPause()
    //{
    //    if (videoPlayer.isPlaying)//
    //        videoPlayer.Pause();
    //    else
    //    {
    //        videoPlayer.Play();
    //    }
    //}

    private void OnVideoFinished(VideoPlayer vp)
    {
        // Load the next scene (replace "NextScene" with your actual scene name)
        SceneManager.LoadScene("GameScene");
    }
}