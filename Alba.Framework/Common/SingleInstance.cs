using System;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
using Alba.Framework.IO;
using Alba.Framework.Security;

namespace Alba.Framework.Common
{
    public static class SingleInstance
    {
        private const string GlobalNamespacePrefix = "Global\\";
        private static readonly object sync = new object();
        private static Mutex mutex;
        private static string mutexName;

        /// <summary>Check if it is the first application instance.</summary>
        /// <param name="instanceName">Unique instance name. If null, executanle path is used.</param>
        /// <param name="isGlobal">Check globally, for all terminal sessions.</param>
        public static bool IsFirstInstance (string instanceName = null, bool isGlobal = true)
        {
            if (instanceName != null && instanceName.Contains("\\"))
                throw new ArgumentException("name");
            if (instanceName == null)
                instanceName = Paths.ExecutableFilePath.Replace('\\', '/');
            string newMutexName = isGlobal ? GlobalNamespacePrefix + instanceName : instanceName;
            lock (sync) {
                // can only be called with the same name
                if (!string.IsNullOrEmpty(mutexName) && string.CompareOrdinal(mutexName, newMutexName) != 0)
                    throw new ArgumentException("Must be unique for application.", "instanceName");
                // has already checked
                if (mutex != null)
                    return true;
                mutexName = newMutexName;

                bool isNew;
                try {
                    // try to create mutex with full permissions
                    mutex = new Mutex(true, mutexName, out isNew, CreateFullAccessMutexSecurity());
                }
                catch (UnauthorizedAccessException) {
                    // not new, another instance created mutex with limited permissions
                    return false;
                }
                if (!isNew) {
                    // not new, another instance exists
                    mutex.Close();
                    mutex = null;
                }
                return isNew;
            }
        }

        private static MutexSecurity CreateFullAccessMutexSecurity ()
        {
            // full permissions for all authorized users
            var mutexSecurity = new MutexSecurity();
            mutexSecurity.AddAccessRule(new MutexAccessRule(
                WellKnownSidType.CreatorOwnerSid.ToIdentifier(), MutexRights.FullControl, AccessControlType.Allow));
            mutexSecurity.AddAccessRule(new MutexAccessRule(
                WellKnownSidType.AuthenticatedUserSid.ToIdentifier(), MutexRights.FullControl, AccessControlType.Allow));
            return mutexSecurity;
        }
    }
}