using UnityEngine;

public class AnimatedRectTransform : MonoBehaviour
{
    [SerializeField] internal float animationSpeed = 1;

    new RectTransform transform;
    CanvasGroup canvasGroup = null;
    float scaleLerpFactor = 1, posLerpFactor = 1, rotLerpFactor = 1, alphaLerpFactor = 1;
    (float src, float dst) src_dst_alpha = new();
    (Vector3 src, Vector3 dst) src_dst_sc = new();
    (Vector2 src, Vector2 dst) src_dst_pos = new();
    (Quaternion src, Quaternion dst, bool global) src_dst_rot = new();

    void Start()
    {
        transform = GetComponent<RectTransform>();
        TryGetComponent(out canvasGroup);
    }

    void Update()
    {
        if (scaleLerpFactor < 1)
            transform.localScale = Vector3.Lerp(src_dst_sc.src, src_dst_sc.dst, scaleLerpFactor);
        else if (scaleLerpFactor > 1)
        {
            transform.localScale = src_dst_sc.dst;
            scaleLerpFactor = 1;
        }
        if (posLerpFactor < 1)
            transform.anchoredPosition = Vector2.Lerp(src_dst_pos.src, src_dst_pos.dst, posLerpFactor);
        else if (posLerpFactor > 1)
        {
            transform.anchoredPosition = src_dst_pos.dst;
            posLerpFactor = 1;
        }
        if (rotLerpFactor < 1)
            if (src_dst_rot.global) transform.rotation = Quaternion.Lerp(src_dst_rot.src, src_dst_rot.dst, rotLerpFactor);
            else transform.localRotation = Quaternion.Lerp(src_dst_rot.src, src_dst_rot.dst, rotLerpFactor);
        else if (rotLerpFactor > 1)
        {
            if (src_dst_rot.global) transform.rotation = src_dst_rot.dst;
            else transform.localRotation = src_dst_rot.dst;
            rotLerpFactor = 1;
        }
        if (alphaLerpFactor < 1)
            canvasGroup.alpha = Mathf.Lerp(src_dst_alpha.src, src_dst_alpha.dst, alphaLerpFactor);
        else if (alphaLerpFactor > 1)
        {
            canvasGroup.alpha = src_dst_alpha.dst;
            alphaLerpFactor = 1;
        }

        if (scaleLerpFactor < 1)
            scaleLerpFactor += Time.deltaTime * animationSpeed;
        if (posLerpFactor < 1)
            posLerpFactor += Time.deltaTime * animationSpeed;
        if (rotLerpFactor < 1)
            rotLerpFactor += Time.deltaTime * animationSpeed;
        if (alphaLerpFactor < 1)
            alphaLerpFactor += Time.deltaTime * animationSpeed;
    }

    internal void ScaleAnimatedTo(Vector3 destination)
    {
        src_dst_sc = (transform.localScale, destination);
        scaleLerpFactor = 0;
    }
    internal void MoveAnimatedTo(Vector2 destination)
    {
        src_dst_pos = (transform.anchoredPosition, destination);
        posLerpFactor = 0;
    }
    internal void RotateAnimatedTo(Quaternion destination, Space relative = Space.Self)
    {
        src_dst_rot = (relative == Space.Self ? transform.localRotation : transform.rotation, destination, relative == Space.World);
        rotLerpFactor = 0;
    }
    internal void AlphaAnimatedTo(float destination)
    {
        if (canvasGroup)
        {
            src_dst_alpha = (canvasGroup.alpha, destination);
            alphaLerpFactor = 0;
        }
    }
}
