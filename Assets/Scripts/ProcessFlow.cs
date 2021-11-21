using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProcessFlow : MonoBehaviour
{
    public GameObject objCanvas;
    private int[,] _subPosArr =
    {
        {0,    0,    0,    0,    0},
        {75,   -75,  0,    0,    0},
        {100,  0,    -100, 0,    0},
        {150,  50,   -50,  -150, 0},
        {170,  85,   0,    -85,  -170}
    };
    [SerializeField] private ArrayList _cookSkillList;
    // Start is called before the first frame update
    private Vector3 _triggerEnterPos;

    public GameObject InputBox;
    public GameObject processedGrid;
    public GameObject CookMaterialBaseImagePrefab;
    void Start()
    {
        _cookSkillList = new ArrayList();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ClearProcessFlow()
    {
        foreach (GameObject obj in _cookSkillList)
        {
            if (obj != null) Destroy(obj);
        }
        _cookSkillList.Clear();
    }

    public void AddCookSkill(GameObject cookSkillObject)
    {
        if (_cookSkillList.Count > 5)
        {
            Debug.Log("_cookSkillList.Count > 5");
            return;
        }

        // Instantiate(cookSkillObject);
        GameObject newCookObject = Instantiate(cookSkillObject, gameObject.transform);
        newCookObject.GetComponent<BoxCollider2D>().size = new Vector2(0, 0);
        if (_cookSkillList.Count == 0)
        {
            _cookSkillList.Add(newCookObject);
            return;
        }

        foreach (GameObject obj in _cookSkillList)
        {
            Debug.Log(obj.transform.position);
        }

        GameObject lastObj = (GameObject)_cookSkillList[_cookSkillList.Count - 1];
        if (_triggerEnterPos.x < lastObj.transform.position.x)
        {
            _cookSkillList.Add(newCookObject);
        }
        else
        {
            for (int i = 0; i < _cookSkillList.Count; i++)
            {
                GameObject obj = (GameObject)_cookSkillList[i];
                if (_triggerEnterPos.x > obj.transform.position.x)
                {
                    Debug.Log("Insert in " + i);
                    _cookSkillList.Insert(i, newCookObject);
                    break;
                }
            }
        }

        UpdateSubObjectPos();
    }

    private void UpdateSubObjectPos()
    {
        int size = _cookSkillList.Count;
        for (int i = 0; i < size; i++)
        {
            GameObject tmpObject = (GameObject)_cookSkillList[i];
            tmpObject.transform.position = gameObject.transform.position + new Vector3(_subPosArr[size - 1, i], 0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter" + other.transform.position.ToString());
        // Debug.Log(gameObject.transform.position.ToString());
        _triggerEnterPos = other.transform.position;
    }

    public void CookSkillElementsGotoLeft()
    {
        int size = _cookSkillList.Count;
        if (InputBox != null && InputBox.transform.GetChild(1).gameObject != null)
        {
            foreach (GameObject obj in _cookSkillList)
            {
                InputBox.transform.GetChild(1).gameObject.GetComponent<CookElement>().AddInfo(obj.transform.GetChild(0).GetComponent<Text>().text);
            }
            Invoke("PutCookMaterialFromInputboxToProcessGrid", 2.0f);
        }
        for (int i = 0; i < size; i++)
        {
            GameObject tmpObject = (GameObject)_cookSkillList[i];
            tmpObject.GetComponent<CookElement>().Move(new Vector3(-200 - _subPosArr[size - 1, i], 0, 0), 80);
        }
        _cookSkillList.Clear();

    }

    private void PutCookMaterialFromInputboxToProcessGrid()
    {
        if (InputBox != null && InputBox.transform.GetChild(1).gameObject != null)
        {
            GameObject parentObj = Instantiate(CookMaterialBaseImagePrefab, processedGrid.transform);
            GameObject baseObj = InputBox.transform.GetChild(1).gameObject;
            GameObject obj = parentObj.transform.GetChild(0).gameObject;
            obj.GetComponent<CookElement>().SetSameInfo(baseObj.GetComponent<CookElement>().GetData());
            obj.GetComponent<CookElement>().objCanvas = objCanvas;
            obj.GetComponent<Image>().color = new Color(1, 1, 0, 1);
            Destroy(baseObj);
        }
    }
}
