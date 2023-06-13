using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDeskScript : MonoBehaviour
{
    private bool canClick = true;
    private float clickDelayTimer = 0f;

    public float clickDelay = 10.0f;


    public static int laptopTableSetCounter= 1; // Variável estática para controlar o ID dos laptops

    public int laptopTableSetID; // ID do laptop atual

    Collider2D collider;
    SpriteRenderer spriteRenderer;

    SpriteRenderer propSpriteRenderer;

    public GameObject PVisualizer;

    private Color Dim = new Color(0.3f, 0.3f, 0.3f, 1f);
    private Color Bright = new Color(1f, 1f, 1f, 1f);

    void Start()
    {
        laptopTableSetID = laptopTableSetCounter;
        laptopTableSetCounter++;

        Debug.Log("DeskSet ID: " + laptopTableSetID);
        collider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        propSpriteRenderer = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();

    }

    void FixedUpdate()
    {
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                if (collider.OverlapPoint(touchPosition))
                {
                    if (touch.phase == TouchPhase.Began && canClick){
                        canClick = false;
                        clickDelayTimer = 0.0f;
                        DimAssets();
                    }
                }
            }
        }

        // For Test Purposes
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (collider.OverlapPoint(mousePosition))
            {
                if (canClick)
                {
                    canClick = false;
                    clickDelayTimer = 0.0f;
                    DimAssets();
                }
            }
        }

        if(!canClick){
            clickDelayTimer += Time.deltaTime;
            if (clickDelayTimer >= clickDelay)
            {
                Debug.Log("Click");
                Debug.Log(clickDelay);
                // pegue o dicionário de laptops do GameManager e pegue as informações do laptop atual que possui o mesmo ID que o laptopTableSetID
                // use essas informações para incrementar o dinheiro do jogador
                // use o método IncrementMoney do GameManager para incrementar o dinheiro do jogador
                // update o clickDelay para o valor do decreaseTime do laptop atual
                Debug.Log ("Olha o ID do laptop: " + laptopTableSetID);
                GameManager.LaptopInfo laptopInfo = GameManager.GetLaptopInfo(laptopTableSetID);
                Debug.Log("Info do laptop: " + laptopInfo);
                Debug.Log("Os earnings do laptop: " + laptopInfo.earnings + " e o decreaseTime: " + laptopInfo.decreaseTime);
                if (laptopInfo != null)
                {
                    clickDelay = laptopInfo.decreaseTime;
                    Debug.Log("earnings do laptop " + laptopTableSetID + ": " + laptopInfo.earnings);

                    GameManager.IncrementMoney(laptopInfo.earnings);
                }
                canClick = true;
                BrightenAssets();
                PVisualizer.GetComponent<ProgressVisualizer>().PlayMoneyAnimation();
            }
        }
    }

    private void DimAssets()
    {
        spriteRenderer.color = Dim;
        propSpriteRenderer.color = Dim;
    }

    private void BrightenAssets()
    {
        spriteRenderer.color = Bright;
        propSpriteRenderer.color = Bright;
    }
}
