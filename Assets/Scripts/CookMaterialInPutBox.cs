using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookMaterialInPutBox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddCookMaterial(GameObject cookMaterialObj)
    {
        GameObject newCookObject = Instantiate(cookMaterialObj, gameObject.transform);
    }

    public void ClearInputBox()
    {
        if (transform.childCount > 1)
        {
            Destroy(transform.GetChild(1).gameObject);
        }
    }
}
