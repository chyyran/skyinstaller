using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrailsHelper.Models
{
    public class InstalledLocation : ReactiveObject
    {
        private bool _isInstalled = false;
        public bool IsInstalled { get => _isInstalled; set => this.RaiseAndSetIfChanged(ref _isInstalled, value); }

        private string? _path = "nah";
        public string Path { get => _path; set => this.RaiseAndSetIfChanged(ref _path, value); }
    }
}
