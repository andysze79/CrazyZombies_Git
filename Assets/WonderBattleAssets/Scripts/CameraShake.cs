using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[ExecuteInEditMode][SaveDuringPlay][AddComponentMenu("")]
public class CameraShake : CinemachineExtension
{
    public float m_Range = 0.5f;

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Body) {
            Vector3 shakeAmount = GetOffset();
            state.PositionCorrection += shakeAmount;
        }
    }

    Vector3 GetOffset() {
        return new Vector3(
            Random.Range(-m_Range, m_Range),
            Random.Range(-m_Range, m_Range),
            Random.Range(-m_Range,m_Range));
    }
}
