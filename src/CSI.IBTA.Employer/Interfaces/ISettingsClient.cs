using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Employer.Interfaces
{
    public interface ISettingsClient
    {
        Task<GenericResponse<SettingsWithEmployerStateDto>> GetClaimSetting(int employerId);
        Task<GenericResponse<SettingsWithEmployerStateDto>> UpdateClaimSetting(
            int employerId, UpdateClaimSettingDto updateClaimSettingDto);
    }
}
