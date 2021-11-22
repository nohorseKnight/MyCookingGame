using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CookElementData
{
    public string _name;
    public ArrayList _processList;
    public HashSet<string> _tasteSet;
    public CookElementData()
    {
        _name = "";
        _processList = new ArrayList();
        _tasteSet = new HashSet<string>();
    }

    public string ProcessListToString()
    {
        string result = "";
        foreach (string str in _processList)
        {
            result += str + " ";
        }
        return result;
    }

    public string TasteSetToString()
    {
        string result = "";
        foreach (string str in _tasteSet)
        {
            result += str + " ";
        }
        return result;
    }

    public string DataToString()
    {
        string result = _name + "\n";
        result += ProcessListToString() + "\n";
        result += TasteSetToString() + "\n";

        return result;
    }
}

public class CookElement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject objCanvas;
    private Transform _orignalParent;
    private Vector3 _orignalPos;
    private CookElementData _data;
    private GameObject _triggerObj;

    public CookElementData GetData()
    {
        return _data;
    }

    void Start()
    {
        _data = _data ?? new CookElementData();
        _data._name = transform.GetChild(0).gameObject.GetComponent<Text>().text;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _orignalParent = transform.parent;
        _orignalPos = transform.position;
        transform.SetParent(objCanvas.transform);
        transform.position = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(_orignalParent);
        transform.localPosition = new Vector3(0, 0, 0);

        if (_triggerObj != null && _triggerObj.tag == "ProcessFlow" && gameObject.tag == "CookSkill")
        {
            _triggerObj.GetComponent<ProcessFlow>().AddCookSkill(gameObject);
        }
        else if (_triggerObj != null && _triggerObj.tag == "CookMaterialInput" && gameObject.tag == "CookMaterial")
        {
            _triggerObj.GetComponent<CookMaterialInPutBox>().AddCookMaterial(gameObject);
        }
        else if (_triggerObj != null && _triggerObj.tag == "GarbageCan" && gameObject.tag == "CookMaterial" && gameObject.GetComponent<Image>().color != new Color(1, 1, 1, 1))
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
        else if (_triggerObj != null && _triggerObj.tag == "Pot" && gameObject.tag == "CookMaterial")
        {
            _triggerObj.GetComponent<Pot>().AddElementIntoPot(_data);
            Destroy(gameObject.transform.parent.gameObject);
        }

        _triggerObj = null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "CookMaterialInput" && gameObject.tag == "CookMaterial")
        {
            _triggerObj = other.gameObject;
        }
        else if (other.gameObject.tag == "ProcessFlow" && gameObject.tag == "CookSkill")
        {
            _triggerObj = other.gameObject;
        }
        else if (other.gameObject.tag == "GarbageCan" && gameObject.tag == "CookMaterial")
        {
            _triggerObj = other.gameObject;
        }
        else if (other.gameObject.tag == "Pot" && gameObject.tag == "CookMaterial")
        {
            _triggerObj = other.gameObject;
        }
    }

    public void AddInfoToProcessList(string info)
    {
        _data._processList.Add(info);
    }

    public void Move(Vector3 destination, float moveSpeed)
    {
        StartCoroutine(MoveBasedestination(destination, moveSpeed));
    }

    IEnumerator MoveBasedestination(Vector3 destination, float moveSpeed)
    {
        // Debug.Log("MoveBasedestination");
        Vector3 des = destination + transform.position;

        while ((transform.position.x != des.x) || (transform.position.y != des.y))
        {
            //Debug.Log(transform.position.ToString() + ", " + trans.position.ToString());
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(des.x, des.y), moveSpeed * Time.deltaTime);
            yield return null;
        }

        Destroy(gameObject);
    }

    public void SetSameInfo(CookElementData data)
    {
        _data = new CookElementData();
        _data._name = data._name;
        gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = _data._name;
        foreach (string str in data._processList)
        {
            _data._processList.Add(str);
        }
        foreach (string str in data._tasteSet)
        {
            _data._tasteSet.Add(str);
        }
    }

}