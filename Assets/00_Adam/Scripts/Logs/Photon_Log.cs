﻿using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System;

namespace Photon_IATK
{
    public class Photon_Log : MonoBehaviour
    {
        public TMPro.TextMeshProUGUI LogText;
        private int nextUpdate = 1;

        public void Awake()
        {

            if (LogText == null)
            {
                if (!HelperFunctions.GetComponentInChild<TMPro.TextMeshProUGUI>(out LogText, this.gameObject, System.Reflection.MethodBase.GetCurrentMethod())){

                    Debug.LogFormat(GlobalVariables.cOnDestory + "{0}." + GlobalVariables.endColor + " {1}: {2} -> {3} -> {4}", "No input found, destorying self", Time.realtimeSinceStartup, this.gameObject.name, this.GetType(), System.Reflection.MethodBase.GetCurrentMethod());

                    Destroy(this.gameObject);
                }
            }

            TMPro.TextMeshPro component;
            if (HelperFunctions.GetComponentInChild<TMPro.TextMeshPro>(out component, this.gameObject, System.Reflection.MethodBase.GetCurrentMethod()))
            {
                component.text = "Photon Log Window";
            }

            LogText.text = "<color=#FF0000>DESKTOP_Photon_Log.cs:</color> Photon Log";

        }

        private void Update()
        {
            if (Time.time >= nextUpdate)
            {
                nextUpdate = Mathf.FloorToInt(Time.time) + 1;
                UpdateEverySecond();
            }
        }

        void UpdateEverySecond()
        {
            LogText.text = formatText("Photon Log","");

            string IsConnected = PhotonNetwork.IsConnected.ToString();
            LogText.text += formatText("IsConnected", IsConnected);

            if (PhotonNetwork.IsConnected)
            {
                string IsMasterClient = PhotonNetwork.IsMasterClient.ToString();
                LogText.text += formatText("IsMasterClient", IsMasterClient);

                string PlayerCount = Convert.ToInt32(PhotonNetwork.CountOfPlayersInRooms).ToString();
                LogText.text += formatText("PlayerCount", PlayerCount);

                string ActiveSceneName = SceneManagerHelper.ActiveSceneName;
                LogText.text += formatText("ActiveSceneName", ActiveSceneName);

                //string CurrentLobby = PhotonNetwork.CurrentLobby.Name;
                //text.text += formatText("CurrentLobby", CurrentLobby);

                string GameVersion = PhotonNetwork.GameVersion;
                LogText.text += formatText("GameVersion", GameVersion);

                string GetPing = PhotonNetwork.GetPing().ToString();
                LogText.text += formatText("Ping", GetPing);

                addPlayerList();

                string CloudRegion = PhotonNetwork.CloudRegion;
                LogText.text += formatText("CloudRegion", CloudRegion);

                string BestRegionSummaryInPreferences = PhotonNetwork.BestRegionSummaryInPreferences;
                LogText.text += formatText("BestRegionSummaryInPreferences", BestRegionSummaryInPreferences);

                LogText.text += formatText("Cameras in Room", Camera.allCameras.Length.ToString());

                LogText.text += formatText("Ping", PhotonNetwork.GetPing().ToString());
                


                //string CurrentRoom = PhotonNetwork.CurrentRoom.Players;
            }
        }

        string formatText(string name, string message, bool isRed=true, bool addNewLine=true)
        {
            string colorStartRed = GlobalVariables.red;
            string colorStartGreen = GlobalVariables.green;
            string colorStartPurple = GlobalVariables.purple;
            string colorEnd = GlobalVariables.endColor;
            string newLine = GlobalVariables.newLine;

            string output = "";

            if (isRed)
            {
                output = colorStartRed + name + ": " + colorEnd + colorStartPurple + message + colorEnd;
            } else
            {
                output = colorStartGreen + name + ": " + colorEnd + colorStartPurple + message + colorEnd;
            }
            if (addNewLine)
            {
                output += newLine;
            } else
            {
                output += " ";
            }

            return output;
        }

        private void addPlayerList()
        {
            string playerLog = "";
            string PlayerList = PhotonNetwork.PlayerList.ToString();
            Player[] players = PhotonNetwork.PlayerList;

            foreach (Player player in players){
                //playerLog += formatText("Player", "", false, false);
                //playerLog += formatText("NickName", player.NickName, true, false);
                //playerLog += formatText("ActorNumber", player.ActorNumber.ToString(), true, false);
                //playerLog += formatText("UserId", player.UserId, true);

                playerLog += string.Format(GlobalVariables.green + "Nickname: {0}, UserID: {1}, ActorNumber: {2}, Local Player: {3}, Master Client: {4}" + GlobalVariables.endColor, player.NickName, player.UserId, player.NickName, player.IsLocal, player.IsMasterClient);
            }

            LogText.text += playerLog;
        }
    }
}
