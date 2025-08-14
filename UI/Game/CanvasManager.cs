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
    [SerializeField] BasicUI baseReticle;
    [SerializeField] BasicUI grabReticle;
    [SerializeField] BasicUI talkReticle;
    [SerializeField] BasicUI washReticle;
    [SerializeField] BasicUI placeReticle;
    private BasicUI activeReticle;
    private void Awake()
    {
        CloseInteractionText();
        baseReticle.Open();
        grabReticle.Close();
        talkReticle.Close();
        washReticle.Close();
        placeReticle.Close();
        activeReticle = baseReticle;
    }

    public void SetInteractionText(string text) => interactionText.Open(text);
    public void CloseInteractionText() => interactionText.Close();

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
