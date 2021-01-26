using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace KanaStudio
{
    public class DrawImage : MonoBehaviour
    {

        //draw setting
        public int imageWidth = 256;
        public int imageHeight = 128;
        public Color brushColor = Color.black;
        public int brushWidth = 5;
        public bool resetOnTouchUp = false;
        public bool resetOnTouchDown = true;

        //Listener
        public delegate void TouchListener(Vector2Int position);
        public TouchListener OnTouchDown;
        public TouchListener OnTouchUp;
        public TouchListener OnDrag;

        //Image
        private Image _image;
        private Sprite _sprite;
        private Texture2D _tex;
        private Color[] _pixels;
        private RectTransform _rectTransform;
        private Vector2 _sizeDiv2;

        private bool touching = false;
        private Vector2 lastPosition;


        void Start()
        {
            _image = gameObject.AddComponent<Image>();
            Reset();

            //Add EventTrigger To Image
            var trigger = GetComponent<EventTrigger>() ?? gameObject.AddComponent<EventTrigger>();
            //Down
            EventTrigger.Entry down = new EventTrigger.Entry();
            down.eventID = EventTriggerType.PointerDown;
            down.callback.AddListener((eventData) => { Down(eventData as PointerEventData); });
            trigger.triggers.Add(down);
            //Up
            EventTrigger.Entry up = new EventTrigger.Entry();
            up.eventID = EventTriggerType.PointerUp;
            up.callback.AddListener((eventData) => { Up(eventData as PointerEventData); });
            trigger.triggers.Add(up);
            //Listener
            OnTouchDown += OnTouchDownEvent;
            OnTouchUp += OnTouchUpEvent;
            OnDrag += OnTouchDragEvent;
        }
        private void Update()
        {
            if (touching)
            {
                Vector2 touchPos;
                if (Application.isEditor)
                {
                    touchPos = Input.mousePosition;
                }
                else
                {
                    touchPos = Input.GetTouch(0).position;
                }

                //call
                OnDrag(GetLocalPosition(Vector2Int.FloorToInt(touchPos)));

                //update
                _tex.SetPixels(_pixels);
                _tex.Apply();
            }
        }

        void Down(PointerEventData eventData)
        {
            touching = true;
            var pos = GetLocalPosition(eventData.position);
            OnTouchDown(pos);
        }

        void Up(PointerEventData eventData)
        {
            touching = false;
            var pos = GetLocalPosition(eventData.position);
            OnTouchUp(pos);
        }

        void OnTouchDownEvent(Vector2Int position)
        {
            var imgPosition = ScreenToImagePosition(position);
            lastPosition = imgPosition;

            if (resetOnTouchDown)
            {
                Reset();
            }
        }

        void OnTouchUpEvent(Vector2Int position)
        {
            var imgPosition = ScreenToImagePosition(position);
            DrawLine(lastPosition, imgPosition);

            if (resetOnTouchUp)
            {
                Reset();
            }
        }

        void OnTouchDragEvent(Vector2Int position)
        {
            var imgPosition = ScreenToImagePosition(position);
            DrawLine(lastPosition, imgPosition);
            lastPosition = imgPosition;
        }

        Vector2Int GetLocalPosition(Vector2 pos)
        {
            return Vector2Int.FloorToInt(pos - (Vector2)_rectTransform.position + _sizeDiv2);
        }

        Vector2Int ScreenToImagePosition(Vector2Int screenPosition)
        {
            var x = Mathf.FloorToInt(imageWidth / _rectTransform.rect.width * screenPosition.x);
            var y = Mathf.FloorToInt(imageHeight / _rectTransform.rect.height * screenPosition.y);
            return new Vector2Int(x, y);
        }

        /// <summary>
        /// Reset Canvas
        /// </summary>
        public void Reset()
        {
            _rectTransform = GetComponent<RectTransform>();
            _sizeDiv2 = _rectTransform.rect.size / 2;

            _tex = new Texture2D(imageWidth, imageHeight, TextureFormat.ARGB32, false);
            Color fillColor = Color.clear;
            _pixels = new Color[_tex.width * _tex.height];
            for (int i = 0; i < _pixels.Length; i++)
            {
                _pixels[i] = fillColor;
            }
            _tex.SetPixels(_pixels);
            _tex.Apply();

            _sprite = Sprite.Create(_tex, new Rect(0, 0, imageWidth, imageHeight), Vector2.zero);
            _image.sprite = _sprite;
        }

        /// <summary>
        /// DrawLine
        /// </summary>
        /// <param name="from">position of start of line</param>
        /// <param name="to">position of end of line</param>
        public void DrawLine(Vector2 from, Vector2 to)
        {

            var delta = to - from;
            var norm = delta.normalized;

            var pos = new Vector2(from.x, from.y);

            var x1 = Mathf.Min(from.x, to.x);
            var x2 = Mathf.Max(from.x, to.x);
            var y1 = Mathf.Min(from.y, to.y);
            var y2 = Mathf.Max(from.y, to.y);
            var fDel = new Vector2(x2, y2) - new Vector2(x1, y1);

            if (fDel.x == 0 && fDel.y == 0)
            {
                return;
            }

            float time;
            if (fDel.x == 0)
            {
                time = fDel.y / norm.y;
            }
            else
            {
                time = fDel.x / norm.x;
            }
            time = Mathf.Abs(time);

            for (int i = 0; i < time; i++)
            {
                var vecInt = Vector2Int.FloorToInt(pos);

                var xFrom = Mathf.Max(0, vecInt.x - brushWidth);
                var xTo = Mathf.Min(imageWidth, vecInt.x + brushWidth);

                var yFrom = Mathf.Max(0, vecInt.y - brushWidth);
                var yTo = Mathf.Min(imageHeight, vecInt.y + brushWidth);

                for (int x = xFrom; x < xTo; x++)
                {
                    for (int y = yFrom; y < yTo; y++)
                    {
                        var index = x + imageWidth * y;
                        _pixels.SetValue(brushColor, index);
                    }
                }
                pos += norm;
            }
        }
    }
}
