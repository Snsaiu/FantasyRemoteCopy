﻿using FantasyRemoteCopy.UI.Interfaces;

using System.Text;

namespace FantasyRemoteCopy.UI.Models;

public class SendTextModel(string flag, string targetFlag, string text, int port) : IFlag, ISize, ITargetFlag, IPort
{
    public string Flag { get; init; } = flag;

    public string Text { get; init; } = text;
    public long Size { get; } = Encoding.UTF8.GetByteCount(text);
    public string TargetFlag { get; init; } = targetFlag;
    public int Port { get; init; } = port;
}