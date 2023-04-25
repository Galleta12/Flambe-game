using UnityEngine;

// namespace src
// {
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {
        private static AudioClip _jump;
        private static AudioClip _run;
        private static AudioClip _swing;
        private static AudioClip _slash;
        private static AudioClip _roll;
        private static AudioClip _windDeath;
        private static AudioClip _impact;
        private static AudioClip _deflect;
        private static AudioClip _healing;



      

        private static AudioSource _audioSrc;

        public static AudioSource AudioForWalk{get{return _audioSrc;}set{_audioSrc = value;}}

        

        public static void PlaySound (string clip)
        {
            switch(clip)
            {
                case "jump":
                    _audioSrc.PlayOneShot(_jump);
                    break;
                case "run":
                    _audioSrc.PlayOneShot(_run);
                    break;
                case "roll":
                    _audioSrc.PlayOneShot(_roll);
                    break;
                case "swing":
                    _audioSrc.PlayOneShot(_swing);
                    break;
                case "slash":
                    _audioSrc.PlayOneShot(_slash);
                    break;   
                case "windDeath":
                    _audioSrc.PlayOneShot(_windDeath);
                    break;   
                case "impact":
                    _audioSrc.PlayOneShot(_impact);
                    break;   
                case "deflect":
                    _audioSrc.PlayOneShot(_deflect);
                    break;   
                case "healing":
                    _audioSrc.PlayOneShot(_healing);
                    break;   
              

                default:
                    Debug.LogError($"Tried to play sound \"{clip}\" but it wasn't available");
                    break;
            }
        }

        
        private void Start()
        {
            _jump  = Resources.Load<AudioClip> ("sfx/jumpTest");
            _swing = Resources.Load<AudioClip> ("sfx/swingTest");
            _slash = Resources.Load<AudioClip> ("sfx/slashTest");
            _run   = Resources.Load<AudioClip> ("sfx/runTest");
            _roll   = Resources.Load<AudioClip> ("sfx/rollTest");
            _windDeath   = Resources.Load<AudioClip> ("sfx/windDeathTest");
            _impact   = Resources.Load<AudioClip> ("sfx/impact");
            _deflect   = Resources.Load<AudioClip> ("sfx/deflect");
            _healing   = Resources.Load<AudioClip> ("sfx/healingSound");


            _audioSrc  = GetComponent<AudioSource> ();
            

            //for the walk
           

        }
    }
//}

