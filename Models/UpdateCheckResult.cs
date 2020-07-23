namespace EWeLink.Api.Models
{
    public class UpdateCheckResult
    {
        public UpdateCheckResult(bool hasUpdate, UpgradeInfo info)
        {
            this.HasUpdate = hasUpdate;
            this.Info = info;
        }

        public string Deviceid => this.Info.DeviceId;

        public bool HasUpdate { get; }

        public UpgradeInfo Info { get; }
    }
}