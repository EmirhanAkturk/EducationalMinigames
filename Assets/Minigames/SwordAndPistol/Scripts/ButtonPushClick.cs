using System.Collections;
using TMPro;
using UnityEngine;

namespace Minigames.SwordAndPistol.Scripts
{
    public class ButtonPushClick : MonoBehaviour
    {
        [SerializeField] private float maxLocalY = 0.55f;
        [SerializeField] private float minLocalY = 0.25f;
  
        [SerializeField] private bool isBeingTouched = false;
        [SerializeField] private bool isClicked = false;

        [SerializeField] private Material greenMat;

        [SerializeField] private GameObject timeCountDownCanvas;
        [SerializeField] private TextMeshProUGUI timeText;

        [SerializeField] private float smooth = 0.1f;
    
    
        // Start is called before the first frame update
        private void Start()
        {
            // Start with button up top / popped up
            var localPos = transform.localPosition;
            transform.localPosition = new Vector3(localPos.x, maxLocalY, localPos.z);
            timeCountDownCanvas.SetActive(false);
        }

        private void Update()
        {
            var localPos = transform.localPosition;

            var buttonDownPosition = new Vector3(localPos.x, minLocalY, localPos.z);
            var buttonUpPosition = new Vector3(localPos.x, maxLocalY, localPos.z);
            
            if (!isClicked)
            {
                if (!isBeingTouched && (localPos.y > maxLocalY  || localPos.y < maxLocalY))
                {
                    transform.localPosition = Vector3.Lerp(localPos, buttonUpPosition, Time.deltaTime * smooth);
                }

                if (localPos.y < minLocalY)
                {
                    isClicked = true;               
                    transform.localPosition = buttonDownPosition;
                    OnButtonDown();
                }
            }
      
        }

        private void OnButtonDown()
        {
            GetComponent<MeshRenderer>().material = greenMat;
            GetComponent<Collider>().isTrigger = true;

            ////Playing Sound
            AudioManager.instance.buttonClickSound.gameObject.transform.position = transform.position;
            AudioManager.instance.buttonClickSound.Play();

            //Start the game
            StartCoroutine(StartGame(3));
      
        }

        private IEnumerator StartGame(float countDownValue)
        {
            timeText.text = countDownValue.ToString();
            timeCountDownCanvas.SetActive(true);
            
            while (countDownValue > 0)
            {

                yield return new WaitForSeconds(1.0f);
                countDownValue -= 1;

                timeText.text = countDownValue.ToString();

            }
            
            MiniGameManager.Instance.LoadMiniGame(MiniGameType.SwordAndPistol);
            //Load Scene
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isClicked)
            {
                ////Playing Sound
                AudioManager.instance.buttonClickSound.gameObject.transform.position = transform.position;
                AudioManager.instance.buttonClickSound.Play();
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            if (!collision.collider.gameObject.CompareTag($"BackButton"))
            {
                isBeingTouched = true;
            }

        }

        private void OnCollisionExit(Collision collision)
        {
            if (!collision.collider.gameObject.CompareTag($"BackButton"))
            {
                isBeingTouched = false;

            }
        }
    }
}
