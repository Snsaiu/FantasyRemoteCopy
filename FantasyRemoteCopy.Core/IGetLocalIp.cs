using System;
using FantasyResultModel;
namespace FantasyRemoteCopy.Core
{
	/// <summary>
	/// 获得本地Ip
	/// </summary>
	public interface IGetLocalIp
	{

		ResultBase<string> GetLocalIp();
		
	}
}

