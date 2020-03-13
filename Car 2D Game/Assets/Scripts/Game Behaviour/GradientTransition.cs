using UnityEngine;
using DG.Tweening;

namespace Assets.Scripts.Game_Behaviour
{
    public class GradientTransition: MonoBehaviour
    {
        public Material gradient;
        
        public Color dayTopColor = Color.white;
        public  Color dayBottomColor = Color.white;

        public Color nightTopColor = Color.white;
        public Color nightBottomColor = Color.white;


        private const string TAG = "Car";
        private Tweener _tweener;


        public void DayToNight()
        {
            _tweener?.Kill();
            _tweener = DOVirtual.Float(0f, 1f, 1.2f, (t)=> 
                { 
                    gradient.SetColor("_TopColor", Color.Lerp(dayTopColor, nightTopColor, t));
                    gradient.SetColor("_BottomColor", Color.Lerp(dayBottomColor, nightBottomColor, t));
                });
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(TAG))
            {
                DayToNight();

                Invoke("QuitGame", 2f);
            }
        }

        private void OnApplicationQuit()
        {
            gradient.SetColor("_TopColor", dayTopColor);
            gradient.SetColor("_BottomColor", dayBottomColor);
        }

        public void QuitGame()
        {
            Debug.Log("QUIT!");
            Application.Quit();
        }
    }
}
