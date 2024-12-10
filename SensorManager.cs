using UnityEngine;
using UnityEngine.UI;

public class SensorManager : MonoBehaviour
{
    public Text angularVelocityText;
    public Text accelerationText;
    public Text altitudeText;
    public Text gravityText;
    public Text latitudeText;
    public Text longitudeText;

    public Transform samuraiTransform; 
    public float smoothing = 0.1f; 

    void Start()
    {
        Input.location.Start();
        Input.compass.enabled = true;
        if (SystemInfo.supportsGyroscope)
            Input.gyro.enabled = true;
    }

    void Update()
    {
        ShowSensorData();
        OrientSamurai();
        MoveSamurai();
    }

    void ShowSensorData()
    {
        angularVelocityText.text = "Velocidad Angular: " + Input.gyro.rotationRate.ToString();
        accelerationText.text = "Aceleración: " + Input.acceleration.ToString();
        if (Input.location.status == LocationServiceStatus.Running)
        {
            altitudeText.text = "Altitud: " + Input.location.lastData.altitude.ToString();
            latitudeText.text = "Latitud: " + Input.location.lastData.latitude.ToString();
            longitudeText.text = "Longitud: " + Input.location.lastData.longitude.ToString();
        }
        else
        {
            altitudeText.text = "Altitud: No disponible";
            latitudeText.text = "Latitud: No disponible";
            longitudeText.text = "Longitud: No disponible";
        }
        gravityText.text = "Gravedad: " + Physics.gravity.ToString();
    }

    void OrientSamurai()
    {
        Quaternion rotator = Quaternion.identity;
        if (Input.compass.enabled)
        {
            float heading = Input.compass.trueHeading;
            rotator = Quaternion.Euler(0f, heading, 0f);
        }
        samuraiTransform.rotation = Quaternion.Slerp(samuraiTransform.rotation, rotator, smoothing);
    }

    void MoveSamurai()
    {
        Vector3 acceleration = Input.acceleration;
        float movementSpeed = acceleration.z * -10f;
        samuraiTransform.Translate(0f, 0f, movementSpeed * Time.deltaTime);
    }

}
