using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance;
    public enum ReticleType
    {
        Base,
        Grab,
        Talk,
        Wash,
        Place,
        Lock,
        Open
    }
    [SerializeField] InteractionText interactionText;
    [SerializeField] InteractionText optionalText;
    [SerializeField] InteractionText worldMessageText;
    [SerializeField] BasicUI baseReticle;
    [SerializeField] BasicUI grabReticle;
    [SerializeField] BasicUI talkReticle;
    [SerializeField] BasicUI washReticle;
    [SerializeField] BasicUI placeReticle;
    [SerializeField] BasicUI lockReticle;
    [SerializeField] BasicUI openReticle;
    [SerializeField] GameObject startingImage;
    [SerializeField] TextMeshProUGUI moneyAmount;
    private BasicUI activeReticle;
    private void Awake()
    {
        Instance = this;
        CloseInteractionText();
        CloseOptionalText();
        worldMessageText.Close();
        baseReticle.Open();
        grabReticle.Close();
        talkReticle.Close();
        washReticle.Close();
        placeReticle.Close();
        lockReticle.Close();
        openReticle.Close();
        activeReticle = baseReticle;
        startingImage.SetActive(true);
    }

    public void SetInteractionText(string text) => interactionText.Open(text);
    public void CloseInteractionText() => interactionText.Close();

    public void SetOptionalText(string text) => optionalText.Open(text);

    public void CloseOptionalText() => optionalText.Close();
    public void SetWorldMessage(string text) => worldMessageText.Open(text);
    public void CloseWorldMessage() => worldMessageText.Close();
    public void SetMoneyAmount(int amount) => moneyAmount.SetText("Money: " + moneyAmount.ToString() + "$");
    public void InitializeCanvas() => Destroy(startingImage);
    private BasicUI Reticle(ReticleType reticleType)
    {
        switch (reticleType)
        {
            case ReticleType.Grab: return grabReticle;
            case ReticleType.Talk: return talkReticle;
            case ReticleType.Wash: return washReticle;
            case ReticleType.Place: return placeReticle;
            case ReticleType.Lock: return lockReticle;
            case ReticleType.Open: return openReticle;
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
