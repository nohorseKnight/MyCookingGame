using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookingPoint : MonoBehaviour
{
    public GameObject resultObj;
    private bool _isSuccess;
    // Start is called before the first frame update
    void Start()
    {
        _isSuccess = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "SuccessArea")
        {
            _isSuccess = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "SuccessArea")
        {
            _isSuccess = false;
        }
    }

    public void MovePoint()
    {
        StartCoroutine(MoveStraight(new Vector3(250, 0, 0), 40));
    }

    public void StopPoint()
    {
        StopAllCoroutines();
        resultObj.SetActive(true);
        resultObj.transform.GetChild(0).GetComponent<Text>().text = _isSuccess ? "Success" : "Fail";
    }

    IEnumerator MoveStraight(Vector3 offset, float moveSpeed)
    {
        Debug.Log("MoveRight");
        Vector3 end = transform.position + offset;
        while (transform.position != end)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(end.x, end.y), moveSpeed * Time.deltaTime);
            yield return null;
        }
        resultObj.SetActive(true);
        resultObj.transform.GetChild(0).GetComponent<Text>().text = _isSuccess ? "Success" : "Fail";
    }
}
