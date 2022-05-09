using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;



public class araba_control : MonoBehaviour
{
    private float horizontalInput; //kullan�c�n�n dikey tu� kontrollerini tutmas� i�in
    private float verticalInput; //kullan�c�n�n yatay tu� kontrollerini tutmas� i�in

    private bool isFren; //kullan�c�n�n fren tu� kontrol�n� tutmas� i�in
    private float currentFrenForce;

    public int kalan_can = 3;
    TextMeshProUGUI can_txt;

    public GameObject kaybettin_pnl;
    public GameObject duraklat_pnl;
    public GameObject kontak_kapat_btn;
    public GameObject bolum_gec_pnl;
    
 

    [SerializeField] private float maxDonusAcisi; //arac� sa�a ve sola y�nlendirmek i�in
    [SerializeField] private float currentDonusAcisi; 
    [SerializeField] private float motorTorqueForce; //gidece�i h�z� uygulamak i�in
    [SerializeField] private float brakeForce; //yava�latmak i�in

    [SerializeField] private WheelCollider onSolTekerlekCollider;
    [SerializeField] private WheelCollider onSagTekerlekCollider;   //bu d�rt sat�r modeldeki wheelcolliderleri tutacak
    [SerializeField] private WheelCollider arkaSolTekerlekCollider;
    [SerializeField] private WheelCollider arkaSagTekerlekCollider;

    [SerializeField] private Transform onSolTekerlekTransform;
    [SerializeField] private Transform onSagTekerlekTransform;  //bu d�rt sat�r modeldeki tekerleklerin transform de�erlerini tutacak
    [SerializeField] private Transform arkaSolTekerlekTransform;
    [SerializeField] private Transform arkaSagTekerlekTransform;


    public void duraklat_btn()
    {
        duraklat_pnl.SetActive(true);
        Time.timeScale = 0f;
    }
    public void devam_et_btn()
    {
        duraklat_pnl.SetActive(false);
        Time.timeScale = 1f;
    }

    public void tekrar_oyna_btn()
    {
        SceneManager.LoadScene("Scenes/bolum_1");   //bu kod ile sahneyi yeniden y�kledik

        Time.timeScale = 1.0f;
    }
    public void ana_men�_btn()
    {
        SceneManager.LoadScene("Scenes/ana_men�");   //bu kod ile sahneyi yeniden y�kledik

        Time.timeScale = 1.0f;
    }
    public void bolum_gec()
    {
        bolum_gec_pnl.SetActive(true);
        Time.timeScale = 0f;
    }


    private void Start()
    {
        can_txt = GameObject.Find("Canvas/can/kalan_can_adet").GetComponent<TextMeshProUGUI>();
    }
    public void OnTriggerStay(Collider other)
    {
        if(other.gameObject.name == "on_sanal_duvar")
        {
            kontak_kapat_btn.SetActive(true);
        }
        else
        {
            kontak_kapat_btn.SetActive(false);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "engel")
        {
            kalan_can -= 1;
            can_txt.text = kalan_can.ToString();
        }
        if(kalan_can <= 0)
        {
            kaybettin_pnl.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        getUserInput(); //arac�n tu�lar�n� tutacak
        moveTheCar();
        rotateTheCar(); //arac�n sa�a ve sola d�nmesi i�in
        rotateTheWheels();
    }
    
    private void getUserInput() //buras� tu� kontrolleri tutan fonksiyon
    {
        horizontalInput = SimpleInput.GetAxis("Horizontal");  //dikey tu� kontrolleri
        verticalInput = SimpleInput.GetAxis("Vertical");  //yatay tu� kontrolleri
        isFren = Input.GetKey(KeyCode.Space);  //frenlemek i�in kullanaca��m�z tu�
        if(Input.GetKey(KeyCode.R)) // arac�n herhangi bir nedenle devrilmesi halinde R tu�una basarak arac� eski hatine getirece�iz
        {
            resetCarRotation();
        }

    }
    
    private void resetCarRotation() //ara� devrilince eski haline gelmesi i�in uygulanan fonksiyon
    {
        Quaternion rotation = transform.rotation;
        rotation.z = 0f;
        rotation.x = 0f;
        transform.rotation = rotation;
    }

    private void moveTheCar() //kullan�c�dan al�nan �nputlar� wheelcolliderlere at�yoruz
    {
        onSolTekerlekCollider.motorTorque = verticalInput * motorTorqueForce;
        onSagTekerlekCollider.motorTorque = verticalInput * motorTorqueForce;
        currentFrenForce = isFren ? brakeForce : 0f;  //kullan�c� space tu�una basm��sa brakeForce de�i�kenini currentFrenForce de�i�kenine at�yoruz
        if(isFren)
        {
            onSolTekerlekCollider.brakeTorque = currentFrenForce;
            onSagTekerlekCollider.brakeTorque = currentFrenForce;   //bu 4 sat�rda fren i�in uygulanan force de�erini her d�rt tekerle�in colliderlerine at�yoruz.
            arkaSolTekerlekCollider.brakeTorque = currentFrenForce;
            arkaSagTekerlekCollider.brakeTorque = currentFrenForce;
        }

    }

    private void rotateTheCar() //arac�n sa�a ve sola d�nmesi i�in
    {
        currentDonusAcisi = maxDonusAcisi * horizontalInput;
        onSolTekerlekCollider.steerAngle = currentDonusAcisi;
        onSagTekerlekCollider.steerAngle = currentDonusAcisi;
    }
    
    private void rotateTheWheels()
    {
        Vector3 position;
        Quaternion rotation;
        onSolTekerlekCollider.GetWorldPose(out position, out rotation);
        onSolTekerlekTransform.position = position;
        onSolTekerlekTransform.rotation = rotation;

        onSagTekerlekCollider.GetWorldPose(out position, out rotation);
        onSagTekerlekTransform.position = position;
        onSagTekerlekTransform.rotation = rotation;

        arkaSolTekerlekCollider.GetWorldPose(out position, out rotation);
        arkaSolTekerlekTransform.position = position;
        arkaSolTekerlekTransform.rotation = rotation;

        arkaSagTekerlekCollider.GetWorldPose(out position, out rotation);
        arkaSagTekerlekTransform.position = position;
        arkaSagTekerlekTransform.rotation = rotation;
       
    }



}
