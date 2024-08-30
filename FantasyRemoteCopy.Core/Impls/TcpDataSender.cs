using FantasyRemoteCopy.Core.Consts;
using FantasyRemoteCopy.Core.Models;

using FantasyResultModel;
using FantasyResultModel.Impls;

using Newtonsoft.Json;

using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FantasyRemoteCopy.Core.Impls;

public class TcpDataSender : ISendData
{
    public TcpDataSender()
    {
    }


    public event SendingDataDelegate SendingDataEvent;

    public event SendFinishedDelegate SendFinishedEvent;


    /// <summary>
    /// ��Զ��������������
    /// </summary>
    /// <param name="socket">Ҫ�����������Ѿ����ӵ�Զ�������� Socket</param>
    /// <param name="buffer">�����͵�����</param>
    /// <param name="outTime">�������ݵĳ�ʱʱ�䣬����Ϊ��λ�����Ծ�ȷ��΢��</param>
    /// <returns>0:�������ݳɹ���-1:��ʱ��-2:�������ݳ��ִ���-3:��������ʱ�����쳣</returns>
    /// <remarks >
    /// �� outTime ָ��Ϊ-1ʱ����һֱ�ȴ�ֱ����������Ҫ����
    /// </remarks>
    private int sendData(Socket socket, byte[] buffer, int outTime)
    {
        if (socket == null || socket.Connected == false)
        {
            throw new ArgumentException("����socket Ϊnull������δ���ӵ�Զ�̼����");
        }
        if (buffer == null || buffer.Length == 0)
        {
            throw new ArgumentException("����buffer Ϊnull ,���߳���Ϊ 0");
        }

        int flag = 0;
        try
        {
            int left = buffer.Length;
            int sndLen = 0;

            while (true)
            {
                if ((socket.Poll(outTime * 100, SelectMode.SelectWrite) == true))
                {        // �ռ����㹻��Ĵ������ݺ�ʼ����
                    sndLen = socket.Send(buffer, sndLen, left, SocketFlags.None);
                    left -= sndLen;
                    if (left == 0)
                    {                                        // �����Ѿ�ȫ������
                        flag = 0;
                        break;
                    }
                    else
                    {
                        if (sndLen > 0)
                        {                                    // ���ݲ����Ѿ�������
                            continue;
                        }
                        else
                        {                                                // �������ݷ�������
                            flag = -2;
                            break;
                        }
                    }
                }
                else
                {                                                        // ��ʱ�˳�
                    flag = -1;
                    break;
                }
            }
        }
        catch (SocketException)
        {

            flag = -3;
        }
        return flag;
    }

    public async Task<ResultBase<bool>> SendRquestBuildConnectionDataAsync(TransformData data)
    {
        Socket udpClient = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        IPAddress ipaddress = IPAddress.Parse(data.TargetIp);
        EndPoint point = new IPEndPoint(ipaddress, int.Parse(data.Port));

        udpClient.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.PacketInformation, true);

        string dataStr = JsonConvert.SerializeObject(data);
        byte[] byteData = Encoding.UTF8.GetBytes(dataStr);
        ArraySegment<byte> s = new ArraySegment<byte>(byteData);
        int res = await udpClient.SendToAsync(s, SocketFlags.None, point);
        udpClient.Close();
        return null;
    }

    public async Task<ResultBase<bool>> SendInviteAsync(TransformData data)
    {
        Socket udpClient = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        IPAddress ipaddress = IPAddress.Parse(data.TargetIp);
        EndPoint point = new IPEndPoint(ipaddress, int.Parse(data.Port));

        udpClient.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.PacketInformation, true);

        string dataStr = JsonConvert.SerializeObject(data);
        byte[] byteData = Encoding.UTF8.GetBytes(dataStr);
        ArraySegment<byte> s = new ArraySegment<byte>(byteData);
        int res = await udpClient.SendToAsync(s, SocketFlags.None, point);
        udpClient.Close();
        return null;
    }

    public async Task<ResultBase<bool>> SendBuildConnectionAsync(TransformData data)
    {
        Socket udpClient = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        IPAddress ipaddress = IPAddress.Parse(data.TargetIp);
        EndPoint point = new IPEndPoint(ipaddress, int.Parse(data.Port));

        udpClient.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.PacketInformation, true);

        string dataStr = JsonConvert.SerializeObject(data);
        byte[] byteData = Encoding.UTF8.GetBytes(dataStr);
        ArraySegment<byte> s = new ArraySegment<byte>(byteData);
        int res = await udpClient.SendToAsync(s, SocketFlags.None, point);
        udpClient.Close();
        return null;
    }

    public async Task<ResultBase<bool>> SendDataAsync(DataMetaModel data, string content, string deviceNickName)
    {
        Socket tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress ipaddress = IPAddress.Parse(data.TargetIp);
        EndPoint point = new IPEndPoint(ipaddress, int.Parse(ConstParams.TcpIp_Port));
        SendingDataEvent?.Invoke(data.TargetIp);
        //only for text
        if (data.DataType == DataType.Text)
        {
            tcpClient.Connect(point);
            TransformData td = new TransformData
            {
                TargetDeviceNickName = deviceNickName,
                DataGuid = data.Guid,
                TargetIp = data.TargetIp,
                Port = ConstParams.TcpIp_Port,

                Type = Enums.TransformType.SendingTxtData,

                Data = Encoding.UTF8.GetBytes(content)
            };

            ArraySegment<byte> b = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(td));
            byte[] contentsize = Encoding.UTF8.GetBytes(b.ToArray().Length.ToString());
            //tcpClient.Send(contentsize, 0, contentsize.Length, SocketFlags.None);
            sendData(tcpClient, contentsize, 10);
            await Task.Delay(1000);
            sendData(tcpClient, b.ToArray(), 10);
            // await tcpClient.SendAsync(b, SocketFlags.None);

            DataMetaModel dmm = ConstParams.WillSendMetasQueue.FirstOrDefault(x => x.Guid == data.Guid);
            if (dmm != null)
            {
                dmm.State = MetaState.Sended;
            }

            tcpClient.Close();
            SendFinishedEvent?.Invoke(data.TargetIp);

        }
        else // for file type
        {
            tcpClient.Connect(point);

            TransformData td = new TransformData
            {
                TargetDeviceNickName = deviceNickName,
                DataGuid = data.Guid,
                TargetIp = data.TargetIp,
                Port = ConstParams.TcpIp_Port,

                Type = Enums.TransformType.SendingFileData
            };

            //todo td.data is filename or file byte[]

            FileDataModel fdm = new FileDataModel
            {
                FileNameWithExtension = Path.GetFileName(content)
            };

            DataMetaModel dmm = ConstParams.WillSendMetasQueue.FirstOrDefault(x => x.Guid == data.Guid);
            if (dmm == null)
            {
                return new ErrorResultModel<bool>("can not find file meta data! send error!");
            }

            FileStream st = new FileStream(content, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            byte[] contentBytes = new byte[st.Length];

            int readAsync = await st.ReadAsync(contentBytes, 0, (int)st.Length);

            st.Close();
            fdm.ContentBytes = contentBytes.ToArray();

            td.Data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(fdm));

            ArraySegment<byte> b = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(td));

            MemoryStream ms = new MemoryStream(b.ToArray());
            byte[] filechunk = new byte[1024];
            int numBytes;
            byte[] contentsize = Encoding.UTF8.GetBytes(b.ToArray().Length.ToString());
            tcpClient.Send(contentsize, 0, contentsize.Length, SocketFlags.None);
            await Task.Delay(1000);
            try
            {
                while ((numBytes = ms.Read(filechunk, 0, 1024)) > 0)
                {
                    if (tcpClient.Send(filechunk, numBytes, SocketFlags.None) != numBytes)
                    {
                        throw new Exception("Error in sending the file");
                    }
                    //bytesSoFar += numBytes;
                    //Byte progress = (byte)(bytesSoFar * 100 / totalBytes);
                    //if (progress > lastStatus && progress != 100)
                    //{
                    //    Console.WriteLine("File sending progress:{0}", lastStatus);
                    //    lastStatus = progress;
                    //}
                }

                // tcpClient.Shutdown(SocketShutdown.Send);
            }
            catch (Exception)
            {


            }
            finally
            {

                tcpClient.Close();
            }


            SendFinishedEvent?.Invoke(data.TargetIp);
            dmm.State = MetaState.Sended;

        }
        return new SuccessResultModel<bool>(true);
    }


}