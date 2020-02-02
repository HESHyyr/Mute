using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct BlobState {
    public float _FresnelPow;
    public float _FresnelBias;
    public float _FresnelIntensity;
    public float _ReflectionIntensity;
    public float _Saturation;
}


public class EnvironmentController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float Saturation;
    [SerializeField] PlayerController playerController;
    [SerializeField] MeshRenderer blobMat;
    [SerializeField] Material mutedMat;
    [SerializeField] Material normalMat;
    [SerializeField] AnimationCurve dyingNoiseFreq;
    [SerializeField] AnimationCurve dyingNoiseAmp;
    [SerializeField] AnimationCurve dyingSpikeAmp;
    [SerializeField] AnimationCurve dyingSize;

    BlobState muted, normal;
    BlobState cur = new BlobState();

    bool isMuted = false;

    public static EnvironmentController instance;

    void Awake() {
        instance = this;
    }
    void Start()
    {
        CopyMaterialToState(mutedMat, ref muted);
        CopyMaterialToState(normalMat, ref normal);
        muted._Saturation = 1f;
        normal._Saturation = 0f;
    }

    void CopyMaterialToState( Material material, ref BlobState state){
        state._FresnelBias = material.GetFloat("_FresnelBias");
        state._FresnelPow = material.GetFloat("_FresnelPow");
        state._FresnelIntensity =material.GetFloat("_FresnelIntensity");
        state._ReflectionIntensity = material.GetFloat("_ReflectionIntensity");
    }

    // Update is called once per frame
    void Update()
    {
        BlobState targetState = (playerController.isMuted) ? muted : normal;
        LerpCurParamsTo(targetState);
        CopyParamsToBlob();

        //player health 
        float playerHealth = 1f - (playerController.playerHealth / 100f);
        blobMat.material.SetFloat("_NoiseAmp", dyingNoiseAmp.Evaluate(playerHealth));
        blobMat.material.SetFloat("_NoiseFreq", dyingNoiseFreq.Evaluate(playerHealth));
        blobMat.material.SetFloat("_SpikeAmp", dyingSpikeAmp.Evaluate(playerHealth));
        blobMat.material.SetFloat("_Size", dyingSize.Evaluate(playerHealth));
    }
    void LerpCurParamsTo(BlobState to){
        float t = 8f * Time.deltaTime;
        cur._FresnelBias = Mathf.MoveTowards(cur._FresnelBias, to._FresnelBias, t);
        cur._FresnelPow = Mathf.MoveTowards(cur._FresnelPow, to._FresnelPow, t);
        cur._FresnelIntensity = Mathf.MoveTowards(cur._FresnelIntensity, to._FresnelIntensity, t);
        cur._ReflectionIntensity = Mathf.MoveTowards(cur._ReflectionIntensity, to._ReflectionIntensity, t);
        cur._Saturation = Mathf.MoveTowards(cur._Saturation, to._Saturation, t);
    }

    Coroutine takingDamageRoutine = null;
    public void TakeDamage(){ 
        Debug.Log("hi");
        if(takingDamageRoutine == null){
            takingDamageRoutine = StartCoroutine(DamageRoutine());
        }
    }

    IEnumerator DamageRoutine() {
        float t = 0f;
        Debug.Log(t);
        while(t <= 1){
            blobMat.material.SetFloat("_HitAmount", t);
            t += 6f * Time.deltaTime;
            Debug.Log(t);
            yield return 0;
        }
        while(t >= 0){
            blobMat.material.SetFloat("_HitAmount", t);
            t -= 6f * Time.deltaTime;
            Debug.Log(t);
            yield return 0;
        }
        takingDamageRoutine = null;
        yield return 0;
    }
    void CopyParamsToBlob() {
        blobMat.material.SetFloat("_FresnelPow", cur._FresnelPow);
        blobMat.material.SetFloat("_FresnelBias", cur._FresnelBias);
        blobMat.material.SetFloat("_FresnelIntensity", cur._FresnelIntensity);
        blobMat.material.SetFloat("_ReflectionIntensity", cur._ReflectionIntensity);
        Shader.SetGlobalFloat("_Saturation", cur._Saturation);
    }
}
