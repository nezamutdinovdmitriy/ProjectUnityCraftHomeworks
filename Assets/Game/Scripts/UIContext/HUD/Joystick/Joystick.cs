using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UIContext
{
    public sealed class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [field: SerializeField]
        public Vector2 Direction { get; private set; }

        [SerializeField] private RectTransform _background;
        [SerializeField] private RectTransform _handle;

        private float _radius;

        private void Awake() => _radius = _background.rect.width * 0.5f;

        public void OnPointerDown(PointerEventData eventData) => HandleDrag(eventData);

        public void OnDrag(PointerEventData eventData) => HandleDrag(eventData);

        public void OnPointerUp(PointerEventData eventData)
        {
            Direction = Vector2.zero;
            _handle.localPosition = Vector2.zero;
        }

        private void HandleDrag(PointerEventData eventData)
        {
            Canvas canvas = GetComponentInParent<Canvas>();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _background,
                eventData.position,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
                out Vector2 localPoint
            );

            Vector2 clamped = Vector2.ClampMagnitude(localPoint, _radius);
            Direction = (clamped / _radius).normalized;
            _handle.localPosition = clamped;
        }
    }
}