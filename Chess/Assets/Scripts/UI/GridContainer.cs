using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;


[ExecuteAlways]
public class GridContainer : MonoBehaviour
{

    [SerializeField] private int collunsCount = 2;
    [SerializeField] private Vector2 customMinimunSize = new Vector2(100, 100);
    [SerializeField] private Vector2 spacing = new Vector2(10, 10);
    RectTransform rectTransform;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnTransformChildrenChanged()
    {
        RefreshLayout();
    }

    private void OnRectTransformDimensionsChange()
    {
        RefreshLayout();
    }

    private void OnValidate()
    {
        RefreshLayout();
    }

    private List<ChildTransformation> CalculateChildTransformations()
    {
        Vector3 newChildSize = CalculateNewChildSize();

        int childCount = transform.childCount;

        return BuildChildrenTransformations(newChildSize, childCount);
    }

    private List<ChildTransformation> BuildChildrenTransformations(Vector3 newChildrenSize, int childCount)
    {
        List<ChildTransformation> childTransformations = new List<ChildTransformation>();
        for (int i = 0; i < childCount; i++)
        {
            int row = i / collunsCount;
            int column = i % collunsCount;
            Vector3 newChildPosition = CalculateChildPosition(row, column, newChildrenSize);
            childTransformations.Add(new ChildTransformation(newChildrenSize, newChildPosition));
        }

        return childTransformations;
    }

    private Vector3 CalculateChildPosition(int row, int column, Vector2 newChildSize)
    {

        Vector3 newChildPosition = new Vector3(newChildSize.x * column, newChildSize.y * -row, 0);
        Vector3 offset = CalculateChildOffSets(newChildSize, column, row);
        return newChildPosition + offset;
    }

    private Vector3 CalculateChildOffSets(Vector3 newChildSize, int column, int row)
    {
        int rowsCount = CalculateRowsCount();
        float gridOffSetX = 0.5f * (collunsCount - 1);
        float gridOffSetY = 0.5f * (rowsCount - 1);
        Vector3 offset = new Vector3(-newChildSize.x * gridOffSetX, newChildSize.y * gridOffSetY, 0);
        Vector3 spacingOffset = new Vector3(spacing.x * column, spacing.y * -row, 0);
        Vector3 gridSpacingOffSet = new Vector3(-spacing.x / 2 * (collunsCount - 1), spacing.y / 2 * (rowsCount - 1), 0);
        return offset + spacingOffset + gridSpacingOffSet;
    }

    private Vector3 CalculateNewChildSize()
    {
        int rowsCount = CalculateRowsCount();
        Vector3 newChildSize = new Vector3(0, 0, 0);
        float totalSpacingX = (collunsCount - 1) * spacing.x;
        float totalSpacingY = (rowsCount - 1) * spacing.y;
        newChildSize.x = (rectTransform.rect.size.x - totalSpacingX) / collunsCount;
        if (rowsCount == 0)
        {
            newChildSize.y = 0;
            return newChildSize;
        }
        newChildSize.y = (rectTransform.rect.size.y - totalSpacingY) / rowsCount;
        return newChildSize;
    }

    private Vector2 CalculateContainerSize()
    {
        int rowsCount = CalculateRowsCount();
        Vector2 newSize = customMinimunSize;
        newSize.y = math.max(customMinimunSize.y, rowsCount * (customMinimunSize.y / collunsCount));
        float totalSpacingY = (rowsCount - 1) * spacing.y;
        newSize.y += totalSpacingY;
        return newSize;
    }

    private int CalculateRowsCount()
    {
        return Mathf.CeilToInt((float)transform.childCount / collunsCount);
    }

    private void ApplyTransformations(List<ChildTransformation> childrenTransformations, Vector3 newContainerSize)
    {
        rectTransform.sizeDelta = newContainerSize;
        for (int i = 0; i < childrenTransformations.Count; i++)
        {
            RectTransform child = transform.GetChild(i) as RectTransform;
            child.sizeDelta = childrenTransformations[i].size;
            child.localPosition = childrenTransformations[i].position;
        }

    }

    private void RefreshLayout()
    {
        CalculateRowsCount();
        List<ChildTransformation> childTransformations = CalculateChildTransformations();
        Vector3 newContainerSize = CalculateContainerSize();
        ApplyTransformations(childTransformations, newContainerSize);
    }
}

public struct ChildTransformation
{
    public Vector3 size;
    public Vector3 position;
    public ChildTransformation(Vector3 size, Vector3 position)
    {
        this.size = size;
        this.position = position;
    }
}

