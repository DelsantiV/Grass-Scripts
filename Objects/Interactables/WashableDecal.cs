using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(DecalProjector))]
[RequireComponent(typeof(Collider))]
public class WashableDecal : MonoBehaviour, IInteractable
{
    [SerializeField] Material decalMaterial;
    [SerializeField] Material decalMaterialSelected;
    [SerializeField] int keyIDToWash;
    [SerializeField] private float washCooldown = 1f;
    [SerializeField] private float washStep = 0.25f;
    [SerializeField] private Animator soapAnimator;
    [SerializeField] private string worldMessage;
    public string ObjectName => string.Empty;
    private new Collider collider;
    public bool ShouldDisplayNameOnMouseOver => false;
    public bool NeedRefresh { get; set; }
    private DecalProjector projector;
    private InteractableObject soap;
    private ParticleSystem soapParticles;
    private Player player;
    private void Awake()
    {
        projector = gameObject.GetOrAddComponent<DecalProjector>();
        collider = GetComponent<Collider>();
        projector.material = decalMaterial;
        if (soapAnimator == null) 
        {
            soapAnimator = GetComponentInChildren<Animator>();
        }
        soapParticles = soapAnimator.gameObject.GetComponent<ParticleSystem>();
        NeedRefresh = false;
    }

    public void OnInteract(Player player)
    {
        if (!player.IsCurrentObjectKey(keyIDToWash) && keyIDToWash != 0) return;
        projector.fadeFactor = Mathf.Clamp01(projector.fadeFactor - washStep);
        if (keyIDToWash != 0)
        {
            soap = player.TakeObject();
            this.player = player;
            AnimateSoap();
        }
        StartCoroutine(CoolDown());
    }

    public void OnLookAt(Player player)
    {
        projector.material = decalMaterialSelected;
    }

    public void OnStopInteract(Player player)
    {
    }

    public void OnStopLookAt(Player player)
    {
        if (projector != null)
        {
            projector.material = decalMaterial;
        }
    }
    private IEnumerator CoolDown()
    {
        projector.material = decalMaterial;
        collider.enabled = false;
        yield return new WaitForSeconds(washCooldown);
        collider.enabled = true;
        if (soap != null)
        {
            soap.TryCollectObject(player);
        }
        if (soapParticles != null) soapParticles.Stop();

        if (projector.fadeFactor == 0f)
        {
            if (worldMessage != string.Empty) CanvasManager.Instance.SetWorldMessage(worldMessage);
            enabled = false;
            projector.enabled = false;
            Destroy(gameObject);
        }

    }
    private void AnimateSoap()
    {
        soap.transform.parent = soapAnimator.transform;
        soap.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        soapAnimator.SetTrigger("StartWashing");
        if (soapParticles != null) soapParticles.Play();
    }
}
