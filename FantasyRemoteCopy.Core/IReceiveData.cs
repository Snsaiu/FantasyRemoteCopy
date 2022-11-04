using System;
using FantasyRemoteCopy.Core.Models;
namespace FantasyRemoteCopy.Core
{

	public delegate void ReceiveDataDelegate(TransformData data);

	public delegate void ReceiveInviteDelegate(TransformData data);

	/// <summary>
	/// 正在接收数据
	/// </summary>
	/// <param name="ip"></param>
    public delegate void ReceivingDataDelegate(string ip);

	public delegate void ReceivedFileFinishedDelegate(string ip);

	public delegate void ReceivingProcessDelegate(string ip, double process);


	/// <summary>
	/// 建立udp连接
	/// </summary>
	/// <param name="data"></param>
	public delegate void ReceiveBuildConnectionDelegate(TransformData data);

	public interface IReceiveData
	{


		event ReceiveDataDelegate ReceiveDataEvent;
		event ReceiveInviteDelegate ReceiveInviteEvent;
		event ReceiveBuildConnectionDelegate ReceiveBuildConnectionEvent;
		event ReceivingDataDelegate ReceivingDataEvent;
		event ReceivingProcessDelegate ReceivingProcessEvent;

		event ReceivedFileFinishedDelegate ReceivedFileFinishedEvent;

        /// <summary>
        /// 接收数据
        /// </summary>
        public void LiseningData(string ip, long byteCount);

		/// <summary>
		/// 接收邀请
		/// </summary>
		public void LiseningInvite();

		/// <summary>
		/// 接收建立连接
		/// </summary>
		public void LiseningBuildConnection();

	}
}

