using System;
using TMPro;
using UnityEngine;

public class DraggableObject : MonoBehaviour
{
    private Vector3 startposition;
    private Vector3 fallposition;
    public string FruitName;
    public float backduration = 3f;
    private Camera mainCamera;
    public Rigidbody rb;
    private bool isDragging = false;
    public SimplePlatform _sp;
    private float elapsedTime = 0;
    public float height = 5f; // Parabolik yüksekliğin miktarı
    private bool isBack;
    public ParticleSystem Partical;

   

    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        fallposition = transform.position;

    }
    void Update()
    {
        /// bu kısım yanlış eşletirme olduğunda geri ye dönmeyi sağlıyor
        
        if(isBack){
            if (elapsedTime < backduration)
            {
                elapsedTime += Time.deltaTime;

                // Zamanın 0 ile 1 arasında normalize edilmesi
                float t = elapsedTime / backduration;

                // Yatay hareket
                Vector3 horizontalPosition = Vector3.Lerp(transform.position, startposition, t);

                // Parabolik yüksekliği ekleme
                float arcHeight = Mathf.Sin(t * Mathf.PI) * height;
                transform.position = new Vector3(horizontalPosition.x, horizontalPosition.y + arcHeight, horizontalPosition.z);
            }
            else // burada tekrar çalışması için ilgili değişkenlerde sıfırlama yapılıyor
            {
                isBack = false;
                rb.isKinematic = false;
                elapsedTime = 0;
            }
        }
    }
    void OnMouseDown()
    {
        isDragging = true;
        rb.isKinematic = true; // �eki� s�ras�nda nesneyi sabitle
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Placement Area" )
        {
            _sp = other.GetComponent<SimplePlatform>();
            
            if (_sp.CurrentFruit == null) 
            {
                _sp.CurrentFruit = this;
            }
            //  puan kazanımı ve düşme kısmı aşağıdaki elseif bloğunda gerçekleşiyor
            else if (_sp.CurrentFruit != this && _sp.CurrentFruit.FruitName == this.FruitName)
            {
                rb.isKinematic = false;
                this.gameObject.layer = 6;
                // "FallFruit layerina alarak fizksel etkileşim layerları değiştirilmiş oluyor ve
                // bu sayede deafult layer sahip silindirin içinden onla etkileşim kurmadan düşüyor" 
                
                _sp.CurrentFruit.rb.isKinematic = false;
                
                _sp.CurrentFruit.gameObject.layer = 6; // "FallFruit" 
                _sp.CurrentFruit = null;
                _sp.Score += 5;
                _sp.ScoreText.text = "Score:" + _sp.Score;
                Partical.gameObject.SetActive(true);
                
                // tüm meyveler eşleştirilirse tebrikler paneli açılıyor
                _sp.FruitsCountsText.text ="Fruit Count: "+ (_sp.Fruits.childCount - 2).ToString();


            }
            // eşleştirme yanlış ise geri dönmesi için altaki elseif bloğu çalışarak update deki kodu tetikliyor
            else if (_sp.CurrentFruit != this && _sp.CurrentFruit.FruitName != this.FruitName)
            {
                isBack = true;
                rb.isKinematic = true;
            }
        }
        
        // eşleşmesi yapılmış ve düşmekte olan objelerin hemen altında bulunan obje ile trigger olduklarında sahneden siliniyorlar.
        if (other.transform.name == "Destory Trigger Area")
        {
            Destroy(this.gameObject);
            if (_sp.Fruits.childCount <= 1)
            {
                _sp.ComplatePanel.SetActive(true);
            }
        }
        
        /// yanlışlıkla meyve uzay'ın bir yerine fırlatılırsa objenin meyvelerin durdukları yerde beklemeleri sağlanıyor.
        if (other.transform.name == "WrongFallArea")
        {
            transform.position = fallposition;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        // bir sürükle bırakta bi sorun olur ve Placement Area'nın kenarından yanlışlıkla düşerse  current fruit null yapıyor
        if (_sp.CurrentFruit == this)
        {
            _sp.CurrentFruit = null;
        }
    }


    void OnMouseDrag()
    {
        if (isDragging)
        {
            if (startposition == Vector3.zero)
            {
                startposition = transform.position;
            }
            // Fare pozisyonuna g�re nesneyi hareket ettir
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = mainCamera.WorldToScreenPoint(transform.position).z; // Derinlik bilgisi
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
            transform.position = new Vector3(worldPosition.x, worldPosition.y, worldPosition.z);
        }
    }
    
    void OnMouseUp()
    {
        isDragging = false;
        rb.isKinematic = false; // S�r�kleme bitti�inde fizik tekrar aktifle�ir
    }
}
