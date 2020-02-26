using Every.Core.Bamboo.Service.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TNetwork;
using TNetwork.Common;
using TNetwork.Data;

namespace Every.Core.Bamboo.Service
{
    public class BambooService
    {
        public readonly string POSTS_LIST_INQUIRY_URL = "/bamboo/post"; // 게시글 목록 조회
        public readonly string MAKE_POST_URL = "/bamboo/post"; // 게시글 작성

        public readonly string REPLIES_LIST_INQUIRY_URL = "/bamboo/reply?post="; // 댓글 목록 조회
        public readonly string MAKE_REPLY_URL = "/bamboo/reply"; // 댓글 작성
        public readonly string MODIFY_REPLY_URL = "/bamboo/reply/"; // 댓글 수정
        public readonly string DELETE_REPLY_URL = "/bamboo/reply/"; // 댓글 삭제 

        public NetworkManager networkManager = new NetworkManager();

        /// <summary>
        /// 게시글 목록 조회 메소드
        /// </summary>
        /// <returns></returns>
        public async Task<TResponse<GetPostsResponse>> GetPosts()
        {
            var client = new RestClient(Options.serverUrl);
            var restRequest = new RestRequest(POSTS_LIST_INQUIRY_URL, Method.GET);
            restRequest.AddHeader("token", Options.tokenInfo.Token);
            var response = await client.ExecuteTaskAsync(restRequest);

            var resp = JsonConvert.DeserializeObject<TResponse<GetPostsResponse>>(response.Content);
            return resp;

            //return await networkManager.GetResponse<GetPostsResponse>(POSTS_LIST_INQUIRY_URL, Method.GET, null);
        }

        /// <summary>
        /// 게시글 작성 메소드
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<TResponse<Nothing>> MakePost(string title, string content)
        {
            JObject jObject = new JObject();
            jObject["title"] = title;
            jObject["content"] = content;
            return await networkManager.GetResponse<Nothing>(MAKE_POST_URL, Method.POST, jObject.ToString());
        }

        /// <summary>
        /// 댓글 목록 조회 메소드
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public async Task<TResponse<GetRepliesResponse>> GetReplies(int idx)
        {
            string requestUrl = REPLIES_LIST_INQUIRY_URL + idx;
            return await networkManager.GetResponse<GetRepliesResponse>(requestUrl, Method.GET, null);
        }

        /// <summary>
        /// 댓글 작성 메소드   
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<TResponse<Nothing>> MakeReply(string content)
        {
            JObject jObject = new JObject();
            jObject["content"] = content;
            return await networkManager.GetResponse<Nothing>(MAKE_REPLY_URL, Method.POST, jObject.ToString());
        }

        /// <summary>
        /// 댓글 수정 메소드
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<TResponse<Nothing>> ModifyReply(int idx, string content)
        {
            string requestUrl = MODIFY_REPLY_URL + idx;
            JObject jObject = new JObject();
            jObject["content"] = content;
            return await networkManager.GetResponse<Nothing>(requestUrl, Method.PUT, jObject.ToString());
        }

        public async Task<TResponse<Nothing>> DeleteReply(int idx)
        {
            string requestUrl = DELETE_REPLY_URL + idx;
            return await networkManager.GetResponse<Nothing>(requestUrl, Method.DELETE, null);
        }
    }
}
