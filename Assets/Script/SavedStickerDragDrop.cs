using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;


    public class SavedStickerDragDrop : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler
    {
        RectTransform rect;
        public string SaveStickerTag = "saveStickerTag";
        [SerializeField] private string DeleteGo = "Delete-Collider";
        bool isDelete = false;
        bool isdrag = false;
        GameObject deleteButton;

        private void Start()
        {
            rect = GetComponent<RectTransform>();
            StickerGame.instance.deleteButton.GetComponent<BoxCollider2D>().enabled = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            StickerGame.instance.backButton.interactable = false;
            transform.SetParent(StickerGame.instance.stickerHolder.transform);
            rect.anchoredPosition += eventData.delta / StickerGame.instance.canvasSavedDrag.scaleFactor;
            StickerGame.instance.deleteButton.GetComponent<BoxCollider2D>().enabled = true;
            transform.GetComponent<BoxCollider2D>().enabled = true;
            //transform.GetComponent<BoxCollider2D>().enabled = false;
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }

            isdrag = true;

            if (isdrag)
            {
                Input.multiTouchEnabled = false;
                if (rect.anchoredPosition.y > StickerGame.instance.gameBoundaries[0].transform.localPosition.y)
                {
                    rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, StickerGame.instance.gameBoundaries[0].transform.localPosition.y);
                }
                else if (rect.anchoredPosition.x < StickerGame.instance.gameBoundaries[1].transform.localPosition.x)
                {
                    rect.anchoredPosition = new Vector2(StickerGame.instance.gameBoundaries[1].transform.localPosition.x, rect.anchoredPosition.y);
                }
                else if (rect.anchoredPosition.x > StickerGame.instance.gameBoundaries[2].transform.localPosition.x)
                {
                    rect.anchoredPosition = new Vector2(StickerGame.instance.gameBoundaries[2].transform.localPosition.x, rect.anchoredPosition.y);
                }
                else if (rect.anchoredPosition.y < StickerGame.instance.gameBoundaries[3].transform.localPosition.y)
                {
                    rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, StickerGame.instance.gameBoundaries[3].transform.localPosition.y);
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            transform.SetParent(StickerGame.instance.mainbg.transform);
            SoundManager.instance.aud.PlayOneShot(SoundManager.instance.inGameClips[15], 1);
            //SoundManager.instance.PlayOneShot(gameAudioClip.sticker_click);

            if (isDelete)
            {
                transform.position = deleteButton.transform.position;
                DeleteAnim();
                DeleteButtonAnim();
            }
            //transform.GetComponent<BoxCollider2D>().enabled = false;
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            StickerGame.instance.backButton.interactable = true;
            transform.GetComponent<BoxCollider2D>().enabled = true;
            Input.multiTouchEnabled = true;
            isdrag = false;
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (SaveStickerTag == "saveStickerTag")
            {
                if (collision.transform.gameObject.name == DeleteGo)
                {
                    isDelete = true;
                    deleteButton = collision.gameObject;
                }
            }
        }

        public void OnCollisionExit2D(Collision2D collision)
        {
            isDelete = false;
        }

        public void DeleteAnim()
        {
            transform.DOScale(0f, 0.75f).OnComplete(CompleteDeleteAnim);
        }

        public void CompleteDeleteAnim()
        {
            Destroy(gameObject);
        }

        public void DeleteButtonAnim()
        {
            StickerGame.instance.deleteButton.transform.DOScale(1.1f, 0.3f).OnComplete(CompleteDeleteButtonAnim);
        }

        public void CompleteDeleteButtonAnim()
        {
            StickerGame.instance.deleteButton.transform.DOScale(1f, 0.3f);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            //transform.GetComponent<PinchZoomInZoomOutSticker>().isClicked = true;
        }
    }
