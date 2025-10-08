using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia.Threading;
using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace TrailsHelper.Support
{
    /// <summary>
    /// Manages taskbar progress indication using Win32 ITaskbarList3 interface.
    /// Replaces deprecated System.Windows.Shell.TaskbarItemInfo functionality.
    /// </summary>
    public partial class TaskbarProgressManager : IDisposable
    {
        private ITaskbarList3? _taskbarInstance;
        private IntPtr _windowHandle;
        private bool _disposed = false;
        private ProgressState _currentState = ProgressState.None;
        private ulong _currentValue = 0;
        private ulong _maximumValue = 1000;

        /// <summary>
        /// Taskbar progress states matching System.Windows.Shell.TaskbarItemProgressState
        /// </summary>
        public enum ProgressState
        {
            /// <summary>No progress indicator displayed</summary>
            None = 0,
            /// <summary>Indeterminate progress (pulsing green)</summary>
            Indeterminate = 0x1,
            /// <summary>Normal progress (green)</summary>
            Normal = 0x2,
            /// <summary>Error state (red)</summary>
            Error = 0x4,
            /// <summary>Paused state (yellow)</summary>
            Paused = 0x8
        }

        #region COM Interfaces
        [GeneratedComInterface]
        [Guid("ea1afb91-9e28-4b86-90e9-9e9f8a5eefaf")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public partial interface ITaskbarList3
        {
            // ITaskbarList
            void HrInit();
            void AddTab(IntPtr hwnd);
            void DeleteTab(IntPtr hwnd);
            void ActivateTab(IntPtr hwnd);
            void SetActiveAlt(IntPtr hwnd);

            // ITaskbarList2
            void MarkFullscreenWindow(IntPtr hwnd, [MarshalAs(UnmanagedType.Bool)] bool fFullscreen);

            // ITaskbarList3
            void SetProgressValue(IntPtr hwnd, ulong ullCompleted, ulong ullTotal);
            void SetProgressState(IntPtr hwnd, ProgressState state);
        }

        #endregion

        #region P/Invoke

        [LibraryImport("ole32.dll")]
        private static partial int CoCreateInstance(
            in Guid rclsid,
            IntPtr pUnkOuter,
            uint dwClsContext,
            in Guid riid,
            out IntPtr ppv);

        private const uint CLSCTX_INPROC_SERVER = 0x1;

        // CLSID_TaskbarList: {56FDF344-FD6D-11d0-958A-006097C9A090}
        private static readonly Guid CLSID_TaskbarList = new Guid(
            0x56FDF344, 0xFD6D, 0x11d0, 0x95, 0x8A, 0x00, 0x60, 0x97, 0xC9, 0xA0, 0x90);

        // IID_ITaskbarList3: {EA1AFB91-9E28-4B86-90E9-9E9F8A5EEFAF}
        private static readonly Guid IID_ITaskbarList3 = new Guid(
            0xEA1AFB91, 0x9E28, 0x4B86, 0x90, 0xE9, 0x9E, 0x9F, 0x8A, 0x5E, 0xEF, 0xAF);

        #endregion

        /// <summary>
        /// Initializes the taskbar progress manager for the specified window.
        /// </summary>
        /// <param name="windowHandle">Handle to the window (HWND)</param>
        public TaskbarProgressManager(IntPtr windowHandle)
        {
            if (windowHandle == IntPtr.Zero)
                throw new ArgumentException("Window handle cannot be zero.", nameof(windowHandle));

            _windowHandle = windowHandle;

            try
            {
                Guid clsid = CLSID_TaskbarList;
                Guid iid = IID_ITaskbarList3;

                int hr = CoCreateInstance(in clsid, IntPtr.Zero, CLSCTX_INPROC_SERVER, in iid, out IntPtr ptr);
                if (hr < 0)
                    Marshal.ThrowExceptionForHR(hr);

                _taskbarInstance = (ITaskbarList3?)new StrategyBasedComWrappers().GetOrCreateObjectForComInstance(ptr, CreateObjectFlags.Unwrap);
                _taskbarInstance?.HrInit();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to initialize taskbar interface. " +
                    "Ensure Windows 7 or later and COM is available.", ex);
            }
        }

        public static TaskbarProgressManager? GetAvaloniaTaskbar(Window? window)
        {
            if (window?.TryGetPlatformHandle() is IPlatformHandle handle)
            {
                try
                {
                    var manager = new TaskbarProgressManager(handle.Handle);
                    return manager;
                }
                catch
                {
                }
            }

            return null;
        }
        
        /// <summary>
        /// Gets or sets the progress state of the taskbar button.
        /// </summary>
        public ProgressState State
        {
            get
            {
                ThrowIfDisposed();
                return _currentState;
            }
            set
            {
                ThrowIfDisposed();
                try
                {
                    Dispatcher.UIThread.Invoke(() => _taskbarInstance.SetProgressState(_windowHandle, value));
                    _currentState = value;
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to set progress state to {value}.", ex);
                }
            }
        }

        /// <summary>
        /// Gets or sets the current progress value.
        /// Use with MaximumValue to set the progress.
        /// </summary>
        public ulong CurrentValue
        {
            get
            {
                ThrowIfDisposed();
                return _currentValue;
            }
            set
            {
                ThrowIfDisposed();
                if (value > _maximumValue)
                    throw new ArgumentException("Current value cannot exceed maximum value.", nameof(value));

                try
                {
                    Dispatcher.UIThread.Invoke(() => _taskbarInstance?.SetProgressValue(_windowHandle, value, _maximumValue));
                    _currentValue = value;
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to set progress value ({value}/{_maximumValue}).", ex);
                }
            }
        }

        /// <summary>
        /// Gets or sets the maximum progress value.
        /// Use with CurrentValue to set the progress.
        /// </summary>
        public ulong MaximumValue
        {
            get
            {
                ThrowIfDisposed();
                return _maximumValue;
            }
            set
            {
                ThrowIfDisposed();
                if (value == 0)
                    throw new ArgumentException("Maximum value cannot be zero.", nameof(value));

                if (_currentValue > value)
                    _currentValue = value;

                try
                {
                    Dispatcher.UIThread.Invoke(() => _taskbarInstance?.SetProgressValue(_windowHandle, _currentValue, value));
                    _maximumValue = value;
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to set maximum value to {value}.", ex);
                }
            }
        }

        /// <summary>
        /// Gets or sets the progress as a percentage (0.0 to 1.0).
        /// </summary>
        public double ProgressPercentage
        {
            get
            {
                ThrowIfDisposed();
                return _maximumValue == 0 ? 0.0 : (double)_currentValue / _maximumValue;
            }
            set
            {
                ThrowIfDisposed();
                if (value < 0.0 || value > 1.0)
                    throw new ArgumentOutOfRangeException(nameof(value), "Percentage must be between 0.0 and 1.0.");

                ulong newValue = (ulong)(value * _maximumValue);
                CurrentValue = newValue;
            }
        }

        /// <summary>
        /// Sets the progress value with current and maximum values in one call.
        /// </summary>
        /// <param name="currentValue">Current progress value</param>
        /// <param name="maximumValue">Maximum progress value</param>
        public void SetProgress(ulong currentValue, ulong maximumValue)
        {
            ThrowIfDisposed();

            if (maximumValue == 0)
                throw new ArgumentException("Maximum value cannot be zero.", nameof(maximumValue));

            if (currentValue > maximumValue)
                throw new ArgumentException("Current value cannot exceed maximum value.", nameof(currentValue));

            try
            {
                _taskbarInstance?.SetProgressValue(_windowHandle, currentValue, maximumValue);
                _currentValue = currentValue;
                _maximumValue = maximumValue;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to set progress ({currentValue}/{maximumValue}).", ex);
            }
        }

        /// <summary>
        /// Clears the progress indicator from the taskbar button.
        /// </summary>
        public void ClearProgress()
        {
            State = ProgressState.None;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(TaskbarProgressManager));
        }

        /// <summary>
        /// Releases COM resources and clears the progress indicator.
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                try
                {
                    // Clear progress before disposing
                    _taskbarInstance?.SetProgressState(_windowHandle, ProgressState.None);
                }
                catch
                {
                    // Ignore errors during cleanup
                }

                _taskbarInstance = null;
                _disposed = true;
            }

            GC.SuppressFinalize(this);
        }

        ~TaskbarProgressManager()
        {
            Dispose();
        }
    }
}
