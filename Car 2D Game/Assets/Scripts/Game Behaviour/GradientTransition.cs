using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game_Behaviour
{
    public class GradientTransition: MonoBehaviour
    {
        public List<SpriteRenderer> sky;

        public Color A = Color.blue;
        public Color B = Color.blue;
        public float speed = 1.0f;

        private const string TAG = "Car";
        private float duration = 1.0f;

        private void Start()
        {
            var t = sky[0].color;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(TAG))
            {
                foreach (var spriteRenderer in sky)
                {
                    spriteRenderer.color = Color.Lerp(spriteRenderer.color, B, Mathf.PingPong(Time.time * speed, 1.0f));

                }
            }
        }

        Color HexToColor(string hex)
        {
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            return new Color32(r, g, b, 255);
        }


        void Update()
        {
            float lerp = Mathf.PingPong(Time.time, duration) / duration;
            //sRender.material.SetColor("_Color", Color.Lerp(HexToColor("fe805e"), HexToColor("fa8856"), lerp)); //bottom
            //sRender.material.SetColor("_Color2", Color.Lerp(HexToColor("527fc1"), HexToColor("7ae0ec"), lerp)); //top
        }
    }
}
