using Every.Core.Bamboo.Service.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public readonly string POSTS_INQUIRY_URL = "/bamboo/post/"; // 게시물 조회

        public readonly string REPLIES_LIST_INQUIRY_URL = "/bamboo/reply?post="; // 댓글 목록 조회
        public readonly string MAKE_REPLY_URL = "/bamboo/reply"; // 댓글 작성
        public readonly string MODIFY_REPLY_URL = "/bamboo/reply/"; // 댓글 수정
        public readonly string DELETE_REPLY_URL = "/bamboo/reply/"; // 댓글 삭제 

        public NetworkManager networkManager = new NetworkManager();

        /// <summary>
        /// 게시글 목록 조회 메소드 GetPosts(), 게시글 조회 GetPost()와 구분할 것.
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
        }

        /// <summary>
        /// 게시글 작성 메소드
        /// </summary>
        /// <param name="content", 게시글 내용></param>
        /// <returns></returns>
        public async Task<TResponse<Nothing>> MakePost(string content)
        {
            var client = new RestClient(Options.serverUrl);
            var restRequest = new RestRequest(MAKE_POST_URL, Method.POST);
            JObject jObject = new JObject();
            jObject["content"] = content;
            restRequest.AddHeader("token", Options.tokenInfo.Token);
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddParameter("application/json", jObject.ToString(), ParameterType.RequestBody);
            var response = await client.ExecuteTaskAsync(restRequest);
            var resp = JsonConvert.DeserializeObject<TResponse<Nothing>>(response.Content);
            return resp;
        }

        /// <summary>
        /// 게시글 조회 GetPost(), 게시글 목록 조회 GetPosts()와 구분할 것.
        /// </summary>
        /// <param name="idx", 선택한 게시글의 idx></param>
        /// <returns></returns>
        public async Task<TResponse<GetPostResponse>> GetPost(int idx)
        {
            string requestUrl = POSTS_INQUIRY_URL + idx;
            var client = new RestClient(Options.serverUrl);
            var restRequest = new RestRequest(requestUrl, Method.GET);
            restRequest.AddHeader("token", Options.tokenInfo.Token);
            var response = await client.ExecuteTaskAsync(restRequest);
            var resp = JsonConvert.DeserializeObject<TResponse<GetPostResponse>>(response.Content);
            return resp;
        }

        /// <summary>
        /// 댓글 목록 조회 메소드
        /// </summary>
        /// <param name="idx", 선택한 게시글의 idx></param>
        /// <returns></returns>
        public async Task<TResponse<GetRepliesResponse>> GetReplies(int idx)
        {
            string requestUrl = REPLIES_LIST_INQUIRY_URL + idx;
            var client = new RestClient(Options.serverUrl);
            var restRequest = new RestRequest(requestUrl, Method.GET);
            restRequest.AddHeader("token", Options.tokenInfo.Token);
            var response = await client.ExecuteTaskAsync(restRequest);
            var resp = JsonConvert.DeserializeObject<TResponse<GetRepliesResponse>>(response.Content);
            return resp;    
        }

        /// <summary>
        /// 댓글 작성 메소드   
        /// </summary>
        /// <param name="content", 댓글 내용></param>
        /// <returns></returns>
        public async Task<TResponse<Nothing>> MakeReply(string content, int idx)
        {
            var client = new RestClient(Options.serverUrl);
            var restRequest = new RestRequest(MAKE_REPLY_URL, Method.POST);
            JObject jObject = new JObject();
            jObject["content"] = content;
            jObject["post"] = idx;
            restRequest.AddHeader("token", Options.tokenInfo.Token);
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddParameter("application/json", jObject.ToString(), ParameterType.RequestBody);
            var response = await client.ExecuteTaskAsync(restRequest);
            var resp = JsonConvert.DeserializeObject<TResponse<Nothing>>(response.Content);
            return resp;
        }

        /// <summary>
        /// 댓글 수정 메소드
        /// </summary>
        /// <param name="content", 수정할 댓글 내용></param>
        /// <returns></returns>
        public async Task<TResponse<Nothing>> ModifyReply(int idx, string content)
        {
            string requestUrl = MODIFY_REPLY_URL + idx;
            var client = new RestClient(Options.serverUrl);
            var restRequest = new RestRequest(requestUrl, Method.PUT);
            JObject jObject = new JObject();
            jObject["content"] = content;
            restRequest.AddHeader("token", Options.tokenInfo.Token);
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddParameter("application/json", jObject.ToString(), ParameterType.RequestBody);
            var response = await client.ExecuteTaskAsync(restRequest);
            var resp = JsonConvert.DeserializeObject<TResponse<Nothing>>(response.Content);
            return resp;
        }

        /// <summary>
        /// 댓글 삭제 메소드
        /// </summary>
        /// <param name="idx", 삭제할 댓글의 게시글 idx></param>
        /// <returns></returns>
        public async Task<TResponse<Nothing>> DeleteReply(int idx)
        {
            string requestUrl = DELETE_REPLY_URL + idx;
            var client = new RestClient(Options.serverUrl);
            var restRequest = new RestRequest(requestUrl, Method.DELETE);
            restRequest.AddHeader("token", Options.tokenInfo.Token);
            var response = await client.ExecuteTaskAsync(restRequest);
            var resp = JsonConvert.DeserializeObject<TResponse<Nothing>>(response.Content);
            return resp;
        }
    }
}
