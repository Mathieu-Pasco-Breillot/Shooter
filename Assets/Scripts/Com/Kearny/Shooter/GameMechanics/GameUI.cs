using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Com.Kearny.Shooter.GameMechanics
{
    public class GameUI : MonoBehaviour
    {
        public Image fadePlane;
        public GameObject gameOverUI;

        private Camera mainCamera;

        // Start is called before the first frame update
        private void Start()
        {
            mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            mainCamera.enabled = false;
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            FindObjectOfType<Player.Player>().OnDeath += OnGameOver;
        }

        private void OnGameOver()
        {
            mainCamera.enabled = true;
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            StartCoroutine(Fade(Color.clear, Color.black, 1));
            gameOverUI.SetActive(true);
        }

        private IEnumerator Fade(Color from, Color to, float time)
        {
            to.a = .5f;

            var speed = 1 / time;
            var percent = 0f;

            while (percent < 1)
            {
                percent += Time.deltaTime * speed;
                fadePlane.color = Color.Lerp(from, to, percent);
                yield return null;
            }
        }

        // UI Input
        public void StartNewGame()
        {
            mainCamera.enabled = false;
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            SceneManager.LoadScene("FirstScene");
        }
    }
}