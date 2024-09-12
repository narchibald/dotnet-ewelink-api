namespace EWeLink.Api.Models;

using System.Runtime.Serialization;

public enum MotorCalibration
{
    [EnumMember(Value = "normal")]
    Normal,

    [EnumMember(Value = "calibration")]
    Calibration,
}