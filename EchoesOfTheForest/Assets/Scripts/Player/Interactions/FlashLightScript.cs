using UnityEngine;

namespace AdvancedHorrorFPS
{
    public class FlashLightScript : MonoBehaviour
{
    public static FlashLightScript Instance;
    public bool isGrabbed = false;
    public Light Light;
    public float BlueBattery = 100;
    public float DamageRate = 0.25f;
    public float BatterySpendNumber = 1;
    RaycastHit hit;
    public AudioSource audioSource;
    public Transform aimPoint;
    public LayerMask layerMask;
    private bool isOn = false;

    // Intensidades de la luz
    public float normalLightIntensity = 20f;
    public float uvLightIntensity = 20f;
    private bool isUVMode = false;  // Para controlar si la luz UV está activa

    void Awake()
    {
        Instance = this;
    }

public void FlashLight_Decision(bool decision)
{
    Light.enabled = decision;

    if (decision)  // Si la linterna está encendida
    {
        if (isUVMode)
        {
            Light.intensity = uvLightIntensity; // Ajusta la intensidad a la de la luz UV
        }
        else
        {
            Light.intensity = normalLightIntensity; // Ajusta la intensidad a la luz normal
        }
    }
}

    private void Start()
    {
        Light = GetComponent<Light>();
    }

    public void Grabbed()
{
    GameCanvas.Instance.ShowHint("Press F for Flashlight!");
    HeroPlayerScript.Instance.FPSHands.SetActive(true);
    HeroPlayerScript.Instance.Hand_FlashLight.SetActive(true);
    isGrabbed = true;
}

private void Update()
{
     if (isOn && Light.intensity < normalLightIntensity && !isUVMode)
    {
        Light.intensity = normalLightIntensity; // Fuerza el valor correcto
    }
    if (GameCanvas.Instance.isPaused) return;
    if (!isGrabbed) return;
    if (InventoryManager.Instance.isInventoryOpened) return;

    if (AdvancedGameManager.Instance.controllerType == ControllerType.PcAndConsole)
    {
        if (Input.GetKeyUp(KeyCode.F))
        {
            isOn = !isOn;
            FlashLight_Decision(isOn);
            AudioManager.Instance.Play_Flashlight_Open();
        }

        if (AdvancedGameManager.Instance.blueUVLightAttack && isOn)
        {
            if (Input.GetMouseButtonDown(1))  // Clic derecho para activar luz UV
            {
                isUVMode = true;
                Light.intensity = uvLightIntensity;
                GameCanvas.Instance.FlashLight_BlueEffect_Down();
            }
            else if (Input.GetMouseButtonUp(1))  // Regresar a luz normal al soltar clic derecho
            {
                isUVMode = false;
                
                // Verifica si la linterna está encendida antes de cambiar la intensidad
                if (isOn)
                {
                    // Asegúrate de que la intensidad se restablezca correctamente
                    Light.intensity = normalLightIntensity;

                    // O fuerza un valor si la intensidad ha cambiado inesperadamente
                    if (Light.intensity < normalLightIntensity)
                    {
                        Light.intensity = normalLightIntensity;
                    }
                }

                GameCanvas.Instance.FlashLight_BlueEffect_Up();
            }
        }
    }
}

        public void PlayAudioBlueLight()
        {
            audioSource.Play();
        }

        public void StopAudioBlueLight()
        {
            audioSource.Stop();
        }

        void LateUpdate()
        {
            if (!isGrabbed) return;

            if (GameCanvas.Instance.isFlashBlueNow && BlueBattery > 0)
            {
                BlueBattery = BlueBattery - Time.deltaTime * BatterySpendNumber * 2;
                if (!audioSource.isPlaying)
                {
                    PlayAudioBlueLight();
                }

                var directionLeft2 = Quaternion.AngleAxis(20, aimPoint.transform.right * -1) * Vector3.forward;
                var directionLeft = Quaternion.AngleAxis(10, aimPoint.transform.right * -1) * Vector3.forward;
                var directionForward = aimPoint.TransformDirection(Vector3.forward);
                var directionRight = Quaternion.AngleAxis(10, aimPoint.transform.right) * Vector3.forward;
                var directionRight2 = Quaternion.AngleAxis(20, aimPoint.transform.right) * Vector3.forward;

                if (Physics.Raycast(aimPoint.position, directionLeft2, out hit, 5, layerMask))
                {
                    hit.transform.GetComponent<DemonScript>().GetDamageByFlashlight(DamageRate);
                }
                else if (Physics.Raycast(aimPoint.position, directionLeft, out hit, 5, layerMask))
                {
                    hit.transform.GetComponent<DemonScript>().GetDamageByFlashlight(DamageRate);
                }
                else if (Physics.Raycast(aimPoint.position, directionForward, out hit, 5, layerMask))
                {
                    hit.transform.GetComponent<DemonScript>().GetDamageByFlashlight(DamageRate);
                }
                else if (Physics.Raycast(aimPoint.position, directionRight, out hit, 5, layerMask))
                {
                    hit.transform.GetComponent<DemonScript>().GetDamageByFlashlight(DamageRate);
                }
                else if (Physics.Raycast(aimPoint.position, directionRight2, out hit, 5, layerMask))
                {
                    hit.transform.GetComponent<DemonScript>().GetDamageByFlashlight(DamageRate);
                }
            }
            else if (BlueBattery < 100)
            {
                BlueBattery = BlueBattery + Time.deltaTime * BatterySpendNumber;
                if (BlueBattery > 100)
                {
                    BlueBattery = 100;
                }
            }
            if (BlueBattery <= 0)
            {
                GameCanvas.Instance.FlashLight_BlueEffect_Up();
                StopAudioBlueLight();
            }
            GameCanvas.Instance.Image_BlueLight.fillAmount = (BlueBattery / 100);
        }
    }
}