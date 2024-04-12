using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Employer.Interfaces
{
    public interface ISettingsClient
    {
        Task<GenericResponse<SettingsWithEmployerStateDto>> GetEmployerSetting(int employerId, string settingCondition);
        Task<GenericResponse<SettingsWithEmployerStateDto>> UpdateClaimSetting(
            int employerId, UpdateClaimSettingDto updateClaimSettingDto);
    }
}
