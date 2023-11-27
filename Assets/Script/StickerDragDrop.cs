using UnityEngine;
using UnityEngine.EventSystems;


    public class StickerDragDrop : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        RectTransform rect;

        void Start()
        {
            rect = GetComponent<RectTransform>();
        }

        public void OnDrag(PointerEventData eventData)
        {
            rect.anchoredPosition += eventData.delta / StickerGame.instance.canvas.scaleFactor;
            StickerGame.instance.backButton.interactable = false;
            StickerGame.instance.sticlerPanelScroll.enabled = false;
            Input.multiTouchEnabled = false;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (transform.localPosition.y < 170f)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    Destroy(transform.GetChild(i).gameObject);
                }
                transform.SetParent(StickerGame.hit.transform);
                GetComponent<StickerDragDrop>().enabled = true;
                transform.localPosition = Vector3.zero;
            }
            else
            {
                transform.SetParent(StickerGame.instance.mainbg.transform);
                Destroy(GetComponent<StickerDragDrop>());
                gameObject.AddComponent<SavedStickerDragDrop>();
                SoundManager.instance.aud.PlayOneShot(SoundManager.instance.inGameClips[15]);
                //SoundManager.instance.PlayOneShot(gameAudioClip.sticker_click);
            }
            StickerGame.instance.sticlerPanelScroll.enabled = true;
            StickerGame.instance.backButton.interactable = true;
            StickerGame.instance.dragParentHolder.SetActive(false);
            Input.multiTouchEnabled = true;
        }
    }
