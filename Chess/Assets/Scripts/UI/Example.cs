using System.Linq.Expressions;
using UnityEngine;


[ExecuteAlways]
public class Example : MonoBehaviour
{

    [SerializeField] private int collunsCount = 2;
    RectTransform rectTransform;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnTransformChildrenChanged()
    {
       ChangeLayout();
    }

    private void OnValidate()
    {
        ChangeLayout();
    }

    private void ChangeLayout()
    {
        Vector2 newChildSize;
        CalculateNewChildSize(out newChildSize);
        foreach (Transform child in transform)
        {
            if (child != null)
            {
                int row, column;
                CalculateIndexes(child, out row, out column);
                SetChildSize(child as RectTransform, newChildSize);
                SetChildPosition(row, column, child as RectTransform, newChildSize);
            }
            Debug.Log(child.localPosition);
        }
    }


    private void SetChildSize(RectTransform child, Vector2 newChildSize)
    {
        child.GetComponent<RectTransform>().sizeDelta = newChildSize;
    }

    private void SetChildPosition(int row, int column, RectTransform child, Vector2 newChildSize)
    {
        float gridOffSet = 0.5f * (collunsCount - 1);
        Vector3 newChildPosition = new Vector3(newChildSize.x * column, newChildSize.y * -row, 0);
        Vector3 offset = new Vector3(-newChildSize.x * gridOffSet, newChildSize.y * gridOffSet, 0);
        child.localPosition = newChildPosition + offset;
    }

    private void CalculateIndexes(Transform child, out int row, out int column)
    {
        int index = child.GetSiblingIndex();
        row = index / collunsCount;
        column = index % collunsCount;
    }

    private void CalculateNewChildSize(out Vector2 newChildSize)
    {
        newChildSize = rectTransform.rect.size / collunsCount;
    }

}
