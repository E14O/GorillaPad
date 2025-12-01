using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GorillaLocomotion;
using GorillaPad.Tools;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace GorillaPad.Functions.Managers
{
    public class PadNetworking : MonoBehaviourPunCallbacks
    {
        public List<GameObject> GorillaPads = new();
        private Dictionary<GameObject, Player> NetworkedPlayers = new();

        public override void OnJoinedRoom()
        {
            foreach (Player player in PhotonNetwork.PlayerListOthers)
            {
                if (player.CustomProperties.ContainsKey(Constants.CustomProp))
                {
                    StartCoroutine(GiveGorillaPad(player));
                }
            }
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (newPlayer.CustomProperties.ContainsKey(Constants.CustomProp))
            {
                StartCoroutine(GiveGorillaPad(newPlayer));
            }
        }

        private IEnumerator GiveGorillaPad(Player _Player)
        {
            VRRig GPRig = null;
            yield return new WaitForSeconds(4f);

            var GPlayerName = _Player.NickName;
            GameObject PadToGive = Instantiate(ContentLoader.NetworkedPad);
            PadToGive.SetActive(true);
            PadToGive.transform.SetLocalPositionAndRotation(new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
            PadToGive.name = $"{GPlayerName}`s Pad";
            PadLogging.LogInfo($"Given Gorilla Pad To: {GPlayerName}");

            bool HoldingGP = !(bool)_Player.CustomProperties["GPHolding"];
            bool MountedGP = !(bool)_Player.CustomProperties["GPMounted"];
            if (HoldingGP)
            {
                PadToGive.transform.localScale = Constants.Chest.Scale;

                bool IsLeft = (bool)_Player.CustomProperties["GPIsLeft"];
                if (IsLeft)
                {
                    PadToGive.transform.parent = GPRig.leftHandTransform;
                    PadToGive.transform.localScale = Vector3.one;
                    PadToGive.transform.SetLocalPositionAndRotation(Constants.LeftHand.Position, Constants.LeftHand.Rotation);
                    PadToGive.transform.localScale = Constants.LeftHand.Scale;
                }
                else
                {
                    PadToGive.transform.parent = GPRig.rightHandTransform;
                    PadToGive.transform.localScale = Vector3.one;
                    PadToGive.transform.SetLocalPositionAndRotation(Constants.RightHand.Position, Constants.RightHand.Rotation);
                    PadToGive.transform.localScale = Constants.RightHand.Scale;
                }
            }
            else if(MountedGP)
            {
                PadToGive.transform.parent = null;
                PadToGive.transform.localPosition = Vector3.zero;
            }
            else
            {
                PadToGive.transform.parent = GPRig.headMesh.transform.parent;
                PadToGive.transform.localScale = Vector3.one;
                PadToGive.transform.SetLocalPositionAndRotation(Constants.Chest.Position, Constants.Chest.Rotation);
                PadToGive.transform.localScale = Constants.Chest.Scale;
            }
            GorillaPads.Add(PadToGive);
            NetworkedPlayers[PadToGive] = _Player;
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
        {
            GameObject PadObj = null;
            foreach (var pad in NetworkedPlayers)
            {
                if (pad.Value == targetPlayer)
                {
                    PadObj = pad.Key;
                    break;
                }
            }

            if (PadObj == null)
                return;

            VRRig rig = GorillaGameManager.StaticFindRigForPlayer(targetPlayer);

            if (!targetPlayer.CustomProperties.ContainsKey("GPHolding") ||
                !targetPlayer.CustomProperties.ContainsKey("GPIsLeft"))
                return;

            bool HoldingGP = (bool)targetPlayer.CustomProperties["GPHolding"];
            bool IsLeft = (bool)targetPlayer.CustomProperties["GPIsLeft"];
            bool MountedGP = !(bool)targetPlayer.CustomProperties["GPMounted"];

            if (HoldingGP)
            {
                PadObj.transform.localScale = Constants.Chest.Scale;

                if (IsLeft)
                {
                    if (rig.leftHandTransform != null)
                    {
                        PadObj.transform.parent = rig.leftHandTransform;
                        PadObj.transform.localScale = Vector3.one;
                        PadObj.transform.SetLocalPositionAndRotation(Constants.LeftHand.Position, Constants.LeftHand.Rotation);
                        PadObj.transform.localScale = Constants.LeftHand.Scale;
                    }
                }
                else
                {
                    if (rig.rightHandTransform != null)
                    {
                        PadObj.transform.parent = rig.rightHandTransform;
                        PadObj.transform.localScale = Vector3.one;
                        PadObj.transform.SetLocalPositionAndRotation(Constants.RightHand.Position, Constants.RightHand.Rotation);
                        PadObj.transform.localScale = Constants.RightHand.Scale;
                    }
                }
            }
            else if (MountedGP)
            {
                PadObj.transform.parent = null;
                PadObj.transform.localPosition = Vector3.zero;
            }
            else
            {
                if (rig.headMesh != null)
                {
                    PadObj.transform.parent = rig.headMesh.transform.parent;
                    PadObj.transform.localScale = Vector3.one;
                    PadObj.transform.SetLocalPositionAndRotation(Constants.Chest.Position, Constants.Chest.Rotation);
                    PadObj.transform.localScale = Constants.Chest.Scale;
                }
            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            List<GameObject> PlayerLeftPads = new();

            foreach (var NetPlayer in NetworkedPlayers)
            {
                if (NetPlayer.Value == otherPlayer)
                    PlayerLeftPads.Add(NetPlayer.Key);
            }

            foreach (var pad in PlayerLeftPads)
            {
                if (pad != null)
                    Destroy(pad);

                GorillaPads.Remove(pad);
                NetworkedPlayers.Remove(pad);
            }

        }

        public override void OnLeftRoom()
        {
            foreach (var Pad in GorillaPads)
                Destroy(Pad);

            GorillaPads.Clear();
            NetworkedPlayers.Clear();
        }
        void Update()
        {
            foreach (var Pad in GorillaPads)
            {
                var model = Pad.transform.Find("Model");
                if (model == null)
                    continue;

                if (NetworkedPlayers.TryGetValue(Pad, out Player NetPlayer))
                {
                    var NetRig = GorillaGameManager.StaticFindRigForPlayer(NetPlayer);
                    if (NetRig != null && NetRig.mainSkin != null)
                        model.GetComponent<MeshRenderer>().material.color = NetRig.playerColor;
                }
            }
        }

    }
}
