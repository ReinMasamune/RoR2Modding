using System.Security;
using System.Security.Permissions;
using R2API.Utils;


[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
[assembly: ManualNetworkRegistration]


namespace R2API.Utils
{
    using System;
    using System.ComponentModel;
    [AttributeUsage(AttributeTargets.Assembly)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class ManualNetworkRegistrationAttribute : Attribute { }
}