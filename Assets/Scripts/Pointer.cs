using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using XboxCtrlrInput;

public class Pointer : MonoBehaviour
{
    // Start is called before the first frame update
    private void Update()
    {
        // Start the game if "Start" clicked or A button pressed
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 500.0f))
            {
                if (hit.transform.gameObject.tag == "MenuStart")
                {
                    SceneManager.LoadScene("Game");
                }
            }
        }
        else if (XCI.GetButtonDown(XboxButton.A))
        {
            SceneManager.LoadScene("Game");
        }
    }
}
