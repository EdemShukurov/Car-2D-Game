using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game_Behaviour
{
    public class GradientTransition: MonoBehaviour
    {
        public Material gradient;


        private const string TAG = "Car";
        private float _duration = 2.5f;
        private bool _carTouched;

        private string _initTopColor = "47188B";
        private string _initBottomColor = "BFBBCB";
        private string _targetTopColor = "fa8856";
        private string _targetBottomColor = "7ae0ec";

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(TAG))
            {
                _carTouched = true;

                //gradient.SetColor("_TopColor", Color.red); /*= Color.Lerp(spriteRenderer.color, B, Mathf.PingPong(Time.time * speed, 1.0f))*/
                //gradient.SetColor("_BottomColor", Color.white);
            }
        }

        private Color HexToColor(string hex)
        {
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            return new Color32(r, g, b, 255);
        }


        private void Update()
        {
            if(_carTouched)
            {
                float lerp = Mathf.PingPong(Time.time, _duration) / _duration;

                gradient.SetColor("_TopColor", Color.Lerp(HexToColor(_initTopColor), HexToColor(_targetTopColor), lerp));
                gradient.SetColor("_BottomColor", Color.Lerp(HexToColor(_initBottomColor), HexToColor(_targetBottomColor), lerp));

                Invoke("Stop", _duration);
                //StartCoroutine(ChangeColor(_targetTopColor, _targetBottomColor, _duration));
                //if (lerp > 0.005f)
                //{
                //    Debug.Log("lerp " + lerp);
                //    gradient.SetColor("_TopColor", Color.Lerp(HexToColor(_initTopColor), HexToColor(_targetTopColor), lerp)); 
                //    gradient.SetColor("_BottomColor", Color.Lerp(HexToColor(_initBottomColor), HexToColor(_targetBottomColor), lerp));

                //}
                //else
                //{
                //    _carTouched = false;
                //}

                //Swap(ref _initTopColor, ref _targetTopColor);
                //Swap(ref _initBottomColor, ref _targetBottomColor);
            }
        }


        private void Stop()
        {
            _carTouched = false;

            //Swap(ref _initTopColor, ref _targetTopColor);
            //Swap(ref _initBottomColor, ref _targetBottomColor);

        }
        /// <summary>
        /// Use Coroutine in order to change color smoothly
        /// </summary>
        /// <param name="targetAngle"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        private IEnumerator ChangeColor(string targetTopColor, string targetBottomColor, float duration)
        {
            float lerp;
            //Quaternion startAngle = pivot.transform.rotation;

            while (duration > 0.0f)
            {
                duration -= Time.deltaTime;
                lerp = Mathf.PingPong(Time.time, duration) / duration;

                gradient.SetColor("_TopColor", Color.Lerp(HexToColor(_initTopColor), HexToColor(_targetTopColor), lerp));
                gradient.SetColor("_BottomColor", Color.Lerp(HexToColor(_initBottomColor), HexToColor(_targetBottomColor), lerp));
                yield return null;
            }
        }

        private void Swap(ref string a, ref string b)
        {
            string temp = a;
            a = b;
            b = temp;
        }
    }
}
