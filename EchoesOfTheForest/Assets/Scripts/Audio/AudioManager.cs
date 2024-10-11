using UnityEngine;

namespace AdvancedHorrorFPS
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;
        public AudioClip[] Door_Wooden_Open;
        public AudioClip[] Door_Close;
        public AudioClip[] PlayerShock;
        public AudioClip Door_TryOpen;
        public AudioClip Door_UnLock;
        public AudioClip Item_Grab;
        public AudioClip Flashlight;
        public AudioClip Audio_Breathing;
        // public AudioClip Audio_TreasureOpen;
        // public AudioClip Audio_BaseBallHit;
        public AudioClip Note_Reading;
        public AudioClip Item_Close;
        public AudioClip Audio_Jump;
        // public AudioClip Audio_PadlockTry;
        public AudioClip Audio_Cabinet_Open;
        // public AudioClip Audio_Drawer_Open;
        public AudioClip[] Audio_DemonKilling;
        public AudioClip Audio_Hide;
        public AudioClip[] Audio_WoodBreakable;
        public AudioSource audioSource;
        public AudioSource audioSourceWalk;
        // public AudioClip Audio_Reload;
        // public AudioClip Audio_PistolFire;
        public AudioClip Audio_Healing;
        // public AudioClip Audio_PistolEmpty;
        public AudioClip Audio_StaminaBreathing; // Asegúrate de tener esta línea para el AudioClip de respiración pesada
        public AudioSource audioSourceBreathing;  // Nuevo AudioSource para la respiración

        [Header("Inventory Sound Effects")]
        public AudioClip Audio_GoLeftRightButton;
        public AudioClip Audio_Inventory_Select;

        [Header("Press and Hold Sound Effects")]
        public AudioClip Audio_PressAndHoldMaintainDone;

        [Header("Footstep Sounds")]
        public AudioClip[] PlayerFootstepSounds; // Añadir aquí tus 5 clips de pasos
        public AudioClip[] PlayerSprintSounds; // Añade un array para los clips de sprint

        private void Awake()
        {
            Instance = this;
        }

        public void Play_Audio_StaminaBreathing()
        {
            if (!audioSourceBreathing.isPlaying)  // Verifica si ya se está reproduciendo la respiración
            {
            audioSourceBreathing.PlayOneShot(Audio_StaminaBreathing);  
            }
        }

        void Start()
        {
            audioSource = GetComponent<AudioSource>();     
            audioSourceBreathing = gameObject.AddComponent<AudioSource>(); // Añade un nuevo AudioSource para la respiración

        }

        public void Play_Audio_GoLeftRightButton()
        {
            audioSource.PlayOneShot(Audio_GoLeftRightButton);
        }

        public void Play_Audio_PressAndHoldMaintainDone()
        {
            audioSource.PlayOneShot(Audio_PressAndHoldMaintainDone);
        }

        public void Play_Audio_Inventory_Select()
        {
            audioSource.PlayOneShot(Audio_Inventory_Select);
        }

        public void Play_Jump()
        {
            audioSource.PlayOneShot(Audio_Jump);
        }
        

        // public void Play_Audio_Reload()
        // {
        //     audioSource.PlayOneShot(Audio_Reload);
        // }

        // public void Play_Audio_PistolFire()
        // {
        //     audioSource.PlayOneShot(Audio_PistolFire);
        // }

        public void Play_Audio_Healing()
        {
            audioSource.PlayOneShot(Audio_Healing);
        }

        public void Play_Audio_Hide()
        {
            audioSource.PlayOneShot(Audio_Hide);
        }

        // public void Play_Audio_PistolEmpty()
        // {
        //     audioSource.PlayOneShot(Audio_PistolEmpty);
        // }

        

        public void Play_WoodBreakable()
        {
            audioSource.PlayOneShot(Audio_WoodBreakable[UnityEngine.Random.Range(0, Audio_WoodBreakable.Length)]);
        }

        public void Play_PlayerShock()
        {
            audioSource.PlayOneShot(PlayerShock[UnityEngine.Random.Range(0, PlayerShock.Length)]);
        }

        // public void Play_PadlockTry()
        // {
        //     audioSource.PlayOneShot(Audio_PadlockTry);
        // }

        // public void Play_TreasureOpen()
        // {
        //     audioSource.PlayOneShot(Audio_TreasureOpen);
        // }

        // public void Play_Audio_BaseBallHit()
        // {
        //     audioSource.PlayOneShot(Audio_BaseBallHit);
        // }

        public void Play_Audio_Cabinet_Open()
        {
            audioSource.PlayOneShot(Audio_Cabinet_Open);
        }

        // public void Play_Audio_Drawer_Open()
        // {
        //     audioSource.PlayOneShot(Audio_Drawer_Open);
        // }
        

        public void Play_Door_Wooden_Open()
        {
            audioSource.PlayOneShot(Door_Wooden_Open[UnityEngine.Random.Range(0, Door_Wooden_Open.Length)]);
        }

        public void Play_Audio_Breathing()
        {
            audioSource.PlayOneShot(Audio_Breathing);
        }

        public void Play_Door_Close()
        {
            audioSource.PlayOneShot(Door_Close[UnityEngine.Random.Range(0, Door_Close.Length)]);
        }

        public void Play_Note_Reading()
        {
            audioSource.PlayOneShot(Note_Reading);
        }

        public void Play_Item_Close()
        {
            audioSource.PlayOneShot(Item_Close);
        }

        public void Play_Door_UnLock()
        {
            audioSource.PlayOneShot(Door_UnLock);
        }

        public void Play_Item_Grab()
        {
            audioSource.PlayOneShot(Item_Grab);
        }

        public void Play_Flashlight_Open()
        {
            audioSource.PlayOneShot(Flashlight);
        }

        public void Play_Door_TryOpen()
        {
            audioSource.PlayOneShot(Door_TryOpen);
        }

        public void Play_Flashlight_Close()
        {
            audioSource.PlayOneShot(Flashlight);
        }


        // Modificación en Play_Player_Walk
        public void Play_Player_Walk()
        {
            if (Time.time > LastTimeWalkSound + WalkSoundPeriod)
            {
                audioSourceWalk.pitch = UnityEngine.Random.Range(1, 1.5f);

                // Reproducir un clip aleatorio de los pasos
                int randomIndex = UnityEngine.Random.Range(0, PlayerFootstepSounds.Length);
                audioSourceWalk.PlayOneShot(PlayerFootstepSounds[randomIndex]);

                LastTimeWalkSound = Time.time;
                WalkSoundPeriod = UnityEngine.Random.Range(0.4f, 0.75f); // Ajusta los valores según el ritmo del paso
            }
        }

        // Método para reproducir sonidos de sprint
        public void Play_Player_Sprint()
        {
            if (Time.time > LastTimeSprintSound + SprintSoundPeriod)
            {
                audioSourceWalk.pitch = UnityEngine.Random.Range(1.2f, 1.6f); // Ajusta el pitch para sprint

                // Reproducir un clip aleatorio de los sonidos de sprint
                int randomIndex = UnityEngine.Random.Range(0, PlayerSprintSounds.Length);
                audioSourceWalk.PlayOneShot(PlayerSprintSounds[randomIndex]);

                LastTimeSprintSound = Time.time;
                SprintSoundPeriod = UnityEngine.Random.Range(0.3f, 0.6f); // Ritmo más rápido que el de caminar
            }
        }

        private float LastTimeSprintSound = 0;
        private float SprintSoundPeriod = 0.4f;

        private float LastTimeWalkSound = 0;
        private float WalkSoundPeriod = 0.5f;

        public void Play_DemonKilling()
        {
            audioSource.PlayOneShot(Audio_DemonKilling[UnityEngine.Random.Range(0, Audio_DemonKilling.Length)]);
        }
    }
}