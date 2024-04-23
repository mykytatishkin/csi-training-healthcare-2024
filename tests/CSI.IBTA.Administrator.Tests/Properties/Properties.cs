using System.Runtime.CompilerServices;

// With this attribute, Tests assembly has an access to internal members of 
// CSI.IBTA.AuthService. Add extra InternalsVisibleToAttribute when needed.
[assembly: InternalsVisibleToAttribute("CSI.IBTA.AuthService")]