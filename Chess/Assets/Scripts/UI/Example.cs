using System.Linq.Expressions;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;


[ExecuteAlways]
public class Example : MonoBehaviour
{

    [SerializeField] private int collunsCount = 2;
    [SerializeField] private Vector2 customMinimunSize = new Vector2(100, 100);
    [SerializeField] private Vector2 spacing = new Vector2(10, 10);
    private int rowsCount = 0;
    RectTransform rectTransform;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnTransformChildrenChanged()
    {
        CalculateRowsCount();
        ResizeContainer();
        ChangeLayout();
    }

    private void OnValidate()
    {
        CalculateRowsCount();
        ResizeContainer();
        ChangeLayout();
    }

    private void ChangeLayout()
    {
        Vector2 newChildSize = new Vector2();
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
        child.sizeDelta = newChildSize;
    }

    private void SetChildPosition(int row, int column, RectTransform child, Vector2 newChildSize)
    {
        
        Vector3 newChildPosition = new Vector3(newChildSize.x * column, newChildSize.y * -row, 0);
        Vector3 offset = CalculateChildOffSets(newChildSize, column, row);
        child.localPosition = newChildPosition + offset;
    }

    private Vector3 CalculateChildOffSets(Vector3 newChildSize, int column, int row)
    {
        float gridOffSetX = 0.5f * (collunsCount - 1);
        float gridOffSetY = 0.5f * (rowsCount - 1);
        Vector3 offset = new Vector3(-newChildSize.x * gridOffSetX, newChildSize.y * gridOffSetY, 0);
        Vector3 spacingOffset = new Vector3(spacing.x * column, spacing.y * -row, 0);
        Vector3 gridSpacingOffSet = new Vector3(-spacing.x / 2 * (collunsCount - 1), spacing.y / 2 * (rowsCount - 1), 0);
        return   offset + spacingOffset + gridSpacingOffSet;
    }

    private void CalculateIndexes(Transform child, out int row, out int column)
    {
        int index = child.GetSiblingIndex();
        row = index / collunsCount;
        column = index % collunsCount;
    }

    private void CalculateNewChildSize(out Vector2 newChildSize)
    {
        float totalSpacingX = (collunsCount - 1) * spacing.x;
        float totalSpacingY = (rowsCount - 1) * spacing.y;
        newChildSize.x = (rectTransform.rect.size.x - totalSpacingX) / collunsCount;
        if (rowsCount == 0)
        {
            newChildSize.y = 0;
            return;
        }
        newChildSize.y = (rectTransform.rect.size.y - totalSpacingY) / rowsCount;
    }

    private void OnRectTransformDimensionsChange()
    {
        ChangeLayout();
    }

    private void ResizeContainer()
    {
        Vector2 newSize = customMinimunSize;
        newSize.y = math.max(customMinimunSize.y, rowsCount * (customMinimunSize.y / collunsCount));
        float totalSpacingY = (rowsCount - 1) * spacing.y;
        newSize.y += totalSpacingY;
        rectTransform.sizeDelta = newSize;
    }

    private void CalculateRowsCount()
    {
        rowsCount = Mathf.CeilToInt((float)transform.childCount / collunsCount);
    }
}
