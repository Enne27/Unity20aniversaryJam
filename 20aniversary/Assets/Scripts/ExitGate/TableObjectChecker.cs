using UnityEngine;
using System.Collections.Generic;

public class TableObjectChecker : MonoBehaviour
{
    [Header("Objetos que deben estar sobre la mesa")]
    public GameObject objeto1;
    public GameObject objeto2;
    public GameObject objeto3;

    private HashSet<GameObject> objetosSobreMesa = new HashSet<GameObject>();

    public Animator doorAnimator;

    public FinishGame finish;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == objeto1 || other.gameObject == objeto2 || other.gameObject == objeto3)
        {
            objetosSobreMesa.Add(other.gameObject);
            RevisarObjetos();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == objeto1 || other.gameObject == objeto2 || other.gameObject == objeto3)
        {
            objetosSobreMesa.Remove(other.gameObject);
            RevisarObjetos();
        }
    }

    private void RevisarObjetos()
    {
        if (objetosSobreMesa.Contains(objeto1) &&
            objetosSobreMesa.Contains(objeto2) &&
            objetosSobreMesa.Contains(objeto3))
        {
            // Aquí puedes llamar a la función de abrir puerta
            doorAnimator.SetTrigger("Open");
            finish.canFinish = true;
        }
    }
}
