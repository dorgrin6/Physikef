using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace MainMenu
{
    public class ButtonController : MonoBehaviour
    {
        [Inject] private ApplicationManager m_ApplicationManager;
        private bool m_gazedAt = false;
        private float m_progressBits;
        private float m_elapsedTime = 0f;
        private Slider m_fillBar;
        [SerializeField] float millsecsToFill = 200f;
        [SerializeField] float value = 0f;
        [SerializeField] private SceneEnum sceneEnum;


        enum SceneEnum
        {
            Back,
            CannonLaunch,
            Pendulum
        }

        // Use this for initialization
        void Start()
        {
            m_progressBits = millsecsToFill / 1000f;
            m_fillBar = GetComponent<Slider>();
        }

        public void SetGazedAt(bool gazedAt)
        {
            Debug.Log(string.Format("{0} Gazed at to {1}", this, gazedAt));
            this.m_gazedAt = gazedAt;
        }

        public void DoAction()
        {
            if (m_fillBar.value < 1f)
            {
                return;
            }

            switch (sceneEnum)
            {
                case SceneEnum.Back:
                    loadScene("MainMenu");
                    break;
                case SceneEnum.CannonLaunch:
                    loadScene("CannonLaunch");
                    break;
                case SceneEnum.Pendulum:
                    loadScene("Pendulum");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void loadScene(string sceneName)
        {
            // temporarily scene name = exercise name
            m_ApplicationManager.ChosenScene = sceneName;
            m_ApplicationManager.ChosenExercise = sceneName;
            SceneManager.LoadScene(sceneName);
        }

        // Update is called once per frame
        void Update()
        {
            if (m_fillBar.value >= 1f)
            {
                DoAction();
            }

            if (!m_gazedAt)
            {
                m_fillBar.value = 0f;
                return;
            }

            m_elapsedTime += Time.deltaTime;
            if (m_elapsedTime >= m_progressBits && m_fillBar.value < 1f)
            {
                m_elapsedTime = m_elapsedTime % m_progressBits;
                m_fillBar.value += m_progressBits;
            }
        }
    }
}