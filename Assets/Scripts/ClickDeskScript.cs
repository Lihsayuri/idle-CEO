using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDeskScript : MonoBehaviour
{
    private bool canClick = true;
    private float clickDelayTimer = 0f;

    public float clickDelay = 10.0f;


    public static int laptopTableSetCounter = 1; // Variável estática para controlar o ID dos laptops

    public int laptopTableSetID; // ID do laptop atual

    Collider2D collider;

    public Collider2D RunButtonCollider;

    public SpriteRenderer ButtonUpSpriteRenderer, ButtonDownSpriteRenderer;

    public Canvas UpgradeCanvas;

    public GameObject PVisualizer;

    private Color Dim = new Color(0.3f, 0.3f, 0.3f, 1f);
    private Color Bright = new Color(1f, 1f, 1f, 1f);

    // Queue Script from Object
    public RoomQueue roomQueue;
    private Transform ManagerTrigger;


    void Start()
    {
        if (laptopTableSetID != 0)
        {
            laptopTableSetCounter++; 
        } else {
            laptopTableSetID = laptopTableSetCounter;
            laptopTableSetCounter++;
        }
        // Debug.Log("DeskSet ID: " + laptopTableSetID);
        collider = GetComponent<Collider2D>();
        ManagerTrigger = transform.Find("ManagerTrigger").transform;

    }

    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0) && GameManager.menuOpen == false)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D clickCollider = Physics2D.OverlapPoint(mousePosition);
            List<GameObject> unityGameObjects = new List<GameObject>();
            if (collider.OverlapPoint(mousePosition)){
                UpgradeCanvas.GetComponent<loadMainUpdate>().openUpdateAll();
            } else if (RunButtonCollider.OverlapPoint(mousePosition) && clickCollider.gameObject == RunButtonCollider.gameObject){
                // Debug.Log("Clique no botão de execução");
                StartClickDelay();
                if (roomQueue.CheckIfTriggerInQueue(ManagerTrigger))
                {
                    roomQueue.RemoveDeskTrigger(ManagerTrigger);
                }
            }
            
            
        }

        if(!canClick){
            clickDelayTimer += Time.deltaTime;
            if (clickDelayTimer >= clickDelay)
            {
                // Debug.Log("Click");
                // Debug.Log(clickDelay);

                // Debug.Log ("Olha o ID do laptop: " + laptopTableSetID);
                GameManager.LaptopInfo laptopInfo = GameManager.GetLaptopInfo(laptopTableSetID);
                GameManager.TableInfo tableInfo = GameManager.GetTableInfo(laptopTableSetID);
                // Debug.Log("Os earnings do laptop: " + laptopInfo.earnings + " e o delayTime: " + laptopInfo.delayTime);
                // Debug.Log("Os earnings da mesa: " + tableInfo.earnings + " e o delayTime: " + tableInfo.delayTime);

                clickDelay = (laptopInfo.delayTime + tableInfo.delayTime)/(float)2;
                if (clickDelay < 0.0f)
                {
                    clickDelay = 0.0f;
                }
                GameManager.IncrementMoney(laptopInfo.earnings + tableInfo.earnings);
                
                canClick = true;
                BrightenAssets();
                PVisualizer.GetComponent<ProgressVisualizer>().PlayMoneyAnimation();
                roomQueue.AddDeskTrigger(ManagerTrigger);
            }
        }
        if (canClick && GameManager.GetLaptopParameters(laptopTableSetID).level > 0){
            if (!roomQueue.CheckIfTriggerInQueue(ManagerTrigger))
            {
                roomQueue.AddDeskTrigger(ManagerTrigger);
            }
        }
    }

    public void StartClickDelay()
    {
        Debug.Log("Ask to StartClickDelay");
        if (canClick)
        {
            Debug.Log("Started ClickDelay!!!");
            canClick = false;
            clickDelayTimer = 0f;
            DimAssets();
        }
    }

    private void DimAssets()
    {
        ButtonUpSpriteRenderer.enabled = false;
        ButtonDownSpriteRenderer.enabled = true;
        ButtonDownSpriteRenderer.color = Dim;
    }

    private void BrightenAssets()
    {
        ButtonUpSpriteRenderer.enabled = true;
        ButtonDownSpriteRenderer.enabled = false;
    }
}
