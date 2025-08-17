using ExitGames.Client.Photon;
using GorillaLocomotion;
using GorillaNetworking;
using Photon.Pun;
using UnityEngine;

/* MIT License

Copyright (c) 2023 dev9998

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.*/

public enum ObjectGrabbyState
{
    Mounted,
    InHand,
    Awake
}

public class PadHolding : HoldableObject
{
    public bool InHand = false, InLeftHand = false, PickUp = true, DidSwap = false, SwappedLeft = true;

    public float GrabDistance = 0.23f, ThrowForce = 1.75f;

   
    public ObjectGrabbyState State { get; set; }
    public float InterpolationTime { get; set; }
    public Vector3 GrabPosition { get; set; }
    public Quaternion GrabQuaternion { get; set; }
    
    
    private Vector3 OriginalMountedPosition;
    private Quaternion OriginalMountedRotation;
    private Transform OriginalParent;

    private Transform PadModel; 
    private Transform PadCanvas; 

    void Awake()
    {
        PadModel = transform.Find("Pad/Model");
        PadCanvas = transform.Find("Pad/Canvas");

        if (PadCanvas != null)
            PadCanvas.localScale = Vector3.one;

       
        OriginalMountedPosition = transform.position;
        OriginalMountedRotation = transform.rotation;
        OriginalParent = transform.parent;

      
        InterpolationTime = 1f;
        State = ObjectGrabbyState.Mounted;
    }

    public virtual void OnGrab(bool isLeft)
    {
<<<<<<< Updated upstream
        transform.localScale = GorillaPad.Constants.RightHand.Scale;
=======
       
        Vector3 targetScale = new Vector3(0.098f, 0.098f, 0.098f);
        
    
        transform.localScale = targetScale;
        
     
        if (PadModel != null)
        {
            PadModel.localScale = targetScale;
           
        }
        
       
        Debug.Log($"Pad grabbed with {(isLeft ? "left" : "right")} hand. Main transform scale set to: {transform.localScale}");
        Debug.Log($"PadModel found: {PadModel != null}");

        
        InterpolationTime = 0f;
        State = ObjectGrabbyState.InHand;
        GrabPosition = transform.position; 
        GrabQuaternion = transform.rotation;
>>>>>>> Stashed changes

        if (isLeft)
        {
            Hashtable hash = new Hashtable();
            ExtensionMethods.AddOrUpdate(hash, "GPHolding", true);
            ExtensionMethods.AddOrUpdate(hash, "GPIsLeft", true);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash, null, null);
        }
        else
        {
            Hashtable hash = new Hashtable();
            ExtensionMethods.AddOrUpdate(hash, "GPHolding", true);
            ExtensionMethods.AddOrUpdate(hash, "GPIsLeft", false);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash, null, null);
        }
    }

    public virtual void OnDrop(bool isLeft)
    {
       
        InterpolationTime = 0f;
        State = ObjectGrabbyState.Mounted;
        GrabPosition = transform.position; 
        GrabQuaternion = transform.rotation; 
        
       
        transform.parent = VRRig.LocalRig.headMesh.transform.parent;
<<<<<<< Updated upstream

        transform.localScale = GorillaPad.Constants.Chest.Scale;
        transform.SetLocalPositionAndRotation(GorillaPad.Constants.Chest.Position, GorillaPad.Constants.Chest.Rotation);
=======
        
        
        Vector3 chestScale = GorillaPad.Constants.Chest.Scale;
        transform.localScale = chestScale;
        
      
        if (PadModel != null)
        {
            PadModel.localScale = chestScale;
            Debug.Log($"PadModel scaled to chest scale: {PadModel.localScale}");
        }
        
        Debug.Log($"Pad dropped. Main transform scale set to chest scale: {transform.localScale}");
>>>>>>> Stashed changes

        Hashtable hash = new Hashtable();
        ExtensionMethods.AddOrUpdate(hash, "GPHolding", false);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash, null, null);
    }

    public void FixedUpdate()
    {
        HandlePadState();
    }

    public void HandlePadState()
    {
        switch (State)
        {
            case ObjectGrabbyState.Mounted:
                
                if (InterpolationTime < 1f)
                {
                   
                    Vector3 chestWorldPos = VRRig.LocalRig.headMesh.transform.parent.TransformPoint(GorillaPad.Constants.Chest.Position);
                    Quaternion chestWorldRot = VRRig.LocalRig.headMesh.transform.parent.rotation * GorillaPad.Constants.Chest.Rotation;
                    
                    transform.position = Vector3.Lerp(GrabPosition, chestWorldPos, InterpolationTime);
                    transform.rotation = Quaternion.Lerp(GrabQuaternion, chestWorldRot, InterpolationTime);
                    InterpolationTime += Time.deltaTime * 5f;
                }
                break;

            case ObjectGrabbyState.InHand:
                if (InHand && InterpolationTime < 1f)
                {
                    Vector3 targetPosition = InLeftHand ? GorillaPad.Constants.LeftHand.Position : GorillaPad.Constants.RightHand.Position;
                    Quaternion targetRotation = InLeftHand ? GorillaPad.Constants.LeftHand.Rotation : GorillaPad.Constants.RightHand.Rotation;
                    
                   
                    Transform handParent = InLeftHand ? 
                        GorillaTagger.Instance.offlineVRRig.leftHandTransform.parent : 
                        GorillaTagger.Instance.offlineVRRig.rightHandTransform.parent;
                    
                    Vector3 handWorldPos = handParent.TransformPoint(targetPosition);
                    Quaternion handWorldRot = handParent.rotation * targetRotation;
                    
                    transform.position = Vector3.Lerp(GrabPosition, handWorldPos, InterpolationTime);
                    transform.rotation = Quaternion.Lerp(GrabQuaternion, handWorldRot, InterpolationTime);
                    InterpolationTime += Time.deltaTime * 5f;
                }
                break;
        }
    }

    public void Update()
    {
        float left = ControllerInputPoller.instance.leftControllerGripFloat;
        bool leftGrip = left >= 0.5f;

        float right = ControllerInputPoller.instance.rightControllerGripFloat;
        bool rightGrip = right >= 0.5f;

        var Distance = GrabDistance * GTPlayer.Instance.scale;
        if (DidSwap && (!SwappedLeft ? !leftGrip : !rightGrip))
            DidSwap = false;

        bool pickLeft = PickUp && leftGrip && Vector3.Distance(GTPlayer.Instance.leftControllerTransform.position, transform.position) < Distance && !InHand && EquipmentInteractor.instance.leftHandHeldEquipment == null && !DidSwap;
        bool swapLeft = InHand && leftGrip && rightGrip && !DidSwap && (Vector3.Distance(GTPlayer.Instance.leftControllerTransform.position, transform.position) < Distance) && !SwappedLeft && EquipmentInteractor.instance.leftHandHeldEquipment == null;
        if (pickLeft || swapLeft)
        {
            DidSwap = swapLeft;
            SwappedLeft = true;
            InLeftHand = true;
            InHand = true;

            transform.SetParent(GorillaTagger.Instance.offlineVRRig.leftHandTransform.parent);

            GorillaTagger.Instance.StartVibration(true, 0.1f, 0.05f);
            EquipmentInteractor.instance.leftHandHeldEquipment = this;
            if (DidSwap) EquipmentInteractor.instance.rightHandHeldEquipment = null;

            OnGrab(true);
        }
        else if (!leftGrip && InHand && InLeftHand)
        {
            InLeftHand = true;
            InHand = false;
            transform.SetParent(null);

            EquipmentInteractor.instance.leftHandHeldEquipment = null;
            OnDrop(true);
        }

        bool pickRight = PickUp && rightGrip && Vector3.Distance(GTPlayer.Instance.rightControllerTransform.position, transform.position) < Distance && !InHand && EquipmentInteractor.instance.rightHandHeldEquipment == null && !DidSwap;
        bool swapRight = InHand && leftGrip && rightGrip && !DidSwap && (Vector3.Distance(GTPlayer.Instance.rightControllerTransform.position, transform.position) < Distance) && SwappedLeft && EquipmentInteractor.instance.rightHandHeldEquipment == null;
        if (pickRight || swapRight)
        {
            DidSwap = swapRight;
            SwappedLeft = false;

            InLeftHand = false;
            InHand = true;
            transform.SetParent(GorillaTagger.Instance.offlineVRRig.rightHandTransform.parent);
            GorillaTagger.Instance.StartVibration(false, 0.1f, 0.05f);
            EquipmentInteractor.instance.rightHandHeldEquipment = this;
            if (DidSwap) EquipmentInteractor.instance.leftHandHeldEquipment = null;

            OnGrab(false);
        }
        else if (!rightGrip && InHand && !InLeftHand)
        {
            InLeftHand = false;
            InHand = false;
            transform.SetParent(null);

            EquipmentInteractor.instance.rightHandHeldEquipment = null;
            OnDrop(false);
        }
    }

    public override void OnHover(InteractionPoint pointHovered, GameObject hoveringHand) { }

    public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand) { }

    public override void DropItemCleanup() { }
}
