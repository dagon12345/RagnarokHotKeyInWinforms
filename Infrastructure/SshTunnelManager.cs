using Domain.Model.SettingModels;
using Renci.SshNet;
using System;

namespace Infrastructure
{
    public class SshTunnelManager: IDisposable
    {
        private readonly SshClient _client;
        private readonly ForwardedPortLocal _portForward;

        public SshTunnelManager(SshSettings ssh, DatabaseSettings db)
        {
            _client = new SshClient(ssh.Host, ssh.Port, ssh.Username, ssh.Password);
            _portForward = new ForwardedPortLocal("127.0.0.1", (uint)db.LocalPort, "127.0.0.1", (uint)db.RemotePort);
        }

        public void Start()
        {
            _client.Connect();
            _client.AddForwardedPort(_portForward);
            _portForward.Start();
        }

        public void Dispose()
        {
            _portForward?.Stop();
            _client?.Disconnect();
            _client?.Dispose();
        }
    }
}
