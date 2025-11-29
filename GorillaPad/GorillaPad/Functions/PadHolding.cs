using ExitGames.Client.Photon;
using GorillaLocomotion;
using GorillaNetworking;
using GorillaPad.Functions;
using GorillaPad.Functions.UI;
using GorillaPad.Tools;
using Photon.Pun;
using System.Collections;
using UnityEngine;

public enum GrabState { Mounted, InHand }

public class PadHolding : HoldableObject
{
    public bool InHand, InLeftHand;
    public bool PickUp = true;
    public float GrabDistance = 0.23f;

    public GrabState State { get; set; }
    public float InterpTime { get; set; }
    public Vector3 StartPos { get; set; }
    public Quaternion StartRot { get; set; }

    Transform padModel, padCanvas;
    bool didSwap, swappedLeft;

    void Awake()
    {
        padModel = transform.Find("Pad/Model");
        padCanvas = transform.Find("Pad/Canvas");
        if (padCanvas) padCanvas.localScale = Vector3.one;
        InterpTime = 1f;
        State = GrabState.Mounted;
    }

    void Grab(bool left)
    {
        transform.SetParent(null, true);

        Vector3 scale = new(0.098f, 0.098f, 0.098f);
        transform.localScale = scale;
        if (padModel) padModel.localScale = scale;
        InterpTime = 0f;
        State = GrabState.InHand;
        StartPos = transform.position;
        StartRot = transform.rotation;
        ExitGames.Client.Photon.Hashtable hash = new();
        ExtensionMethods.AddOrUpdate(hash, "GPHolding", true);
        ExtensionMethods.AddOrUpdate(hash, "GPIsLeft", left);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

        InHand = true;
        InLeftHand = left;

        StartCoroutine(AddReturnScript());

        if (ContentLoader.SignParent.transform.Find("Sign").GetComponent<PadButton>() == null)
        {
            PadButton.Create(ContentLoader.SignParent.transform, "Sign", SelectedAudio.ButtonAudio, PadHandler.ReturnPad);
        }
    }

    private IEnumerator AddReturnScript()
    {
        yield return new WaitForSeconds(5f);

        Transform sign = ContentLoader.SignParent.transform.Find("Sign");

        if (sign != null && sign.GetComponent<PadButton>() == null)
        {
            PadButton.Create(sign, "Sign", SelectedAudio.ButtonAudio, PadHandler.ReturnPad);
        }
    }

    void Drop()
    {
        InterpTime = 0f;
        State = GrabState.Mounted;
        StartPos = transform.position;
        StartRot = transform.rotation;
        ExitGames.Client.Photon.Hashtable hash = new();
        ExtensionMethods.AddOrUpdate(hash, "GPHolding", false);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    void FixedUpdate()
    {
        if (InterpTime >= 1f) return;

        switch (State)
        {
            case GrabState.Mounted:
                {
                    var chestParent = VRRig.LocalRig.headMesh.transform.parent;
                    var chestPos = chestParent.TransformPoint(GorillaPad.Constants.Chest.Position);
                    var chestRot = chestParent.rotation * GorillaPad.Constants.Chest.Rotation;

                    transform.position = Vector3.Lerp(StartPos, chestPos, InterpTime);
                    transform.rotation = Quaternion.Lerp(StartRot, chestRot, InterpTime);

                    InterpTime += Time.deltaTime * 5f;

                    if (InterpTime >= 1f)
                    {
                        transform.SetParent(chestParent, false);
                        transform.localPosition = GorillaPad.Constants.Chest.Position;
                        transform.localRotation = GorillaPad.Constants.Chest.Rotation;
                        Vector3 scale = GorillaPad.Constants.Chest.Scale;
                        transform.localScale = scale;
                        if (padModel) padModel.localScale = scale;
                    }
                    break;
                }

            case GrabState.InHand:
                {
                    if (InHand)
                    {
                        var parent = InLeftHand
                            ? GorillaTagger.Instance.offlineVRRig.leftHandTransform.parent
                            : GorillaTagger.Instance.offlineVRRig.rightHandTransform.parent;

                        var pos = InLeftHand
                            ? GorillaPad.Constants.LeftHand.Position
                            : GorillaPad.Constants.RightHand.Position;

                        var rot = InLeftHand
                            ? GorillaPad.Constants.LeftHand.Rotation
                            : GorillaPad.Constants.RightHand.Rotation;

                        transform.position = Vector3.Lerp(StartPos, parent.TransformPoint(pos), InterpTime);
                        transform.rotation = Quaternion.Lerp(StartRot, parent.rotation * rot, InterpTime);

                        InterpTime += Time.deltaTime * 5f;

                        if (InterpTime >= 1f)
                        {
                            transform.SetParent(parent, false);
                            transform.localPosition = pos;
                            transform.localRotation = rot;
                            Vector3 scale = new(0.098f, 0.098f, 0.098f);
                            transform.localScale = scale;
                            if (padModel) padModel.localScale = scale;
                        }
                    }
                    break;
                }
        }
    }

    void Update()
    {
        float leftGrip = ControllerInputPoller.instance.leftControllerGripFloat;
        float rightGrip = ControllerInputPoller.instance.rightControllerGripFloat;
        bool l = leftGrip >= 0.5f, r = rightGrip >= 0.5f;
        float dist = GrabDistance * GTPlayer.Instance.scale;

        if (didSwap && (!swappedLeft ? !l : !r)) didSwap = false;

        bool pickL = PickUp && l && Vector3.Distance(GTPlayer.Instance.LeftHand.controllerTransform.position, transform.position) < dist && !InHand && EquipmentInteractor.instance.leftHandHeldEquipment == null && !didSwap;
        bool swapL = InHand && l && r && !didSwap && Vector3.Distance(GTPlayer.Instance.LeftHand.controllerTransform.position, transform.position) < dist && !swappedLeft && EquipmentInteractor.instance.leftHandHeldEquipment == null;
        if (pickL || swapL)
        {
            didSwap = swapL;
            swappedLeft = true;
            InLeftHand = true;
            InHand = true;
            transform.SetParent(GorillaTagger.Instance.offlineVRRig.leftHandTransform.parent);
            GorillaTagger.Instance.StartVibration(true, 0.1f, 0.05f);
            EquipmentInteractor.instance.leftHandHeldEquipment = this;
            if (didSwap) EquipmentInteractor.instance.rightHandHeldEquipment = null;
            Grab(true);
        }
        else if (!l && InHand && InLeftHand)
        {
            InHand = false;
            InLeftHand = false;
            transform.SetParent(null);
            EquipmentInteractor.instance.leftHandHeldEquipment = null;
            Drop();
        }

        bool pickR = PickUp && r && Vector3.Distance(GTPlayer.Instance.RightHand.controllerTransform.position, transform.position) < dist && !InHand && EquipmentInteractor.instance.rightHandHeldEquipment == null && !didSwap;
        bool swapR = InHand && l && r && !didSwap && Vector3.Distance(GTPlayer.Instance.RightHand.controllerTransform.position, transform.position) < dist && swappedLeft && EquipmentInteractor.instance.rightHandHeldEquipment == null;
        if (pickR || swapR)
        {
            didSwap = swapR;
            swappedLeft = false;
            InLeftHand = false;
            InHand = true;
            transform.SetParent(GorillaTagger.Instance.offlineVRRig.rightHandTransform.parent);
            GorillaTagger.Instance.StartVibration(false, 0.1f, 0.05f);
            EquipmentInteractor.instance.rightHandHeldEquipment = this;
            if (didSwap) EquipmentInteractor.instance.leftHandHeldEquipment = null;
            Grab(false);
        }
        else if (!r && InHand && !InLeftHand)
        {
            InHand = false;
            transform.SetParent(null);
            EquipmentInteractor.instance.rightHandHeldEquipment = null;
            Drop();
        }
    }

    public override void OnHover(InteractionPoint pointHovered, GameObject hoveringHand) { }
    public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand) { }
    public override void DropItemCleanup() { }
}
