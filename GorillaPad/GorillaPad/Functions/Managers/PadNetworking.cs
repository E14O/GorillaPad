using System.Collections;
using System.Collections.Generic;
using GorillaPad.Tools;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using ExitGames.Client.Photon;

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
            yield return new WaitForSeconds(4f);

            var GPlayerName = _Player.NickName;
            VRRig GPRig = GorillaGameManager.StaticFindRigForPlayer(_Player);
            GameObject PadToGive = Instantiate(ContentLoader.NetworkedPad);
            PadToGive.name = $"{GPlayerName}`s Pad";

            bool HoldingGP = !(bool)_Player.CustomProperties["GPHolding"];
			if (HoldingGP)
			{
				PadToGive.transform.localScale = Constants.Chest.Scale;

                bool IsLeft = !(bool)_Player.CustomProperties["GPIsLeft"];
				if (IsLeft)
				{
					PadToGive.transform.parent = GPRig.rightHandTransform;
                    PadToGive.transform.SetLocalPositionAndRotation(Constants.LeftHand.Position, Constants.LeftHand.Rotation);
                }
				else
				{
					PadToGive.transform.parent = GPRig.leftHandTransform;
                    PadToGive.transform.SetLocalPositionAndRotation(Constants.RightHand.Position, Constants.RightHand.Rotation);
                }
			}
			else
			{
				PadToGive.transform.parent = GPRig.headMesh.transform.parent;
                PadToGive.transform.SetLocalPositionAndRotation(Constants.Chest.Position, Constants.Chest.Rotation);
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

            VRRig rig = GorillaGameManager.StaticFindRigForPlayer(targetPlayer);
            bool HoldingGP = !(bool)targetPlayer.CustomProperties["GPHolding"];

            if (HoldingGP)
            {
                PadObj.transform.localScale = Constants.Chest.Scale;

                bool IsLeft = !(bool)targetPlayer.CustomProperties["GPIsLeft"];
                if (IsLeft)
                {
                    PadObj.transform.parent = rig.rightHandTransform;
                    PadObj.transform.SetLocalPositionAndRotation(Constants.LeftHand.Position, Constants.LeftHand.Rotation);
                }
                else
                {
                    PadObj.transform.parent = rig.leftHandTransform;
                    PadObj.transform.SetLocalPositionAndRotation(Constants.RightHand.Position, Constants.RightHand.Rotation);
                }
            }
            else
            {
                PadObj.transform.parent = rig.headMesh.transform.parent;
                PadObj.transform.SetLocalPositionAndRotation(Constants.Chest.Position, Constants.Chest.Rotation);
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
                Pad.Destroy();

            GorillaPads.Clear();
            NetworkedPlayers.Clear();
        }

        void Update()
        {
            foreach (var Pad in GorillaPads)
            {
                GameObject PadModel = Pad.transform.Find("Model").gameObject;
                if (NetworkedPlayers.TryGetValue(Pad, out Player NetPlayer))
                {
                    var NetRig = GorillaGameManager.StaticFindRigForPlayer(NetPlayer);
                    if (NetRig != null && NetRig.mainSkin != null)
                        PadModel.GetComponent<MeshRenderer>().material.color = NetRig.playerColor;
                }
            }
        }
    }
}
