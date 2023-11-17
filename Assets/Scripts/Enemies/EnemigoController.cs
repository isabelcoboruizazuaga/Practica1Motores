using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;


public class EnemigoController : MonoBehaviour
{
    private PlayerController playerController;
    private GameObject player;

    public GameObject enemigo;
    public float velocidad;
    public Vector3 posicionFin;
    public Vector3 posicionInicial;
    private bool moviendoAFin;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    public bool flip = true; //Marca si hay giro o no bas�ndose en la posici�n inicial del sprite en unity
    public bool staticFlip=false;

    public int finX, finY;

    // Start is called before the first frame update
    void Start()
    {
        //Se obtiene la posici�n inicial y se calcula la final
        posicionInicial = transform.position;
        posicionFin = new Vector3(posicionInicial.x + finX, posicionInicial.y + finY, posicionInicial.z);
        moviendoAFin = true;

        //Inicializamos los componentes
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        enemigo = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        MoverEnemigo();
    }

    private void MoverEnemigo()
    {
        Vector3 posicionDestino;

        if (moviendoAFin)
        {
            posicionDestino = posicionFin;

            if (!staticFlip)
                sprite.flipX = flip;
        }
        else
        {
            posicionDestino = posicionInicial;

            if (!staticFlip)
                sprite.flipX = !flip;
        }


        transform.position = Vector3.MoveTowards(transform.position, posicionDestino, velocidad * Time.deltaTime);
        //mueve desde la posicion en la que estemos ahota a la posicion que se le indica a esa velocidad

        if (transform.position == posicionFin)
            moviendoAFin = false;
        if (transform.position == posicionInicial)
            moviendoAFin = true;
    }


    public void Flip()
    {
        if (rb.velocity.x > 0f)
        {
            sprite.flipX = false;
        }
        else if (rb.velocity.x < 0f)
        {
            sprite.flipX = true;
        }
    }


   
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<PlayerController>().vulnerable)
        {
            player = collision.gameObject;
            this.playerController =player.GetComponent<PlayerController>();

            this.playerController.vulnerable = false;

            //vidas-- <=1 -> prrimero compara y luego resta (por ejemplo 2<1, vidas=1)
            //si es --vidas seria al reves, primero resta y despues compara
            if (this.playerController.vidas-- <= 1)
            {
                //sonido.Play();
                StartCoroutine(FinJuego(collision));


            }
            else
            {
                StartCoroutine(QuitaVida(collision));
            }

            //hud.SetVidasTxt(collision.gameObject.GetComponent<PlayerController>().vidas);

        }
    }
    IEnumerator QuitaVida(Collision2D collision)
    {
       player.GetComponent<SpriteRenderer>().color = Color.red;

        yield return new WaitForSeconds(3f);
        this.playerController.vulnerable = true;
        player.GetComponent<SpriteRenderer>().color = Color.white; //blanco lo deja con el color normal
    }

    IEnumerator FinJuego(Collision2D collision)
    {
        Camera.main.transform.parent = null; //dejamos a la c�mara hu�rfana
       player.GetComponent<Transform>().Rotate(new Vector3(0, 0, 90)); //se desmaya el player
        //collision.gameObject.GetComponent<PlayerController>().muerto = true;

        player.GetComponent<SpriteRenderer>().color = Color.red;

        yield return new WaitForSeconds(1f);
        this.playerController.FinJuego();
       // collision.gameObject.GetComponent<PlayerController>().Perder();

        //coge el game object con el que chocamos y obtiene el script de ese objeto
        //y ejecuta el m�todo que tiene dentro
    }


    /* private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<PlayerController>().vulnerable)
        {
            collision.gameObject.GetComponent<PlayerController>().SetVulnerable(false);

            //vidas-- <=1 -> prrimero compara y luego resta (por ejemplo 2<1, vidas=1)
            //si es --vidas seria al reves, primero resta y despues compara
            if (collision.gameObject.GetComponent<PlayerController>().vidas-- <= 1)
            {
               // sonido.Play();
                //StartCoroutine(FinJuego(collision));


}
            else
{
    Debug.Log("choque");
    StartCoroutine(QuitaVida(collision));
    //Invoke("funcion", 2); llama a la funcion "funcion" en 2 segundos, no admite par�metros, es de unity
}

            //hud.SetVidasTxt(collision.gameObject.GetComponent<PlayerController>().vidas);

        }
    }
*/
}
