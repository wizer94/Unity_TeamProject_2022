using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory {

    /// <summary>
    /// 収束してイベントと内容を処理するところ
    /// </summary>

    [RequireComponent(typeof(CanvasGroup))]
    public abstract class ContentViewer : MonoBehaviour {

        public enum ViewerAnchor { TopLeft, TopRight, BottomLeft, BottomRight }
        public enum ViewerStandard { Slot, Cursor }

        [Header("Viewer Setting")]
        public RectTransform viewer; //Viewer（移動用）
        public CanvasGroup group; //Canvasグループ（Fade用）

        [Space (8f)]
        public ViewerAnchor anchor = ViewerAnchor.TopLeft; //Viewerのソート
        public ViewerStandard standard = ViewerStandard.Slot; //Viewerの位置の基準

        [Space(8f)]
        public float displayDelay = 0.5f; //カーソルを重ねる時Viewerが出る時間
        public float disappearDelay = 0.2f; //カーソルが離れた後Viewerがなくなる時間
        public float fadeDuration = 0.1f; //CanvasグループがFadeOutする時間
        public bool blockRaycast = false;

        public bool IsEnabled { get; private set; } = false;
        private IEnumerator displayViewer;
        private IEnumerator disappearViewer;
        private Vector3 viewerPos;

        private void Awake () {
            if (viewer == null) viewer = GetComponent<RectTransform> ();
            if (group == null) group = GetComponent<CanvasGroup> ();
            if (!viewer.gameObject.activeSelf) viewer.gameObject.SetActive (true); //ViewerOn

            //Canvasグループ初期設定
            group.alpha = 0;
            group.interactable = false;
            group.blocksRaycasts = false;
        }

        private void Start () {
            EventCall (); //イベント登録関数呼び出し（Overriding）
        }

        //ViewerOn
        public void ViewerEnable (InventorySlot slot) {
            IsEnabled = true;
            if (disappearViewer != null) StopCoroutine (disappearViewer);
            displayViewer = DisplayViewer (slot);
            StartCoroutine (displayViewer);
        }

        IEnumerator DisplayViewer (InventorySlot slot) {
            if (group.alpha <= 0) yield return new WaitForSeconds (displayDelay);

            if (IsEnabled) {

                //Contents内容の設定
                DrawContent (slot);

                //Viewerの位置設定（Slotを基準として）
                Vector2 viewerSize = new Vector2 (viewer.sizeDelta.x * viewer.localScale.x,
                    viewer.sizeDelta.y * viewer.localScale.y); //Viewerのサイズ読み込み

                if (standard == ViewerStandard.Slot) {

                    //Slotの位置、サイズ読み込み
                    RectTransform slot_rt = slot.GetComponent<RectTransform> ();
                    Vector3 slotPos = slot_rt.position;
                    Vector2 slotSize = new Vector2 (slot_rt.sizeDelta.x * slot_rt.localScale.x, 
                        slot_rt.sizeDelta.y * slot_rt.localScale.y);

                    viewerPos = slotPos; //Slotの位置にViewerの位置設定
                    viewerPos += GetFirstViewerPos (viewerSize.x / 2 + slotSize.x / 2, viewerSize.y / 2 - slotSize.y / 2);

                //Viewerの位置設定（カーソルを基準として）
                } else if (standard == ViewerStandard.Cursor) {
                    viewerPos = Input.mousePosition; //カーソルを位置でViewerの位置設定
                    viewerPos += GetFirstViewerPos (viewerSize.x / 2, viewerSize.y / 2);
                }

                viewerPos.x += GetFixedViewerPosX (viewerPos, viewerSize);
                viewerPos.y += GetFixedViewerPosY (viewerPos, viewerSize);
                viewer.position = viewerPos; //Viewerの位置適用

                //CanvasグループFadeIn
                group.interactable = true;
                if (blockRaycast) group.blocksRaycasts = true;

                if (group.alpha < 1) {
                    float firstAlpha = group.alpha;
                    float alpha = firstAlpha;
                    float time = 0f;

                    while (alpha < 1f && IsEnabled) {
                        time += Time.deltaTime / fadeDuration;
                        alpha = Mathf.Lerp (firstAlpha, 1, time);
                        group.alpha = alpha;
                        yield return null;
                    }
                }
            }
        }

        //ソートの基準によってViewerの初期位置設定
        private Vector3 GetFirstViewerPos (float posX, float posY) {
            switch (anchor) {
                case ViewerAnchor.TopLeft: return new Vector3 (-posX, posY);
                case ViewerAnchor.TopRight: return new Vector3 (posX, posY);
                case ViewerAnchor.BottomLeft: return new Vector3 (-posX, -posY);
                case ViewerAnchor.BottomRight: return new Vector3 (posX, -posY);
                default: return Vector3.zero;
            }
        }

        //ソートの基準（左、右）によってViewerの位置再設定
        private float GetFixedViewerPosX (Vector3 pos, Vector3 size) {
            switch (anchor) {
                case ViewerAnchor.TopLeft:
                case ViewerAnchor.BottomLeft:
                    float limit_Left = pos.x - size.x / 2;
                    if (limit_Left < 0) return -limit_Left;
                    else return 0;

                case ViewerAnchor.TopRight:
                case ViewerAnchor.BottomRight:
                    float limit_Right = Screen.width - (pos.x + size.x / 2);
                    if (limit_Right < 0) return limit_Right;
                    else return 0;

                default: return 0;
            }
        }

        //ソートの基準（上、下）によってViewerの位置再設定
        private float GetFixedViewerPosY (Vector3 pos, Vector3 size) {
            switch (anchor) {
                case ViewerAnchor.TopLeft:
                case ViewerAnchor.TopRight:
                    float limit_Top = Screen.height - (pos.y + size.y);
                    if (limit_Top < 0) return limit_Top;
                    else return 0;

                case ViewerAnchor.BottomLeft:
                case ViewerAnchor.BottomRight:
                    float limit_Bottom = pos.y - size.y;
                    if (limit_Bottom < 0) return -limit_Bottom;
                    else return 0;

                default: return 0;
            }
        }

        //Viewer表示をOFF
        public void ViewerDisable () {
            IsEnabled = false;
            if (displayViewer != null) StopCoroutine (displayViewer);
            disappearViewer = DisappearViewer ();
            StartCoroutine (disappearViewer);
        }

        IEnumerator DisappearViewer () {
            if (group.alpha >= 1) yield return new WaitForSeconds (disappearDelay);

            //ViewerのFadeOut
            if (!IsEnabled && group.alpha > 0) {
                float firstAlpha = group.alpha;
                float alpha = firstAlpha;
                float time = 0f;

                while (alpha > 0f && !IsEnabled) {
                    time += Time.deltaTime / fadeDuration;
                    alpha = Mathf.Lerp (firstAlpha, 0, time);
                    group.alpha = alpha;
                    yield return null;
                }

                group.interactable = false;
                group.blocksRaycasts = false;
            }
        }

        //abstractメソッド
        protected abstract void EventCall (); //イベント呼び出しメソッド
        protected abstract void OnDisplay (PointerEventData eventData, InventorySlot slot); //表示イベント
        protected abstract void OnDisappear (PointerEventData eventData, InventorySlot slot); //非表示イベント
        protected abstract void DrawContent (InventorySlot slot); //Viewerの内容設定メソッド
    }
}
