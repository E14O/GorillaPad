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
SOFTWARE.
*/

using GorillaLocomotion;
using GorillaNetworking;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using ExitGames.Client.Photon;

public class PadHolding : HoldableObject
{
    public bool
        InHand = false,
        InLeftHand = false,
        PickUp = true,
        DidSwap = false,
        SwappedLeft = true;

    public float
        GrabDistance = 0.23f,
        ThrowForce = 1.75f;

    public float InterpolationTime { get; set; }
    public Vector3 GrabPosition { get; set; }
    public Quaternion GrabQuaternion { get; set; }
    public Vector3 GrabScale { get; set; }

    public virtual void OnGrab(bool isLeft)
    {
        InterpolationTime = 0f;
        GrabPosition = transform.localPosition;
        GrabQuaternion = transform.localRotation;
        GrabScale = transform.localScale;

        transform.localScale = GorillaPad.Constants.RightHand.Scale;

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
        GrabPosition = transform.localPosition;
        GrabQuaternion = transform.localRotation;
        GrabScale = transform.localScale;

        transform.parent = VRRig.LocalRig.headMesh.transform.parent;
        
        transform.localScale = GorillaPad.Constants.Chest.Scale;

        Hashtable hash = new Hashtable();
        ExtensionMethods.AddOrUpdate(hash, "GPHolding", false);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash, null, null);
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

        HandleSmoothInterpolation();
    }

    private void HandleSmoothInterpolation()
    {
        if (InHand)
        {
            Vector3 targetPosition = InLeftHand ? GorillaPad.Constants.LeftHand.Position : GorillaPad.Constants.RightHand.Position;
            Quaternion targetRotation = InLeftHand ? GorillaPad.Constants.LeftHand.Rotation : GorillaPad.Constants.RightHand.Rotation;
            Vector3 targetScale = InLeftHand ? GorillaPad.Constants.LeftHand.Scale : GorillaPad.Constants.RightHand.Scale;

            transform.localPosition = Vector3.Lerp(GrabPosition, targetPosition, InterpolationTime);
            transform.localRotation = Quaternion.Lerp(GrabQuaternion, targetRotation, InterpolationTime);
            transform.localScale = Vector3.Lerp(GrabScale, targetScale, InterpolationTime);

            InterpolationTime += Time.deltaTime * 5f;
        }
        else
        {
            Vector3 targetPosition = GorillaPad.Constants.Chest.Position;
            Quaternion targetRotation = GorillaPad.Constants.Chest.Rotation;
            Vector3 targetScale = GorillaPad.Constants.Chest.Scale;

            transform.localPosition = Vector3.Lerp(GrabPosition, targetPosition, InterpolationTime);
            transform.localRotation = Quaternion.Lerp(GrabQuaternion, targetRotation, InterpolationTime);
            transform.localScale = Vector3.Lerp(GrabScale, targetScale, InterpolationTime);

            InterpolationTime += Time.deltaTime * 5f;
        }
    }

    public override void OnHover(InteractionPoint pointHovered, GameObject hoveringHand)
    {

    }

    public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
    {
    }

    public override void DropItemCleanup()
    {

    }


}