using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
  [SerializeField] private NetworkPrefabRef _playerPrefab;
  [SerializeField] private NetworkPrefabRef _map;
  // [SerializeField] private NetworkObjectPo
  private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
  public Joystick joystick;

    void Awake()
    {
        if (joystick == null) {
            joystick = FindObjectOfType<Joystick>();
            if (joystick == null) print("Không thấy joystick trong scene!");
        }  
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
  {
      if (runner.IsServer)
      {
          // Create a unique position for the player
          Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.PlayerCount) * 3, 1, 0);
          NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
          // Keep track of the player avatars for easy access
          _spawnedCharacters.Add(player, networkPlayerObject);

          runner.Spawn(_map, new Vector3(0,0,0), Quaternion.identity);
      }
  }

  public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
  {
      if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
      {
          runner.Despawn(networkObject);
          _spawnedCharacters.Remove(player);
      }
  }
  private NetworkRunner _runner;

  async void StartGame(GameMode mode)
  {
      // Create the Fusion runner and let it know that we will be providing user input
      _runner = gameObject.AddComponent<NetworkRunner>();
      _runner.ProvideInput = true;

      // Create the NetworkSceneInfo from the current scene
      var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
      var sceneInfo = new NetworkSceneInfo();
      if (scene.IsValid) {
          sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
      }
      
      // Start or join (depends on gamemode) a session with a specific name
      await _runner.StartGame(new StartGameArgs()
      {
          GameMode = mode,
          SessionName = "TestRoom",
          Scene = scene,
          SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
      });
  }
  private void OnGUI()
  {
    if (_runner == null)
    {
      if (GUI.Button(new Rect(100,100,400,80), "Host"))
      {
          StartGame(GameMode.Host);
      }
      if (GUI.Button(new Rect(100,180,400,80), "Join"))
      {
          StartGame(GameMode.Client);
      }
    }
  }
  public void OnInput(NetworkRunner runner, NetworkInput input)
  {
    var data = new NetworkInputData();

    if (Input.GetKey(KeyCode.W))
      data.direction += new Vector3(0, 2, 0);

    if (Input.GetKey(KeyCode.S))
      data.direction += new Vector3(0, -2, 0);

    if (Input.GetKey(KeyCode.A))
      data.direction += Vector3.left;

    if (Input.GetKey(KeyCode.D))
      data.direction += Vector3.right;

    data.direction += new Vector3(joystick.Horizontal, joystick.Vertical, 0);

    data.fire = VirtualFireButtonNet.IsFiring || Input.GetKey(KeyCode.Space);
    input.Set(data);
  }
  public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
  public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
  public void OnConnectedToServer(NetworkRunner runner) { }
  public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
  public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
  public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
  public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
  public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
  public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
  public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
  public void OnSceneLoadDone(NetworkRunner runner) { }
  public void OnSceneLoadStart(NetworkRunner runner) { }
  public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
  public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
  public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
  public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
}