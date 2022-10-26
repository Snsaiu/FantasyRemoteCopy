using System;
using FantasyRemoteCopy.Core.Models;
namespace FantasyRemoteCopy.Core
{

	public delegate void ReceiveDataDelegate(TransformData data);

	public delegate void ReceiveInviteDelegate(TransformData data);
	public interface IReceiveData
	{


		event ReceiveDataDelegate ReceiveDataEvent;
		event ReceiveInviteDelegate ReceiveInviteEvent;

		/// <summary>
		/// 接收数据
		/// </summary>
		public void LiseningData();

		/// <summary>
		/// 接收邀请
		/// </summary>
		public void LiseningInvite();

	}
}

