using UnityEngine;

public class MultiMethodBeatDetection : MonoBehaviour
{
    public DetectionToData detectionToData;
    public AudioClip audioClip;
    private AudioSource audioSource;
    private float[] samples = new float[1024];
    private float timeSinceStart = 0f;
    public float beatsPerMinute = 120f; 
    private float beatInterval;
    private float lastQuarterBeat = -1; // 마지막 감지된 박자
    public float minimumGap = 0.5f; // 최소 간격 설정 가능
    private float sensitivity = 0.05f;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.Play();
        beatInterval = 60f / beatsPerMinute; // 1박자에 해당하는 시간
    }

    void Update()
    {
        if (audioSource.isPlaying)
        {
            timeSinceStart += Time.deltaTime; 
            DetectBeats();
        }
    }

    void DetectBeats()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);

        bool beatDetected = false;

        // 주파수 스펙트럼 분석
        if (DetectBeatBySpectrum())
            beatDetected = true;

        // 오디오 에너지 감지
        if (DetectBeatByEnergy())
            beatDetected = true;

        // 피크 감지
        if (DetectBeatByPeak())
            beatDetected = true;

        // 중복 방지 및 0.25 단위 박자 계산
        if (beatDetected)
        {
            float timeInBeats = timeSinceStart / beatInterval; // 박자 단위로 변환
            float quarterBeat = Mathf.Floor(timeInBeats * 4) / 4; // 0.25 단위로 계산

            // 박자 간격이 최소 간격 이상인 경우만 감지
            if (quarterBeat != lastQuarterBeat && Mathf.Abs(quarterBeat - lastQuarterBeat) >= minimumGap)
            {
                Debug.Log($"Beat detected at quarter beat: {quarterBeat:F2}, time: {timeSinceStart:F2} seconds");
                detectionToData.BeatAdd(quarterBeat);
                lastQuarterBeat = quarterBeat; // 마지막 감지된 박자 업데이트
            }
        }
    }

    bool DetectBeatBySpectrum()
    {
        float threshold = sensitivity;
        for (int i = 0; i < samples.Length; i++)
        {
            if (samples[i] > threshold)
            {
                return true; // 비트 감지
            }
        }
        return false;
    }

    bool DetectBeatByEnergy()
    {
        float energy = 0f;
        for (int i = 0; i < samples.Length; i++)
        {
            energy += samples[i] * samples[i];
        }
        energy = Mathf.Sqrt(energy / samples.Length); // RMS 계산
        return energy > sensitivity; // 비트 감지
    }

    bool DetectBeatByPeak()
    {
        // 피크 감지 로직을 구현 (예: 간단한 배열에서 피크 찾기)
        return false; // 임시로 false 리턴
    }
}
