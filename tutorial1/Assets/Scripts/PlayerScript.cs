using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("PlayerComponents")]
    Rigidbody rb;
    [Header("VectorComponents")]
    Vector3 position;
    Vector3 mesafe;
    float horizontal = 0, vertical = 0;
    public float playerSpeed = 5f;
    [Header("Animator")]
    public Animator animator;
    bool zipla = true;
    [Header("Camera")]
    public GameObject headCamera;
    float headCameraLeftRight = 0f, headCameraUpBottom = 0f;
    bool kameraRotPos = false;
    GameObject kamera, cameraPosition1, cameraPosition2;
    [Header("Raycast")]
    RaycastHit hit;
    RaycastHit fireHit;
    [Header("Objeler")]
    public GameObject kursun, kursunyeri;
    void Start()
    {
      //Objelere Ulaşıyoruz  
        mesafe = headCamera.transform.position - transform.position;
        rb = GetComponent<Rigidbody>();
        kamera = Camera.main.gameObject;
        cameraPosition1 = headCamera.transform.Find("CameraPosition1").gameObject;
        cameraPosition2 = headCamera.transform.Find("CameraPosition2").gameObject;
        
    }
    void Update()
    {
        AnimationEvents();
        if(Input.GetMouseButtonDown(1))
        {
            kameraRotPos = true;
        }
         if(Input.GetMouseButtonUp(1))
        {
            kameraRotPos = false;
        }
    }
    void FixedUpdate()
    {
        playerMovement();
        headCameraRotation();
        if(Input.GetMouseButton(0))
        {
            Fire();
        }
        
        if (kameraRotPos)
        {
            playerRotation2();
            kamera.transform.position = cameraPosition2.transform.position;
        }
        if(!kameraRotPos)
        {
            playerRotation1();
            kamera.transform.position = cameraPosition1.transform.position;
        }
        if(hit.point==null)
        {
            Debug.Log("null");
        }
    }
    void Fire()
    {

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, .8f));
        Physics.Raycast(ray, out fireHit);
        Debug.DrawLine(ray.origin, fireHit.point, Color.black);
        if(fireHit.point!=null)
        {
            GameObject bullets = Instantiate(kursun, kursunyeri.transform.position, kursunyeri.transform.rotation);
            bullets.GetComponent<Rigidbody>().AddForce((fireHit.point - kursunyeri.transform.position).normalized * 10000);
        }
        
    }
    void playerMovement()
    {   
       
        //Yatayda Değer alıyoruz
        horizontal = Input.GetAxis("Horizontal");
        //Dikeyde Değer Alıyoruz
        vertical = Input.GetAxis("Vertical");
        //Aldığımız değerleri pozisyon Vector3'ün içerisine aktarıyoruz bunu transform.TransfromDirection parametresi içerisinde veriyoruz ki
        //obje dönüş yaptışında önünü güncelleyerek dönüş yaptığı pozisyonu alsın
        position = new Vector3(horizontal, 0, vertical);
        position = transform.TransformDirection(position);
        //daha sonra fizik komponenti içerisine ekliyerek hareket ettiyoruz
        rb.position += position * Time.fixedDeltaTime * playerSpeed;
        //Zıpladıgı zaman aynı tonda hareket edemesin diye hızını kesiyorum
       
    }
    //Player rotasyonu hareket ediyorken değişsin
    void playerRotation1()
    {
        //0,5,0 noktasından başlayıp  headKamera objesinin 1.yavrusuna ulaşıp onun ilerisine doğru bir ışık çizdiriyoruz
        Physics.Raycast(new Vector3(0, 5, 0), headCamera.transform.GetChild(0).forward, out hit);
        //karakterimizin rotasyonu mevcut rotasyonundan,baktığımız rotasyona doğru yani hit'in çarmmış olduğu kısmın rotasyonlarına doğru dönüş
        //yapmaktadır
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(new Vector3(hit.point.x, 0, hit.point.z)), .3f);
        //Bu çizgiyi kırmızı renk ile çizdirmeye çalıştım fakat çizmedi 
        Debug.DrawLine(Vector3.zero, hit.point, Color.red);
    }
    void playerRotation2()
    {
            Physics.Raycast(new Vector3(0,5,0), headCamera.transform.GetChild(0).forward, out hit);
            //karakterimizin rotasyonu mevcut rotasyonundan,baktığımız rotasyona doğru yani hit'in çarmmış olduğu kısmın rotasyonlarına doğru dönüş
            //yapmaktadır
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(new Vector3(hit.point.x, 0, hit.point.z)), .3f);
            //Bu çizgiyi kırmızı renk ile çizdirmeye çalıştım fakat çizmedi 
            Debug.DrawLine(Vector3.zero, hit.point,Color.red);
      
    }
    void headCameraRotation()
    {
        mouseRotation();
        //HeadKamera rotasyonuna almış olduğumuz left-right , top-bottom parametrelerini ekliyoruz
        headCamera.transform.rotation = Quaternion.Euler(headCameraUpBottom, headCameraLeftRight, transform.eulerAngles.z);
    }
    void mouseRotation()
    {
        //Kamera takibi headKamera pozisyon değerini karakterimize eşitliyoruz ve ilk başta almış olduğumuz mesafeyi ekliyoruz bu sayede
        //aradaki mesafeyi koruyarak bizi takip ediyor.
        headCamera.transform.position = transform.position + mesafe;
        //Mouse'nin x koordinatındaki hareketini alıyoruz ve 100 ile çarpıyoruz.
        headCameraLeftRight += Input.GetAxis("Mouse X") * Time.deltaTime * 100;
        //Aynı şekilde Mouse'nin Y koordinatındaki hareketini alıyoruz ve 100 ile çarpıyoruz fakat eksi 100 ile çarpıyoruz çünkü biz mouse yukarı
        // kaldırdıgımız zaman ekran aşşağı iniyor yani istediğimizin tersi oluyor bunun tersini alırsak istediğimiz kıvama getirmiş oluruz
        headCameraUpBottom += Input.GetAxis("Mouse Y") * Time.deltaTime * -100;
        //Kameranın yukarı ve aşşağı bakma açısını kısıtlıyoruz
        headCameraUpBottom = Mathf.Clamp(headCameraUpBottom, -30, 30);
    }
    void AnimationEvents()
    {
        //yürüyorsa parametre değeri gönderiyoruz
        animator.SetFloat("Horizontal",horizontal);
        animator.SetFloat("Vertical", vertical);
        // Mouse Sol tık Ateş etsin
        if(Input.GetMouseButton(0))
        {
            animator.SetBool("fire", true);
        }
        // Mouse Sol tık basmıyorsa ateş etmesin
        else
        {
            animator.SetBool("fire", false);
        }
        //Shift tusuna basıyorsa koşma parametresi true
        if(Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetBool("RunnerBoolen", true);
            playerSpeed = 15;
        }
        //shift tusuna basmıyor ise kosma parametresi false
        else
        {
            animator.SetBool("RunnerBoolen", false);
            playerSpeed = 5;
        }
        if(Input.GetKeyDown(KeyCode.Space) && zipla)
        {
            animator.SetBool("Jump", true);
        }
    }
    //yürürken ya da koşarken Zıpladığımızda animasyon bitince parametreyi false yapıp walk'a geçsin
    void walkJumperFalse()
    {
        animator.SetBool("Jump", false);
    }
    // Space tuşuna basınca karaktere zıplaması için gereken gücü veren method
    void rbAddforce()
    {
        rb.AddForce(0,200,0);
        zipla = false;
    }
    void LayerWeight0()
    {
        animator.SetLayerWeight(animator.GetLayerIndex("Base Layer"), 0);
    }
    void LayerWeight()
    {
        animator.SetLayerWeight(animator.GetLayerIndex("Base Layer"), .5f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        zipla = true;   
    }

}
