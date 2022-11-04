using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using FantasyRemoteCopy.Core.Consts;
using FantasyRemoteCopy.Core.Models;
using FantasyResultModel;
using FantasyResultModel.Impls;
using Newtonsoft.Json;

namespace FantasyRemoteCopy.Core.Impls;

public class TcpDataSender:ISendData
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
    private  int sendData(Socket socket, byte[] buffer, int outTime)
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
        catch (SocketException e)
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
        var s = new ArraySegment<byte>(byteData);
        int res = await udpClient.SendToAsync(s, SocketFlags.None, point);
        udpClient.Close();
        return null;
    }

    public async Task<ResultBase<bool>> SendInviteAsync(TransformData data)
    {
        Socket udpClient = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        IPAddress ipaddress = IPAddress.Parse(data.TargetIp);
        EndPoint point = new IPEndPoint(ipaddress,int.Parse(data.Port));

        udpClient.SetSocketOption(SocketOptionLevel.IP,SocketOptionName.PacketInformation,true);

        string dataStr = JsonConvert.SerializeObject(data);
        byte[] byteData = Encoding.UTF8.GetBytes(dataStr);
        var s = new ArraySegment<byte>(byteData);
       int res=  await udpClient.SendToAsync(s, SocketFlags.None, point);
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
        var s = new ArraySegment<byte>(byteData);
        int res = await udpClient.SendToAsync(s, SocketFlags.None, point);
        udpClient.Close();
        return null;
    }

    public async Task<ResultBase<bool>> SendDataAsync(DataMetaModel data,string content,string deviceNickName)
    {
        var tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress ipaddress = IPAddress.Parse(data.TargetIp);
        EndPoint point = new IPEndPoint(ipaddress, int.Parse(ConstParams.TcpIp_Port));
        this.SendingDataEvent?.Invoke(data.TargetIp);
        //only for text
        if (data.DataType==DataType.Text)
        {
            tcpClient.Connect(point);
            TransformData td = new TransformData();
            td.TargetDeviceNickName= deviceNickName;
            td.DataGuid = data.Guid ;
            td.TargetIp = data.TargetIp;
            td.Port = ConstParams.TcpIp_Port;
           
            td.Type = Enums.TransformType.SendingTxtData;

            td.Data= Encoding.UTF8.GetBytes(content);

            ArraySegment<byte>b= Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(td));
           
            this.sendData(tcpClient, b.ToArray(),10);
            // await tcpClient.SendAsync(b, SocketFlags.None);
        
           var dmm= ConstParams.WillSendMetasQueue.FirstOrDefault(x => x.Guid == data.Guid);
            if(dmm!=null)
            {
                dmm.State = MetaState.Sended;
            }
           
            tcpClient.Close();
         
        }
        else // for file type
        {
            tcpClient.Connect(point);
        
            TransformData td = new TransformData();
            td.TargetDeviceNickName = deviceNickName;
            td.DataGuid = data.Guid;
            td.TargetIp = data.TargetIp;
            td.Port = ConstParams.TcpIp_Port;

            td.Type = Enums.TransformType.SendingFileData;
            
            //todo td.data is filename or file byte[]

            FileDataModel fdm=new FileDataModel();
            fdm.FileNameWithExtension = Path.GetFileName( content);

            var dmm = ConstParams.WillSendMetasQueue.FirstOrDefault(x => x.Guid == data.Guid);
            if (dmm == null)
            {
                return new ErrorResultModel<bool>("can not find file meta data! send error!");
            }

            FileStream st = new FileStream(content, FileMode.Open,FileAccess.Read,FileShare.ReadWrite);
            byte[] contentBytes = new byte[st.Length];

            int readAsync = await st.ReadAsync(contentBytes,0,(int)st.Length);

            st.Close();
            fdm.ContentBytes = contentBytes.ToArray();

            td.Data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(fdm));

            ArraySegment<byte> b = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(td));

            MemoryStream ms = new MemoryStream(b.ToArray());
            byte[] filechunk = new byte[1024];
            int numBytes;

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

                tcpClient.Shutdown(SocketShutdown.Both);
            }
            catch (Exception e)
            {

                
            }
            finally
            {
                 
            tcpClient.Close();
            }
            
          
            this.SendFinishedEvent?.Invoke(data.TargetIp);
            dmm.State = MetaState.Sended;
           
        }
        return new SuccessResultModel<bool>(true);
    }


}