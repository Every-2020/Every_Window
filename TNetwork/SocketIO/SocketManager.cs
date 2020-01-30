using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Quobject.EngineIoClientDotNet.ComponentEmitter;
using Quobject.SocketIoClientDotNet.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNetwork.Data;
using TNetwork.Common;
using Options = TNetwork.Common.Options;
using TNetwork.SocketIO.Request.Data;
using TNetwork.SocketIO.Response;

namespace TNetwork
{
    public partial class SocketManager
    {

        
        private Manager manager;

        public delegate void SocketEventHandler(object sender, string resp);
        public delegate void SocketEventHandler<T>(object sender, SResponse<T> resp);
        public delegate void NoStatusSocketEventHandler<T>(object sender, T resp);

        public event EventHandler Connected;

        private const int MAX_CONNECTION = 2;

        private int retrivedConnection = 0;

        public SocketManager()
        {
            //here we define the connection options
            IO.Options op = new IO.Options();
            op.Query = new Dictionary<string, string>() { { "token", Options.tokenInfo.Token } };
            op.ForceNew = true;
            op.AutoConnect = true;
            op.Reconnection = true;
            op.ReconnectionDelay = 500;
            op.ReconnectionAttempts = 3;
            op.Secure = true;
            op.Upgrade = true;

            manager = new Manager(new Uri(Options.serverUrl), op);

            //ConnectTo(ref chatSocket, NAMESPACE_CHAT);
            //ConnectTo(ref boardSocket, NAMESPACE_BOARD);
        }

        //public void SocketRelease()
        //{
        //    DisConnectTo(ref chatSocket);
        //    DisConnectTo(ref boardSocket);
        //}

        //TODO : APP에서 소켓 생성할 때 이걸로 호출해야함 ㅇㅋ?
        public void ConnectTo(ref Socket target, string targetNamespace)
        {
            if(target != null)
            {
                target.Off();
                target = null;
            }
            target = manager.Socket(targetNamespace);
            target.Io();
            
            target.On(Socket.EVENT_CONNECT, () =>
            {
                Debug.WriteLine(targetNamespace + " 소켓 연결됨");
                retrivedConnection++;

                if (retrivedConnection >= MAX_CONNECTION)
                {
                    Connected?.Invoke(this, null);
                    retrivedConnection = 0;
                }
            });

            target.On(Socket.EVENT_DISCONNECT, () =>
            {
                Debug.WriteLine(targetNamespace + " 소켓 연결 끊어짐");
                //ConnectTo(ref temp, NAMESPACE_CHAT);
            });
        }

        public void DisConnectTo(ref Socket target)
        {
            if(target != null)
            {
                target.Close();
                target = null;
            }
        }

        private void OnSocketAuthenticated(object sender, string resp)
        {
            Debug.WriteLine("소켓 재연결 및 인증 됨");
        }

        /// <summary>
        /// 소켓 인증하기
        /// </summary>
        /// <param name="tokenInfo">사용자 TokenInfo</param>
        /// <returns></returns>
        //public void Authentication(TokenInfo tokenInfo)
        //{
        //    var data = new JObject();
        //    data["token"] = tokenInfo.Token;
        //    data["device"] = "pc";

        //     Emit("authentication", data);
        //    Debug.WriteLine("소켓 인증시도중");
        //}

        /// <summary>
        /// 소켓이 인증 성공 및 연결 유지
        /// </summary>
        /// <param name="socketEventHandler"></param>
        /// <returns></returns>
        //public void Authenticated(SocketEventHandler socketEventHandler)
        //{
        //     EventOn("authenticated", (s, e) =>
        //    {
        //        Debug.WriteLine("소켓 인증됨");
        //        socketEventHandler(s, e);
        //    });
        //}

        /// <summary>
        /// 소켓 인증 실패 및 연결 해제
        /// </summary>
        /// <param name="socketEventHandler"></param>
        /// <returns></returns>
        //public void Disconnect(SocketEventHandler socketEventHandler)
        //{
        //    EventOn("disconnect", (s, e) =>
        //    {
        //        Debug.WriteLine("소켓 인증 실패");
        //        socketEventHandler(s, e);
        //    });
        //}


        /// <summary>
        /// 소켓 이벤트 생성
        /// </summary>
        /// <param name="eventString">요청받을 이벤트 이름</param>
        /// <param name="OnDataGetEnded">요청받은 후 발생시킬 이벤트</param>
        // TODO : Warning 제거
        public void EventOn(Socket socket, string eventString, SocketEventHandler OnDataGetEnded)
        {
            socket.On(eventString, (data) =>
            {
                if (data != null)
                {
                    if (OnDataGetEnded != null)
                    {
                        OnDataGetEnded(this, data.ToString());
                    }
                }
            });
        }

        /// <summary>
        /// 일회용 소켓 이벤트 생성
        /// </summary>
        /// <param name="eventString">요청받을 이벤트 이름</param>
        /// <param name="OnDataGetEnded">요청받은 후 발생시킬 이벤트</param>
        public void EventOnce(Socket socket, string eventString, SocketEventHandler OnDataGetEnded)
        {
            socket.On(eventString, (data) =>
            {
                if (data != null)
                {
                    if (OnDataGetEnded != null)
                    {
                        OnDataGetEnded(this, data.ToString());
                    }
                }
                socket.Off(eventString);
            });
        }

        /// <summary>
        /// 파라미터로 받은 객체를 카멜 케이스로 변환 후 이벤트를 발생시킴
        /// </summary>
        /// <param name="data">보낼 데이터</param>
        public void EmitEvent(Socket socket, string eventName, IEmitData data)
        {
            var json = SerializeSnakeCase(data);
#if true //chris - 필요할때만 풀어서 사용
            Debug.WriteLine(json);
#endif
            Emit(socket, eventName, json);
        }

        /// <summary>
        /// 이미 변환된 데이터로 이벤트 발생시킴
        /// </summary>
        /// <param name="json"></param>
        public void Emit(Socket socket, string eventName, JObject json)
        {
            socket.Emit(eventName, json);
        }

        /// <summary>
        /// 소켓 이벤트 발생시키기
        /// </summary>
        /// <param name="eventString">발생시킬 이벤트 이름</param>
        /// <param name="data">보낼 데이터</param>
        public void Emit(Socket socket, string eventStr, object data)
        {
            socket.Emit(eventStr, data);
        }

        

        /// <summary>
        /// 서버로부터 받은 데이터 파싱
        /// </summary>
        /// <typeparam name="T">파싱할 Response 모델</typeparam>
        /// <param name="json">파싱할 데이터</param>
        /// <returns></returns>
        public SResponse<T> ParseData<T>(string json)
        {
            var resp = DeserializeSnakeCase<SResponse<T>>(json);
            return resp;
        }

        public T ParseDataNoStatus<T>(string json)
        {
            var resp = DeserializeSnakeCase<T>(json);
            return resp;
        }



        /// <summary>
        /// snake_case로 이뤄진 Json을 PascalCase로 역직렬화.
        /// </summary>
        /// <typeparam name="T">반환 형태</typeparam>
        /// <param name="json">목표 Json</param>
        /// <returns>역직렬화된 Json</returns>
        private static T DeserializeSnakeCase<T>(string json)
        {
            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };

            try
            {
                var resp = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
                {
                    ContractResolver = contractResolver,
                    Formatting = Formatting.Indented
                });
                return resp;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return default(T);
        }

        /// <summary>
        /// PascalCase로 이뤄진 객체를 snake_case json으로 직렬화.
        /// </summary>
        /// <typeparam name="T">변환할 객체 형태</typeparam>
        /// <param name="obj">변환할 객체</param>
        /// <returns></returns>
        private static JObject SerializeSnakeCase<T>(T obj)
        {
            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };

            try
            {
                var json = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
                {
                    ContractResolver = contractResolver,
                    Formatting = Formatting.Indented
                });
                return JObject.Parse(json);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return new JObject();
        }
    }
}
