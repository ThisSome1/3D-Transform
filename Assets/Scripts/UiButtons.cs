using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine;
using TMPro;

public class UiButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] RectTransform tooltipRect;
    [SerializeField] TextMeshProUGUI tooltipTMP;
    [SerializeField] string text;

    static bool hovered = false;

    void Start()
    {
    }

    void Update()
    {
        if (!hovered) tooltipRect.gameObject.SetActive(false);
        else SetTooltipPosition();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovered = true;
        Invoke("ShowTooltip", 2);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("PointerExit");
        hovered = false;
    }

    void ShowTooltip()
    {
        if (hovered)
        {
            float width = 8;
            // int width = 10;
            // foreach (char c in text)
            // {
            //     tooltipTMP.font.sourceFontFile.GetCharacterInfo(text[0], out CharacterInfo info);
            //     width += info.glyphWidth;
            // }

            TMP_FontAsset fontAsset = tooltipTMP.font;
            float pointSizeScale = tooltipTMP.fontSize / (fontAsset.faceInfo.pointSize * fontAsset.faceInfo.scale);
            float emScale = tooltipTMP.fontSize * 0.01f;

            float styleSpacingAdjustment = tooltipTMP.fontStyle == FontStyles.Bold ? fontAsset.boldSpacing : 0;
            float normalSpacingAdjustment = fontAsset.normalSpacingOffset;

            for (int i = 0; i < text.Length; i++)
            {
                char unicode = text[i];
                // Make sure the given unicode exists in the font asset.
                if (fontAsset.characterLookupTable.TryGetValue(unicode, out TMP_Character character))
                    width += character.glyph.metrics.horizontalAdvance * pointSizeScale + (styleSpacingAdjustment + normalSpacingAdjustment) * emScale;
            }

            tooltipTMP.text = text;
            tooltipRect.pivot = new(0, 0);
            tooltipRect.sizeDelta = new(width, tooltipRect.sizeDelta.y);
            tooltipRect.gameObject.SetActive(true);
            SetTooltipPosition();
        }

    }
    void SetTooltipPosition()
    {
        Vector2 currentMousePos = Mouse.current.position.ReadValue();
        tooltipRect.transform.position = new(currentMousePos.x + 5, currentMousePos.y, 5);
    }
}