using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pot : MonoBehaviour
{
    public GameObject objCanvas;
    private ArrayList elementInPotList;
    public GameObject processedGrid;
    public GameObject CookMaterialBaseImagePrefab;

    // Start is called before the first frame update
    void Start()
    {
        elementInPotList = new ArrayList();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

    }

    public void AddElementIntoPot(CookElementData newData)
    {
        // foreach (CookElementData data in elementInPotList)
        // {
        //     data._tasteSet.Add(newData._name);
        //     newData._tasteSet.Add(data._name);
        // }
        elementInPotList.Add(newData);
        gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text += newData.DataToString();
    }

    public void MoveElementToProcessGrid()
    {
        foreach (CookElementData data in elementInPotList)
        {
            GameObject parentObj = Instantiate(CookMaterialBaseImagePrefab, processedGrid.transform);
            GameObject obj = parentObj.transform.GetChild(0).gameObject;
            obj.GetComponent<CookElement>().SetSameInfo(data);
            obj.GetComponent<CookElement>().objCanvas = objCanvas;
            obj.GetComponent<Image>().color = new Color(1, 0, 0, 1);
        }
        elementInPotList.Clear();
        gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
    }

    public void StartCooking()
    {
        string cookingStr = "";
        for (int i = 1; i < 4; i++)
        {
            Debug.Log(gameObject.transform.GetChild(i) == null ? "null " + i : "not null");
            cookingStr += "-" + gameObject.transform.GetChild(i).GetChild(0).GetComponent<Text>().text;
            Debug.Log(cookingStr);
        }
        foreach (CookElementData data in elementInPotList)
        {
            data._processList.Add(cookingStr);
        }
        foreach (CookElementData data_0 in elementInPotList)
        {
            foreach (CookElementData data_1 in elementInPotList)
            {
                if (data_0 == data_1) continue;
                data_0._tasteSet.Add(data_1._name);
            }
        }
    }

    public void ShowObjectInPot()
    {

    }
}
