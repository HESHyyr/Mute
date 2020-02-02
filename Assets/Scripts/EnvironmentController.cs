using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct BlobState {
    public float _FresnelPow;
    public float _FresnelBias;
    public float _FresnelIntensity;
    public float _ReflectionIntensity;
}


public class EnvironmentController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float Saturation;

    [SerializeField] BlobState muted;
    [SerializeField] BlobState normal;
    BlobState cur = new BlobState();
    [SerializeField] MeshRenderer blobMat;
    public bool mute = false;

    bool isMuted = false;
    void Start()
    {
        muted._FresnelBias = 0.01f;
        muted._FresnelPow = 5.74f;
        muted._FresnelIntensity = 4.41f;
        muted._ReflectionIntensity = 1.81f;

        normal._FresnelBias = 0.75f;
        normal._FresnelPow = 3.32f;
        normal._FresnelIntensity = 0f;
        normal._ReflectionIntensity = 1.26f;
    }

    // Update is called once per frame
    void Update()
    {
        if(mute && !isMuted){
            isMuted = true;
            StartCoroutine(Mute());
        } else if(!mute && isMuted){
            isMuted = false;
            StartCoroutine(UnMute());
        }
    }

    IEnumerator Mute() {
        BlobState starting = cur;
        float t = 0f;
        float startingSaturation = Shader.GetGlobalFloat("_Saturation");
        while(t <= 1f){
            float sat = Mathf.Lerp(startingSaturation, 1f, t);
            Shader.SetGlobalFloat("_Saturation", sat);
            LerpCurParamsTo(starting, muted, t);
            CopyParamsToBlob();
            t += 5f * Time.deltaTime;
            yield return 0;
        }
        Shader.SetGlobalFloat("_Saturation", 1f);
        LerpCurParamsTo(starting, muted, 1f);
        CopyParamsToBlob();
        yield return 0;
    }

    IEnumerator UnMute() {
        BlobState starting = cur;
        float t = 0f;
        float startingSaturation = Shader.GetGlobalFloat("_Saturation");
        while(t <= 1f){
            float sat = Mathf.Lerp(startingSaturation, 0f, t);
            Shader.SetGlobalFloat("_Saturation", sat);
            LerpCurParamsTo(starting, normal, t);
            CopyParamsToBlob();
            t += 5f * Time.deltaTime;
            yield return 0;
        }
        Shader.SetGlobalFloat("_Saturation", 0f);
        LerpCurParamsTo(starting, normal, 1f);
        CopyParamsToBlob();
        yield return 0;
    }
    void LerpCurParamsTo(BlobState from, BlobState to, float t){
        cur._FresnelBias = Mathf.Lerp(from._FresnelBias, to._FresnelBias, t);
        cur._FresnelPow = Mathf.Lerp(from._FresnelPow, to._FresnelPow, t);
        cur._FresnelIntensity = Mathf.Lerp(from._FresnelIntensity, to._FresnelIntensity, t);
        cur._ReflectionIntensity = Mathf.Lerp(from._ReflectionIntensity, to._ReflectionIntensity, t);
    }

    void CopyParamsToBlob() {
        blobMat.material.SetFloat("_FresnelPow", cur._FresnelPow);
        blobMat.material.SetFloat("_FresnelBias", cur._FresnelBias);
        blobMat.material.SetFloat("_FresnelIntensity", cur._FresnelIntensity);
        blobMat.material.SetFloat("_ReflectionIntensity", cur._ReflectionIntensity);
    }
}
