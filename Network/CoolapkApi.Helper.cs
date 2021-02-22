using Aliyun.OSS;
using Aliyun.OSS.Common;
using Aliyun.OSS.Util;
using Coolapk_UWP.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coolapk_UWP.Network {
    public class CoolapkApiHelper {
        public delegate void OssUploadProgressHandler(OssUploadPicturePrepareResultFileInfo fileInfo, object sender, StreamTransferProgressArgs args);
        /// <summary>
        /// oss上传
        /// </summary>
        /// <param name="prepareInfo">酷安接口返回的信息</param>
        /// <param name="fileInfo">酷安接口返回的文件信息</param>
        /// <param name="stream">文件流</param>
        /// <param name="contentType">上传文件的imei类型</param>
        /// <returns>响应数据</returns>
        /// <exception cref="OssException">OSS异常</exception>
        /// <exception cref="Newtonsoft.Json.JsonSerializationException">JSON序列化异常</exception>
        public static Resp<OssUploadPictureResponse> OssUpload(
            OssUploadPicturePrepareResultUploadPrepareInfo prepareInfo,
            OssUploadPicturePrepareResultFileInfo fileInfo,
            Stream stream,
            string contentType,
            OssUploadProgressHandler progressHandler = null) {
            var oss = new Aliyun.OSS.OssClient(
                    prepareInfo.EndPoint.Replace("https://", ""),
                    prepareInfo.AccessKeyId,
                    prepareInfo.AccessKeySecret,
                    prepareInfo.SecurityToken
                );
            var callback = "eyJjYWxsYmFja0JvZHlUeXBlIjoiYXBwbGljYXRpb25cL2pzb24iLCJjYWxsYmFj" +
                "a0hvc3QiOiJhcGkuY29vbGFway5jb20iLCJjYWxsYmFja1VybCI6Imh0dHBzOlwvXC9hcGkuY29vbGF" +
                "way5jb21cL3Y2XC9jYWxsYmFja1wvbW9iaWxlT3NzVXBsb2FkU3VjY2Vzc0NhbGxiYWNrP2NoZWNrQX" +
                "J0aWNsZUNvdmVyUmVzb2x1dGlvbj0wJnZlcnNpb25Db2RlPTIxMDIwMzEiLCJjYWxsYmFja0JvZHkiO" +
                "iJ7XCJidWNrZXRcIjoke2J1Y2tldH0sXCJvYmplY3RcIjoke29iamVjdH0sXCJoYXNQcm9jZXNzXCI6" +
                "JHt4OnZhcjF9fSJ9";
            var callbackVar = "eyJ4OnZhcjEiOiJmYWxzZSJ9";
            var metadata = new ObjectMetadata {
                ContentMd5 = OssUtils.ComputeContentMd5(stream, stream.Length),
                ContentType = contentType
            };
            metadata.AddHeader(HttpHeaders.Callback, callback);
            metadata.AddHeader(HttpHeaders.CallbackVar, callbackVar);

            var request = new PutObjectRequest(
                prepareInfo.Bucket,
                fileInfo.UploadFileName,
                stream,
                metadata);
            request.StreamTransferProgress += (object sender, StreamTransferProgressArgs args) => {
                // 文件上传进度回调
                progressHandler?.Invoke(fileInfo, sender, args);
            };
            var putResult = oss.PutObject(request);

            // 相应数据
            var response = GetCallbackResponse(putResult);
            var jsonObj = JsonConvert.DeserializeObject<Resp<OssUploadPictureResponse>>(response);
            return jsonObj;
        }
        private static string GetCallbackResponse(PutObjectResult putObjectResult) {
            string callbackResponse = null;
            using (var stream = putObjectResult.ResponseStream) {
                var buffer = new byte[4 * 1024];
                var bytesRead = stream.Read(buffer, 0, buffer.Length);
                callbackResponse = Encoding.Default.GetString(buffer, 0, bytesRead);
            }
            return callbackResponse;
        }
    }
}
