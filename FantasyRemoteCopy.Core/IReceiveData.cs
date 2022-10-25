using System;
using FantasyRemoteCopy.Core.Models;
namespace FantasyRemoteCopy.Core
{

	public delegate void ReceiveDataDelegate(TransformData data);
	public interface IReceiveData
	{
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

