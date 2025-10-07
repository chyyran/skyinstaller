namespace TrailsHelper.Support.WakeScope.Native
{
    using System;
    using Tmds.DBus.Protocol;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    record LogControl1Properties
    {
        public string LogLevel { get; set; } = default!;
        public string LogTarget { get; set; } = default!;
        public string SyslogIdentifier { get; set; } = default!;
    }
    partial class LogControl1 : login1Object
    {
        private const string __Interface = "org.freedesktop.LogControl1";
        public LogControl1(login1Service service, ObjectPath path) : base(service, path)
        { }
        public Task SetLogLevelAsync(string value)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("LogLevel");
                writer.WriteSignature("s");
                writer.WriteString(value);
                return writer.CreateMessage();
            }
        }
        public Task SetLogTargetAsync(string value)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("LogTarget");
                writer.WriteSignature("s");
                writer.WriteString(value);
                return writer.CreateMessage();
            }
        }
        public Task<string> GetLogLevelAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "LogLevel"), (m, s) => ReadMessage_v_s(m, (login1Object)s!), this);
        public Task<string> GetLogTargetAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "LogTarget"), (m, s) => ReadMessage_v_s(m, (login1Object)s!), this);
        public Task<string> GetSyslogIdentifierAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "SyslogIdentifier"), (m, s) => ReadMessage_v_s(m, (login1Object)s!), this);
        public Task<LogControl1Properties> GetPropertiesAsync()
        {
            return Connection.CallMethodAsync(CreateGetAllPropertiesMessage(__Interface), (m, s) => ReadMessage(m, (login1Object)s!), this);
            static LogControl1Properties ReadMessage(Message message, login1Object _)
            {
                var reader = message.GetBodyReader();
                return ReadProperties(ref reader);
            }
        }
        public ValueTask<IDisposable> WatchPropertiesChangedAsync(Action<Exception?, PropertyChanges<LogControl1Properties>> handler, bool emitOnCapturedContext = true, ObserverFlags flags = ObserverFlags.None)
        {
            return WatchPropertiesChangedAsync(__Interface, (m, s) => ReadMessage(m, (login1Object)s!), handler, emitOnCapturedContext, flags);
            static PropertyChanges<LogControl1Properties> ReadMessage(Message message, login1Object _)
            {
                var reader = message.GetBodyReader();
                reader.ReadString(); // interface
                List<string> changed = new(), invalidated = new();
                return new PropertyChanges<LogControl1Properties>(ReadProperties(ref reader, changed), ReadInvalidated(ref reader), changed.ToArray());
            }
            static string[] ReadInvalidated(ref Reader reader)
            {
                List<string>? invalidated = null;
                ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.String);
                while (reader.HasNext(arrayEnd))
                {
                    invalidated ??= new();
                    var property = reader.ReadString();
                    switch (property)
                    {
                        case "LogLevel": invalidated.Add("LogLevel"); break;
                        case "LogTarget": invalidated.Add("LogTarget"); break;
                        case "SyslogIdentifier": invalidated.Add("SyslogIdentifier"); break;
                    }
                }
                return invalidated?.ToArray() ?? Array.Empty<string>();
            }
        }
        private static LogControl1Properties ReadProperties(ref Reader reader, List<string>? changedList = null)
        {
            var props = new LogControl1Properties();
            ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.Struct);
            while (reader.HasNext(arrayEnd))
            {
                var property = reader.ReadString();
                switch (property)
                {
                    case "LogLevel":
                        reader.ReadSignature("s"u8);
                        props.LogLevel = reader.ReadString();
                        changedList?.Add("LogLevel");
                        break;
                    case "LogTarget":
                        reader.ReadSignature("s"u8);
                        props.LogTarget = reader.ReadString();
                        changedList?.Add("LogTarget");
                        break;
                    case "SyslogIdentifier":
                        reader.ReadSignature("s"u8);
                        props.SyslogIdentifier = reader.ReadString();
                        changedList?.Add("SyslogIdentifier");
                        break;
                    default:
                        reader.ReadVariantValue();
                        break;
                }
            }
            return props;
        }
    }
    record ManagerProperties
    {
        public bool EnableWallMessages { get; set; } = default!;
        public string WallMessage { get; set; } = default!;
        public uint NAutoVTs { get; set; } = default!;
        public string[] KillOnlyUsers { get; set; } = default!;
        public string[] KillExcludeUsers { get; set; } = default!;
        public bool KillUserProcesses { get; set; } = default!;
        public string RebootParameter { get; set; } = default!;
        public bool RebootToFirmwareSetup { get; set; } = default!;
        public ulong RebootToBootLoaderMenu { get; set; } = default!;
        public string RebootToBootLoaderEntry { get; set; } = default!;
        public string[] BootLoaderEntries { get; set; } = default!;
        public bool IdleHint { get; set; } = default!;
        public ulong IdleSinceHint { get; set; } = default!;
        public ulong IdleSinceHintMonotonic { get; set; } = default!;
        public string BlockInhibited { get; set; } = default!;
        public string BlockWeakInhibited { get; set; } = default!;
        public string DelayInhibited { get; set; } = default!;
        public ulong InhibitDelayMaxUSec { get; set; } = default!;
        public ulong UserStopDelayUSec { get; set; } = default!;
        public string[] SleepOperation { get; set; } = default!;
        public string HandlePowerKey { get; set; } = default!;
        public string HandlePowerKeyLongPress { get; set; } = default!;
        public string HandleRebootKey { get; set; } = default!;
        public string HandleRebootKeyLongPress { get; set; } = default!;
        public string HandleSuspendKey { get; set; } = default!;
        public string HandleSuspendKeyLongPress { get; set; } = default!;
        public string HandleHibernateKey { get; set; } = default!;
        public string HandleHibernateKeyLongPress { get; set; } = default!;
        public string HandleLidSwitch { get; set; } = default!;
        public string HandleLidSwitchExternalPower { get; set; } = default!;
        public string HandleLidSwitchDocked { get; set; } = default!;
        public string HandleSecureAttentionKey { get; set; } = default!;
        public ulong HoldoffTimeoutUSec { get; set; } = default!;
        public string IdleAction { get; set; } = default!;
        public ulong IdleActionUSec { get; set; } = default!;
        public bool PreparingForShutdown { get; set; } = default!;
        public Dictionary<string, VariantValue> PreparingForShutdownWithMetadata { get; set; } = default!;
        public bool PreparingForSleep { get; set; } = default!;
        public (string, ulong) ScheduledShutdown { get; set; } = default!;
        public string DesignatedMaintenanceTime { get; set; } = default!;
        public bool Docked { get; set; } = default!;
        public bool LidClosed { get; set; } = default!;
        public bool OnExternalPower { get; set; } = default!;
        public bool RemoveIPC { get; set; } = default!;
        public ulong RuntimeDirectorySize { get; set; } = default!;
        public ulong RuntimeDirectoryInodesMax { get; set; } = default!;
        public ulong InhibitorsMax { get; set; } = default!;
        public ulong NCurrentInhibitors { get; set; } = default!;
        public ulong SessionsMax { get; set; } = default!;
        public ulong NCurrentSessions { get; set; } = default!;
        public ulong StopIdleSessionUSec { get; set; } = default!;
    }
    partial class Manager : login1Object
    {
        private const string __Interface = "org.freedesktop.login1.Manager";
        public Manager(login1Service service, ObjectPath path) : base(service, path)
        { }
        public Task<ObjectPath> GetSessionAsync(string sessionId)
        {
            return Connection.CallMethodAsync(CreateMessage(), (m, s) => ReadMessage_o(m, (login1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "s",
                    member: "GetSession");
                writer.WriteString(sessionId);
                return writer.CreateMessage();
            }
        }
        public Task<ObjectPath> GetSessionByPIDAsync(uint pid)
        {
            return Connection.CallMethodAsync(CreateMessage(), (m, s) => ReadMessage_o(m, (login1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "u",
                    member: "GetSessionByPID");
                writer.WriteUInt32(pid);
                return writer.CreateMessage();
            }
        }
        public Task<ObjectPath> GetUserAsync(uint uid)
        {
            return Connection.CallMethodAsync(CreateMessage(), (m, s) => ReadMessage_o(m, (login1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "u",
                    member: "GetUser");
                writer.WriteUInt32(uid);
                return writer.CreateMessage();
            }
        }
        public Task<ObjectPath> GetUserByPIDAsync(uint pid)
        {
            return Connection.CallMethodAsync(CreateMessage(), (m, s) => ReadMessage_o(m, (login1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "u",
                    member: "GetUserByPID");
                writer.WriteUInt32(pid);
                return writer.CreateMessage();
            }
        }
        public Task<ObjectPath> GetSeatAsync(string seatId)
        {
            return Connection.CallMethodAsync(CreateMessage(), (m, s) => ReadMessage_o(m, (login1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "s",
                    member: "GetSeat");
                writer.WriteString(seatId);
                return writer.CreateMessage();
            }
        }
        public Task<(string, uint, string, string, ObjectPath)[]> ListSessionsAsync()
        {
            return Connection.CallMethodAsync(CreateMessage(), (m, s) => ReadMessage_arsussoz(m, (login1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "ListSessions");
                return writer.CreateMessage();
            }
        }
        public Task<(string, uint, string, string, uint, string, string, bool, ulong, ObjectPath)[]> ListSessionsExAsync()
        {
            return Connection.CallMethodAsync(CreateMessage(), (m, s) => ReadMessage_arsussussbtoz(m, (login1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "ListSessionsEx");
                return writer.CreateMessage();
            }
        }
        public Task<(uint, string, ObjectPath)[]> ListUsersAsync()
        {
            return Connection.CallMethodAsync(CreateMessage(), (m, s) => ReadMessage_arusoz(m, (login1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "ListUsers");
                return writer.CreateMessage();
            }
        }
        public Task<(string, ObjectPath)[]> ListSeatsAsync()
        {
            return Connection.CallMethodAsync(CreateMessage(), (m, s) => ReadMessage_arsoz(m, (login1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "ListSeats");
                return writer.CreateMessage();
            }
        }
        public Task<(string, string, string, string, uint, uint)[]> ListInhibitorsAsync()
        {
            return Connection.CallMethodAsync(CreateMessage(), (m, s) => ReadMessage_arssssuuz(m, (login1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "ListInhibitors");
                return writer.CreateMessage();
            }
        }
        public Task<(string SessionId, ObjectPath ObjectPath, string RuntimePath, System.Runtime.InteropServices.SafeHandle? FifoFd, uint Uid, string SeatId, uint Vtnr, bool Existing)> CreateSessionAsync(uint uid, uint pid, string service, string @type, string @class, string desktop, string seatId, uint vtnr, string tty, string display, bool remote, string remoteUser, string remoteHost, (string, VariantValue)[] properties)
        {
            return Connection.CallMethodAsync(CreateMessage(), (m, s) => ReadMessage_soshusub(m, (login1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "uusssssussbssa(sv)",
                    member: "CreateSession");
                writer.WriteUInt32(uid);
                writer.WriteUInt32(pid);
                writer.WriteString(service);
                writer.WriteString(@type);
                writer.WriteString(@class);
                writer.WriteString(desktop);
                writer.WriteString(seatId);
                writer.WriteUInt32(vtnr);
                writer.WriteString(tty);
                writer.WriteString(display);
                writer.WriteBool(remote);
                writer.WriteString(remoteUser);
                writer.WriteString(remoteHost);
                WriteType_arsvz(ref writer, properties);
                return writer.CreateMessage();
            }
        }
        public Task<(string SessionId, ObjectPath ObjectPath, string RuntimePath, System.Runtime.InteropServices.SafeHandle? FifoFd, uint Uid, string SeatId, uint Vtnr, bool Existing)> CreateSessionWithPIDFDAsync(uint uid, System.Runtime.InteropServices.SafeHandle pidfd, string service, string @type, string @class, string desktop, string seatId, uint vtnr, string tty, string display, bool remote, string remoteUser, string remoteHost, ulong flags, (string, VariantValue)[] properties)
        {
            return Connection.CallMethodAsync(CreateMessage(), (m, s) => ReadMessage_soshusub(m, (login1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "uhsssssussbssta(sv)",
                    member: "CreateSessionWithPIDFD");
                writer.WriteUInt32(uid);
                writer.WriteHandle(pidfd);
                writer.WriteString(service);
                writer.WriteString(@type);
                writer.WriteString(@class);
                writer.WriteString(desktop);
                writer.WriteString(seatId);
                writer.WriteUInt32(vtnr);
                writer.WriteString(tty);
                writer.WriteString(display);
                writer.WriteBool(remote);
                writer.WriteString(remoteUser);
                writer.WriteString(remoteHost);
                writer.WriteUInt64(flags);
                WriteType_arsvz(ref writer, properties);
                return writer.CreateMessage();
            }
        }
        public Task ReleaseSessionAsync(string sessionId)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "s",
                    member: "ReleaseSession");
                writer.WriteString(sessionId);
                return writer.CreateMessage();
            }
        }
        public Task ActivateSessionAsync(string sessionId)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "s",
                    member: "ActivateSession");
                writer.WriteString(sessionId);
                return writer.CreateMessage();
            }
        }
        public Task ActivateSessionOnSeatAsync(string sessionId, string seatId)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "ss",
                    member: "ActivateSessionOnSeat");
                writer.WriteString(sessionId);
                writer.WriteString(seatId);
                return writer.CreateMessage();
            }
        }
        public Task LockSessionAsync(string sessionId)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "s",
                    member: "LockSession");
                writer.WriteString(sessionId);
                return writer.CreateMessage();
            }
        }
        public Task UnlockSessionAsync(string sessionId)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "s",
                    member: "UnlockSession");
                writer.WriteString(sessionId);
                return writer.CreateMessage();
            }
        }
        public Task LockSessionsAsync()
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "LockSessions");
                return writer.CreateMessage();
            }
        }
        public Task UnlockSessionsAsync()
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "UnlockSessions");
                return writer.CreateMessage();
            }
        }
        public Task KillSessionAsync(string sessionId, string whom, int signalNumber)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "ssi",
                    member: "KillSession");
                writer.WriteString(sessionId);
                writer.WriteString(whom);
                writer.WriteInt32(signalNumber);
                return writer.CreateMessage();
            }
        }
        public Task KillUserAsync(uint uid, int signalNumber)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "ui",
                    member: "KillUser");
                writer.WriteUInt32(uid);
                writer.WriteInt32(signalNumber);
                return writer.CreateMessage();
            }
        }
        public Task TerminateSessionAsync(string sessionId)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "s",
                    member: "TerminateSession");
                writer.WriteString(sessionId);
                return writer.CreateMessage();
            }
        }
        public Task TerminateUserAsync(uint uid)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "u",
                    member: "TerminateUser");
                writer.WriteUInt32(uid);
                return writer.CreateMessage();
            }
        }
        public Task TerminateSeatAsync(string seatId)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "s",
                    member: "TerminateSeat");
                writer.WriteString(seatId);
                return writer.CreateMessage();
            }
        }
        public Task SetUserLingerAsync(uint uid, bool enable, bool interactive)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "ubb",
                    member: "SetUserLinger");
                writer.WriteUInt32(uid);
                writer.WriteBool(enable);
                writer.WriteBool(interactive);
                return writer.CreateMessage();
            }
        }
        public Task AttachDeviceAsync(string seatId, string sysfsPath, bool interactive)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "ssb",
                    member: "AttachDevice");
                writer.WriteString(seatId);
                writer.WriteString(sysfsPath);
                writer.WriteBool(interactive);
                return writer.CreateMessage();
            }
        }
        public Task FlushDevicesAsync(bool interactive)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "b",
                    member: "FlushDevices");
                writer.WriteBool(interactive);
                return writer.CreateMessage();
            }
        }
        public Task PowerOffAsync(bool interactive)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "b",
                    member: "PowerOff");
                writer.WriteBool(interactive);
                return writer.CreateMessage();
            }
        }
        public Task PowerOffWithFlagsAsync(ulong flags)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "t",
                    member: "PowerOffWithFlags");
                writer.WriteUInt64(flags);
                return writer.CreateMessage();
            }
        }
        public Task RebootAsync(bool interactive)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "b",
                    member: "Reboot");
                writer.WriteBool(interactive);
                return writer.CreateMessage();
            }
        }
        public Task RebootWithFlagsAsync(ulong flags)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "t",
                    member: "RebootWithFlags");
                writer.WriteUInt64(flags);
                return writer.CreateMessage();
            }
        }
        public Task HaltAsync(bool interactive)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "b",
                    member: "Halt");
                writer.WriteBool(interactive);
                return writer.CreateMessage();
            }
        }
        public Task HaltWithFlagsAsync(ulong flags)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "t",
                    member: "HaltWithFlags");
                writer.WriteUInt64(flags);
                return writer.CreateMessage();
            }
        }
        public Task SuspendAsync(bool interactive)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "b",
                    member: "Suspend");
                writer.WriteBool(interactive);
                return writer.CreateMessage();
            }
        }
        public Task SuspendWithFlagsAsync(ulong flags)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "t",
                    member: "SuspendWithFlags");
                writer.WriteUInt64(flags);
                return writer.CreateMessage();
            }
        }
        public Task HibernateAsync(bool interactive)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "b",
                    member: "Hibernate");
                writer.WriteBool(interactive);
                return writer.CreateMessage();
            }
        }
        public Task HibernateWithFlagsAsync(ulong flags)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "t",
                    member: "HibernateWithFlags");
                writer.WriteUInt64(flags);
                return writer.CreateMessage();
            }
        }
        public Task HybridSleepAsync(bool interactive)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "b",
                    member: "HybridSleep");
                writer.WriteBool(interactive);
                return writer.CreateMessage();
            }
        }
        public Task HybridSleepWithFlagsAsync(ulong flags)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "t",
                    member: "HybridSleepWithFlags");
                writer.WriteUInt64(flags);
                return writer.CreateMessage();
            }
        }
        public Task SuspendThenHibernateAsync(bool interactive)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "b",
                    member: "SuspendThenHibernate");
                writer.WriteBool(interactive);
                return writer.CreateMessage();
            }
        }
        public Task SuspendThenHibernateWithFlagsAsync(ulong flags)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "t",
                    member: "SuspendThenHibernateWithFlags");
                writer.WriteUInt64(flags);
                return writer.CreateMessage();
            }
        }
        public Task SleepAsync(ulong flags)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "t",
                    member: "Sleep");
                writer.WriteUInt64(flags);
                return writer.CreateMessage();
            }
        }
        public Task<string> CanPowerOffAsync()
        {
            return Connection.CallMethodAsync(CreateMessage(), (m, s) => ReadMessage_s(m, (login1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "CanPowerOff");
                return writer.CreateMessage();
            }
        }
        public Task<string> CanRebootAsync()
        {
            return Connection.CallMethodAsync(CreateMessage(), (m, s) => ReadMessage_s(m, (login1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "CanReboot");
                return writer.CreateMessage();
            }
        }
        public Task<string> CanHaltAsync()
        {
            return Connection.CallMethodAsync(CreateMessage(), (m, s) => ReadMessage_s(m, (login1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "CanHalt");
                return writer.CreateMessage();
            }
        }
        public Task<string> CanSuspendAsync()
        {
            return Connection.CallMethodAsync(CreateMessage(), (m, s) => ReadMessage_s(m, (login1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "CanSuspend");
                return writer.CreateMessage();
            }
        }
        public Task<string> CanHibernateAsync()
        {
            return Connection.CallMethodAsync(CreateMessage(), (m, s) => ReadMessage_s(m, (login1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "CanHibernate");
                return writer.CreateMessage();
            }
        }
        public Task<string> CanHybridSleepAsync()
        {
            return Connection.CallMethodAsync(CreateMessage(), (m, s) => ReadMessage_s(m, (login1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "CanHybridSleep");
                return writer.CreateMessage();
            }
        }
        public Task<string> CanSuspendThenHibernateAsync()
        {
            return Connection.CallMethodAsync(CreateMessage(), (m, s) => ReadMessage_s(m, (login1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "CanSuspendThenHibernate");
                return writer.CreateMessage();
            }
        }
        public Task<string> CanSleepAsync()
        {
            return Connection.CallMethodAsync(CreateMessage(), (m, s) => ReadMessage_s(m, (login1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "CanSleep");
                return writer.CreateMessage();
            }
        }
        public Task ScheduleShutdownAsync(string @type, ulong usec)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "st",
                    member: "ScheduleShutdown");
                writer.WriteString(@type);
                writer.WriteUInt64(usec);
                return writer.CreateMessage();
            }
        }
        public Task<bool> CancelScheduledShutdownAsync()
        {
            return Connection.CallMethodAsync(CreateMessage(), (m, s) => ReadMessage_b(m, (login1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "CancelScheduledShutdown");
                return writer.CreateMessage();
            }
        }
        public Task<System.Runtime.InteropServices.SafeHandle?> InhibitAsync(string what, string who, string why, string mode)
        {
            return Connection.CallMethodAsync(CreateMessage(), (m, s) => ReadMessage_h(m, (login1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "ssss",
                    member: "Inhibit");
                writer.WriteString(what);
                writer.WriteString(who);
                writer.WriteString(why);
                writer.WriteString(mode);
                return writer.CreateMessage();
            }
        }
        public Task<string> CanRebootParameterAsync()
        {
            return Connection.CallMethodAsync(CreateMessage(), (m, s) => ReadMessage_s(m, (login1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "CanRebootParameter");
                return writer.CreateMessage();
            }
        }
        public Task SetRebootParameterAsync(string parameter)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "s",
                    member: "SetRebootParameter");
                writer.WriteString(parameter);
                return writer.CreateMessage();
            }
        }
        public Task<string> CanRebootToFirmwareSetupAsync()
        {
            return Connection.CallMethodAsync(CreateMessage(), (m, s) => ReadMessage_s(m, (login1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "CanRebootToFirmwareSetup");
                return writer.CreateMessage();
            }
        }
        public Task SetRebootToFirmwareSetupAsync(bool enable)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "b",
                    member: "SetRebootToFirmwareSetup");
                writer.WriteBool(enable);
                return writer.CreateMessage();
            }
        }
        public Task<string> CanRebootToBootLoaderMenuAsync()
        {
            return Connection.CallMethodAsync(CreateMessage(), (m, s) => ReadMessage_s(m, (login1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "CanRebootToBootLoaderMenu");
                return writer.CreateMessage();
            }
        }
        public Task SetRebootToBootLoaderMenuAsync(ulong timeout)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "t",
                    member: "SetRebootToBootLoaderMenu");
                writer.WriteUInt64(timeout);
                return writer.CreateMessage();
            }
        }
        public Task<string> CanRebootToBootLoaderEntryAsync()
        {
            return Connection.CallMethodAsync(CreateMessage(), (m, s) => ReadMessage_s(m, (login1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "CanRebootToBootLoaderEntry");
                return writer.CreateMessage();
            }
        }
        public Task SetRebootToBootLoaderEntryAsync(string bootLoaderEntry)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "s",
                    member: "SetRebootToBootLoaderEntry");
                writer.WriteString(bootLoaderEntry);
                return writer.CreateMessage();
            }
        }
        public Task SetWallMessageAsync(string wallMessage, bool enable)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "sb",
                    member: "SetWallMessage");
                writer.WriteString(wallMessage);
                writer.WriteBool(enable);
                return writer.CreateMessage();
            }
        }
        public ValueTask<IDisposable> WatchSecureAttentionKeyAsync(Action<Exception?, (string SeatId, ObjectPath ObjectPath)> handler, bool emitOnCapturedContext = true, ObserverFlags flags = ObserverFlags.None)
            => WatchSignalAsync(Service.Destination, __Interface, Path, "SecureAttentionKey", (m, s) => ReadMessage_so(m, (login1Object)s!), handler, emitOnCapturedContext, flags);
        public ValueTask<IDisposable> WatchSessionNewAsync(Action<Exception?, (string SessionId, ObjectPath ObjectPath)> handler, bool emitOnCapturedContext = true, ObserverFlags flags = ObserverFlags.None)
            => WatchSignalAsync(Service.Destination, __Interface, Path, "SessionNew", (m, s) => ReadMessage_so(m, (login1Object)s!), handler, emitOnCapturedContext, flags);
        public ValueTask<IDisposable> WatchSessionRemovedAsync(Action<Exception?, (string SessionId, ObjectPath ObjectPath)> handler, bool emitOnCapturedContext = true, ObserverFlags flags = ObserverFlags.None)
            => WatchSignalAsync(Service.Destination, __Interface, Path, "SessionRemoved", (m, s) => ReadMessage_so(m, (login1Object)s!), handler, emitOnCapturedContext, flags);
        public ValueTask<IDisposable> WatchUserNewAsync(Action<Exception?, (uint Uid, ObjectPath ObjectPath)> handler, bool emitOnCapturedContext = true, ObserverFlags flags = ObserverFlags.None)
            => WatchSignalAsync(Service.Destination, __Interface, Path, "UserNew", (m, s) => ReadMessage_uo(m, (login1Object)s!), handler, emitOnCapturedContext, flags);
        public ValueTask<IDisposable> WatchUserRemovedAsync(Action<Exception?, (uint Uid, ObjectPath ObjectPath)> handler, bool emitOnCapturedContext = true, ObserverFlags flags = ObserverFlags.None)
            => WatchSignalAsync(Service.Destination, __Interface, Path, "UserRemoved", (m, s) => ReadMessage_uo(m, (login1Object)s!), handler, emitOnCapturedContext, flags);
        public ValueTask<IDisposable> WatchSeatNewAsync(Action<Exception?, (string SeatId, ObjectPath ObjectPath)> handler, bool emitOnCapturedContext = true, ObserverFlags flags = ObserverFlags.None)
            => WatchSignalAsync(Service.Destination, __Interface, Path, "SeatNew", (m, s) => ReadMessage_so(m, (login1Object)s!), handler, emitOnCapturedContext, flags);
        public ValueTask<IDisposable> WatchSeatRemovedAsync(Action<Exception?, (string SeatId, ObjectPath ObjectPath)> handler, bool emitOnCapturedContext = true, ObserverFlags flags = ObserverFlags.None)
            => WatchSignalAsync(Service.Destination, __Interface, Path, "SeatRemoved", (m, s) => ReadMessage_so(m, (login1Object)s!), handler, emitOnCapturedContext, flags);
        public ValueTask<IDisposable> WatchPrepareForShutdownAsync(Action<Exception?, bool> handler, bool emitOnCapturedContext = true, ObserverFlags flags = ObserverFlags.None)
            => WatchSignalAsync(Service.Destination, __Interface, Path, "PrepareForShutdown", (m, s) => ReadMessage_b(m, (login1Object)s!), handler, emitOnCapturedContext, flags);
        public ValueTask<IDisposable> WatchPrepareForShutdownWithMetadataAsync(Action<Exception?, (bool Start, Dictionary<string, VariantValue> Metadata)> handler, bool emitOnCapturedContext = true, ObserverFlags flags = ObserverFlags.None)
            => WatchSignalAsync(Service.Destination, __Interface, Path, "PrepareForShutdownWithMetadata", (m, s) => ReadMessage_baesv(m, (login1Object)s!), handler, emitOnCapturedContext, flags);
        public ValueTask<IDisposable> WatchPrepareForSleepAsync(Action<Exception?, bool> handler, bool emitOnCapturedContext = true, ObserverFlags flags = ObserverFlags.None)
            => WatchSignalAsync(Service.Destination, __Interface, Path, "PrepareForSleep", (m, s) => ReadMessage_b(m, (login1Object)s!), handler, emitOnCapturedContext, flags);
        public Task SetEnableWallMessagesAsync(bool value)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("EnableWallMessages");
                writer.WriteSignature("b");
                writer.WriteBool(value);
                return writer.CreateMessage();
            }
        }
        public Task SetWallMessageAsync(string value)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: "org.freedesktop.DBus.Properties",
                    signature: "ssv",
                    member: "Set");
                writer.WriteString(__Interface);
                writer.WriteString("WallMessage");
                writer.WriteSignature("s");
                writer.WriteString(value);
                return writer.CreateMessage();
            }
        }
        public Task<bool> GetEnableWallMessagesAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "EnableWallMessages"), (m, s) => ReadMessage_v_b(m, (login1Object)s!), this);
        public Task<string> GetWallMessageAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "WallMessage"), (m, s) => ReadMessage_v_s(m, (login1Object)s!), this);
        public Task<uint> GetNAutoVTsAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "NAutoVTs"), (m, s) => ReadMessage_v_u(m, (login1Object)s!), this);
        public Task<string[]> GetKillOnlyUsersAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "KillOnlyUsers"), (m, s) => ReadMessage_v_as(m, (login1Object)s!), this);
        public Task<string[]> GetKillExcludeUsersAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "KillExcludeUsers"), (m, s) => ReadMessage_v_as(m, (login1Object)s!), this);
        public Task<bool> GetKillUserProcessesAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "KillUserProcesses"), (m, s) => ReadMessage_v_b(m, (login1Object)s!), this);
        public Task<string> GetRebootParameterAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "RebootParameter"), (m, s) => ReadMessage_v_s(m, (login1Object)s!), this);
        public Task<bool> GetRebootToFirmwareSetupAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "RebootToFirmwareSetup"), (m, s) => ReadMessage_v_b(m, (login1Object)s!), this);
        public Task<ulong> GetRebootToBootLoaderMenuAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "RebootToBootLoaderMenu"), (m, s) => ReadMessage_v_t(m, (login1Object)s!), this);
        public Task<string> GetRebootToBootLoaderEntryAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "RebootToBootLoaderEntry"), (m, s) => ReadMessage_v_s(m, (login1Object)s!), this);
        public Task<string[]> GetBootLoaderEntriesAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "BootLoaderEntries"), (m, s) => ReadMessage_v_as(m, (login1Object)s!), this);
        public Task<bool> GetIdleHintAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "IdleHint"), (m, s) => ReadMessage_v_b(m, (login1Object)s!), this);
        public Task<ulong> GetIdleSinceHintAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "IdleSinceHint"), (m, s) => ReadMessage_v_t(m, (login1Object)s!), this);
        public Task<ulong> GetIdleSinceHintMonotonicAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "IdleSinceHintMonotonic"), (m, s) => ReadMessage_v_t(m, (login1Object)s!), this);
        public Task<string> GetBlockInhibitedAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "BlockInhibited"), (m, s) => ReadMessage_v_s(m, (login1Object)s!), this);
        public Task<string> GetBlockWeakInhibitedAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "BlockWeakInhibited"), (m, s) => ReadMessage_v_s(m, (login1Object)s!), this);
        public Task<string> GetDelayInhibitedAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "DelayInhibited"), (m, s) => ReadMessage_v_s(m, (login1Object)s!), this);
        public Task<ulong> GetInhibitDelayMaxUSecAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "InhibitDelayMaxUSec"), (m, s) => ReadMessage_v_t(m, (login1Object)s!), this);
        public Task<ulong> GetUserStopDelayUSecAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "UserStopDelayUSec"), (m, s) => ReadMessage_v_t(m, (login1Object)s!), this);
        public Task<string[]> GetSleepOperationAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "SleepOperation"), (m, s) => ReadMessage_v_as(m, (login1Object)s!), this);
        public Task<string> GetHandlePowerKeyAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "HandlePowerKey"), (m, s) => ReadMessage_v_s(m, (login1Object)s!), this);
        public Task<string> GetHandlePowerKeyLongPressAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "HandlePowerKeyLongPress"), (m, s) => ReadMessage_v_s(m, (login1Object)s!), this);
        public Task<string> GetHandleRebootKeyAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "HandleRebootKey"), (m, s) => ReadMessage_v_s(m, (login1Object)s!), this);
        public Task<string> GetHandleRebootKeyLongPressAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "HandleRebootKeyLongPress"), (m, s) => ReadMessage_v_s(m, (login1Object)s!), this);
        public Task<string> GetHandleSuspendKeyAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "HandleSuspendKey"), (m, s) => ReadMessage_v_s(m, (login1Object)s!), this);
        public Task<string> GetHandleSuspendKeyLongPressAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "HandleSuspendKeyLongPress"), (m, s) => ReadMessage_v_s(m, (login1Object)s!), this);
        public Task<string> GetHandleHibernateKeyAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "HandleHibernateKey"), (m, s) => ReadMessage_v_s(m, (login1Object)s!), this);
        public Task<string> GetHandleHibernateKeyLongPressAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "HandleHibernateKeyLongPress"), (m, s) => ReadMessage_v_s(m, (login1Object)s!), this);
        public Task<string> GetHandleLidSwitchAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "HandleLidSwitch"), (m, s) => ReadMessage_v_s(m, (login1Object)s!), this);
        public Task<string> GetHandleLidSwitchExternalPowerAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "HandleLidSwitchExternalPower"), (m, s) => ReadMessage_v_s(m, (login1Object)s!), this);
        public Task<string> GetHandleLidSwitchDockedAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "HandleLidSwitchDocked"), (m, s) => ReadMessage_v_s(m, (login1Object)s!), this);
        public Task<string> GetHandleSecureAttentionKeyAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "HandleSecureAttentionKey"), (m, s) => ReadMessage_v_s(m, (login1Object)s!), this);
        public Task<ulong> GetHoldoffTimeoutUSecAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "HoldoffTimeoutUSec"), (m, s) => ReadMessage_v_t(m, (login1Object)s!), this);
        public Task<string> GetIdleActionAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "IdleAction"), (m, s) => ReadMessage_v_s(m, (login1Object)s!), this);
        public Task<ulong> GetIdleActionUSecAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "IdleActionUSec"), (m, s) => ReadMessage_v_t(m, (login1Object)s!), this);
        public Task<bool> GetPreparingForShutdownAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "PreparingForShutdown"), (m, s) => ReadMessage_v_b(m, (login1Object)s!), this);
        public Task<Dictionary<string, VariantValue>> GetPreparingForShutdownWithMetadataAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "PreparingForShutdownWithMetadata"), (m, s) => ReadMessage_v_aesv(m, (login1Object)s!), this);
        public Task<bool> GetPreparingForSleepAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "PreparingForSleep"), (m, s) => ReadMessage_v_b(m, (login1Object)s!), this);
        public Task<(string, ulong)> GetScheduledShutdownAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "ScheduledShutdown"), (m, s) => ReadMessage_v_rstz(m, (login1Object)s!), this);
        public Task<string> GetDesignatedMaintenanceTimeAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "DesignatedMaintenanceTime"), (m, s) => ReadMessage_v_s(m, (login1Object)s!), this);
        public Task<bool> GetDockedAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Docked"), (m, s) => ReadMessage_v_b(m, (login1Object)s!), this);
        public Task<bool> GetLidClosedAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "LidClosed"), (m, s) => ReadMessage_v_b(m, (login1Object)s!), this);
        public Task<bool> GetOnExternalPowerAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "OnExternalPower"), (m, s) => ReadMessage_v_b(m, (login1Object)s!), this);
        public Task<bool> GetRemoveIPCAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "RemoveIPC"), (m, s) => ReadMessage_v_b(m, (login1Object)s!), this);
        public Task<ulong> GetRuntimeDirectorySizeAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "RuntimeDirectorySize"), (m, s) => ReadMessage_v_t(m, (login1Object)s!), this);
        public Task<ulong> GetRuntimeDirectoryInodesMaxAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "RuntimeDirectoryInodesMax"), (m, s) => ReadMessage_v_t(m, (login1Object)s!), this);
        public Task<ulong> GetInhibitorsMaxAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "InhibitorsMax"), (m, s) => ReadMessage_v_t(m, (login1Object)s!), this);
        public Task<ulong> GetNCurrentInhibitorsAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "NCurrentInhibitors"), (m, s) => ReadMessage_v_t(m, (login1Object)s!), this);
        public Task<ulong> GetSessionsMaxAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "SessionsMax"), (m, s) => ReadMessage_v_t(m, (login1Object)s!), this);
        public Task<ulong> GetNCurrentSessionsAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "NCurrentSessions"), (m, s) => ReadMessage_v_t(m, (login1Object)s!), this);
        public Task<ulong> GetStopIdleSessionUSecAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "StopIdleSessionUSec"), (m, s) => ReadMessage_v_t(m, (login1Object)s!), this);
        public Task<ManagerProperties> GetPropertiesAsync()
        {
            return Connection.CallMethodAsync(CreateGetAllPropertiesMessage(__Interface), (m, s) => ReadMessage(m, (login1Object)s!), this);
            static ManagerProperties ReadMessage(Message message, login1Object _)
            {
                var reader = message.GetBodyReader();
                return ReadProperties(ref reader);
            }
        }
        public ValueTask<IDisposable> WatchPropertiesChangedAsync(Action<Exception?, PropertyChanges<ManagerProperties>> handler, bool emitOnCapturedContext = true, ObserverFlags flags = ObserverFlags.None)
        {
            return WatchPropertiesChangedAsync(__Interface, (m, s) => ReadMessage(m, (login1Object)s!), handler, emitOnCapturedContext, flags);
            static PropertyChanges<ManagerProperties> ReadMessage(Message message, login1Object _)
            {
                var reader = message.GetBodyReader();
                reader.ReadString(); // interface
                List<string> changed = new(), invalidated = new();
                return new PropertyChanges<ManagerProperties>(ReadProperties(ref reader, changed), ReadInvalidated(ref reader), changed.ToArray());
            }
            static string[] ReadInvalidated(ref Reader reader)
            {
                List<string>? invalidated = null;
                ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.String);
                while (reader.HasNext(arrayEnd))
                {
                    invalidated ??= new();
                    var property = reader.ReadString();
                    switch (property)
                    {
                        case "EnableWallMessages": invalidated.Add("EnableWallMessages"); break;
                        case "WallMessage": invalidated.Add("WallMessage"); break;
                        case "NAutoVTs": invalidated.Add("NAutoVTs"); break;
                        case "KillOnlyUsers": invalidated.Add("KillOnlyUsers"); break;
                        case "KillExcludeUsers": invalidated.Add("KillExcludeUsers"); break;
                        case "KillUserProcesses": invalidated.Add("KillUserProcesses"); break;
                        case "RebootParameter": invalidated.Add("RebootParameter"); break;
                        case "RebootToFirmwareSetup": invalidated.Add("RebootToFirmwareSetup"); break;
                        case "RebootToBootLoaderMenu": invalidated.Add("RebootToBootLoaderMenu"); break;
                        case "RebootToBootLoaderEntry": invalidated.Add("RebootToBootLoaderEntry"); break;
                        case "BootLoaderEntries": invalidated.Add("BootLoaderEntries"); break;
                        case "IdleHint": invalidated.Add("IdleHint"); break;
                        case "IdleSinceHint": invalidated.Add("IdleSinceHint"); break;
                        case "IdleSinceHintMonotonic": invalidated.Add("IdleSinceHintMonotonic"); break;
                        case "BlockInhibited": invalidated.Add("BlockInhibited"); break;
                        case "BlockWeakInhibited": invalidated.Add("BlockWeakInhibited"); break;
                        case "DelayInhibited": invalidated.Add("DelayInhibited"); break;
                        case "InhibitDelayMaxUSec": invalidated.Add("InhibitDelayMaxUSec"); break;
                        case "UserStopDelayUSec": invalidated.Add("UserStopDelayUSec"); break;
                        case "SleepOperation": invalidated.Add("SleepOperation"); break;
                        case "HandlePowerKey": invalidated.Add("HandlePowerKey"); break;
                        case "HandlePowerKeyLongPress": invalidated.Add("HandlePowerKeyLongPress"); break;
                        case "HandleRebootKey": invalidated.Add("HandleRebootKey"); break;
                        case "HandleRebootKeyLongPress": invalidated.Add("HandleRebootKeyLongPress"); break;
                        case "HandleSuspendKey": invalidated.Add("HandleSuspendKey"); break;
                        case "HandleSuspendKeyLongPress": invalidated.Add("HandleSuspendKeyLongPress"); break;
                        case "HandleHibernateKey": invalidated.Add("HandleHibernateKey"); break;
                        case "HandleHibernateKeyLongPress": invalidated.Add("HandleHibernateKeyLongPress"); break;
                        case "HandleLidSwitch": invalidated.Add("HandleLidSwitch"); break;
                        case "HandleLidSwitchExternalPower": invalidated.Add("HandleLidSwitchExternalPower"); break;
                        case "HandleLidSwitchDocked": invalidated.Add("HandleLidSwitchDocked"); break;
                        case "HandleSecureAttentionKey": invalidated.Add("HandleSecureAttentionKey"); break;
                        case "HoldoffTimeoutUSec": invalidated.Add("HoldoffTimeoutUSec"); break;
                        case "IdleAction": invalidated.Add("IdleAction"); break;
                        case "IdleActionUSec": invalidated.Add("IdleActionUSec"); break;
                        case "PreparingForShutdown": invalidated.Add("PreparingForShutdown"); break;
                        case "PreparingForShutdownWithMetadata": invalidated.Add("PreparingForShutdownWithMetadata"); break;
                        case "PreparingForSleep": invalidated.Add("PreparingForSleep"); break;
                        case "ScheduledShutdown": invalidated.Add("ScheduledShutdown"); break;
                        case "DesignatedMaintenanceTime": invalidated.Add("DesignatedMaintenanceTime"); break;
                        case "Docked": invalidated.Add("Docked"); break;
                        case "LidClosed": invalidated.Add("LidClosed"); break;
                        case "OnExternalPower": invalidated.Add("OnExternalPower"); break;
                        case "RemoveIPC": invalidated.Add("RemoveIPC"); break;
                        case "RuntimeDirectorySize": invalidated.Add("RuntimeDirectorySize"); break;
                        case "RuntimeDirectoryInodesMax": invalidated.Add("RuntimeDirectoryInodesMax"); break;
                        case "InhibitorsMax": invalidated.Add("InhibitorsMax"); break;
                        case "NCurrentInhibitors": invalidated.Add("NCurrentInhibitors"); break;
                        case "SessionsMax": invalidated.Add("SessionsMax"); break;
                        case "NCurrentSessions": invalidated.Add("NCurrentSessions"); break;
                        case "StopIdleSessionUSec": invalidated.Add("StopIdleSessionUSec"); break;
                    }
                }
                return invalidated?.ToArray() ?? Array.Empty<string>();
            }
        }
        private static ManagerProperties ReadProperties(ref Reader reader, List<string>? changedList = null)
        {
            var props = new ManagerProperties();
            ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.Struct);
            while (reader.HasNext(arrayEnd))
            {
                var property = reader.ReadString();
                switch (property)
                {
                    case "EnableWallMessages":
                        reader.ReadSignature("b"u8);
                        props.EnableWallMessages = reader.ReadBool();
                        changedList?.Add("EnableWallMessages");
                        break;
                    case "WallMessage":
                        reader.ReadSignature("s"u8);
                        props.WallMessage = reader.ReadString();
                        changedList?.Add("WallMessage");
                        break;
                    case "NAutoVTs":
                        reader.ReadSignature("u"u8);
                        props.NAutoVTs = reader.ReadUInt32();
                        changedList?.Add("NAutoVTs");
                        break;
                    case "KillOnlyUsers":
                        reader.ReadSignature("as"u8);
                        props.KillOnlyUsers = reader.ReadArrayOfString();
                        changedList?.Add("KillOnlyUsers");
                        break;
                    case "KillExcludeUsers":
                        reader.ReadSignature("as"u8);
                        props.KillExcludeUsers = reader.ReadArrayOfString();
                        changedList?.Add("KillExcludeUsers");
                        break;
                    case "KillUserProcesses":
                        reader.ReadSignature("b"u8);
                        props.KillUserProcesses = reader.ReadBool();
                        changedList?.Add("KillUserProcesses");
                        break;
                    case "RebootParameter":
                        reader.ReadSignature("s"u8);
                        props.RebootParameter = reader.ReadString();
                        changedList?.Add("RebootParameter");
                        break;
                    case "RebootToFirmwareSetup":
                        reader.ReadSignature("b"u8);
                        props.RebootToFirmwareSetup = reader.ReadBool();
                        changedList?.Add("RebootToFirmwareSetup");
                        break;
                    case "RebootToBootLoaderMenu":
                        reader.ReadSignature("t"u8);
                        props.RebootToBootLoaderMenu = reader.ReadUInt64();
                        changedList?.Add("RebootToBootLoaderMenu");
                        break;
                    case "RebootToBootLoaderEntry":
                        reader.ReadSignature("s"u8);
                        props.RebootToBootLoaderEntry = reader.ReadString();
                        changedList?.Add("RebootToBootLoaderEntry");
                        break;
                    case "BootLoaderEntries":
                        reader.ReadSignature("as"u8);
                        props.BootLoaderEntries = reader.ReadArrayOfString();
                        changedList?.Add("BootLoaderEntries");
                        break;
                    case "IdleHint":
                        reader.ReadSignature("b"u8);
                        props.IdleHint = reader.ReadBool();
                        changedList?.Add("IdleHint");
                        break;
                    case "IdleSinceHint":
                        reader.ReadSignature("t"u8);
                        props.IdleSinceHint = reader.ReadUInt64();
                        changedList?.Add("IdleSinceHint");
                        break;
                    case "IdleSinceHintMonotonic":
                        reader.ReadSignature("t"u8);
                        props.IdleSinceHintMonotonic = reader.ReadUInt64();
                        changedList?.Add("IdleSinceHintMonotonic");
                        break;
                    case "BlockInhibited":
                        reader.ReadSignature("s"u8);
                        props.BlockInhibited = reader.ReadString();
                        changedList?.Add("BlockInhibited");
                        break;
                    case "BlockWeakInhibited":
                        reader.ReadSignature("s"u8);
                        props.BlockWeakInhibited = reader.ReadString();
                        changedList?.Add("BlockWeakInhibited");
                        break;
                    case "DelayInhibited":
                        reader.ReadSignature("s"u8);
                        props.DelayInhibited = reader.ReadString();
                        changedList?.Add("DelayInhibited");
                        break;
                    case "InhibitDelayMaxUSec":
                        reader.ReadSignature("t"u8);
                        props.InhibitDelayMaxUSec = reader.ReadUInt64();
                        changedList?.Add("InhibitDelayMaxUSec");
                        break;
                    case "UserStopDelayUSec":
                        reader.ReadSignature("t"u8);
                        props.UserStopDelayUSec = reader.ReadUInt64();
                        changedList?.Add("UserStopDelayUSec");
                        break;
                    case "SleepOperation":
                        reader.ReadSignature("as"u8);
                        props.SleepOperation = reader.ReadArrayOfString();
                        changedList?.Add("SleepOperation");
                        break;
                    case "HandlePowerKey":
                        reader.ReadSignature("s"u8);
                        props.HandlePowerKey = reader.ReadString();
                        changedList?.Add("HandlePowerKey");
                        break;
                    case "HandlePowerKeyLongPress":
                        reader.ReadSignature("s"u8);
                        props.HandlePowerKeyLongPress = reader.ReadString();
                        changedList?.Add("HandlePowerKeyLongPress");
                        break;
                    case "HandleRebootKey":
                        reader.ReadSignature("s"u8);
                        props.HandleRebootKey = reader.ReadString();
                        changedList?.Add("HandleRebootKey");
                        break;
                    case "HandleRebootKeyLongPress":
                        reader.ReadSignature("s"u8);
                        props.HandleRebootKeyLongPress = reader.ReadString();
                        changedList?.Add("HandleRebootKeyLongPress");
                        break;
                    case "HandleSuspendKey":
                        reader.ReadSignature("s"u8);
                        props.HandleSuspendKey = reader.ReadString();
                        changedList?.Add("HandleSuspendKey");
                        break;
                    case "HandleSuspendKeyLongPress":
                        reader.ReadSignature("s"u8);
                        props.HandleSuspendKeyLongPress = reader.ReadString();
                        changedList?.Add("HandleSuspendKeyLongPress");
                        break;
                    case "HandleHibernateKey":
                        reader.ReadSignature("s"u8);
                        props.HandleHibernateKey = reader.ReadString();
                        changedList?.Add("HandleHibernateKey");
                        break;
                    case "HandleHibernateKeyLongPress":
                        reader.ReadSignature("s"u8);
                        props.HandleHibernateKeyLongPress = reader.ReadString();
                        changedList?.Add("HandleHibernateKeyLongPress");
                        break;
                    case "HandleLidSwitch":
                        reader.ReadSignature("s"u8);
                        props.HandleLidSwitch = reader.ReadString();
                        changedList?.Add("HandleLidSwitch");
                        break;
                    case "HandleLidSwitchExternalPower":
                        reader.ReadSignature("s"u8);
                        props.HandleLidSwitchExternalPower = reader.ReadString();
                        changedList?.Add("HandleLidSwitchExternalPower");
                        break;
                    case "HandleLidSwitchDocked":
                        reader.ReadSignature("s"u8);
                        props.HandleLidSwitchDocked = reader.ReadString();
                        changedList?.Add("HandleLidSwitchDocked");
                        break;
                    case "HandleSecureAttentionKey":
                        reader.ReadSignature("s"u8);
                        props.HandleSecureAttentionKey = reader.ReadString();
                        changedList?.Add("HandleSecureAttentionKey");
                        break;
                    case "HoldoffTimeoutUSec":
                        reader.ReadSignature("t"u8);
                        props.HoldoffTimeoutUSec = reader.ReadUInt64();
                        changedList?.Add("HoldoffTimeoutUSec");
                        break;
                    case "IdleAction":
                        reader.ReadSignature("s"u8);
                        props.IdleAction = reader.ReadString();
                        changedList?.Add("IdleAction");
                        break;
                    case "IdleActionUSec":
                        reader.ReadSignature("t"u8);
                        props.IdleActionUSec = reader.ReadUInt64();
                        changedList?.Add("IdleActionUSec");
                        break;
                    case "PreparingForShutdown":
                        reader.ReadSignature("b"u8);
                        props.PreparingForShutdown = reader.ReadBool();
                        changedList?.Add("PreparingForShutdown");
                        break;
                    case "PreparingForShutdownWithMetadata":
                        reader.ReadSignature("a{sv}"u8);
                        props.PreparingForShutdownWithMetadata = reader.ReadDictionaryOfStringToVariantValue();
                        changedList?.Add("PreparingForShutdownWithMetadata");
                        break;
                    case "PreparingForSleep":
                        reader.ReadSignature("b"u8);
                        props.PreparingForSleep = reader.ReadBool();
                        changedList?.Add("PreparingForSleep");
                        break;
                    case "ScheduledShutdown":
                        reader.ReadSignature("(st)"u8);
                        props.ScheduledShutdown = ReadType_rstz(ref reader);
                        changedList?.Add("ScheduledShutdown");
                        break;
                    case "DesignatedMaintenanceTime":
                        reader.ReadSignature("s"u8);
                        props.DesignatedMaintenanceTime = reader.ReadString();
                        changedList?.Add("DesignatedMaintenanceTime");
                        break;
                    case "Docked":
                        reader.ReadSignature("b"u8);
                        props.Docked = reader.ReadBool();
                        changedList?.Add("Docked");
                        break;
                    case "LidClosed":
                        reader.ReadSignature("b"u8);
                        props.LidClosed = reader.ReadBool();
                        changedList?.Add("LidClosed");
                        break;
                    case "OnExternalPower":
                        reader.ReadSignature("b"u8);
                        props.OnExternalPower = reader.ReadBool();
                        changedList?.Add("OnExternalPower");
                        break;
                    case "RemoveIPC":
                        reader.ReadSignature("b"u8);
                        props.RemoveIPC = reader.ReadBool();
                        changedList?.Add("RemoveIPC");
                        break;
                    case "RuntimeDirectorySize":
                        reader.ReadSignature("t"u8);
                        props.RuntimeDirectorySize = reader.ReadUInt64();
                        changedList?.Add("RuntimeDirectorySize");
                        break;
                    case "RuntimeDirectoryInodesMax":
                        reader.ReadSignature("t"u8);
                        props.RuntimeDirectoryInodesMax = reader.ReadUInt64();
                        changedList?.Add("RuntimeDirectoryInodesMax");
                        break;
                    case "InhibitorsMax":
                        reader.ReadSignature("t"u8);
                        props.InhibitorsMax = reader.ReadUInt64();
                        changedList?.Add("InhibitorsMax");
                        break;
                    case "NCurrentInhibitors":
                        reader.ReadSignature("t"u8);
                        props.NCurrentInhibitors = reader.ReadUInt64();
                        changedList?.Add("NCurrentInhibitors");
                        break;
                    case "SessionsMax":
                        reader.ReadSignature("t"u8);
                        props.SessionsMax = reader.ReadUInt64();
                        changedList?.Add("SessionsMax");
                        break;
                    case "NCurrentSessions":
                        reader.ReadSignature("t"u8);
                        props.NCurrentSessions = reader.ReadUInt64();
                        changedList?.Add("NCurrentSessions");
                        break;
                    case "StopIdleSessionUSec":
                        reader.ReadSignature("t"u8);
                        props.StopIdleSessionUSec = reader.ReadUInt64();
                        changedList?.Add("StopIdleSessionUSec");
                        break;
                    default:
                        reader.ReadVariantValue();
                        break;
                }
            }
            return props;
        }
    }
    record UserProperties
    {
        public uint UID { get; set; } = default!;
        public uint GID { get; set; } = default!;
        public string Name { get; set; } = default!;
        public ulong Timestamp { get; set; } = default!;
        public ulong TimestampMonotonic { get; set; } = default!;
        public string RuntimePath { get; set; } = default!;
        public string Service { get; set; } = default!;
        public string Slice { get; set; } = default!;
        public (string, ObjectPath) Display { get; set; } = default!;
        public string State { get; set; } = default!;
        public (string, ObjectPath)[] Sessions { get; set; } = default!;
        public bool IdleHint { get; set; } = default!;
        public ulong IdleSinceHint { get; set; } = default!;
        public ulong IdleSinceHintMonotonic { get; set; } = default!;
        public bool Linger { get; set; } = default!;
    }
    partial class User : login1Object
    {
        private const string __Interface = "org.freedesktop.login1.User";
        public User(login1Service service, ObjectPath path) : base(service, path)
        { }
        public Task TerminateAsync()
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "Terminate");
                return writer.CreateMessage();
            }
        }
        public Task KillAsync(int signalNumber)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "i",
                    member: "Kill");
                writer.WriteInt32(signalNumber);
                return writer.CreateMessage();
            }
        }
        public Task<uint> GetUIDAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "UID"), (m, s) => ReadMessage_v_u(m, (login1Object)s!), this);
        public Task<uint> GetGIDAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "GID"), (m, s) => ReadMessage_v_u(m, (login1Object)s!), this);
        public Task<string> GetNameAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Name"), (m, s) => ReadMessage_v_s(m, (login1Object)s!), this);
        public Task<ulong> GetTimestampAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Timestamp"), (m, s) => ReadMessage_v_t(m, (login1Object)s!), this);
        public Task<ulong> GetTimestampMonotonicAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "TimestampMonotonic"), (m, s) => ReadMessage_v_t(m, (login1Object)s!), this);
        public Task<string> GetRuntimePathAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "RuntimePath"), (m, s) => ReadMessage_v_s(m, (login1Object)s!), this);
        public Task<string> GetServiceAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Service"), (m, s) => ReadMessage_v_s(m, (login1Object)s!), this);
        public Task<string> GetSliceAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Slice"), (m, s) => ReadMessage_v_s(m, (login1Object)s!), this);
        public Task<(string, ObjectPath)> GetDisplayAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Display"), (m, s) => ReadMessage_v_rsoz(m, (login1Object)s!), this);
        public Task<string> GetStateAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "State"), (m, s) => ReadMessage_v_s(m, (login1Object)s!), this);
        public Task<(string, ObjectPath)[]> GetSessionsAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Sessions"), (m, s) => ReadMessage_v_arsoz(m, (login1Object)s!), this);
        public Task<bool> GetIdleHintAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "IdleHint"), (m, s) => ReadMessage_v_b(m, (login1Object)s!), this);
        public Task<ulong> GetIdleSinceHintAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "IdleSinceHint"), (m, s) => ReadMessage_v_t(m, (login1Object)s!), this);
        public Task<ulong> GetIdleSinceHintMonotonicAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "IdleSinceHintMonotonic"), (m, s) => ReadMessage_v_t(m, (login1Object)s!), this);
        public Task<bool> GetLingerAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Linger"), (m, s) => ReadMessage_v_b(m, (login1Object)s!), this);
        public Task<UserProperties> GetPropertiesAsync()
        {
            return Connection.CallMethodAsync(CreateGetAllPropertiesMessage(__Interface), (m, s) => ReadMessage(m, (login1Object)s!), this);
            static UserProperties ReadMessage(Message message, login1Object _)
            {
                var reader = message.GetBodyReader();
                return ReadProperties(ref reader);
            }
        }
        public ValueTask<IDisposable> WatchPropertiesChangedAsync(Action<Exception?, PropertyChanges<UserProperties>> handler, bool emitOnCapturedContext = true, ObserverFlags flags = ObserverFlags.None)
        {
            return WatchPropertiesChangedAsync(__Interface, (m, s) => ReadMessage(m, (login1Object)s!), handler, emitOnCapturedContext, flags);
            static PropertyChanges<UserProperties> ReadMessage(Message message, login1Object _)
            {
                var reader = message.GetBodyReader();
                reader.ReadString(); // interface
                List<string> changed = new(), invalidated = new();
                return new PropertyChanges<UserProperties>(ReadProperties(ref reader, changed), ReadInvalidated(ref reader), changed.ToArray());
            }
            static string[] ReadInvalidated(ref Reader reader)
            {
                List<string>? invalidated = null;
                ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.String);
                while (reader.HasNext(arrayEnd))
                {
                    invalidated ??= new();
                    var property = reader.ReadString();
                    switch (property)
                    {
                        case "UID": invalidated.Add("UID"); break;
                        case "GID": invalidated.Add("GID"); break;
                        case "Name": invalidated.Add("Name"); break;
                        case "Timestamp": invalidated.Add("Timestamp"); break;
                        case "TimestampMonotonic": invalidated.Add("TimestampMonotonic"); break;
                        case "RuntimePath": invalidated.Add("RuntimePath"); break;
                        case "Service": invalidated.Add("Service"); break;
                        case "Slice": invalidated.Add("Slice"); break;
                        case "Display": invalidated.Add("Display"); break;
                        case "State": invalidated.Add("State"); break;
                        case "Sessions": invalidated.Add("Sessions"); break;
                        case "IdleHint": invalidated.Add("IdleHint"); break;
                        case "IdleSinceHint": invalidated.Add("IdleSinceHint"); break;
                        case "IdleSinceHintMonotonic": invalidated.Add("IdleSinceHintMonotonic"); break;
                        case "Linger": invalidated.Add("Linger"); break;
                    }
                }
                return invalidated?.ToArray() ?? Array.Empty<string>();
            }
        }
        private static UserProperties ReadProperties(ref Reader reader, List<string>? changedList = null)
        {
            var props = new UserProperties();
            ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.Struct);
            while (reader.HasNext(arrayEnd))
            {
                var property = reader.ReadString();
                switch (property)
                {
                    case "UID":
                        reader.ReadSignature("u"u8);
                        props.UID = reader.ReadUInt32();
                        changedList?.Add("UID");
                        break;
                    case "GID":
                        reader.ReadSignature("u"u8);
                        props.GID = reader.ReadUInt32();
                        changedList?.Add("GID");
                        break;
                    case "Name":
                        reader.ReadSignature("s"u8);
                        props.Name = reader.ReadString();
                        changedList?.Add("Name");
                        break;
                    case "Timestamp":
                        reader.ReadSignature("t"u8);
                        props.Timestamp = reader.ReadUInt64();
                        changedList?.Add("Timestamp");
                        break;
                    case "TimestampMonotonic":
                        reader.ReadSignature("t"u8);
                        props.TimestampMonotonic = reader.ReadUInt64();
                        changedList?.Add("TimestampMonotonic");
                        break;
                    case "RuntimePath":
                        reader.ReadSignature("s"u8);
                        props.RuntimePath = reader.ReadString();
                        changedList?.Add("RuntimePath");
                        break;
                    case "Service":
                        reader.ReadSignature("s"u8);
                        props.Service = reader.ReadString();
                        changedList?.Add("Service");
                        break;
                    case "Slice":
                        reader.ReadSignature("s"u8);
                        props.Slice = reader.ReadString();
                        changedList?.Add("Slice");
                        break;
                    case "Display":
                        reader.ReadSignature("(so)"u8);
                        props.Display = ReadType_rsoz(ref reader);
                        changedList?.Add("Display");
                        break;
                    case "State":
                        reader.ReadSignature("s"u8);
                        props.State = reader.ReadString();
                        changedList?.Add("State");
                        break;
                    case "Sessions":
                        reader.ReadSignature("a(so)"u8);
                        props.Sessions = ReadType_arsoz(ref reader);
                        changedList?.Add("Sessions");
                        break;
                    case "IdleHint":
                        reader.ReadSignature("b"u8);
                        props.IdleHint = reader.ReadBool();
                        changedList?.Add("IdleHint");
                        break;
                    case "IdleSinceHint":
                        reader.ReadSignature("t"u8);
                        props.IdleSinceHint = reader.ReadUInt64();
                        changedList?.Add("IdleSinceHint");
                        break;
                    case "IdleSinceHintMonotonic":
                        reader.ReadSignature("t"u8);
                        props.IdleSinceHintMonotonic = reader.ReadUInt64();
                        changedList?.Add("IdleSinceHintMonotonic");
                        break;
                    case "Linger":
                        reader.ReadSignature("b"u8);
                        props.Linger = reader.ReadBool();
                        changedList?.Add("Linger");
                        break;
                    default:
                        reader.ReadVariantValue();
                        break;
                }
            }
            return props;
        }
    }
    record SessionProperties
    {
        public string Id { get; set; } = default!;
        public (uint, ObjectPath) User { get; set; } = default!;
        public string Name { get; set; } = default!;
        public ulong Timestamp { get; set; } = default!;
        public ulong TimestampMonotonic { get; set; } = default!;
        public uint VTNr { get; set; } = default!;
        public (string, ObjectPath) Seat { get; set; } = default!;
        public string TTY { get; set; } = default!;
        public string Display { get; set; } = default!;
        public bool Remote { get; set; } = default!;
        public string RemoteHost { get; set; } = default!;
        public string RemoteUser { get; set; } = default!;
        public string Service { get; set; } = default!;
        public string Desktop { get; set; } = default!;
        public string Scope { get; set; } = default!;
        public uint Leader { get; set; } = default!;
        public uint Audit { get; set; } = default!;
        public string Type { get; set; } = default!;
        public string Class { get; set; } = default!;
        public bool Active { get; set; } = default!;
        public string State { get; set; } = default!;
        public bool IdleHint { get; set; } = default!;
        public ulong IdleSinceHint { get; set; } = default!;
        public ulong IdleSinceHintMonotonic { get; set; } = default!;
        public bool CanIdle { get; set; } = default!;
        public bool CanLock { get; set; } = default!;
        public bool LockedHint { get; set; } = default!;
    }
    partial class Session : login1Object
    {
        private const string __Interface = "org.freedesktop.login1.Session";
        public Session(login1Service service, ObjectPath path) : base(service, path)
        { }
        public Task TerminateAsync()
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "Terminate");
                return writer.CreateMessage();
            }
        }
        public Task ActivateAsync()
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "Activate");
                return writer.CreateMessage();
            }
        }
        public Task LockAsync()
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "Lock");
                return writer.CreateMessage();
            }
        }
        public Task UnlockAsync()
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "Unlock");
                return writer.CreateMessage();
            }
        }
        public Task SetIdleHintAsync(bool idle)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "b",
                    member: "SetIdleHint");
                writer.WriteBool(idle);
                return writer.CreateMessage();
            }
        }
        public Task SetLockedHintAsync(bool locked)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "b",
                    member: "SetLockedHint");
                writer.WriteBool(locked);
                return writer.CreateMessage();
            }
        }
        public Task KillAsync(string whom, int signalNumber)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "si",
                    member: "Kill");
                writer.WriteString(whom);
                writer.WriteInt32(signalNumber);
                return writer.CreateMessage();
            }
        }
        public Task TakeControlAsync(bool force)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "b",
                    member: "TakeControl");
                writer.WriteBool(force);
                return writer.CreateMessage();
            }
        }
        public Task ReleaseControlAsync()
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "ReleaseControl");
                return writer.CreateMessage();
            }
        }
        public Task SetTypeAsync(string @type)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "s",
                    member: "SetType");
                writer.WriteString(@type);
                return writer.CreateMessage();
            }
        }
        public Task SetClassAsync(string @class)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "s",
                    member: "SetClass");
                writer.WriteString(@class);
                return writer.CreateMessage();
            }
        }
        public Task SetDisplayAsync(string display)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "s",
                    member: "SetDisplay");
                writer.WriteString(display);
                return writer.CreateMessage();
            }
        }
        public Task SetTTYAsync(System.Runtime.InteropServices.SafeHandle ttyFd)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "h",
                    member: "SetTTY");
                writer.WriteHandle(ttyFd);
                return writer.CreateMessage();
            }
        }
        public Task<(System.Runtime.InteropServices.SafeHandle? Fd, bool Inactive)> TakeDeviceAsync(uint major, uint minor)
        {
            return Connection.CallMethodAsync(CreateMessage(), (m, s) => ReadMessage_hb(m, (login1Object)s!), this);
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "uu",
                    member: "TakeDevice");
                writer.WriteUInt32(major);
                writer.WriteUInt32(minor);
                return writer.CreateMessage();
            }
        }
        public Task ReleaseDeviceAsync(uint major, uint minor)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "uu",
                    member: "ReleaseDevice");
                writer.WriteUInt32(major);
                writer.WriteUInt32(minor);
                return writer.CreateMessage();
            }
        }
        public Task PauseDeviceCompleteAsync(uint major, uint minor)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "uu",
                    member: "PauseDeviceComplete");
                writer.WriteUInt32(major);
                writer.WriteUInt32(minor);
                return writer.CreateMessage();
            }
        }
        public Task SetBrightnessAsync(string subsystem, string name, uint brightness)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "ssu",
                    member: "SetBrightness");
                writer.WriteString(subsystem);
                writer.WriteString(name);
                writer.WriteUInt32(brightness);
                return writer.CreateMessage();
            }
        }
        public ValueTask<IDisposable> WatchPauseDeviceAsync(Action<Exception?, (uint Major, uint Minor, string Type)> handler, bool emitOnCapturedContext = true, ObserverFlags flags = ObserverFlags.None)
            => WatchSignalAsync(Service.Destination, __Interface, Path, "PauseDevice", (m, s) => ReadMessage_uus(m, (login1Object)s!), handler, emitOnCapturedContext, flags);
        public ValueTask<IDisposable> WatchResumeDeviceAsync(Action<Exception?, (uint Major, uint Minor, System.Runtime.InteropServices.SafeHandle? Fd)> handler, bool emitOnCapturedContext = true, ObserverFlags flags = ObserverFlags.None)
            => WatchSignalAsync(Service.Destination, __Interface, Path, "ResumeDevice", (m, s) => ReadMessage_uuh(m, (login1Object)s!), handler, emitOnCapturedContext, flags);
        public ValueTask<IDisposable> WatchLockAsync(Action<Exception?> handler, bool emitOnCapturedContext = true, ObserverFlags flags = ObserverFlags.None)
            => WatchSignalAsync(Service.Destination, __Interface, Path, "Lock", handler, emitOnCapturedContext, flags);
        public ValueTask<IDisposable> WatchUnlockAsync(Action<Exception?> handler, bool emitOnCapturedContext = true, ObserverFlags flags = ObserverFlags.None)
            => WatchSignalAsync(Service.Destination, __Interface, Path, "Unlock", handler, emitOnCapturedContext, flags);
        public Task<string> GetIdAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Id"), (m, s) => ReadMessage_v_s(m, (login1Object)s!), this);
        public Task<(uint, ObjectPath)> GetUserAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "User"), (m, s) => ReadMessage_v_ruoz(m, (login1Object)s!), this);
        public Task<string> GetNameAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Name"), (m, s) => ReadMessage_v_s(m, (login1Object)s!), this);
        public Task<ulong> GetTimestampAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Timestamp"), (m, s) => ReadMessage_v_t(m, (login1Object)s!), this);
        public Task<ulong> GetTimestampMonotonicAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "TimestampMonotonic"), (m, s) => ReadMessage_v_t(m, (login1Object)s!), this);
        public Task<uint> GetVTNrAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "VTNr"), (m, s) => ReadMessage_v_u(m, (login1Object)s!), this);
        public Task<(string, ObjectPath)> GetSeatAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Seat"), (m, s) => ReadMessage_v_rsoz(m, (login1Object)s!), this);
        public Task<string> GetTTYAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "TTY"), (m, s) => ReadMessage_v_s(m, (login1Object)s!), this);
        public Task<string> GetDisplayAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Display"), (m, s) => ReadMessage_v_s(m, (login1Object)s!), this);
        public Task<bool> GetRemoteAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Remote"), (m, s) => ReadMessage_v_b(m, (login1Object)s!), this);
        public Task<string> GetRemoteHostAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "RemoteHost"), (m, s) => ReadMessage_v_s(m, (login1Object)s!), this);
        public Task<string> GetRemoteUserAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "RemoteUser"), (m, s) => ReadMessage_v_s(m, (login1Object)s!), this);
        public Task<string> GetServiceAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Service"), (m, s) => ReadMessage_v_s(m, (login1Object)s!), this);
        public Task<string> GetDesktopAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Desktop"), (m, s) => ReadMessage_v_s(m, (login1Object)s!), this);
        public Task<string> GetScopeAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Scope"), (m, s) => ReadMessage_v_s(m, (login1Object)s!), this);
        public Task<uint> GetLeaderAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Leader"), (m, s) => ReadMessage_v_u(m, (login1Object)s!), this);
        public Task<uint> GetAuditAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Audit"), (m, s) => ReadMessage_v_u(m, (login1Object)s!), this);
        public Task<string> GetTypeAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Type"), (m, s) => ReadMessage_v_s(m, (login1Object)s!), this);
        public Task<string> GetClassAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Class"), (m, s) => ReadMessage_v_s(m, (login1Object)s!), this);
        public Task<bool> GetActiveAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Active"), (m, s) => ReadMessage_v_b(m, (login1Object)s!), this);
        public Task<string> GetStateAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "State"), (m, s) => ReadMessage_v_s(m, (login1Object)s!), this);
        public Task<bool> GetIdleHintAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "IdleHint"), (m, s) => ReadMessage_v_b(m, (login1Object)s!), this);
        public Task<ulong> GetIdleSinceHintAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "IdleSinceHint"), (m, s) => ReadMessage_v_t(m, (login1Object)s!), this);
        public Task<ulong> GetIdleSinceHintMonotonicAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "IdleSinceHintMonotonic"), (m, s) => ReadMessage_v_t(m, (login1Object)s!), this);
        public Task<bool> GetCanIdleAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "CanIdle"), (m, s) => ReadMessage_v_b(m, (login1Object)s!), this);
        public Task<bool> GetCanLockAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "CanLock"), (m, s) => ReadMessage_v_b(m, (login1Object)s!), this);
        public Task<bool> GetLockedHintAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "LockedHint"), (m, s) => ReadMessage_v_b(m, (login1Object)s!), this);
        public Task<SessionProperties> GetPropertiesAsync()
        {
            return Connection.CallMethodAsync(CreateGetAllPropertiesMessage(__Interface), (m, s) => ReadMessage(m, (login1Object)s!), this);
            static SessionProperties ReadMessage(Message message, login1Object _)
            {
                var reader = message.GetBodyReader();
                return ReadProperties(ref reader);
            }
        }
        public ValueTask<IDisposable> WatchPropertiesChangedAsync(Action<Exception?, PropertyChanges<SessionProperties>> handler, bool emitOnCapturedContext = true, ObserverFlags flags = ObserverFlags.None)
        {
            return WatchPropertiesChangedAsync(__Interface, (m, s) => ReadMessage(m, (login1Object)s!), handler, emitOnCapturedContext, flags);
            static PropertyChanges<SessionProperties> ReadMessage(Message message, login1Object _)
            {
                var reader = message.GetBodyReader();
                reader.ReadString(); // interface
                List<string> changed = new(), invalidated = new();
                return new PropertyChanges<SessionProperties>(ReadProperties(ref reader, changed), ReadInvalidated(ref reader), changed.ToArray());
            }
            static string[] ReadInvalidated(ref Reader reader)
            {
                List<string>? invalidated = null;
                ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.String);
                while (reader.HasNext(arrayEnd))
                {
                    invalidated ??= new();
                    var property = reader.ReadString();
                    switch (property)
                    {
                        case "Id": invalidated.Add("Id"); break;
                        case "User": invalidated.Add("User"); break;
                        case "Name": invalidated.Add("Name"); break;
                        case "Timestamp": invalidated.Add("Timestamp"); break;
                        case "TimestampMonotonic": invalidated.Add("TimestampMonotonic"); break;
                        case "VTNr": invalidated.Add("VTNr"); break;
                        case "Seat": invalidated.Add("Seat"); break;
                        case "TTY": invalidated.Add("TTY"); break;
                        case "Display": invalidated.Add("Display"); break;
                        case "Remote": invalidated.Add("Remote"); break;
                        case "RemoteHost": invalidated.Add("RemoteHost"); break;
                        case "RemoteUser": invalidated.Add("RemoteUser"); break;
                        case "Service": invalidated.Add("Service"); break;
                        case "Desktop": invalidated.Add("Desktop"); break;
                        case "Scope": invalidated.Add("Scope"); break;
                        case "Leader": invalidated.Add("Leader"); break;
                        case "Audit": invalidated.Add("Audit"); break;
                        case "Type": invalidated.Add("Type"); break;
                        case "Class": invalidated.Add("Class"); break;
                        case "Active": invalidated.Add("Active"); break;
                        case "State": invalidated.Add("State"); break;
                        case "IdleHint": invalidated.Add("IdleHint"); break;
                        case "IdleSinceHint": invalidated.Add("IdleSinceHint"); break;
                        case "IdleSinceHintMonotonic": invalidated.Add("IdleSinceHintMonotonic"); break;
                        case "CanIdle": invalidated.Add("CanIdle"); break;
                        case "CanLock": invalidated.Add("CanLock"); break;
                        case "LockedHint": invalidated.Add("LockedHint"); break;
                    }
                }
                return invalidated?.ToArray() ?? Array.Empty<string>();
            }
        }
        private static SessionProperties ReadProperties(ref Reader reader, List<string>? changedList = null)
        {
            var props = new SessionProperties();
            ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.Struct);
            while (reader.HasNext(arrayEnd))
            {
                var property = reader.ReadString();
                switch (property)
                {
                    case "Id":
                        reader.ReadSignature("s"u8);
                        props.Id = reader.ReadString();
                        changedList?.Add("Id");
                        break;
                    case "User":
                        reader.ReadSignature("(uo)"u8);
                        props.User = ReadType_ruoz(ref reader);
                        changedList?.Add("User");
                        break;
                    case "Name":
                        reader.ReadSignature("s"u8);
                        props.Name = reader.ReadString();
                        changedList?.Add("Name");
                        break;
                    case "Timestamp":
                        reader.ReadSignature("t"u8);
                        props.Timestamp = reader.ReadUInt64();
                        changedList?.Add("Timestamp");
                        break;
                    case "TimestampMonotonic":
                        reader.ReadSignature("t"u8);
                        props.TimestampMonotonic = reader.ReadUInt64();
                        changedList?.Add("TimestampMonotonic");
                        break;
                    case "VTNr":
                        reader.ReadSignature("u"u8);
                        props.VTNr = reader.ReadUInt32();
                        changedList?.Add("VTNr");
                        break;
                    case "Seat":
                        reader.ReadSignature("(so)"u8);
                        props.Seat = ReadType_rsoz(ref reader);
                        changedList?.Add("Seat");
                        break;
                    case "TTY":
                        reader.ReadSignature("s"u8);
                        props.TTY = reader.ReadString();
                        changedList?.Add("TTY");
                        break;
                    case "Display":
                        reader.ReadSignature("s"u8);
                        props.Display = reader.ReadString();
                        changedList?.Add("Display");
                        break;
                    case "Remote":
                        reader.ReadSignature("b"u8);
                        props.Remote = reader.ReadBool();
                        changedList?.Add("Remote");
                        break;
                    case "RemoteHost":
                        reader.ReadSignature("s"u8);
                        props.RemoteHost = reader.ReadString();
                        changedList?.Add("RemoteHost");
                        break;
                    case "RemoteUser":
                        reader.ReadSignature("s"u8);
                        props.RemoteUser = reader.ReadString();
                        changedList?.Add("RemoteUser");
                        break;
                    case "Service":
                        reader.ReadSignature("s"u8);
                        props.Service = reader.ReadString();
                        changedList?.Add("Service");
                        break;
                    case "Desktop":
                        reader.ReadSignature("s"u8);
                        props.Desktop = reader.ReadString();
                        changedList?.Add("Desktop");
                        break;
                    case "Scope":
                        reader.ReadSignature("s"u8);
                        props.Scope = reader.ReadString();
                        changedList?.Add("Scope");
                        break;
                    case "Leader":
                        reader.ReadSignature("u"u8);
                        props.Leader = reader.ReadUInt32();
                        changedList?.Add("Leader");
                        break;
                    case "Audit":
                        reader.ReadSignature("u"u8);
                        props.Audit = reader.ReadUInt32();
                        changedList?.Add("Audit");
                        break;
                    case "Type":
                        reader.ReadSignature("s"u8);
                        props.Type = reader.ReadString();
                        changedList?.Add("Type");
                        break;
                    case "Class":
                        reader.ReadSignature("s"u8);
                        props.Class = reader.ReadString();
                        changedList?.Add("Class");
                        break;
                    case "Active":
                        reader.ReadSignature("b"u8);
                        props.Active = reader.ReadBool();
                        changedList?.Add("Active");
                        break;
                    case "State":
                        reader.ReadSignature("s"u8);
                        props.State = reader.ReadString();
                        changedList?.Add("State");
                        break;
                    case "IdleHint":
                        reader.ReadSignature("b"u8);
                        props.IdleHint = reader.ReadBool();
                        changedList?.Add("IdleHint");
                        break;
                    case "IdleSinceHint":
                        reader.ReadSignature("t"u8);
                        props.IdleSinceHint = reader.ReadUInt64();
                        changedList?.Add("IdleSinceHint");
                        break;
                    case "IdleSinceHintMonotonic":
                        reader.ReadSignature("t"u8);
                        props.IdleSinceHintMonotonic = reader.ReadUInt64();
                        changedList?.Add("IdleSinceHintMonotonic");
                        break;
                    case "CanIdle":
                        reader.ReadSignature("b"u8);
                        props.CanIdle = reader.ReadBool();
                        changedList?.Add("CanIdle");
                        break;
                    case "CanLock":
                        reader.ReadSignature("b"u8);
                        props.CanLock = reader.ReadBool();
                        changedList?.Add("CanLock");
                        break;
                    case "LockedHint":
                        reader.ReadSignature("b"u8);
                        props.LockedHint = reader.ReadBool();
                        changedList?.Add("LockedHint");
                        break;
                    default:
                        reader.ReadVariantValue();
                        break;
                }
            }
            return props;
        }
    }
    record SeatProperties
    {
        public string Id { get; set; } = default!;
        public (string, ObjectPath) ActiveSession { get; set; } = default!;
        public bool CanTTY { get; set; } = default!;
        public bool CanGraphical { get; set; } = default!;
        public (string, ObjectPath)[] Sessions { get; set; } = default!;
        public bool IdleHint { get; set; } = default!;
        public ulong IdleSinceHint { get; set; } = default!;
        public ulong IdleSinceHintMonotonic { get; set; } = default!;
    }
    partial class Seat : login1Object
    {
        private const string __Interface = "org.freedesktop.login1.Seat";
        public Seat(login1Service service, ObjectPath path) : base(service, path)
        { }
        public Task TerminateAsync()
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "Terminate");
                return writer.CreateMessage();
            }
        }
        public Task ActivateSessionAsync(string sessionId)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "s",
                    member: "ActivateSession");
                writer.WriteString(sessionId);
                return writer.CreateMessage();
            }
        }
        public Task SwitchToAsync(uint vtnr)
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    signature: "u",
                    member: "SwitchTo");
                writer.WriteUInt32(vtnr);
                return writer.CreateMessage();
            }
        }
        public Task SwitchToNextAsync()
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "SwitchToNext");
                return writer.CreateMessage();
            }
        }
        public Task SwitchToPreviousAsync()
        {
            return Connection.CallMethodAsync(CreateMessage());
            MessageBuffer CreateMessage()
            {
                var writer = Connection.GetMessageWriter();
                writer.WriteMethodCallHeader(
                    destination: Service.Destination,
                    path: Path,
                    @interface: __Interface,
                    member: "SwitchToPrevious");
                return writer.CreateMessage();
            }
        }
        public Task<string> GetIdAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Id"), (m, s) => ReadMessage_v_s(m, (login1Object)s!), this);
        public Task<(string, ObjectPath)> GetActiveSessionAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "ActiveSession"), (m, s) => ReadMessage_v_rsoz(m, (login1Object)s!), this);
        public Task<bool> GetCanTTYAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "CanTTY"), (m, s) => ReadMessage_v_b(m, (login1Object)s!), this);
        public Task<bool> GetCanGraphicalAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "CanGraphical"), (m, s) => ReadMessage_v_b(m, (login1Object)s!), this);
        public Task<(string, ObjectPath)[]> GetSessionsAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "Sessions"), (m, s) => ReadMessage_v_arsoz(m, (login1Object)s!), this);
        public Task<bool> GetIdleHintAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "IdleHint"), (m, s) => ReadMessage_v_b(m, (login1Object)s!), this);
        public Task<ulong> GetIdleSinceHintAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "IdleSinceHint"), (m, s) => ReadMessage_v_t(m, (login1Object)s!), this);
        public Task<ulong> GetIdleSinceHintMonotonicAsync()
            => Connection.CallMethodAsync(CreateGetPropertyMessage(__Interface, "IdleSinceHintMonotonic"), (m, s) => ReadMessage_v_t(m, (login1Object)s!), this);
        public Task<SeatProperties> GetPropertiesAsync()
        {
            return Connection.CallMethodAsync(CreateGetAllPropertiesMessage(__Interface), (m, s) => ReadMessage(m, (login1Object)s!), this);
            static SeatProperties ReadMessage(Message message, login1Object _)
            {
                var reader = message.GetBodyReader();
                return ReadProperties(ref reader);
            }
        }
        public ValueTask<IDisposable> WatchPropertiesChangedAsync(Action<Exception?, PropertyChanges<SeatProperties>> handler, bool emitOnCapturedContext = true, ObserverFlags flags = ObserverFlags.None)
        {
            return WatchPropertiesChangedAsync(__Interface, (m, s) => ReadMessage(m, (login1Object)s!), handler, emitOnCapturedContext, flags);
            static PropertyChanges<SeatProperties> ReadMessage(Message message, login1Object _)
            {
                var reader = message.GetBodyReader();
                reader.ReadString(); // interface
                List<string> changed = new(), invalidated = new();
                return new PropertyChanges<SeatProperties>(ReadProperties(ref reader, changed), ReadInvalidated(ref reader), changed.ToArray());
            }
            static string[] ReadInvalidated(ref Reader reader)
            {
                List<string>? invalidated = null;
                ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.String);
                while (reader.HasNext(arrayEnd))
                {
                    invalidated ??= new();
                    var property = reader.ReadString();
                    switch (property)
                    {
                        case "Id": invalidated.Add("Id"); break;
                        case "ActiveSession": invalidated.Add("ActiveSession"); break;
                        case "CanTTY": invalidated.Add("CanTTY"); break;
                        case "CanGraphical": invalidated.Add("CanGraphical"); break;
                        case "Sessions": invalidated.Add("Sessions"); break;
                        case "IdleHint": invalidated.Add("IdleHint"); break;
                        case "IdleSinceHint": invalidated.Add("IdleSinceHint"); break;
                        case "IdleSinceHintMonotonic": invalidated.Add("IdleSinceHintMonotonic"); break;
                    }
                }
                return invalidated?.ToArray() ?? Array.Empty<string>();
            }
        }
        private static SeatProperties ReadProperties(ref Reader reader, List<string>? changedList = null)
        {
            var props = new SeatProperties();
            ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.Struct);
            while (reader.HasNext(arrayEnd))
            {
                var property = reader.ReadString();
                switch (property)
                {
                    case "Id":
                        reader.ReadSignature("s"u8);
                        props.Id = reader.ReadString();
                        changedList?.Add("Id");
                        break;
                    case "ActiveSession":
                        reader.ReadSignature("(so)"u8);
                        props.ActiveSession = ReadType_rsoz(ref reader);
                        changedList?.Add("ActiveSession");
                        break;
                    case "CanTTY":
                        reader.ReadSignature("b"u8);
                        props.CanTTY = reader.ReadBool();
                        changedList?.Add("CanTTY");
                        break;
                    case "CanGraphical":
                        reader.ReadSignature("b"u8);
                        props.CanGraphical = reader.ReadBool();
                        changedList?.Add("CanGraphical");
                        break;
                    case "Sessions":
                        reader.ReadSignature("a(so)"u8);
                        props.Sessions = ReadType_arsoz(ref reader);
                        changedList?.Add("Sessions");
                        break;
                    case "IdleHint":
                        reader.ReadSignature("b"u8);
                        props.IdleHint = reader.ReadBool();
                        changedList?.Add("IdleHint");
                        break;
                    case "IdleSinceHint":
                        reader.ReadSignature("t"u8);
                        props.IdleSinceHint = reader.ReadUInt64();
                        changedList?.Add("IdleSinceHint");
                        break;
                    case "IdleSinceHintMonotonic":
                        reader.ReadSignature("t"u8);
                        props.IdleSinceHintMonotonic = reader.ReadUInt64();
                        changedList?.Add("IdleSinceHintMonotonic");
                        break;
                    default:
                        reader.ReadVariantValue();
                        break;
                }
            }
            return props;
        }
    }
    partial class login1Service
    {
        public Connection Connection { get; }
        public string Destination { get; }
        public login1Service(Connection connection, string destination)
            => (Connection, Destination) = (connection, destination);
        public LogControl1 CreateLogControl1(ObjectPath path) => new LogControl1(this, path);
        public Manager CreateManager(ObjectPath path) => new Manager(this, path);
        public User CreateUser(ObjectPath path) => new User(this, path);
        public Session CreateSession(ObjectPath path) => new Session(this, path);
        public Seat CreateSeat(ObjectPath path) => new Seat(this, path);
    }
    class login1Object
    {
        public login1Service Service { get; }
        public ObjectPath Path { get; }
        protected Connection Connection => Service.Connection;
        protected login1Object(login1Service service, ObjectPath path)
            => (Service, Path) = (service, path);
        protected MessageBuffer CreateGetPropertyMessage(string @interface, string property)
        {
            var writer = Connection.GetMessageWriter();
            writer.WriteMethodCallHeader(
                destination: Service.Destination,
                path: Path,
                @interface: "org.freedesktop.DBus.Properties",
                signature: "ss",
                member: "Get");
            writer.WriteString(@interface);
            writer.WriteString(property);
            return writer.CreateMessage();
        }
        protected MessageBuffer CreateGetAllPropertiesMessage(string @interface)
        {
            var writer = Connection.GetMessageWriter();
            writer.WriteMethodCallHeader(
                destination: Service.Destination,
                path: Path,
                @interface: "org.freedesktop.DBus.Properties",
                signature: "s",
                member: "GetAll");
            writer.WriteString(@interface);
            return writer.CreateMessage();
        }
        protected ValueTask<IDisposable> WatchPropertiesChangedAsync<TProperties>(string @interface, MessageValueReader<PropertyChanges<TProperties>> reader, Action<Exception?, PropertyChanges<TProperties>> handler, bool emitOnCapturedContext, ObserverFlags flags)
        {
            var rule = new MatchRule
            {
                Type = MessageType.Signal,
                Sender = Service.Destination,
                Path = Path,
                Interface = "org.freedesktop.DBus.Properties",
                Member = "PropertiesChanged",
                Arg0 = @interface
            };
            return Connection.AddMatchAsync(rule, reader,
                                                    (ex, changes, rs, hs) => ((Action<Exception?, PropertyChanges<TProperties>>)hs!).Invoke(ex, changes),
                                                    this, handler, emitOnCapturedContext, flags);
        }
        public ValueTask<IDisposable> WatchSignalAsync<TArg>(string sender, string @interface, ObjectPath path, string signal, MessageValueReader<TArg> reader, Action<Exception?, TArg> handler, bool emitOnCapturedContext, ObserverFlags flags)
        {
            var rule = new MatchRule
            {
                Type = MessageType.Signal,
                Sender = sender,
                Path = path,
                Member = signal,
                Interface = @interface
            };
            return Connection.AddMatchAsync(rule, reader,
                                                    (ex, arg, rs, hs) => ((Action<Exception?, TArg>)hs!).Invoke(ex, arg),
                                                    this, handler, emitOnCapturedContext, flags);
        }
        public ValueTask<IDisposable> WatchSignalAsync(string sender, string @interface, ObjectPath path, string signal, Action<Exception?> handler, bool emitOnCapturedContext, ObserverFlags flags)
        {
            var rule = new MatchRule
            {
                Type = MessageType.Signal,
                Sender = sender,
                Path = path,
                Member = signal,
                Interface = @interface
            };
            return Connection.AddMatchAsync(rule, (message, state) => null!,
                                                            (Exception? ex, object v, object? rs, object? hs) => ((Action<Exception?>)hs!).Invoke(ex), this, handler, emitOnCapturedContext, flags);
        }
        protected static string ReadMessage_v_s(Message message, login1Object _)
        {
            var reader = message.GetBodyReader();
            reader.ReadSignature("s"u8);
            return reader.ReadString();
        }
        protected static ObjectPath ReadMessage_o(Message message, login1Object _)
        {
            var reader = message.GetBodyReader();
            return reader.ReadObjectPath();
        }
        protected static (string, uint, string, string, ObjectPath)[] ReadMessage_arsussoz(Message message, login1Object _)
        {
            var reader = message.GetBodyReader();
            return ReadType_arsussoz(ref reader);
        }
        protected static (string, uint, string, string, uint, string, string, bool, ulong, ObjectPath)[] ReadMessage_arsussussbtoz(Message message, login1Object _)
        {
            var reader = message.GetBodyReader();
            return ReadType_arsussussbtoz(ref reader);
        }
        protected static (uint, string, ObjectPath)[] ReadMessage_arusoz(Message message, login1Object _)
        {
            var reader = message.GetBodyReader();
            return ReadType_arusoz(ref reader);
        }
        protected static (string, ObjectPath)[] ReadMessage_arsoz(Message message, login1Object _)
        {
            var reader = message.GetBodyReader();
            return ReadType_arsoz(ref reader);
        }
        protected static (string, string, string, string, uint, uint)[] ReadMessage_arssssuuz(Message message, login1Object _)
        {
            var reader = message.GetBodyReader();
            return ReadType_arssssuuz(ref reader);
        }
        protected static (string, ObjectPath, string, System.Runtime.InteropServices.SafeHandle?, uint, string, uint, bool) ReadMessage_soshusub(Message message, login1Object _)
        {
            var reader = message.GetBodyReader();
            var arg0 = reader.ReadString();
            var arg1 = reader.ReadObjectPath();
            var arg2 = reader.ReadString();
            var arg3 = reader.ReadHandle<Microsoft.Win32.SafeHandles.SafeFileHandle>();
            var arg4 = reader.ReadUInt32();
            var arg5 = reader.ReadString();
            var arg6 = reader.ReadUInt32();
            var arg7 = reader.ReadBool();
            return (arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }
        protected static string ReadMessage_s(Message message, login1Object _)
        {
            var reader = message.GetBodyReader();
            return reader.ReadString();
        }
        protected static bool ReadMessage_b(Message message, login1Object _)
        {
            var reader = message.GetBodyReader();
            return reader.ReadBool();
        }
        protected static System.Runtime.InteropServices.SafeHandle? ReadMessage_h(Message message, login1Object _)
        {
            var reader = message.GetBodyReader();
            return reader.ReadHandle<Microsoft.Win32.SafeHandles.SafeFileHandle>();
        }
        protected static (string, ObjectPath) ReadMessage_so(Message message, login1Object _)
        {
            var reader = message.GetBodyReader();
            var arg0 = reader.ReadString();
            var arg1 = reader.ReadObjectPath();
            return (arg0, arg1);
        }
        protected static (uint, ObjectPath) ReadMessage_uo(Message message, login1Object _)
        {
            var reader = message.GetBodyReader();
            var arg0 = reader.ReadUInt32();
            var arg1 = reader.ReadObjectPath();
            return (arg0, arg1);
        }
        protected static (bool, Dictionary<string, VariantValue>) ReadMessage_baesv(Message message, login1Object _)
        {
            var reader = message.GetBodyReader();
            var arg0 = reader.ReadBool();
            var arg1 = reader.ReadDictionaryOfStringToVariantValue();
            return (arg0, arg1);
        }
        protected static bool ReadMessage_v_b(Message message, login1Object _)
        {
            var reader = message.GetBodyReader();
            reader.ReadSignature("b"u8);
            return reader.ReadBool();
        }
        protected static uint ReadMessage_v_u(Message message, login1Object _)
        {
            var reader = message.GetBodyReader();
            reader.ReadSignature("u"u8);
            return reader.ReadUInt32();
        }
        protected static string[] ReadMessage_v_as(Message message, login1Object _)
        {
            var reader = message.GetBodyReader();
            reader.ReadSignature("as"u8);
            return reader.ReadArrayOfString();
        }
        protected static ulong ReadMessage_v_t(Message message, login1Object _)
        {
            var reader = message.GetBodyReader();
            reader.ReadSignature("t"u8);
            return reader.ReadUInt64();
        }
        protected static Dictionary<string, VariantValue> ReadMessage_v_aesv(Message message, login1Object _)
        {
            var reader = message.GetBodyReader();
            reader.ReadSignature("a{sv}"u8);
            return reader.ReadDictionaryOfStringToVariantValue();
        }
        protected static (string, ulong) ReadMessage_v_rstz(Message message, login1Object _)
        {
            var reader = message.GetBodyReader();
            reader.ReadSignature("(st)"u8);
            return ReadType_rstz(ref reader);
        }
        protected static (string, ObjectPath) ReadMessage_v_rsoz(Message message, login1Object _)
        {
            var reader = message.GetBodyReader();
            reader.ReadSignature("(so)"u8);
            return ReadType_rsoz(ref reader);
        }
        protected static (string, ObjectPath)[] ReadMessage_v_arsoz(Message message, login1Object _)
        {
            var reader = message.GetBodyReader();
            reader.ReadSignature("a(so)"u8);
            return ReadType_arsoz(ref reader);
        }
        protected static (System.Runtime.InteropServices.SafeHandle?, bool) ReadMessage_hb(Message message, login1Object _)
        {
            var reader = message.GetBodyReader();
            var arg0 = reader.ReadHandle<Microsoft.Win32.SafeHandles.SafeFileHandle>();
            var arg1 = reader.ReadBool();
            return (arg0, arg1);
        }
        protected static (uint, uint, string) ReadMessage_uus(Message message, login1Object _)
        {
            var reader = message.GetBodyReader();
            var arg0 = reader.ReadUInt32();
            var arg1 = reader.ReadUInt32();
            var arg2 = reader.ReadString();
            return (arg0, arg1, arg2);
        }
        protected static (uint, uint, System.Runtime.InteropServices.SafeHandle?) ReadMessage_uuh(Message message, login1Object _)
        {
            var reader = message.GetBodyReader();
            var arg0 = reader.ReadUInt32();
            var arg1 = reader.ReadUInt32();
            var arg2 = reader.ReadHandle<Microsoft.Win32.SafeHandles.SafeFileHandle>();
            return (arg0, arg1, arg2);
        }
        protected static (uint, ObjectPath) ReadMessage_v_ruoz(Message message, login1Object _)
        {
            var reader = message.GetBodyReader();
            reader.ReadSignature("(uo)"u8);
            return ReadType_ruoz(ref reader);
        }
        protected static (string, ulong) ReadType_rstz(ref Reader reader)
        {
            return (reader.ReadString(), reader.ReadUInt64());
        }
        protected static (string, ObjectPath) ReadType_rsoz(ref Reader reader)
        {
            return (reader.ReadString(), reader.ReadObjectPath());
        }
        protected static (string, ObjectPath)[] ReadType_arsoz(ref Reader reader)
        {
            List<(string, ObjectPath)> list = new();
            ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.Struct);
            while (reader.HasNext(arrayEnd))
            {
                list.Add(ReadType_rsoz(ref reader));
            }
            return list.ToArray();
        }
        protected static (uint, ObjectPath) ReadType_ruoz(ref Reader reader)
        {
            return (reader.ReadUInt32(), reader.ReadObjectPath());
        }
        protected static (string, uint, string, string, ObjectPath)[] ReadType_arsussoz(ref Reader reader)
        {
            List<(string, uint, string, string, ObjectPath)> list = new();
            ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.Struct);
            while (reader.HasNext(arrayEnd))
            {
                list.Add(ReadType_rsussoz(ref reader));
            }
            return list.ToArray();
        }
        protected static (string, uint, string, string, ObjectPath) ReadType_rsussoz(ref Reader reader)
        {
            return (reader.ReadString(), reader.ReadUInt32(), reader.ReadString(), reader.ReadString(), reader.ReadObjectPath());
        }
        protected static (string, uint, string, string, uint, string, string, bool, ulong, ObjectPath)[] ReadType_arsussussbtoz(ref Reader reader)
        {
            List<(string, uint, string, string, uint, string, string, bool, ulong, ObjectPath)> list = new();
            ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.Struct);
            while (reader.HasNext(arrayEnd))
            {
                list.Add(ReadType_rsussussbtoz(ref reader));
            }
            return list.ToArray();
        }
        protected static (string, uint, string, string, uint, string, string, bool, ulong, ObjectPath) ReadType_rsussussbtoz(ref Reader reader)
        {
            return (reader.ReadString(), reader.ReadUInt32(), reader.ReadString(), reader.ReadString(), reader.ReadUInt32(), reader.ReadString(), reader.ReadString(), reader.ReadBool(), reader.ReadUInt64(), reader.ReadObjectPath());
        }
        protected static (uint, string, ObjectPath)[] ReadType_arusoz(ref Reader reader)
        {
            List<(uint, string, ObjectPath)> list = new();
            ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.Struct);
            while (reader.HasNext(arrayEnd))
            {
                list.Add(ReadType_rusoz(ref reader));
            }
            return list.ToArray();
        }
        protected static (uint, string, ObjectPath) ReadType_rusoz(ref Reader reader)
        {
            return (reader.ReadUInt32(), reader.ReadString(), reader.ReadObjectPath());
        }
        protected static (string, string, string, string, uint, uint)[] ReadType_arssssuuz(ref Reader reader)
        {
            List<(string, string, string, string, uint, uint)> list = new();
            ArrayEnd arrayEnd = reader.ReadArrayStart(DBusType.Struct);
            while (reader.HasNext(arrayEnd))
            {
                list.Add(ReadType_rssssuuz(ref reader));
            }
            return list.ToArray();
        }
        protected static (string, string, string, string, uint, uint) ReadType_rssssuuz(ref Reader reader)
        {
            return (reader.ReadString(), reader.ReadString(), reader.ReadString(), reader.ReadString(), reader.ReadUInt32(), reader.ReadUInt32());
        }
        protected static void WriteType_arsvz(ref MessageWriter writer, (string, VariantValue)[] value)
        {
            ArrayStart arrayStart = writer.WriteArrayStart(DBusType.Struct);
            foreach (var item in value)
            {
                WriteType_rsvz(ref writer, item);
            }
            writer.WriteArrayEnd(arrayStart);
        }
        protected static void WriteType_rsvz(ref MessageWriter writer, (string, VariantValue) value)
        {
            writer.WriteStructureStart();
            writer.WriteString(value.Item1);
            writer.WriteVariant(value.Item2);
        }
    }
    class PropertyChanges<TProperties>
    {
        public PropertyChanges(TProperties properties, string[] invalidated, string[] changed)
        	=> (Properties, Invalidated, Changed) = (properties, invalidated, changed);
        public TProperties Properties { get; }
        public string[] Invalidated { get; }
        public string[] Changed { get; }
        public bool HasChanged(string property) => Array.IndexOf(Changed, property) != -1;
        public bool IsInvalidated(string property) => Array.IndexOf(Invalidated, property) != -1;
    }
}
