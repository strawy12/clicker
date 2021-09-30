using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IllustratedBookPanal : MonoBehaviour
{
    [SerializeField] private ScrollRect srollRect = null;
    [SerializeField] private RectTransform[] contants = null;
    [SerializeField] private IBookPet petIBookTemp = null;
    [SerializeField] private IBookStaff staffIBookTemp = null;
    private List<IBookObject> iBookList = new List<IBookObject>();

    private void Start()
    {
        SetScrollActive(0);
    }

    public void SpawnillustratedBook(Staff staff)
    {
        IBookStaff obj = Instantiate(staffIBookTemp, staffIBookTemp.transform.parent);
        iBookList.Add(obj);
        obj.SetIBookStaff(staff);
        obj.gameObject.SetActive(true);
    }

    public void SpawnillustratedBook(Pet pet)
    {
        IBookPet obj = Instantiate(petIBookTemp, petIBookTemp.transform.parent);
        iBookList.Add(obj);
        obj.SetIBookPet(pet);
        obj.gameObject.SetActive(true);
    }

    public void UpdateIBook()
    {
        for (int i = 0; i < iBookList.Count; i++)
        {
            iBookList[i].UpdatePanal();
        }
    }
    public void SetScrollActive(int num)
    {
        for (int i = 0; i < contants.Length; i++)
        {
            if (i == num)
            {
                contants[i].gameObject.SetActive(true);
                srollRect.content = contants[i];
                continue;
            }
            contants[i].gameObject.SetActive(false);
        }
    }
}
