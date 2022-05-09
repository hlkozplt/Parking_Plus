using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;



public class araba_control : MonoBehaviour
{
    private float horizontalInput; //kullanýcýnýn dikey tuþ kontrollerini tutmasý için
    private float verticalInput; //kullanýcýnýn yatay tuþ kontrollerini tutmasý için

    private bool isFren; //kullanýcýnýn fren tuþ kontrolünü tutmasý için
    private float currentFrenForce;

    public int kalan_can = 3;
    TextMeshProUGUI can_txt;

    public GameObject kaybettin_pnl;
    public GameObject duraklat_pnl;
    public GameObject kontak_kapat_btn;
    public GameObject bolum_gec_pnl;
    
 

    [SerializeField] private float maxDonusAcisi; //aracý saða ve sola yönlendirmek için
    [SerializeField] private float currentDonusAcisi; 
    [SerializeField] private float motorTorqueForce; //gideceði hýzý uygulamak için
    [SerializeField] private float brakeForce; //yavaþlatmak için

    [SerializeField] private WheelCollider onSolTekerlekCollider;
    [SerializeField] private WheelCollider onSagTekerlekCollider;   //bu dört satýr modeldeki wheelcolliderleri tutacak
    [SerializeField] private WheelCollider arkaSolTekerlekCollider;
    [SerializeField] private WheelCollider arkaSagTekerlekCollider;

    [SerializeField] private Transform onSolTekerlekTransform;
    [SerializeField] private Transform onSagTekerlekTransform;  //bu dört satýr modeldeki tekerleklerin transform deðerlerini tutacak
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
        SceneManager.LoadScene("Scenes/bolum_1");   //bu kod ile sahneyi yeniden yükledik

        Time.timeScale = 1.0f;
    }
    public void ana_menü_btn()
    {
        SceneManager.LoadScene("Scenes/ana_menü");   //bu kod ile sahneyi yeniden yükledik

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
        getUserInput(); //aracýn tuþlarýný tutacak
        moveTheCar();
        rotateTheCar(); //aracýn saða ve sola dönmesi için
        rotateTheWheels();
    }
    
    private void getUserInput() //burasý tuþ kontrolleri tutan fonksiyon
    {
        horizontalInput = SimpleInput.GetAxis("Horizontal");  //dikey tuþ kontrolleri
        verticalInput = SimpleInput.GetAxis("Vertical");  //yatay tuþ kontrolleri
        isFren = Input.GetKey(KeyCode.Space);  //frenlemek için kullanacaðýmýz tuþ
        if(Input.GetKey(KeyCode.R)) // aracýn herhangi bir nedenle devrilmesi halinde R tuþuna basarak aracý eski hatine getireceðiz
        {
            resetCarRotation();
        }

    }
    
    private void resetCarRotation() //araç devrilince eski haline gelmesi için uygulanan fonksiyon
    {
        Quaternion rotation = transform.rotation;
        rotation.z = 0f;
        rotation.x = 0f;
        transform.rotation = rotation;
    }

    private void moveTheCar() //kullanýcýdan alýnan ýnputlarý wheelcolliderlere atýyoruz
    {
        onSolTekerlekCollider.motorTorque = verticalInput * motorTorqueForce;
        onSagTekerlekCollider.motorTorque = verticalInput * motorTorqueForce;
        currentFrenForce = isFren ? brakeForce : 0f;  //kullanýcý space tuþuna basmýþsa brakeForce deðiþkenini currentFrenForce deðiþkenine atýyoruz
        if(isFren)
        {
            onSolTekerlekCollider.brakeTorque = currentFrenForce;
            onSagTekerlekCollider.brakeTorque = currentFrenForce;   //bu 4 satýrda fren için uygulanan force deðerini her dört tekerleðin colliderlerine atýyoruz.
            arkaSolTekerlekCollider.brakeTorque = currentFrenForce;
            arkaSagTekerlekCollider.brakeTorque = currentFrenForce;
        }

    }

    private void rotateTheCar() //aracýn saða ve sola dönmesi için
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
