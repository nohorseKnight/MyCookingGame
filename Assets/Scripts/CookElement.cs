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
        _data = new CookElementData();
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
        if (_triggerObj != null && _triggerObj.tag == "ProcessFlow" && gameObject.tag == "CookSkill")
        {
            _triggerObj.GetComponent<ProcessFlow>().AddCookSkill(gameObject);
        }
        else if (_triggerObj != null && _triggerObj.tag == "CookMaterialInput" && gameObject.tag == "CookMaterial")
        {
            _triggerObj.GetComponent<CookMaterialInPutBox>().AddCookMaterial(gameObject);
        }
        else if (_triggerObj != null && _triggerObj.tag == "GarbageCan" && gameObject.tag == "CookMaterial")
        {
            //
        }
        else if (_triggerObj != null && _triggerObj.tag == "Pot" && gameObject.tag == "CookMaterial")
        {
            //
        }

        transform.SetParent(_orignalParent);
        transform.position = _orignalPos;
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

    public void Move(Vector3 offset, float moveSpeed)
    {
        StartCoroutine(MoveBaseOffset(offset, moveSpeed));
    }

    IEnumerator MoveBaseOffset(Vector3 offset, float moveSpeed)
    {
        Debug.Log("MoveTileByTile");

        Transform trans = transform;
        trans.position += offset;

        while ((transform.position.x != trans.position.x) || (transform.position.y != trans.position.y))
        {
            //Debug.Log(transform.position.ToString() + ", " + trans.position.ToString());
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(trans.position.x, trans.position.y), moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void SetSameInfo(CookElementData data)
    {
        _data = new CookElementData();
        _data._name = data._name;
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