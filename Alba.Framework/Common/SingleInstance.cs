using System.Security.AccessControl;
using System.Security.Principal;
using Alba.Framework.IO;
using Alba.Framework.Security;
using Alba.Framework.Text;

namespace Alba.Framework.Common;

public static class SingleInstance
{
    private const string GlobalNamespacePrefix = "Global\\";
    private static readonly Lock Lock = new();
    private static Mutex? _Mutex;
    private static string? _MutexName;

    /// <summary>Check if it is the first application instance.</summary>
    /// <param name="instanceName">Unique instance name. If null, executanle path is used.</param>
    /// <param name="isGlobal">Check globally, for all terminal sessions.</param>
    [SuppressMessage("ReSharper", "ConvertIfStatementToConditionalTernaryExpression")]
    public static bool IsFirstInstance(string? instanceName = null, bool isGlobal = true)
    {
        if (instanceName?.Contains('\\') == true)
            throw new ArgumentException($"${nameof(instanceName)} must not contain '\\'", nameof(instanceName));

        instanceName ??= Paths.ExecutableFilePath.Replace('\\', '/');
        string newMutexName = isGlobal ? GlobalNamespacePrefix + instanceName : instanceName;
        lock (Lock) {
            // can only be called with the same name
            if (!_MutexName.IsNullOrEmpty() && string.CompareOrdinal(_MutexName, newMutexName) != 0)
                throw new ArgumentException($"{nameof(instanceName)} must be unique for application", nameof(instanceName));
            // has already checked
            if (_Mutex != null)
                return true;
            _MutexName = newMutexName;

            bool isNew;
            try {
                // try to create mutex with full permissions
                if (OperatingSystem.IsWindows()) {
                    var sec = new MutexSecurity();
                    sec.AddAccessRule(new(WellKnownSidType.CreatorOwnerSid.ToIdentifier(), MutexRights.FullControl, AccessControlType.Allow));
                    sec.AddAccessRule(new(WellKnownSidType.AuthenticatedUserSid.ToIdentifier(), MutexRights.FullControl, AccessControlType.Allow));
                  #if NET6_0_OR_GREATER
                    _Mutex = MutexAcl.Create(true, _MutexName, out isNew, sec);
                  #else
                    _Mutex = new(true, _MutexName, out isNew, sec);
                  #endif
                }
                else {
                    _Mutex = new(true, _MutexName, out isNew);
                }
            }
            catch (UnauthorizedAccessException) {
                // not new, another instance created mutex with limited permissions
                return false;
            }
            if (!isNew) {
                // not new, another instance exists
                _Mutex.Close();
                _Mutex = null;
            }
            return isNew;
        }
    }
}