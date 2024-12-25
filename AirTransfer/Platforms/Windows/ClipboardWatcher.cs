using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using AirTransfer.Interfaces;
using AirTransfer.Interfaces.Impls;

namespace AirTransfer;

public sealed class ClipboardWatcher : LoopClipboardWatcherBase;