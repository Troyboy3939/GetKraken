using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Pointer : MonoBehaviour
{
    // Start is called before the first frame update
    private void Update()
    {
        Clicked();
    }

    private void Clicked()
    {

        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 500.0f))
            {
                if (hit.transform.gameObject.tag == "MenuStart")
                {
                    SceneManager.LoadScene("CalibrationScreen");
                }
            }
        }
    }
}
