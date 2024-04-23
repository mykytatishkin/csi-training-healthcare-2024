using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Administrator.Models;

public class EmployerSettingsViewModel
{
    public int EmployerId { get; set; }
    public List<SettingsDto> EmployerSettings { get; set; } = new List<SettingsDto>();
}