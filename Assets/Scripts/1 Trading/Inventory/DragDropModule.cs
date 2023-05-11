using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;

public class DragDropModule : MonoBehaviour, IEndDragHandler, IDragHandler, IBeginDragHandler
{
    public event Action<bool> OnMoveingObject;

    private Tween flyingOnSpotTween;
    private RectTransform rectToDrag;
    private float canvasScalerValue;

    public void InitDragingModule(Canvas workingCanvas)
    {
        canvasScalerValue = workingCanvas.scaleFactor;
        rectToDrag = GetComponent<RectTransform>();

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        OnMoveingObject?.Invoke(true);

        var canvasOverride = rectToDrag.gameObject.AddComponent<Canvas>();
        canvasOverride.overrideSorting = true;
        canvasOverride.sortingOrder = 3;
    }

    public void OnDrag(PointerEventData eventData)
    {
        flyingOnSpotTween?.Kill();

        rectToDrag.anchoredPosition += eventData.delta / canvasScalerValue;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(rectToDrag.gameObject.GetComponent<Canvas>());

        flyingOnSpotTween?.Kill();

        flyingOnSpotTween = rectToDrag.DOAnchorPos(Vector2.zero, 0.5f)
            .SetEase(Ease.OutBack)
            .OnKill(() => OnMoveingObject?.Invoke(false))
            .OnComplete(() =>
            {
                flyingOnSpotTween = null;
                OnMoveingObject?.Invoke(false);
            });
    }
}
