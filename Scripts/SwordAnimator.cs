using UnityEngine;

[System.Serializable]
public class AnimationKeyFrame
{
    public float time; // 시간 (0~1 범위)
    public Vector3 position; // 위치
    public Vector3 rotation; // 각도 (Euler Angles)
}

[System.Serializable]
public class AnimationClip
{
    public AnimationKeyFrame[] keyFrames;
}

public class SwordAnimator : MonoBehaviour
{
    public Transform swordTransform; // 칼의 트랜스폼
    public Transform target;
    public float lerpValue;
    public AnimationKeyFrame[] keyFrames; // 애니메이션 키프레임 배열
    public AnimationClip[] anims;
    public float animationDuration = 1.0f; // 애니메이션 총 지속 시간

    private float animationTime = 0.0f;
    private bool isAnimating = false;

    void Update()
    {
        LerpRotation();
        OperateAnimation();
    }
    public void SwingAnimate()
    {
        RandomAnimation();
        isAnimating = true;
        animationTime = 0.0f;
    }
    private void OperateAnimation()
    {
        // 애니메이션 진행 중인 경우
        if (isAnimating)
        {
            animationTime += Time.deltaTime / animationDuration;

            // 애니메이션 시간 클램프
            if (animationTime >= 1.0f)
            {
                animationTime = 1.0f;
                isAnimating = false;
            }

            // 현재 시간에 맞는 키프레임 계산
            int frameIndex = 0;
            for (int i = 0; i < keyFrames.Length; i++)
            {
                if (keyFrames[i].time > animationTime)
                {
                    break;
                }
                frameIndex = i;
            }

            // 보간을 위한 다음 키프레임 인덱스
            int nextFrameIndex = Mathf.Min(frameIndex + 1, keyFrames.Length - 1);
            float lerpFactor = 0.0f;

            // 다음 키프레임으로 보간 가능 여부 확인
            if (keyFrames[nextFrameIndex].time != keyFrames[frameIndex].time)
            {
                lerpFactor = (animationTime - keyFrames[frameIndex].time) / (keyFrames[nextFrameIndex].time - keyFrames[frameIndex].time);
            }

            // 위치와 각도 보간
            Vector3 position = Vector3.Lerp(keyFrames[frameIndex].position, keyFrames[nextFrameIndex].position, lerpFactor);
            Vector3 rotation = Vector3.Lerp(keyFrames[frameIndex].rotation, keyFrames[nextFrameIndex].rotation, lerpFactor);

            // NaN 값이 있는지 확인
            if (float.IsNaN(position.x) || float.IsNaN(position.y) || float.IsNaN(position.z) ||
                float.IsNaN(rotation.x) || float.IsNaN(rotation.y) || float.IsNaN(rotation.z))
            {
                Debug.LogError("NaN value detected in position or rotation.");
                return;
            }

            // 칼의 위치와 각도 업데이트
            swordTransform.localPosition = position;
            swordTransform.localRotation = Quaternion.Euler(rotation);
        }
    }
    
    private void LerpRotation()
    {
        //transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, lerpValue * Time.deltaTime);
        transform.rotation = target.rotation;
    }

    private void RandomAnimation()
    {
        keyFrames = anims[Random.Range(0, anims.Length)].keyFrames;
    }
}
