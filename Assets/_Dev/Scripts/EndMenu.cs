using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Dev.Scripts
{
    public class EndMenu : MonoBehaviour
    {
        [HideInInspector] public int score;
        
        [Header("References")] 
        [SerializeField] private Image endMenuBackground;
        [SerializeField] private TextMeshProUGUI youDiedText;
        [SerializeField] private GameObject buttonScoreField;
        [SerializeField] private TextMeshProUGUI scoreText;
        private void OnEnable()
        {
            endMenuBackground.DOFade(0.7f, 2f);
            var youDiedTextTransform = youDiedText.transform;
            youDiedTextTransform.localScale = Vector3.zero;
            youDiedTextTransform.DOScale(Vector3.one, 2f).SetDelay(0.5f);
            scoreText.text = "";
            Invoke(nameof(OpenButtonScoreField),2f);
        }

        public void OpenButtonScoreField()
        {
            buttonScoreField.SetActive(true);
            var value = 0;
            DOTween.To(()=> value, x=> value = x, score, 1).OnUpdate(() => scoreText.text = value.ToString());
        }

        public void OnRestartButtonClick()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}