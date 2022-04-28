using System.Globalization;
using System;
using UnityEngine;
using UnityEngine.UI;

public class displayManager : MonoBehaviour
{

    public Main main;

    public CameraMover cameraMover;

    public Slider slider;

    public Text TotalObjects;
    public Text TotalFolds;



    private string TotalObjectsText = "Equivelent # Objects:\n";
    private string TotalFoldsText = "Total # Folds:\n";

    // Start is called before the first frame update
    void Start()
    {
        UpdateIterations();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateIterations()
    {
        Debug.Log(slider.value);

        main.Iterations = (int) slider.value;

        cameraMover.orbitDist = 3 * Mathf.Pow(3, slider.value);

        TotalObjects.text = TotalObjectsText + String.Format("{0:n0}", Mathf.Pow(20, slider.value));
        TotalFolds.text = TotalFoldsText + String.Format("{0:n0}", 8 * slider.value);

    }
}
