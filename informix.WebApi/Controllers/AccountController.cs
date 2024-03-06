using informix.BLL;
using informix.WebApi.Common;
using informix.WebApi.Dto;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace informix.WebApi.Controllers
{
    /// <summary>
    /// 账户webapi
    /// </summary>
    public class AccountController : ApiController
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILog _log = LogManager.GetLogger(typeof(AccountController));

        /// <summary>
        /// 返回结果集
        /// </summary>
        public ResultHelper _resultHelper = new ResultHelper();
        /// <summary>
        /// 已排序的字典
        /// </summary>
        public SortedDictionary<string,string> _user = null;
        // GET: Account
        /// <summary>
        /// 登陆
        /// </summary>
        /// <returns></returns>
       
        [Route("api/Account/LogInOrOut")]
        [HttpPost]
        public  HttpResponseMessage LogInOrOut([FromBody]UserDto input)
        {
            _user =  new User().GetUserInfoByUserNameAndPwd(input.userName, input.password);

            if (_user != null)
            {
                var u = from c in _user
                        select c;
                u = u.Where(p => p.Key == "State" && p.Value == "1");
                //判断用户是否被冻结
                if (u.Count() > 0)
                {
                    _resultHelper.code = "FAIL";
                    _resultHelper.msg = "您的用户已经被冻结,请联系管理员";
                    _resultHelper.data = "[]";
                }
                else 
                {
                    _resultHelper.code = "SUCCESS";
                    _resultHelper.msg = "ok";
                    _resultHelper.data = _user;
                }
            }
            else
            {
                //判断用户和密码是否错误
                _resultHelper.code = "FAIL";
                _resultHelper.msg = "用户名或密码错误,请重新输入！";
                _resultHelper.data = "[]";
            }

            return new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(_resultHelper), Encoding.UTF8, "application/json"),
            };
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns>文件存储路径</returns>
        [Route("PackCode/PostFiles")]
        [HttpPost]
        public  HttpResponseMessage UploadZipFile(FileAttributes fc)
        {
            //提供客户端上传文件访问权限，获取文件列表
            HttpFileCollection filelist = HttpContext.Current.Request.Files;
            if (filelist != null && filelist.Count > 0)
            {
                for (int i = 0; i < filelist.Count; i++)
                {
                    HttpPostedFile file = filelist[i];
                    string filename = file.FileName;
                    string[] sArray = filename.Split(',');
                    //文件存放路径
                    string FilePath =new DBUtility.ConfigHelper().GetConfigPath("zip") + "\\" + sArray[0] + "\\";
                    DirectoryInfo di = new DirectoryInfo(FilePath);
                    //如果不存在此文件夹，则创建文件夹路径
                    if (!di.Exists) { di.Create(); }
                    try
                    {
                        //保存文件
                        file.SaveAs(FilePath + "\\" + sArray[1] + "," + sArray[2]);
                        _resultHelper.code = "SUCCESS";
                        //返回路径
                        _resultHelper.msg = FilePath + "\\" + sArray[1] + "," + sArray[2];
                    }
                    catch (Exception ex)
                    {
                        _log.Error(ex.Message);
                        _resultHelper.code = "FAIL";
                        _resultHelper.msg = "数据异常，请联系管理员！";
                    }
                }
            }
            else
            {
                _resultHelper.code = "FAIL";
                _resultHelper.msg = "上传的文件信息不存在！";
            }
            _resultHelper.data = "[]";
            return  new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(_resultHelper), Encoding.UTF8, "application/json"),
            };

        }

    }
}