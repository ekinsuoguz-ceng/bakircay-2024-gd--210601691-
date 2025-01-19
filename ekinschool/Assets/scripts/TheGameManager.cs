using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class TheGameManager : MonoBehaviour
{
   // public Transform anchorPoint; // Platform �zerindeki sabit nokta
    public DraggableObject CurrentFruit;
    public int Score = 0;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI FruitsCountsText;
    public Transform Fruits;
    public GameObject ComplatePanel;
    public List<GameObject> FruitsPrefabs;
    public GameObject SpawnpointsParent;
    public float MixingForce = 5f; // Kuvvet büyüklüğü
    public float MixingDuration = 2f; // Karıştırma süresi
    private bool isMixing = false;
   
    public float CameraShakeduration; public float CameraShakestrength; public int CameraShakevibrato; public float CameraShakerandomness;
    
    
    public static TheGameManager instance;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Eğer sahneler arasında taşınmasını istiyorsanız
        }
        else
        {
            Destroy(gameObject); // Zaten bir instance varsa, yenisini yok et
        }
    }
    
    private void Start()
    {
        FruitsCountsText.text ="Fruit Count: "+ Fruits.childCount;
    }
    void Update()
    {
        if (isMixing)
        {
            Camera.main.transform.DOShakePosition(CameraShakeduration, CameraShakestrength, CameraShakevibrato, CameraShakerandomness);
            StartCoroutine(MixFruits());
            
        }
    }

    public void NewFruitsSpawn()
    {
        if (Fruits.childCount <= 1)
        {
            List<GameObject> tempFruitsPrefabs = new List<GameObject>(FruitsPrefabs);
            
            for (int i = 0; i < SpawnpointsParent.transform.childCount; i++)
            {
                int no = Random.Range(0, tempFruitsPrefabs.Count);
                    
                var _fruit= Instantiate(tempFruitsPrefabs[no].gameObject,Fruits.transform);
                    
                _fruit.transform.position = SpawnpointsParent.transform.GetChild(i).position;

                var _fruit2=  Instantiate(tempFruitsPrefabs[no].gameObject,Fruits.transform);
                    
                _fruit2.transform.position = SpawnpointsParent.transform.GetChild(i+SpawnpointsParent.transform.childCount/2 -1).position;
                    
                tempFruitsPrefabs.RemoveAt(no);
                    
                    
                if (i >=SpawnpointsParent.transform.childCount/2-1)
                {
                    break;
                }
                    
                    
            }
                
        }
        FruitsCountsText.text ="Fruit Count: "+ (Fruits.childCount -1).ToString();

        
    }

    public void DestoryGameObject(GameObject item)
    {
        Destroy(item);
    }

    public void Shake()
    {
        isMixing = true;
       

    }
    IEnumerator MixFruits()
    {
        isMixing = true;
        float timer = 0f;

        // Mixing sırasında meyvelere rastgele kuvvetler uygula
        while (timer < MixingDuration)
        {
            foreach (Transform fruit in Fruits)
            {
                Rigidbody rb = fruit.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // Rastgele bir yön ve kuvvet uygula
                    Vector3 randomForce = new Vector3(
                        Random.Range(-1f, 1f),
                        Random.Range(-1f, 1f),
                        0f // Sadece 2D düzlemde kuvvet uyguluyoruz
                    ).normalized * MixingForce;

                    rb.AddForce(randomForce, ForceMode.Impulse);
                }
            }

            timer += Time.deltaTime;
            yield return null;
        }

        isMixing = false;
    }
    
}
