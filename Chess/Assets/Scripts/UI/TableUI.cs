using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TableUI : MonoBehaviour
{
    [SerializeField]
    Color32 blackHouseColor;
    [SerializeField]
    Color32 markedHouseColor;
    [SerializeField]
    GameObject markerPrefab;

    ObjectPool<GameObject> markerPrefabsPool;

    Event<OnHouseSelectedEvent> onHouseSelectedEvent = new Event<OnHouseSelectedEvent>();
    Event<OnShowPromotionSelection> onShowPromotionSelectionEvent = new Event<OnShowPromotionSelection>();

    void Start()
    {
        markerPrefabsPool = new ObjectPool<GameObject>(markerPrefab, DuplicatePrefab);
        markerPrefabsPool.SetPool(10);
        EventBus.instance.AddBroadCaster(onHouseSelectedEvent);
        EventBus.instance.Subscribe<OnTableChangedEvent>(HandleTableChanged);
        EventBus.instance.Subscribe<OnPieceSelectedEvent>(HandlePieceSelected);
        EventBus.instance.Subscribe<OnPawnReachedPromotion>(HandlePawnReachedPromotion);
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Transform child = transform.GetChild(i * 7 + j + i);
                BindButtonFunction(child, i, j);
                if ((i + j) % 2 != 0)
                {
                    SetHouseColor(child, blackHouseColor);
                }
            }
        }
    }

    void SetHouseColor(Transform house, Color32 color)
    {
        if(house == null) { Debug.Log("Invalid child"); return; }
        Image houseImageComponent = house.GetComponent<Image>();
        if (houseImageComponent == null) { Debug.Log("Invalid child"); return;}
        houseImageComponent.color = color;
    }
    
    void BindButtonFunction(Transform button, int x, int y)
    {
        if(button == null) { Debug.Log("Invalid child"); return;}
        Button buttonComponent = button.GetComponent<Button>();
        if(buttonComponent == null) { Debug.Log("Invalid child"); return;}
        buttonComponent.enabled = true;
        buttonComponent.onClick.AddListener(() =>  onHouseSelected(x, y));
    }

    void onHouseSelected(int x, int y)
    {
        BoardPosition coordinates = new BoardPosition(x, y);
        OnHouseSelectedEvent _event = new OnHouseSelectedEvent(coordinates);
        EventBus.instance.Invoke(_event);
    }

    void PlacePiece(int pieceCoordinateX, int pieceCoordinatey, string pieceFirstChar, bool isPieceWhite)
    {
        Transform button = transform.GetChild(pieceCoordinateX * 7 + pieceCoordinatey + pieceCoordinateX);
        TMP_Text text = button.GetChild(0).GetComponent<TMP_Text>();
        text.text = pieceFirstChar;
        text.color = isPieceWhite? Color.grey : Color.black;
    }

    void HandleTableChanged(OnTableChangedEvent _event)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                PlacePiece(i, j, _event.data[i, j], _event.colors[i,j]);
            }
        }
        markerPrefabsPool.ReturnAllToPool(TurnOffMarker);
    }

    void HandlePieceSelected(OnPieceSelectedEvent _event)
    {
        markerPrefabsPool.ReturnAllToPool(TurnOffMarker);
        foreach (BoardPosition possibleMove in _event.possibleMovesCoordinates)
        {
            Transform button = transform.GetChild(possibleMove.x * 7 + possibleMove.y + possibleMove.x);
            GameObject marker = markerPrefabsPool.GetOjbectFromPool();
            marker.transform.position = button.position;
            marker.SetActive(true);
        }
    }
 
    GameObject DuplicatePrefab(GameObject gameObject)
    {
        GameObject obj = Instantiate(gameObject, new Vector3(0, 0, 0), Quaternion.identity);
        obj.SetActive(false);
        obj.transform.SetParent(transform);
        return obj;
    }

    void TurnOffMarker(GameObject marker)
    {
        marker.SetActive(false);
    }

    void HandlePawnReachedPromotion(OnPawnReachedPromotion data)
    {
        BoardPosition pawnPosition = data.pawnPosition;
        Vector2 _position = transform.GetChild(pawnPosition.x * 7 + pawnPosition.y + pawnPosition.x).position;
        EventBus.instance.Invoke<OnShowPromotionSelection>(new OnShowPromotionSelection(_position));
        
    }

}
