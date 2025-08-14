using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public enum ReticleType
    {
        Base,
        Grab,
        Talk,
        Wash,
        Place
    }
    [SerializeField] InteractionText interactionText;
    [SerializeField] InteractionText optionalText;
    [SerializeField] BasicUI baseReticle;
    [SerializeField] BasicUI grabReticle;
    [SerializeField] BasicUI talkReticle;
    [SerializeField] BasicUI washReticle;
    [SerializeField] BasicUI placeReticle;
    [SerializeField] GameObject startingImage;
    private BasicUI activeReticle;
    private void Awake()
    {
        CloseInteractionText();
        CloseOptionalText();
        baseReticle.Open();
        grabReticle.Close();
        talkReticle.Close();
        washReticle.Close();
        placeReticle.Close();
        activeReticle = baseReticle;
        startingImage.SetActive(true);
    }

    public void SetInteractionText(string text) => interactionText.Open(text);
    public void CloseInteractionText() => interactionText.Close();

    public void SetOptionalText(string text) => optionalText.Open(text);

    public void CloseOptionalText() => optionalText.Close();

    public void InitializeCanvas() => Destroy(startingImage);
    private BasicUI Reticle(ReticleType reticleType)
    {
        switch (reticleType)
        {
            case ReticleType.Grab: return grabReticle;
            case ReticleType.Talk: return talkReticle;
            case ReticleType.Wash: return washReticle;
            case ReticleType.Place: return placeReticle;
            default: return baseReticle;
        }
    }
    public void SetReticle(ReticleType reticleType)
    {
        activeReticle.Close();
        activeReticle = Reticle(reticleType);
        activeReticle.Open();
    }
}
