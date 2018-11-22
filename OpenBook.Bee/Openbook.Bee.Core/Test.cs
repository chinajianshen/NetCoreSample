using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Openbook.Bee.Core
{
    class Test
    {
    }
    /*
     * 
     using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using CyberResource.Transfer8.T8Utils;
using System.Runtime.Remoting;
using System.Threading;
using System.Windows.Forms;
using System.Reflection;
using System.Security.Cryptography;
using EnterpriseDT.Net.Ftp;
using CyberResource.Transfer8.T8Common;
using CyberResource.Transfer8.T8POSPreDefine;
using System.Configuration;

namespace CyberResource.Transfer8.T8RemoteObjects
{

    /// <summary>
    /// 远程处理对象，可以使用的是GetTask，GetNotification，AddTask，UpdateTask，UpdateNotifaction
    /// </summary>
    public class T8Service : MarshalByRefObject
    {
        const int MAX_RETRY_TIMES = 5;
        const string processing_task = "processing_task.dat";
        const string finished_task = "finisheding_task.dat";
        const string error_task = "error_task.dat";
        const string DOWNLOAD_SYSTEM_UPGRADE_DIR = "downloadsys";
        const string processing_notifaction = "processing_notifaction.dat";
        const string finished_notifaction = "finisheding_notifaction.dat";
        const string remoting_server_config = "remoting_server.config";
        const string sql_start_time = "@StartTime";
        const string sql_end_time = "@EndTime";
        const string t8_config = "t8.config";
        //-------------------------------
        /// <summary>
        /// 下载的更新文件，Exe和DLL，Config等，自动更新使用的文件夹，会自动备份
        /// </summary>
        const string work_dir_for_download_exe_file_saved = "DownloadUpdate";
        const string work_dir_for_download_exe_backup = "DownloadBackup";
        /// <summary>
        /// 正在生成Access文件时用的文件夹，临时文件夹，生成完毕后搬动到WaitUpload目录
        /// </summary>
        const string work_temp_dir = "work_temp";
        const string work_dir_for_generated_access = "Generating";
        const string work_dir_for_upload_bak = "UploadBak";
        /// <summary>
        /// 下载的用户需要的文件，下面有Frequency和日期的目录，日期为其第一个日期，如：每月的第一天，每周的第一天
        /// </summary>
        const string work_dir_for_download_business = "DownLoads";

        const string ACCESS_FILE_EXT = "mdb";
        const string ZIP_FILE_EXT = "zip";

        //----------------------------------------------------------------
        static bool Inited = false;
        static T8Config config_obj = null;
        static List<FTPThread> FTPThreads = new List<FTPThread>();
        static IFormatter GetFormatter()
        {
            return new BinaryFormatter();
        }
        static bool is_loged_config_error = false;
        static AutoDownloadStatus auto_downloads_status = new AutoDownloadStatus();
        static bool IsWaittingStop = false;
        static bool IsWaittingAutoUploadThreadStop = false;
        static Thread auto_upload_thread = null;
        static Thread auto_download_thread = null;
        static Thread auto_moniter_thread = null;
        static Thread auto_deletefile_thread = null;
        const string str_auto_upload_thread = "auto_upload_thread";
        const string str_auto_download_thread = "auto_download_thread";
        const string str_auto_moniter_thread = "auto_moniter_thread";
        const string str_ftp_download_thread = "ftp_down_load_thread";
        const string str_ftp_upload_thread = "ftp_upload_thread";
        const string str_webservice_thread = "web_service_call_thread";
        const string str_generate_and_send_thread = "generate_and_send_thread";

        static object START_LOCKER = new object();
        static object TASK_LOCKER = new object();
        static object NOTIFY_LOCKER = new object();
        static object STOP_LOCKER = new object();

        static bool IsService_Started = false;

        static List<T8Task> _TasksProcessing;
        static List<T8Task> TasksProcessing
        {
            get
            {
                if (_TasksProcessing == null)
                    _TasksProcessing = new List<T8Task>();
                return _TasksProcessing;
            }
        }

        static List<T8Task> _TasksFinished;
        static List<T8Task> TasksFinished
        {
            get
            {
                if (_TasksFinished == null)
                    _TasksFinished = new List<T8Task>();
                return _TasksFinished;
            }
        }

        static List<T8Notification> _NotifactionProcessing;
        static List<T8Notification> NotifactionProcessing
        {
            get
            {
                if (_NotifactionProcessing == null)
                    _NotifactionProcessing = new List<T8Notification>();
                return _NotifactionProcessing;
            }
        }

        static List<T8Notification> _NotifactionFinished;
        protected static List<T8Notification> NotifactionFinished
        {
            get
            {
                if (_NotifactionFinished == null)
                    _NotifactionFinished = new List<T8Notification>();
                return _NotifactionFinished;
            }
        }

        static List<T8Task> _TaskError;
        static List<T8Task> TaskError
        {
            get
            {
                if (_TaskError == null)
                    _TaskError = new List<T8Task>();
                return _TaskError;
            }
        }


        public T8Service()
        {
            lock (typeof(T8Service))
            {
                if (Inited == false)
                {
                    InitTaskAndNotification();
                    Inited = true;
                }
            }
        }

        /// <summary>
        /// 当不销毁对象重新启动时需要传递false,此时不重新配置Remoting,一个完全全新的需要传递true
        /// </summary>
        /// <param name="using_remoting"></param>
        /// <returns></returns>
        bool Start(bool using_remoting)
        {
            try
            {
                #region
                bool rtn = true;
                LogUtil.WriteLog("T8Service::Start() Start 传吧服务开始启动");
                lock (START_LOCKER)
                {
                    if (IsService_Started == false)//没有启动才需要启动
                    {
                        //保证读取最新配置
                        config_obj = null;
                        T8Config config = this.GetConfig();
                        IsService_Started = true;
                        IsWaittingAutoUploadThreadStop = false;
                        IsWaittingStop = false;
                        if (using_remoting)
                        {
                            if (SetRemotingConfig() == false)
                            {
                                LogUtil.WriteLog("T8Service::Start() StartConfigRemoting 返回错误,启动失败");
                                rtn = false;
                            }
                        }
                        else
                        {
                            //调用方法ApplyConfig()时，初始化任务队列
                            InitTaskAndNotification();
                        }
                        if (rtn == true)
                        {
                            //遍历TasksProcessing列表，没有完成的重新执行一次，完成的移动到TasksFinished列表
                            CheckUnfinishedWorkAndReStarted();
                        }
                        if (rtn == true)
                        {
                            auto_moniter_thread = new Thread(new ThreadStart(StartMoitorThread));
                            auto_moniter_thread.Name = str_auto_moniter_thread;
                            auto_moniter_thread.Start();
                        }
                        if (rtn == true)
                        {
                            if ((config != null) && (config.IsAutoRunUpload))
                            {
                                auto_upload_thread = new Thread(new ThreadStart(StartAutoUploadThread));
                                auto_upload_thread.Name = str_auto_upload_thread;
                                auto_upload_thread.Start();
                            }
                        }
                        if (rtn == true)
                        {
                            if (config != null)
                            {
                                LogUtil.WriteLog(string.Format("自动上传设置：{0}", config.IsAutoRunUpload));
                                LogUtil.WriteLog(string.Format("自动下载设置：{0}", config.IsAutoDownload));
                            }

                            if ((config != null) && (config.IsAutoDownload))
                            {
                                auto_download_thread = new Thread(new ThreadStart(StartAutoDownLoadThread));
                                auto_download_thread.Name = str_auto_download_thread;
                                //auto_download_thread.Start(new ThreadStart(StartAutoDownLoadThread));
                                auto_download_thread.Start();
                            }
                        }
                        if (rtn == true)
                        {
                            auto_deletefile_thread = new Thread(new ThreadStart(DeleteLogFile));
                            auto_deletefile_thread.Name = "auto_deletefile_thread";
                            auto_deletefile_thread.Start();

                        }
                        if (config != null)
                        {
                            WebServiceWapper wsw = new WebServiceWapper();
                            wsw.ReportMsgToServer("服务启动", this.GetConfig().FTPUserName, CyberResource.Transfer8.T8RemoteObjects.T8Server.ClientMsgType.ServiceStart, "");
                        }
                    }
                    else
                    {
                        rtn = false;
                        LogUtil.WriteLog(string.Format("T8RemoteObjects.T8Service::Start() End,服务已经启动，启动失败,rtn={0}", rtn));

                    }
                }
                LogUtil.WriteLog(string.Format("T8Service::Start() End 传吧服务启动完成,return value={0}", rtn));

                return rtn;
                #endregion
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(string.Format("Start():服务启动过程中出现异常：({0}{1}{2})", ex.Message, ex.StackTrace, ex.Source));
                return false;
            }
        }

        #region 外界可以访问的方法，包括，启动，停止，增加/更新任务，更新通知，得到任务，通知列表
        /// <summary>
        /// 当前执行程序的路径，返回值不包括最后的"\"
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentPath()
        {
            return Path.GetDirectoryName(typeof(T8Service).Assembly.Location);
        }

        public bool Start()
        {
            return Start(true);
        }

        public int Test(int i)
        {
            return 1 + i;
        }

        public void TestAddTaskSendFile(T8Task task)
        {
            this.AddTaskSendFile(task);
        }

        public void Stop()
        {
            LogUtil.WriteLog("T8Service::Stop() Start 开始终止传吧服务");
            IsWaittingStop = true; //传吧服务终止
            IsWaittingAutoUploadThreadStop = true;//自动上传线程终止

            if (auto_download_thread != null)
                auto_download_thread.Join();
            if (auto_upload_thread != null)
                auto_upload_thread.Join();
            if (auto_moniter_thread != null)
                auto_moniter_thread.Join();
            foreach (FTPThread ft in FTPThreads)
            {
                ft.Stop();
            }

            if (this.GetConfig() != null)
            {
                WebServiceWapper wsw = new WebServiceWapper();
                wsw.ReportMsgToServer("服务已经停止", this.GetConfig().FTPUserName, CyberResource.Transfer8.T8RemoteObjects.T8Server.ClientMsgType.ServiceEnd, "");
            }

            lock (STOP_LOCKER)
            {
                IsService_Started = false;
                config_obj = null;
                //队列置为空
                _TasksProcessing = null;
                _TasksFinished = null;
                _NotifactionProcessing = null;
                _NotifactionFinished = null;
                _TaskError = null;
            }

            LogUtil.WriteLog("T8Service::Stop() End 传吧服务中止");
        }

        /// <summary>
        /// 仅仅是保存，不会启动工作线程
        /// </summary>
        /// <param name="task"></param>
        public void AddTask(T8Task task)
        {
            LogUtil.WriteLog("T8RemoteObjects.T8Service::AddTask() Start");
            //LogUtil.WriteLog(task.ToString());
            lock (TASK_LOCKER)
            {
                _TasksProcessing.Add(task);
                SaveTasksToDisk(_TasksProcessing, true);
            }
            LogUtil.WriteLog("T8RemoteObjects.T8Service::AddTask() End");

        }

        /// <summary>
        /// 返回最近的Task，如果需要全部，使用GetTasks；仅仅返回拷贝
        /// </summary>
        /// <returns></returns>
        public List<T8Task> GetRecentTasks()
        {
            List<T8Task> tasks = new List<T8Task>();

            lock (TASK_LOCKER)
            {
                foreach (T8Task t in _TasksProcessing)
                    tasks.Add((T8Task)t.Clone());
                foreach (T8Task t in _TasksFinished)
                    tasks.Add((T8Task)t.Clone());
            }
            return tasks;
        }

        /// <summary>
        /// 从所有Task中返回Task，包括磁盘中的，内存中的，仅仅建议日志使用，如果为了比较处理，应当使用GetRecentTasks，注意，内存中的仅仅是副本
        /// </summary>
        /// <returns></returns>
        public List<T8Task> GetTasks()
        {
            List<T8Task> tasks = new List<T8Task>();
            List<T8Task> temp = null;
            string path = Path.GetDirectoryName(typeof(T8Service).Assembly.Location);
            IFormatter formatter = GetFormatter();
            Stream stream = null;
            string[] files = Directory.GetFiles(path, "*.task");
            foreach (string file in files)
            {
                try
                {
                    stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);
                    temp = (List<T8Task>)formatter.Deserialize(stream);
                    if (temp != null)
                        tasks.AddRange(temp);
                }
                catch (Exception e)
                {
                    LogUtil.WriteLog(String.Format("T8Service::GetTasks(), Load task {0} from disk Error:{1}", file, e.ToString()));
                }
                if (stream != null)
                    stream.Close();
            }

            lock (TASK_LOCKER)
            {
                foreach (T8Task t in _TasksProcessing)
                    tasks.Add((T8Task)t.Clone());
                foreach (T8Task t in _TasksFinished)
                    tasks.Add((T8Task)t.Clone());
            }

            return tasks;
        }

        /// <summary>
        /// 仅仅建议日志使用，返回全部的声明,如果为了比较处理，应当使用GetRecentNotifications
        /// </summary>
        /// <returns></returns>
        public List<T8Notification> GetNotifications()
        {
            List<T8Notification> notifications = new List<T8Notification>();
            List<T8Notification> temp = null;
            string path = Path.GetDirectoryName(typeof(T8Service).Assembly.Location);
            IFormatter formatter = GetFormatter();
            Stream stream = null;
            string[] files = Directory.GetFiles(path, "*.notify");
            foreach (string file in files)
            {
                try
                {
                    stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);
                    temp = (List<T8Notification>)formatter.Deserialize(stream);
                    if (temp != null)
                        notifications.AddRange(temp);
                }
                catch (Exception e)
                {
                    LogUtil.WriteLog(String.Format("T8Service::GetNotifications(), Load notifications {0} from disk Error:{1}", file, e.ToString()));
                }
                if (stream != null)
                    stream.Close();
            }
            lock (NOTIFY_LOCKER)
            {
                foreach (T8Notification n in _NotifactionProcessing)
                    notifications.Add((T8Notification)n.Clone());
                foreach (T8Notification n in _NotifactionFinished)
                    notifications.Add((T8Notification)n.Clone());
            }
            return notifications;
        }

        /// <summary>
        /// 返回内存中的，没有处理和已处理的，如果需要全部，使用GetNotifications
        /// </summary>
        /// <returns></returns>
        public List<T8Notification> GetRecentNotifications()
        {
            List<T8Notification> notifications = new List<T8Notification>();
            lock (NOTIFY_LOCKER)
            {
                foreach (T8Notification n in _NotifactionProcessing)
                    notifications.Add((T8Notification)n.Clone());
                foreach (T8Notification n in _NotifactionFinished)
                    notifications.Add((T8Notification)n.Clone());
            }
            return notifications;
        }

        /// <summary>
        /// 更改T8配置文件 同步执行,这个方法去服务器上取得当前用户可以看到文件的信息
        /// </summary>
        /// <returns></returns>

        public void ApplyConfig(T8Config config)
        {
            try
            {
                LogUtil.WriteLog("T8Service::ApplyConfig() Start");

                LogUtil.WriteLog(string.Format("\r\n T8Service::ApplyConfig() 配置文件变动新配置文件为:{0}\r\n", config.ToString()));

                bool need_restart = false;

                lock (typeof(T8Config))
                {
                    if (config_obj == null)
                    {
                        need_restart = true;
                    }

                    string file_name = Path.GetDirectoryName(typeof(T8Service).Assembly.Location) + "\\" + t8_config;
                    IFormatter formatter = GetFormatter();
                    Stream stream = new FileStream(file_name, FileMode.Create, FileAccess.Write, FileShare.Read);
                    formatter.Serialize(stream, config);
                    stream.Close();

                    //重新加载T8配置文件
                    config_obj = null;
                    this.GetConfig();
                    LogUtil.WriteLog("\r\n T8Service::ApplyConfig() 配置文件保存成功");
                }
                lock (START_LOCKER)
                {
                    if (IsService_Started)//IsService_Started== false 表示服务没有启动过根本不需要启动 IsService_Started==true表示服务启动过
                    {
                        //队列中有数据则需要重新启动
                        if (_TasksProcessing.Count > 0 || _TasksFinished.Count > 0 || _NotifactionProcessing.Count > 0 || _NotifactionFinished.Count > 0 || _TaskError.Count > 0)
                        {
                            need_restart = true;
                        }

                        if (need_restart)//有些线程没有活动，需要全部重新启动
                        {
                            LogUtil.WriteLog("T8Service::ApplyConfig() 有些线程没有活动，服务需要全部重新启动");
                            this.Stop();
                            //config_obj = config.Clone();
                            Start(false);
                        }
                        else //等于true时不用重新启动，因为上面会重新启动全部
                        {
                            LogUtil.WriteLog("T8Service::ApplyConfig() 上传服务需要重新启动一下");
                            IsWaittingAutoUploadThreadStop = true;
                            if (auto_upload_thread != null)
                                auto_upload_thread.Join();

                            IsWaittingAutoUploadThreadStop = false;
                            //config_obj = config.Clone();
                            if (config_obj.IsAutoRunUpload)
                            {
                                auto_upload_thread = new Thread(new ThreadStart(StartAutoUploadThread));
                                auto_upload_thread.Name = str_auto_upload_thread;
                                auto_upload_thread.Start();
                            }
                        }
                    }
                    else
                    {
                        LogUtil.WriteLog("T8Service::ApplyConfig()服务没有启动无需初始化");
                        //config_obj = config.Clone();
                    }
                    WebServiceWapper wsw = new WebServiceWapper();
                    wsw.SaveConfigToServer(config.FTPUserName, config);
                }
                LogUtil.WriteLog("T8Service::ApplyConfig() End");
            }
            catch (Exception ex) {
                LogUtil.WriteLog("T8Service::ApplyConfig()异常："+ex.Message + "(" + ex.StackTrace+")");
            }           
        }

        /// <summary>
        /// 返回的是副本
        /// </summary>
        /// <returns></returns>
        public T8Config GetConfig()
        {
            T8Config t = GetConfigInternal();
            if (t != null)
                return t.Clone();
            return null;
        }

        /// <summary>
        /// Task可能是远程的，或者是非远程的，远程的调用这个函数,仅仅当状态为Cancel时仅仅会更新状态，本地的直接更新
        /// </summary>
        /// <param name="task"></param>
        public void UpdateTask(T8Task task)
        {
            bool find = false;
            lock (TASK_LOCKER)
            {
                if (_TasksProcessing.Contains(task))
                {
                    find = true;
                }
            }
            if (find == false)
            {
                lock (TASK_LOCKER)
                {
                    for (int i = 0; i < _TasksProcessing.Count; i++)
                    {
                        T8Task t = _TasksProcessing[i];
                        if (t.TaskGuid == task.TaskGuid)
                        {
                            // if (task.TaskStatus == TaskStatus.UserCanceled)
                            //     _TasksProcessing[i].TaskStatus = task.TaskStatus;
                            _TasksProcessing.Remove(t);
                            _TasksProcessing.Add(task);
                        }
                    }
                }
            }
            SaveTasksToDisk(_TasksProcessing, true);
        }

        /// <summary>
        /// notifaction可能是远程的，或者是非远程的，远程的调用这个函数,仅仅会更新DialogResult，本地的直接更新
        /// </summary>
        /// <param name="notifaction"></param>
        public void UpdateNotifaction(T8Notification notifaction)
        {
            bool find = false;
            lock (NOTIFY_LOCKER)
            {
                if (_NotifactionProcessing.Contains(notifaction))
                {
                    find = true;
                }
            }
            if (find == false)
            {
                lock (NOTIFY_LOCKER)
                {
                    for (int i = 0; i < _NotifactionProcessing.Count; i++)
                    {
                        T8Notification t = _NotifactionProcessing[i];
                        if (t.NotifactionGuid == notifaction.NotifactionGuid)
                        {
                            _NotifactionProcessing[i].DialogResult = notifaction.DialogResult;
                        }
                    }
                }
            }
            SaveNotifactionToDisk(_NotifactionProcessing, true);
        }

        #endregion

        internal static T8Config GetConfigInternal()
        {
            T8Config obj = null;
            lock (typeof(T8Config))
            {
                if (config_obj == null)
                {
                    if (is_loged_config_error == false)
                    {
                        is_loged_config_error = true;
                        LogUtil.WriteLog("T8Service::GetConfig config_obj=null,will load from disk.");
                    }
                    Stream stream = null;

                    try
                    {
                        string file_name = Path.GetDirectoryName(typeof(T8Service).Assembly.Location) + "\\" + t8_config;
                        if (File.Exists(file_name))
                        {
                            IFormatter formatter = GetFormatter();
                            stream = new FileStream(file_name, FileMode.Open, FileAccess.Read, FileShare.Read);
                            config_obj = (T8Config)formatter.Deserialize(stream);
                            if (config_obj != null)
                            {
                                if (config_obj["周上架数据"] == null)
                                {
                                    //周上架数据
                                    ReportDataItem weekShell = new ReportDataItem();
                                    weekShell.Suffix = "WS";
                                    weekShell.Frequency = ReportFrequencyType.WeekStock;
                                    weekShell.IsUserCanChangeRunTime = true;
                                    weekShell.ID = DataModelType.OnWeekShelves;
                                    T8TimeSpan t8weekShelf = new T8TimeSpan();
                                    t8weekShelf.Days = 5;
                                    t8weekShelf.Hour = 23;
                                    t8weekShelf.Minute = 59;
                                    t8weekShelf.Second = 59;
                                    weekShell.ReportEndTimeSpan = t8weekShelf;
                                    T8TimeSpan t8weekShelf1 = new T8TimeSpan();
                                    t8weekShelf1.Days = 1;
                                    t8weekShelf1.Hour = 1;
                                    t8weekShelf1.Minute = 0;
                                    t8weekShelf1.Second = 0;
                                    weekShell.ReportStartTime = t8weekShelf1;
                                    UserChangedReportSetting weekShellUser = new UserChangedReportSetting();
                                    weekShellUser.IsChecked = false;
                                    // shellUser.StartTime = Shell.ReportStartTime;
                                    weekShell.UserChangedReportSetting = weekShellUser;
                                    config_obj.Items.Add(weekShell);
                                }

                                LogUtil.WriteLog("T8Service::GetConfig config_obj load from disk success");
                                is_loged_config_error = false;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        LogUtil.WriteLog("T8Service::GetConfig Error:" + e.ToString());
                        config_obj = null;
                    }
                    finally
                    {
                        if (stream != null)
                            stream.Close();
                    }
                }
                else
                {
                    try
                    {
                        if (config_obj["周上架数据"] == null)
                        {
                            //周上架数据
                            ReportDataItem weekShell = new ReportDataItem();
                            weekShell.Suffix = "WS";
                            weekShell.Frequency = ReportFrequencyType.WeekStock;
                            weekShell.IsUserCanChangeRunTime = true;
                            weekShell.ID = DataModelType.OnWeekShelves;
                            T8TimeSpan t8weekShelf = new T8TimeSpan();
                            t8weekShelf.Days = 5;
                            t8weekShelf.Hour = 23;
                            t8weekShelf.Minute = 59;
                            t8weekShelf.Second = 59;
                            weekShell.ReportEndTimeSpan = t8weekShelf;
                            T8TimeSpan t8weekShelf1 = new T8TimeSpan();
                            t8weekShelf1.Days = 1;
                            t8weekShelf1.Hour = 1;
                            t8weekShelf1.Minute = 0;
                            t8weekShelf1.Second = 0;
                            weekShell.ReportStartTime = t8weekShelf1;
                            UserChangedReportSetting weekShellUser = new UserChangedReportSetting();
                            weekShellUser.IsChecked = false;
                            // shellUser.StartTime = Shell.ReportStartTime;
                            weekShell.UserChangedReportSetting = weekShellUser;
                            config_obj.Items.Add(weekShell);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogUtil.WriteLog("T8Service::GetConfigInternal()内存中配置文件添加周上架选项失败 ");
                    }
                }
                obj = config_obj;
            }
            return obj;
        }

        #region 线程函数StartMoitorThread区域，清理工作，通知处理工作,挪动太旧的task和notify数据到磁盘

        /// <summary>
        /// 已经完成的，Cancel的，过期的，永久无法完成的任务，应当被挪走；已经应答的通知应当被挪走或者进行下一步；已经完成时间太长（超过一年）的从队列挪走
        /// </summary>
        void StartMoitorThread()
        {
            try
            {
                #region
                LogUtil.WriteLog(string.Format("T8Service::StartMoitorThread() start开始执行"));

                int tmp_count = 10;
                while (IsWaittingStop == false)
                {
                    lock (TASK_LOCKER)
                    {
                        for (int i = _TasksProcessing.Count; i > 0; i--)
                        {
                            #region
                            T8Task t8t = _TasksProcessing[i - 1];
                            if ((t8t.TaskStatus == TaskStatus.Complete))
                            {
                                LogUtil.WriteLog(string.Format("T8Service::StartMoitorThread() Task:{0} 已经完成，将被移动 \r\n", t8t.ToString()));
                                this.RemoveTaskToFinishedList(t8t);
                            }
                            if ((t8t.TaskStatus == TaskStatus.ErrorFileNotFind))
                            {
                                LogUtil.WriteLog(string.Format("\r\n T8Service::StartMoitorThread() Task:{0} 由于文件使用过程中丢失，任务已经无法完成，将被移动到错误队列 \r\n", t8t.ToString()));
                                t8t.EndExecTime = DateTime.Now;
                                //this.RemoveTaskToFinishedList(t8t);
                                this.RemoveTaskToErrorList(t8t);
                            }
                            if ((t8t.TaskStatus == TaskStatus.ExecTiemExceed))
                            {
                                LogUtil.WriteLog(string.Format("\r\n T8Service::StartMoitorThread() Task:{0} 任务已经已经过期，将被移动到错误队列 \r\n", t8t.ToString()));
                                t8t.EndExecTime = DateTime.Now;
                                //this.RemoveTaskToFinishedList(t8t);
                                this.RemoveTaskToErrorList(t8t);
                            }
                            if ((t8t.TaskStatus == TaskStatus.MaxErrorAutoCanceled))
                            {
                                LogUtil.WriteLog(string.Format("\r\n T8Service::StartMoitorThread() Task:{0} 任务已经到达最大错误次数，将被移动到错误队列 \r\n", t8t.ToString()));
                                t8t.EndExecTime = DateTime.Now;
                                //this.RemoveTaskToFinishedList(t8t);
                                this.RemoveTaskToErrorList(t8t);
                            }
                            if ((t8t.TaskStatus == TaskStatus.ErrorWhileFileGenerating))
                            {
                                LogUtil.WriteLog(string.Format("\r\n T8Service::StartMoitorThread() Task:{0} 文件生成时发生错误，系统不会对此种错误自动重试，将被移动到已经完成队列 \r\n", t8t.ToString()));
                                t8t.EndExecTime = DateTime.Now;
                                //this.RemoveTaskToFinishedList(t8t);
                                this.RemoveTaskToErrorList(t8t);
                            }
                            #endregion
                        }

                        //--------------------------------------------
                        for (int j = _TasksFinished.Count; j > 0; j--)
                        {
                            T8Task t8t = _TasksFinished[j - 1];
                            if (t8t.TaskStatus == TaskStatus.ErrorFileNotFind || t8t.TaskStatus == TaskStatus.ExecTiemExceed || t8t.TaskStatus == TaskStatus.MaxErrorAutoCanceled || t8t.TaskStatus == TaskStatus.ErrorWhileFileGenerating)
                            {
                                LogUtil.WriteLog(string.Format("\r\n T8Service::StartMoitorThread() Task:(状态：{0}){1} 任务被移动错误队列 \r\n", t8t.TaskStatus.ToString(), t8t.ToString()));
                                t8t.EndExecTime = DateTime.Now;
                                this.RemoveTaskToErrorList(t8t);
                            }
                        }
                    }
                    bool task_changed = false;
                    lock (NOTIFY_LOCKER)
                    {
                        for (int i = _NotifactionProcessing.Count; i > 0; i--)
                        {
                            #region
                            T8Notification t8n = _NotifactionProcessing[i - 1];
                            if ((t8n.IsNeedUserInteractive == true) && (t8n.DialogResult != DialogResult.None))
                            {
                                LogUtil.WriteLog(string.Format("T8Service::StartMoitorThread() Notifaction:{0} ，将被移动到已经完成队列", t8n.ToString()));

                                if (string.IsNullOrEmpty(t8n.RelTaskGuid) == false)
                                {
                                    LogUtil.WriteLog(string.Format("T8Service::StartMoitorThread() Notifaction:{0} 有关联的任务，GUID为{1}，将找到并启动它", t8n.ToString(), t8n.RelTaskGuid));
                                    T8Task task = FindTaskFromProcessing(t8n.RelTaskGuid);

                                    if (task != null)//为空可能是因为已经取消了
                                    {
                                        if ((t8n.DialogResult == DialogResult.OK) || (t8n.DialogResult == DialogResult.Yes))
                                        {
                                            task.TaskStatus = TaskStatus.NotStart;
                                            task_changed = true;
                                            this.ReStartTask(task);
                                        }
                                        else if ((t8n.DialogResult == DialogResult.No) || (t8n.DialogResult == DialogResult.Cancel) || (t8n.DialogResult == DialogResult.Abort))
                                        {
                                            task.TaskStatus = TaskStatus.UserCanceled;
                                            task_changed = true;
                                        }
                                    }
                                    else
                                    {
                                        LogUtil.WriteLog(string.Format("T8Service::StartMoitorThread() Notifaction:{0} 关联的任务，GUID为{1}，没有找到，可能是因为已经取消了", t8n.ToString(), t8n.RelTaskGuid));

                                    }
                                }
                                this.RemoveNotifactionToFinished(t8n);
                            }
                            #endregion
                        }
                    }
                    if (task_changed)
                        this.SaveTasksToDisk(_TasksProcessing, true);
                    if (tmp_count == 10)
                    {
                        ArchiveOldestTask();
                        ArchiveOldestNotify();
                        tmp_count = 0;
                    }
                    //--已经完成的，把他们挪走------------------
                    lock (FTPThreads)
                    {
                        for (int i = FTPThreads.Count; i > 0; i--)
                        {
                            if (FTPThreads[i - 1].Stoped == true)
                            {
                                FTPThreads.Remove(FTPThreads[i - 1]);
                            }
                        }
                    }

                    Thread.Sleep(1000);
                    tmp_count = tmp_count + 1;
                    if (tmp_count == 60)
                        tmp_count = 0;

                    int moitorThreadCounter = 0;
                    while (moitorThreadCounter < 60 && IsWaittingStop == false)
                    {
                        //间隔1分钟执行一次
                        Thread.Sleep(1000);
                        moitorThreadCounter++;
                    }
                    //LogUtil.WriteLog("StartMoitorThread()间隔一分钟自动执行");
                }
                LogUtil.WriteLog(string.Format("T8Service::StartMoitorThread() End"));
                #endregion
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(string.Format("StartMoitorThread():({0}{1}{2})", ex.Message, ex.StackTrace, ex.Source));
            }
        }

        T8Task FindTaskFromProcessing(string guid)
        {
            int j = _TasksProcessing.Count;
            for (int i = j; i > 0; i--)
            {
                T8Task task = _TasksProcessing[i - 1];
                if (task.TaskGuid == guid)
                    return task;
            }
            return null;
        }

        /// <summary>
        /// 超过60天的Task把它们挪走
        /// </summary>
        void ArchiveOldestTask()
        {
            try
            {
                #region
                bool changed = false;
                List<List<T8Task>> tasks = new List<List<T8Task>>();
                lock (TASK_LOCKER)
                {
                    for (int i = _TasksFinished.Count; i > 0; i--)
                    {
                        T8Task t = _TasksFinished[i - 1];
                        if (((TimeSpan)(DateTime.Now - t.GenerateTime)).TotalDays > 60)
                        {
                            List<T8Task> tlist = FindTaskInYear(tasks, t.GenerateTime.Year);
                            if (tlist == null)
                            {
                                tlist = new List<T8Task>();
                                tasks.Add(tlist);
                            }
                            tlist.Add(t);

                            _TasksFinished.Remove(t);
                            changed = true;
                        }
                    }

                    for (int k = _TaskError.Count; k > 0; k--)
                    {
                        //错误队列清理大于1天的错误任务
                        T8Task t8t = _TaskError[k - 1];
                        if (((TimeSpan)(DateTime.Now - t8t.GenerateTime)).Days >= 1)
                        {
                            List<T8Task> tlist = FindTaskInYear(tasks, t8t.GenerateTime.Year);
                            if (tlist == null)
                            {
                                tlist = new List<T8Task>();
                                tasks.Add(tlist);
                            }
                            tlist.Add(t8t);

                            RemoveErrorTask(t8t);
                        }
                    }
                }
                //把Task列表按年写入磁盘，逻辑：先看是否存在，如果存在，则先读入合并，再保存。
                foreach (List<T8Task> task_list in tasks)
                {
                    if (task_list.Count > 0)
                    {
                        string file_name = task_list[0].GenerateTime.ToString("yyyy") + ".task";
                        string path = Path.Combine(GetCurrentPath(), file_name);
                        List<T8Task> task_list_temp = null;
                        IFormatter formatter = GetFormatter();
                        Stream stream = null;
                        //存在文件，则读入并合并
                        if (File.Exists(path))
                        {
                            try
                            {
                                stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                                task_list_temp = (List<T8Task>)formatter.Deserialize(stream);
                                if (task_list_temp != null)
                                    task_list.AddRange(task_list_temp);

                            }
                            catch (Exception e)
                            {
                                LogUtil.WriteLog("T8Service::ArchiveOldestTask() Error：" + e.ToString());
                            }
                            if (stream != null)
                                stream.Close();
                        }
                        //写回磁盘
                        try
                        {
                            stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read);
                            formatter.Serialize(stream, task_list);
                            stream.Close();
                        }
                        catch (Exception e)
                        {
                            LogUtil.WriteLog("T8Service::ArchiveOldestTask() Error：" + e.ToString());
                        }
                        if (stream != null)
                            stream.Close();
                    }

                }
                if (changed)
                    this.SaveTasksToDisk(_TasksFinished, false);
                #endregion
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog("ArchiveOldestTask():" + ex.Message);
            }
        }

        /// <summary>
        /// Task搬动功能帮助函数
        /// </summary>
        /// <param name="tasks"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        List<T8Task> FindTaskInYear(List<List<T8Task>> tasks, int year)
        {
            foreach (List<T8Task> ts in tasks)
            {
                if ((ts != null) && ts.Count > 0)
                {
                    if (ts[0].GenerateTime.Year == year)
                        return ts;
                }
            }
            return null;
        }

        /// <summary>
        /// 超过60天的Task把它们挪走
        /// </summary>
        void ArchiveOldestNotify()
        {
            bool changed = false;
            List<List<T8Notification>> notify_lists_lists = new List<List<T8Notification>>();
            lock (NOTIFY_LOCKER)
            {
                for (int i = _NotifactionFinished.Count; i > 0; i--)
                {
                    T8Notification t = _NotifactionFinished[i - 1];
                    if (((TimeSpan)(DateTime.Now - t.GenerateTime)).TotalDays > 60)
                    {
                        List<T8Notification> tlist = FindNotificationInYear(notify_lists_lists, t.GenerateTime.Year);
                        if (tlist == null)
                        {
                            tlist = new List<T8Notification>();
                            notify_lists_lists.Add(tlist);
                        }
                        tlist.Add(t);

                        _NotifactionFinished.Remove(t);
                        changed = true;
                    }
                }
            }
            //把Notification列表按年写入磁盘，逻辑：先看是否存在，如果存在，则先读入合并，再保存。
            foreach (List<T8Notification> notify_list in notify_lists_lists)
            {
                if (notify_list.Count > 0)
                {
                    string file_name = notify_list[0].GenerateTime.ToString("yyyy") + ".task";
                    string path = Path.Combine(GetCurrentPath(), file_name);
                    List<T8Notification> notify_list_temp = null;
                    IFormatter formatter = GetFormatter();
                    Stream stream = null;
                    //存在文件，则读入并合并
                    if (File.Exists(path))
                    {
                        try
                        {
                            stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                            notify_list_temp = (List<T8Notification>)formatter.Deserialize(stream);
                            if (notify_list_temp != null)
                                notify_list.AddRange(notify_list_temp);

                        }
                        catch (Exception e)
                        {
                            LogUtil.WriteLog("T8Service::ArchiveOldestNotify() Error：" + e.ToString());
                        }
                        if (stream != null)
                            stream.Close();
                    }
                    //写回磁盘
                    try
                    {
                        stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read);
                        formatter.Serialize(stream, notify_list);
                        stream.Close();
                    }
                    catch (Exception e)
                    {
                        LogUtil.WriteLog("T8Service::ArchiveOldestNotify() Error：" + e.ToString());
                    }
                    if (stream != null)
                        stream.Close();
                }

            }
            if (changed)
                this.SaveNotifactionToDisk(_NotifactionFinished, false);

        }

        /// <summary>
        /// T8Notification搬动功能帮助函数
        /// </summary>
        /// <param name="tasks"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        List<T8Notification> FindNotificationInYear(List<List<T8Notification>> notifications, int year)
        {
            foreach (List<T8Notification> ts in notifications)
            {
                if ((ts != null) && ts.Count > 0)
                {
                    if (ts[0].GenerateTime.Year == year)
                        return ts;
                }
            }
            return null;
        }
        #endregion

        #region 线程函数StartAutoUploadThread区域,自动任务执行相关区域
        /// <summary>
        /// 自动上传线程每间隔5分钟执行一次
        /// 这个线程用来判定是否已经到了设定的时间，如果到了已经设定的时间，则启动任务；已经失败的，重新启动任务
        /// </summary>
        void StartAutoUploadThread()
        {
            try
            {
                #region
                LogUtil.WriteLog(string.Format("T8Service::StartAutoUploadThread() start 自动上传线程开始"));
                int j = 0;
                int retryInterval = 600;//10 minutes
                int i = retryInterval - 1;
                int autoUploadInterval = 300; //间隔5分钟执行一次自动上传操作（新加）
                int autoUploadCounter = 0;

                int interval_when_config_null = 60;// 1 minute
                while ((T8Service.IsWaittingStop == false) && (IsWaittingAutoUploadThreadStop == false))
                {
                    if (this.GetConfig() != null)
                    {
                        RefreshScheduledTask();
                        i++;
                        if (i == retryInterval)
                        {
                            //每隔10分钟，执行一次检查错误任务队列
                            i = 0;
                            RestartFailureUploadTasks();
                        }

                        //自动上传线程每间隔5分钟执行一次
                        autoUploadCounter = 0;
                        while (autoUploadCounter < autoUploadInterval && T8Service.IsWaittingStop == false && IsWaittingAutoUploadThreadStop == false)
                        {
                            Thread.Sleep(1000);
                            autoUploadCounter++;
                        }
                        // LogUtil.WriteLog("StartAutoUploadThread()间隔5分钟自动执行");
                    }
                    else //没有设定配置文件，每分钟测试一次，休眠60秒
                    {
                        j = 0;
                        while ((j < interval_when_config_null) && (T8Service.IsWaittingStop == false) && (IsWaittingAutoUploadThreadStop == false))
                        {
                            j++;
                            Thread.Sleep(1000);
                        }
                    }
                }
                LogUtil.WriteLog(string.Format("T8Service::StartAutoUploadThread() End 自动上传线程结束"));
                #endregion
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(string.Format("StartAutoUploadThread():({0}{1}{2})", ex.Message, ex.StackTrace, ex.Source));
            }
        }

        /// <summary>
        /// 检查所有执行队列，时间过期了没有？，过期的需要搬走，状态更新为MaxErrorAutoCanceled，没有过期的检查是否有Thread关联，没有关联的启动一个线程
        /// </summary>
        void RestartFailureUploadTasks()
        {
            try
            {
                lock (TASK_LOCKER)
                {
                    foreach (T8Task t in _TasksProcessing)
                    {
                        if ((t.TaskStatus == TaskStatus.ErrorNeedRetry) && (t.TaskSource == TaskSourceType.System))
                        {
                            //时间过期了没有？，过期的需要搬走，状态更新为MaxErrorAutoCanceled，没有过期的检查是否有Thread关联，没有关联的启动一个线程
                            lock (t)
                            {
                                if ((t.TaskType == T8TaskType.SendFile) ||
                                    (t.TaskType == T8TaskType.DownLoadFiles) ||
                                    (t.TaskType == T8TaskType.GenerateAndSendFile)
                                    )
                                {
                                    if ((t.IsProcessing == false) && (t.ExecFailureTime >= MAX_RETRY_TIMES))
                                    {
                                        LogUtil.WriteLog("T8Service::RestartFailureUploadTasks will Set TaskStatus to MaxErrorAutoCanceled, task :" + t.ToString());
                                        t.TaskStatus = TaskStatus.MaxErrorAutoCanceled;
                                    }
                                    else if (((TimeSpan)(DateTime.Now - t.GenerateTime)).Days > 30)
                                    {
                                        LogUtil.WriteLog("T8Service::RestartFailureUploadTasks will Set TaskStatus to ExecTiemExceed, task :" + t.ToString());

                                        t.TaskStatus = TaskStatus.ExecTiemExceed;
                                    }
                                    else if ((t.IsProcessing == false) && (t.ExecFailureTime < MAX_RETRY_TIMES) && (((TimeSpan)(DateTime.Now - t.EndExecTime)).Minutes >= 5))
                                    {
                                        LogUtil.WriteLog("T8Service::RestartFailureUploadTasks will Restart Task:" + t.ToString());
                                        ReStartTask(t);
                                    }
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog("RestartFailureUploadTasks():" + ex.Message);
            }
        }

        /// <summary>
        /// 根据当前时间，循环月 周 月在架 周在架，符合进入条件，则生成并上传本期数据
        /// </summary>
        void RefreshScheduledTask()
        {
            try
            {
                T8Config tc = this.GetConfig();

                if (tc == null)
                {
                    LogUtil.WriteLog("T8Service::RefreshScheduledTask() Invalid T8Config, will exit");

                    return;
                }
                foreach (ReportDataItem rdi in tc.Items)
                {
                    if (rdi.UserChangedReportSetting.IsChecked)//只有在用户选中的情况下才能执行
                    {
                        TimeSpan ts = T8TimeSpan2TimeSpan(rdi.UserChangedReportSetting.StartTime);
                        if (ts.Equals(new TimeSpan(0, 0, 0, 0, 0)) == false)//时间不为空
                        {
                            //是否是当前时间之内？
                            DateTime now = DateTime.Now;
                            T8Task task = null;
                            if (rdi.Frequency == ReportFrequencyType.Month)
                            {
                                #region
                                //如果是月，则是每月的第几天，算法为：月开始+用户时间<现在<月+开卷最终TimeSpan
                                if ((now <= (new DateTime(now.Year, now.Month, 1, 0, 0, 0) - new TimeSpan(1, 0, 0, 0, 0) + T8TimeSpan2TimeSpan(rdi.ReportEndTimeSpan))) &&
                                    (now >= (new DateTime(now.Year, now.Month, 1, 0, 0, 0) - new TimeSpan(1, 0, 0, 0, 0) + T8TimeSpan2TimeSpan(rdi.UserChangedReportSetting.StartTime)))
                                    )
                                {
                                    DateTime dtSchedule = new DateTime(now.Year, now.Month, 1, 0, 0, 0, 0);
                                    DateTime dtData = new DateTime(dtSchedule.Ticks).AddMonths(-1);
                                    string fileName = GetSendFileName(dtData, rdi, tc);

                                    //判断是否上传月数据 条件：_TasksProcessing队列和_TasksFinished队列中没有月数据文件，可以上传本月数据
                                    if (!IsCanUploadDataByDate(rdi, fileName, rdi.ID))
                                    {
                                        continue;
                                    }
                                    string datetimeperiod = string.Format("\r\n 定时触发上传月数据条件：{0}<={1}<={2} \r\n",
                                                           new DateTime(now.Year, now.Month, 1, 0, 0, 0) - new TimeSpan(1, 0, 0, 0, 0) + T8TimeSpan2TimeSpan(rdi.UserChangedReportSetting.StartTime),
                                                           now.ToString("yyyy/MM/dd HH:mm:ss"),
                                                           new DateTime(now.Year, now.Month, 1, 0, 0, 0) - new TimeSpan(1, 0, 0, 0, 0) + T8TimeSpan2TimeSpan(rdi.ReportEndTimeSpan)
                                                           );
                                    LogUtil.WriteLog(datetimeperiod);

                                    //时间没有过期,运行周期时间属于                              
                                    task = new T8Task();
                                    task.Config = tc;
                                    task.TaskType = T8TaskType.GenerateAndSendFile;
                                    task.TaskStatus = TaskStatus.NotStart;
                                    task.TaskSource = TaskSourceType.System;
                                    task.TaskOriginalTime = dtSchedule;
                                    task.GenerateTime = DateTime.Now;
                                    task.TaskGuid = Guid.NewGuid().ToString();
                                    task.ExecFailureTime = 0;
                                    task.GenerateFileInfo = new T8GenerateFileInfo();
                                    task.GenerateFileInfo.IsGenerated = false;
                                    //DateTime dtData = new DateTime(dtSchedule.Ticks).AddMonths(-1);
                                    task.GenerateFileInfo.LocalTempFileName = GetSendFileName(dtData, rdi, tc);
                                    task.GenerateFileInfo.ReportDataItemID = rdi.ID;
                                    task.GenerateFileInfo.FileName = GetSendFileBakName(dtData, rdi, tc);
                                    task.GenerateFileInfo.Sql = rdi.UserChangedReportSetting.SavedSql;
                                    task.SendFilesInfo = new List<T8SendFileInfo>();
                                    T8SendFileInfo tsfi = new T8SendFileInfo();
                                    tsfi.CurrentLength = 0;
                                    tsfi.IsComplete = false;
                                    tsfi.LocalFileName = task.GenerateFileInfo.LocalTempFileName;
                                    tsfi.RemoteFileName = Path.GetFileName(tsfi.LocalFileName);
                                    tsfi.ServerAddress = tc.FTPServerAddress;
                                    tsfi.ServerDir = ConstString.DIR_NAME_UPLOAD;
                                    tsfi.TotalLength = 0;//应当Generate File 功能设置
                                    tsfi.WaitFileGenerated = true;
                                    task.SendFilesInfo.Add(tsfi);
                                }
                                #endregion
                            }
                            else if (rdi.Frequency == ReportFrequencyType.Week)
                            {
                                #region
                                //如果是星期，则是星期的第几天，算法为，用户星期<=现在星期<=开卷最后期限星期,TimeSpan此时是第几天,
                                //截至星期二，是包括星期二                         

                                if (
                                    (DateTime.Now >= (DateTimeUtil.MondayOfWeek(DateTime.Now) - new TimeSpan(1, 0, 0, 0, 0) + T8TimeSpan2TimeSpan(rdi.UserChangedReportSetting.StartTime)))
                                    &&
                                    (DateTime.Now <= (DateTimeUtil.MondayOfWeek(DateTime.Now) - new TimeSpan(1, 0, 0, 0, 0) + T8TimeSpan2TimeSpan(rdi.ReportEndTimeSpan)))
                                    )
                                {

                                    DateTime dtSchedule = DateTimeUtil.WeekBegin(DateTime.Now);
                                    DateTime dtData = new DateTime(dtSchedule.Ticks).AddDays(-7);
                                    string fileName = GetSendFileName(dtData, rdi, tc);

                                    //判断本期数据是否已传送 当前任务队列和已完成任务队列都没有当前文件名，可以上传本期文件名
                                    if (!IsCanUploadDataByDate(rdi, fileName, rdi.ID))
                                    {
                                        continue;
                                    }


                                    string datetimeperiod = string.Format("\r\n 定时触发上传周数据条件：{0}<={1}<={2} \r\n",
                                               DateTimeUtil.MondayOfWeek(DateTime.Now) - new TimeSpan(1, 0, 0, 0, 0) + T8TimeSpan2TimeSpan(rdi.UserChangedReportSetting.StartTime),
                                               now.ToString("yyyy/MM/dd HH:mm:ss"),
                                               DateTimeUtil.MondayOfWeek(DateTime.Now) - new TimeSpan(1, 0, 0, 0, 0) + T8TimeSpan2TimeSpan(rdi.ReportEndTimeSpan)
                                               );
                                    LogUtil.WriteLog(datetimeperiod);

                                    //时间没有过期,运行周期时间属于                             
                                    task = new T8Task();
                                    task.Config = tc;
                                    task.TaskType = T8TaskType.GenerateAndSendFile;
                                    task.TaskStatus = TaskStatus.NotStart;
                                    task.TaskSource = TaskSourceType.System;
                                    task.GenerateTime = DateTime.Now;
                                    task.TaskOriginalTime = dtSchedule;
                                    task.TaskGuid = Guid.NewGuid().ToString();
                                    task.ExecFailureTime = 0;
                                    task.GenerateFileInfo = new T8GenerateFileInfo();
                                    task.GenerateFileInfo.IsGenerated = false;
                                    //DateTime dtData = new DateTime(dtSchedule.Ticks).AddDays(-7);

                                    task.GenerateFileInfo.LocalTempFileName = GetSendFileName(dtData, rdi, tc);
                                    task.GenerateFileInfo.ReportDataItemID = rdi.ID;
                                    task.GenerateFileInfo.FileName = GetSendFileBakName(dtData, rdi, tc);
                                    task.GenerateFileInfo.Sql = rdi.UserChangedReportSetting.SavedSql;
                                    task.SendFilesInfo = new List<T8SendFileInfo>();
                                    T8SendFileInfo tsfi = new T8SendFileInfo();
                                    tsfi.CurrentLength = 0;
                                    tsfi.IsComplete = false;
                                    tsfi.LocalFileName = task.GenerateFileInfo.LocalTempFileName;
                                    tsfi.RemoteFileName = Path.GetFileName(tsfi.LocalFileName);
                                    tsfi.ServerAddress = tc.FTPServerAddress;
                                    tsfi.ServerDir = ConstString.DIR_NAME_UPLOAD;
                                    tsfi.TotalLength = 0;//应当Generate File 功能设置
                                    tsfi.WaitFileGenerated = true;
                                    task.SendFilesInfo.Add(tsfi);
                                }
                                #endregion
                            }
                            else if (rdi.Frequency == ReportFrequencyType.Stock)
                            {
                                #region
                                //如果是月，则是每月的第几天，算法为：当月的下一个月-用户时间<现在<当月的下一个月1号0点

                                if ((now < (new DateTime(now.Year, now.Month, 1, 0, 0, 0).AddMonths(1)) &&
                                    (now >= (new DateTime(now.Year, now.Month, 1, 0, 0, 0).AddMonths(1) - new TimeSpan(rdi.UserChangedReportSetting.StartTime.Days, 0, 0, 0, 0) + new TimeSpan(rdi.UserChangedReportSetting.StartTime.Hour, rdi.UserChangedReportSetting.StartTime.Minute, rdi.UserChangedReportSetting.StartTime.Second))
                                    )))
                                {
                                    DateTime dtSchedule = DateTimeUtil.FirstDateOfStock(DateTime.Now);
                                    DateTime dtData = now;
                                    string fileName = GetSendFileName(dtData, rdi, tc);

                                    //判断本期数据是否已传送 当前任务队列和已完成任务队列都没有当前文件名，可以上传本期文件名
                                    if (!IsCanUploadDataByDate(rdi, fileName, rdi.ID))
                                    {
                                        continue;
                                    }

                                    //月在架条件 当前时间<下月1号  且 当前时间>=当前设置的倒数天数及时间
                                    string datetimeperiod = string.Format("\r\n 定时触发上传月在架数据条件：{0}<={1}<{2} \r\n",
                                           new DateTime(now.Year, now.Month, 1, 0, 0, 0).AddMonths(1) - new TimeSpan(rdi.UserChangedReportSetting.StartTime.Days, 0, 0, 0, 0) + new TimeSpan(rdi.UserChangedReportSetting.StartTime.Hour, rdi.UserChangedReportSetting.StartTime.Minute, rdi.UserChangedReportSetting.StartTime.Second),
                                           now.ToString("yyyy/MM/dd HH:mm:ss"),
                                           new DateTime(now.Year, now.Month, 1, 0, 0, 0).AddMonths(1)
                                           );
                                    LogUtil.WriteLog(datetimeperiod);

                                    //时间没有过期,运行周期时间属于                               
                                    task = new T8Task();
                                    task.Config = tc;
                                    task.TaskType = T8TaskType.GenerateAndSendFile;
                                    task.TaskStatus = TaskStatus.NotStart;
                                    task.TaskSource = TaskSourceType.System;
                                    task.TaskOriginalTime = dtSchedule;
                                    task.GenerateTime = DateTime.Now;
                                    task.TaskGuid = Guid.NewGuid().ToString();
                                    task.ExecFailureTime = 0;
                                    task.GenerateFileInfo = new T8GenerateFileInfo();
                                    task.GenerateFileInfo.IsGenerated = false;
                                    //DateTime dtData = now;//new DateTime(dtSchedule.Ticks);//.AddMonths(-3);
                                    task.GenerateFileInfo.LocalTempFileName = GetSendFileName(dtData, rdi, tc);
                                    task.GenerateFileInfo.ReportDataItemID = rdi.ID;
                                    task.GenerateFileInfo.FileName = GetSendFileBakName(dtData, rdi, tc);
                                    task.GenerateFileInfo.Sql = rdi.UserChangedReportSetting.SavedSql;
                                    task.SendFilesInfo = new List<T8SendFileInfo>();
                                    T8SendFileInfo tsfi = new T8SendFileInfo();
                                    tsfi.CurrentLength = 0;
                                    tsfi.IsComplete = false;
                                    tsfi.LocalFileName = task.GenerateFileInfo.LocalTempFileName;
                                    tsfi.RemoteFileName = Path.GetFileName(tsfi.LocalFileName);
                                    tsfi.ServerAddress = tc.FTPServerAddress;
                                    tsfi.ServerDir = ConstString.DIR_NAME_UPLOAD;
                                    tsfi.TotalLength = 0;//应当Generate File 功能设置
                                    tsfi.WaitFileGenerated = true;
                                    task.SendFilesInfo.Add(tsfi);
                                }
                                #endregion
                            }
                            else if (rdi.Frequency == ReportFrequencyType.WeekStock)
                            {
                                #region
                                //月在架条件 指定本周星期时间<=当前时间<本周五
                                if (
                                    (DateTime.Now >= (DateTimeUtil.MondayOfWeek(DateTime.Now) - new TimeSpan(1, 0, 0, 0, 0) + T8TimeSpan2TimeSpan(rdi.UserChangedReportSetting.StartTime)))
                                    &&
                                    (DateTime.Now < (DateTimeUtil.MondayOfWeek(DateTime.Now) - new TimeSpan(1, 0, 0, 0, 0) + T8TimeSpan2TimeSpan(rdi.ReportEndTimeSpan)))
                                    )
                                {
                                    DateTime dtSchedule = DateTimeUtil.WeekBegin(DateTime.Now);
                                    DateTime dtData = new DateTime(dtSchedule.Ticks).AddDays(-7);
                                    string fileName = GetSendFileName(dtData, rdi, tc);

                                    //判断本期数据是否已传送 当前任务队列和已完成任务队列都没有当前文件名，可以上传本期文件名
                                    if (!IsCanUploadDataByDate(rdi, fileName, rdi.ID))
                                    {
                                        continue;
                                    }
                                    string datetimeperiod = string.Format("\r\n 定时触发上传周在架数据条件：{0}<={1}<={2} \r\n",
                                           DateTimeUtil.MondayOfWeek(DateTime.Now) - new TimeSpan(1, 0, 0, 0, 0) + T8TimeSpan2TimeSpan(rdi.UserChangedReportSetting.StartTime),
                                           now.ToString("yyyy/MM/dd HH:mm:ss"),
                                           DateTimeUtil.MondayOfWeek(DateTime.Now) - new TimeSpan(1, 0, 0, 0, 0) + T8TimeSpan2TimeSpan(rdi.ReportEndTimeSpan)
                                           );
                                    LogUtil.WriteLog(datetimeperiod);

                                    //时间没有过期,运行周期时间属于                              
                                    task = new T8Task();
                                    task.Config = tc;
                                    task.TaskType = T8TaskType.GenerateAndSendFile;
                                    task.TaskStatus = TaskStatus.NotStart;
                                    task.TaskSource = TaskSourceType.System;
                                    task.GenerateTime = DateTime.Now;
                                    task.TaskOriginalTime = dtSchedule;
                                    task.TaskGuid = Guid.NewGuid().ToString();
                                    task.ExecFailureTime = 0;
                                    task.GenerateFileInfo = new T8GenerateFileInfo();
                                    task.GenerateFileInfo.IsGenerated = false;
                                    //DateTime dtData = new DateTime(dtSchedule.Ticks).AddDays(-7);

                                    task.GenerateFileInfo.LocalTempFileName = GetSendFileName(dtData, rdi, tc);
                                    task.GenerateFileInfo.ReportDataItemID = rdi.ID;
                                    task.GenerateFileInfo.FileName = GetSendFileBakName(dtData, rdi, tc);
                                    task.GenerateFileInfo.Sql = rdi.UserChangedReportSetting.SavedSql;
                                    task.SendFilesInfo = new List<T8SendFileInfo>();
                                    T8SendFileInfo tsfi = new T8SendFileInfo();
                                    tsfi.CurrentLength = 0;
                                    tsfi.IsComplete = false;
                                    tsfi.LocalFileName = task.GenerateFileInfo.LocalTempFileName;
                                    tsfi.RemoteFileName = Path.GetFileName(tsfi.LocalFileName);
                                    tsfi.ServerAddress = tc.FTPServerAddress;
                                    tsfi.ServerDir = ConstString.DIR_NAME_UPLOAD;
                                    tsfi.TotalLength = 0;//应当Generate File 功能设置
                                    tsfi.WaitFileGenerated = true;
                                    task.SendFilesInfo.Add(tsfi);
                                }
                                #endregion
                            }

                            if (task != null)
                            {
                                CheckAndAddTask(task);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog("RefreshScheduledTask():" + ex.Message);
            }
        }


        /// <summary>
        /// 这个函数仅仅应当被用在自动计算是否应当启动任务上来，因为它仅仅比较生成文件种类和所属时间
        /// </summary>
        /// <param name="task"></param>
        void CheckAndAddTask(T8Task task)
        {
            try
            {
                LogUtil.WriteLog("T8Service:CheckAndAddTask 开始");
                lock (TASK_LOCKER)
                {
                    foreach (T8Task t in _TasksProcessing)
                    {
                        if ((t.TaskOriginalTime == task.TaskOriginalTime) &&
                            (t.GenerateFileInfo.ReportDataItemID == task.GenerateFileInfo.ReportDataItemID) &&
                            (string.IsNullOrEmpty(t.GenerateFileInfo.ReportDataItemID) == false &&
                            (t.TaskStatus == TaskStatus.Complete || t.TaskStatus == TaskStatus.NotStart || t.TaskStatus == TaskStatus.FileGenerated || t.TaskStatus == TaskStatus.SendingFile))
                            )
                            return;
                    }
                    foreach (T8Task t in _TasksFinished)
                    {
                        if ((t.TaskOriginalTime == task.TaskOriginalTime) &&
                            (t.GenerateFileInfo.ReportDataItemID == task.GenerateFileInfo.ReportDataItemID) &&
                            (string.IsNullOrEmpty(t.GenerateFileInfo.ReportDataItemID) == false) && (t.TaskStatus == TaskStatus.Complete)
                            )
                            return;
                    }
                }
                //LogUtil.WriteLog("T8Service::CheckAndAddTask Will Add Task:" + task.ToString());
                AddTask(task);
                ReStartTask(task);
                LogUtil.WriteLog("T8Service:CheckAndAddTask 结束");
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(string.Format("CheckAndAddTask():({0}{1}{2})", ex.Message, ex.StackTrace, ex.Source));
            }
        }
        #endregion

        #region 线程函数AutoDownLoadThread区域， 自动下载相关的功能，包括验证是否是新文件的逻辑
        /// <summary>
        /// 执行自动下载任务的线程，保证每日下载至少一次,如果今天已经执行过，且成功，则1天一次，否则6小时再测试一次
        /// </summary>
        void StartAutoDownLoadThread()
        {
            try
            {
                #region
                const int interval = 60;//seconds
                LogUtil.WriteLog(string.Format("T8Service::StartAutoDownLoadThread() start"));
                while (IsWaittingStop == false)
                {
                    bool need_exec = false;
                    int i = 0;


                    T8Config config = this.GetConfig();
                    if ((config != null) && (config.FTPUserName != null))
                    {
                        //开卷强调只要6小时运行一次即可，所以关于每天只保证运行一次的代码被屏蔽了
                        //work here;
                        //if (auto_downloads_status.LastExecTime.Day != DateTime.Now.Day)
                        //{
                        //    need_exec = true;
                        //}
                        //else
                        //{
                        //    if (auto_downloads_status.LastExecSuccess == false)
                        //    {
#if DEBUG
                        if (Math.Abs(((TimeSpan)(auto_downloads_status.LastExecTime - DateTime.Now)).Minutes) >= 1)
                        {
                            need_exec = true;
                        }
#else
                        if (Math.Abs(((TimeSpan)(auto_downloads_status.LastExecTime - DateTime.Now)).Hours) >= 6)
                        {
                            need_exec = true;
                        }
#endif
                        //    }
                        //}
                        try
                        {
                            if (need_exec == true)
                            {
                                if ((config != null) && (config.IsAutoDownload))
                                {
                                    ServerDir sd = this.GetServerDir();
                                    //检查是否有新文件，有创建下载任务
                                    auto_downloads_status.LastExecTime = DateTime.Now;
                                    CheckAndCreateTasks(sd);
                                }
                                //是否有Config不同？
                                T8Server.T8WebService service = new CyberResource.Transfer8.T8RemoteObjects.T8Server.T8WebService();
                                CyberResource.Transfer8.T8RemoteObjects.T8Server.ReportDataItem[] rdis = service.GetConfigsInServer(config.FTPUserName);

                                //检查Config是否有所不同？如果不同，则（应用？提示？）

                                ProcessNewConfig(rdis);
                            }
                        }
                        catch (Exception e)
                        {
                            LogUtil.WriteLog(e.ToString());
                        }
                    }
                    else
                    {

                        LogUtil.WriteLog(string.Format("T8Service::StartAutoDownLoadThread() Config is null or incorrect, AutoDownLoad do nothing"));

                    }
                    while (i < interval)
                    {
                        if (T8Service.IsWaittingStop == true)
                            break;
                        Thread.Sleep(1000);
                        i++;
                    }
                }
                LogUtil.WriteLog(string.Format("T8Service::StartAutoDownLoadThread() end"));
                #endregion
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog("StartAutoDownLoadThread()" + ex.Message);
            }
        }

        /// <summary>
        /// 比对T8配置文件
        /// </summary>
        /// <param name="rdis"></param>
        void ProcessNewConfig(CyberResource.Transfer8.T8RemoteObjects.T8Server.ReportDataItem[] rdis)
        {
            T8Config config = this.GetConfig();
            bool need_notify = false;
            bool nee_apply = false;
            foreach (CyberResource.Transfer8.T8RemoteObjects.T8Server.ReportDataItem rdi in rdis)
            {
                ReportDataItem rdi_local = config[rdi.ID];
                if (rdi_local == null)
                {
                    //config.Items.Add(rdi);
                    need_notify = true;
                }
                if (rdi.Version > rdi_local.Version)
                {
                    nee_apply = true;
                    LogUtil.WriteLog(string.Format("T8Service::ProcessNewConfig() detected new Config, Config Item ID is:{0},version is:{1}", rdi.ID, rdi.Version));

                    rdi_local.Description = rdi.Description;
                    rdi_local.IsCurrentDay = rdi.IsCurrentDay;
                    rdi_local.IsUserCanChangeRunTime = rdi.IsUserCanChangeRunTime;
                    rdi_local.ReportEndTimeSpan = GetT8TimeSpanFromServiceObj(rdi.ReportEndTimeSpan);
                    rdi_local.ReportStartTime = GetT8TimeSpanFromServiceObj(rdi.ReportStartTime);
                    rdi_local.Suffix = rdi.Suffix;
                    rdi_local.Version = rdi.Version;
                    if ((rdi.IsUserCanChangeRunTime == false) && (T8TimeSpan2TimeSpan(rdi_local.UserChangedReportSetting.StartTime) != new TimeSpan(0, 0, 0, 0, 0)))
                    {
                        need_notify = true;
                    }
                    if (T8TimeSpan2TimeSpan(rdi_local.UserChangedReportSetting.StartTime) > T8TimeSpan2TimeSpan(rdi.ReportEndTimeSpan))
                    {
                        need_notify = true;
                    }
                }
            }
            if (nee_apply)
                this.ApplyConfig(config);
            if (need_notify)
            {
                LogUtil.WriteLog(string.Format("T8Service::ProcessNewConfig() detected need notify user"));
                T8Notification tn = new T8Notification();
                tn.DialogResult = DialogResult.None;
                tn.GenerateTime = DateTime.Now;
                tn.IsNeedUserInteractive = false;
                tn.NotifactionDesc = "需要用户重新配置信息";
                tn.NotifactionGuid = Guid.NewGuid().ToString();
                tn.NotificationType = T8NotificationType.NeedUserChangeConfig;
                AddNotifacation(tn);
            }

        }

        void CheckAndCreateTasks(ServerDir sd)
        {
#if DEBUG
            LogUtil.WriteLog(string.Format("T8Service::CheckAndCreateTasks() Start"));
#endif
            //检查是否有新系统文件，有创建下载任务,如果存在，仅仅生成一个下载任务
            T8Config config = this.GetConfig();
            T8Task task_sys_upgrade = new T8Task();
            task_sys_upgrade.Config = config;
            task_sys_upgrade.ExecFailureTime = 0;
            task_sys_upgrade.GenerateTime = DateTime.Now;
            task_sys_upgrade.TaskGuid = Guid.NewGuid().ToString();
            task_sys_upgrade.TaskOriginalTime = DateTime.Now;
            task_sys_upgrade.TaskSource = TaskSourceType.System;
            task_sys_upgrade.TaskStatus = TaskStatus.WaittingUserConfirm;
            task_sys_upgrade.TaskType = T8TaskType.DownLoadFiles;
            foreach (ServerFileInfo fi in sd.SystemFiles)
            {

                T8DownLoadFileInfo t8dlfi = new T8DownLoadFileInfo();
                t8dlfi.DownLoadedSize = 0;
                t8dlfi.FileHash = fi.HashCode;
                t8dlfi.IsDownLoadComplete = false;
                t8dlfi.IsSysFile = true;
                t8dlfi.LocalTempDir = this.GetTempDir();
                t8dlfi.RemoteFileHash = fi.HashCode;
                t8dlfi.RemoteFileName = fi.FileName;
                t8dlfi.ServerDir = fi.RemoteDir;
                t8dlfi.RemoteLastModifiedTime = fi.LastModifiedTime;
                t8dlfi.LocalFullFileName = Path.Combine(Path.Combine(T8Service.GetCurrentPath(), DOWNLOAD_SYSTEM_UPGRADE_DIR), t8dlfi.LocalFileName);
                t8dlfi.TotalLength = fi.FileLength;
                task_sys_upgrade.DownloadingFiles.Add(t8dlfi);
            }
            if (task_sys_upgrade.DownloadingFiles.Count > 0)
            {
                if (IsExistsUpgradeTask(task_sys_upgrade) == false)
                {
                    AddTask(task_sys_upgrade);

                    T8Notification tn = new T8Notification();
                    tn.RelTaskGuid = task_sys_upgrade.TaskGuid;
                    tn.DialogResult = DialogResult.None;
                    tn.GenerateTime = DateTime.Now;
                    tn.IsNeedUserInteractive = true;
                    tn.NotifactionDesc = "发现新版本的传吧，正在准备下载升级文件...";
                    tn.NotifactionGuid = Guid.NewGuid().ToString();
                    tn.NotificationType = T8NotificationType.ConfigDownloaded;
                    AddNotifacation(tn);
                }
            }
            //--公共和私有文件-----------------------------------------------------
            T8Task task_common_download = new T8Task();
            task_common_download.Config = this.GetConfig();
            task_common_download.TaskType = T8TaskType.DownLoadFiles;
            task_common_download.TaskStatus = TaskStatus.NotStart;
            task_common_download.TaskGuid = Guid.NewGuid().ToString();
            task_common_download.TaskOriginalTime = DateTime.Now;
            task_common_download.TaskSource = TaskSourceType.System;
            task_common_download.GenerateTime = DateTime.Now;
            task_common_download.ExecFailureTime = 0;
            List<ServerFileInfo> files_to_check = new List<ServerFileInfo>();
            files_to_check.AddRange(sd.PrivateFiles);
            files_to_check.AddRange(sd.PublicFiles);
            foreach (ServerFileInfo fi in files_to_check)
            {
                //是否有下载文件?
                //文件是否存在？
                T8DownLoadFileInfo t8dlfi = new T8DownLoadFileInfo();
                t8dlfi.DownLoadedSize = 0;
                t8dlfi.FileHash = fi.HashCode;
                t8dlfi.IsDownLoadComplete = false;
                t8dlfi.IsSysFile = false;
                t8dlfi.IsPublicFile = fi.IsPublicFile;
                t8dlfi.LocalTempDir = this.GetTempDir();
                t8dlfi.ServerDir = fi.RemoteDir;
                t8dlfi.RemoteFileHash = fi.HashCode;
                t8dlfi.RemoteFileName = fi.FileName;
                t8dlfi.RemoteLastModifiedTime = fi.LastModifiedTime;
                t8dlfi.TotalLength = fi.FileLength;
                t8dlfi.LocalFullFileName = Path.Combine(config.PersonalDownloadFolder, t8dlfi.LocalFileName);

                task_common_download.DownloadingFiles.Add(t8dlfi);

            }
            if (task_common_download.DownloadingFiles.Count > 0)
            {
                if (IsExistsUpgradeTask(task_common_download) == false)
                {
                    T8Task t_removed = RemoveDownloadedFileFromTask(task_common_download);
                    AddTask(t_removed);
                    DownLoadFile(t_removed);
                }
            }
#if DEBUG
            LogUtil.WriteLog(string.Format("T8Service::CheckAndCreateTasks() End"));
#endif
        }
        #region 本地文件是否最新判断，对象是本地文件，此方法组暂且保留，但不使用，已经标记为过期
        /// <summary>
        /// 保留这个方法，先不用这个方法了,原来调用是在判断是否下载过，这个方法中的判断是和本地文件对比
        /// </summary>
        /// <param name="fi"></param>
        /// <returns></returns>
        [Obsolete]
        private bool HasDownloadedFile(ServerFileInfo fi)
        {
            bool need_down = false;
            //如果有hash,则比较hash
            if (string.IsNullOrEmpty(fi.HashCode) == false)
            {
                if (IsHashSame(fi) == false)
                    need_down = true;
            }
            //如果有版本信息，则比较版本信息，否则比较文件名和大小
            if ((string.IsNullOrEmpty(fi.Version) == false))
            {
                if (IsNewVersionHigh(fi.Version, fi.FileName))
                    need_down = true;
            }
            else//比较文件名和大小
            {
                if (IsFileSame(fi) == false)
                    need_down = true;
            }
            if (need_down == false)//如果上面都认为一样，我们坚查是否已经下载过，没有，则任务需要
            {
                if (HasDownloadedSameFile(fi) == false)
                    need_down = true;
            }
            return need_down;
        }
        /// <summary>
        /// 当前目录存在制定的文件，并且大小相同，认为其版本一样，这个函数适用升级系统时，没有hash和版本时
        /// ,如果文件不存在，返回false
        /// </summary>
        /// <param name="fi"></param>
        /// <returns></returns>
        [Obsolete]
        bool IsFileSame(ServerFileInfo fi)
        {
            string path = Path.GetDirectoryName(this.GetType().Assembly.Location);
            string file = Path.Combine(path, fi.FileName);
            if (File.Exists(file) == false)
                return false;
            FileInfo f = new FileInfo(file);
            if (f.Length - fi.FileLength != 0)
                return true;
            return false;
        }
        /// <summary>
        /// 如果文件不存在，返回true
        /// </summary>
        /// <param name="new_version"></param>
        /// <param name="assembly_name"></param>
        /// <returns></returns>
        [Obsolete]
        bool IsNewVersionHigh(string new_version, string assembly_name)
        {
            string path = Path.GetDirectoryName(this.GetType().Assembly.Location);
            string file = Path.Combine(path, assembly_name);
            if (File.Exists(file) == false)
                return true;
            Assembly a = Assembly.Load(file);
            Version new_ver = new Version(new_version);
            if (new_ver > a.GetName().Version)
                return true;
            return false;
        }
        /// <summary>
        /// 计算两个同名文件的hash是否相同，如果文件不存在,返回false
        /// </summary>
        /// <param name="sfi"></param>
        /// <returns></returns>
        [Obsolete]
        bool IsHashSame(ServerFileInfo sfi)
        {
            string path = Path.GetDirectoryName(this.GetType().Assembly.Location);
            string file = Path.Combine(path, sfi.FileName);
            if (File.Exists(file) == false)
                return false;
            string hash = FileHashUtil.GetFileHash(file);
            return sfi.HashCode == hash;
        }

        /// <summary>
        /// 自动下载用，逻辑：已经存在的（文件名相同）有Hash的比较Hash，否则比较最后修改日期
        /// </summary>
        /// <param name="fi"></param>
        /// <returns></returns>
        [Obsolete]
        bool HasDownloadedSameFile(ServerFileInfo fi)
        {
            lock (TASK_LOCKER)
            {
                for (int i = 0; i < _TasksFinished.Count; i++)
                {
                    if (_TasksFinished[i].TaskType == T8TaskType.DownLoadFiles)
                    {
                        foreach (T8DownLoadFileInfo t8dfi in _TasksFinished[i].DownloadingFiles)
                        {
                            if (string.IsNullOrEmpty(fi.HashCode) == false)
                            {//hash存在
                                if ((t8dfi.LocalFileName == fi.FileName) && (t8dfi.FileHash == fi.HashCode))
                                {
                                    //已经存在
                                    return true;
                                }
                            }
                            else if (t8dfi.LocalFileName == fi.FileName)
                            {
                                if (t8dfi.RemoteLastModifiedTime >= fi.LastModifiedTime)
                                    return true;
                            }
                        }
                    }
                }
                for (int i = 0; i < _TasksProcessing.Count; i++)
                {
                    if (_TasksProcessing[i].TaskType == T8TaskType.DownLoadFiles)
                    {
                        foreach (T8DownLoadFileInfo t8dfi in _TasksProcessing[i].DownloadingFiles)
                        {
                            if (string.IsNullOrEmpty(fi.HashCode) == false)
                            {//hash存在
                                if ((t8dfi.LocalFileName == fi.FileName) && (t8dfi.FileHash == fi.HashCode))
                                {
                                    //已经存在
                                    return true;
                                }
                            }
                            else if (t8dfi.LocalFileName == fi.FileName)
                            {
                                if (t8dfi.RemoteLastModifiedTime >= fi.LastModifiedTime)
                                    return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        #endregion

        /// <summary>
        /// 是否已经存在文件？依据是全部的名称，大小相同，最后修改日期，比较范围是下载记录！
        /// </summary>
        /// <param name="task_sys_upgrade"></param>
        /// <returns></returns>
        private bool IsExistsUpgradeTask(T8Task task_sys_upgrade)
        {
            List<T8Task> tasks = null;
            lock (TASK_LOCKER)
            {
                tasks = this.GetTasks();
            }
            foreach (T8Task t in tasks)
            {
                if ((t.TaskType == T8TaskType.DownLoadFiles) && (t.TaskSource == TaskSourceType.System))
                {
                    if ((t.DownloadingFiles.Count > 0))
                    {
                        if (IsBothTaskSameDownload(task_sys_upgrade, t))
                            return true;
                    }
                }
            }

            return false;

        }

        /// <summary>
        /// 从制定任务中删除已经下载过的文件，比较文件大小和长度,服务器上最后修改日期
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        private T8Task RemoveDownloadedFileFromTask(T8Task task)
        {
            List<T8Task> tasks = this.GetTasks();
            foreach (T8Task t in tasks)
            {
                if ((t.TaskType == T8TaskType.DownLoadFiles) && (t.TaskSource == TaskSourceType.System))
                {
                    for (int i = task.DownloadingFiles.Count; i > 0; i--)
                    {
                        T8DownLoadFileInfo dlfi2 = task.DownloadingFiles[i - 1];
                        foreach (T8DownLoadFileInfo tdlfi in t.DownloadingFiles)
                        {
                            if (IsTwoDownLoadFileSame(tdlfi, dlfi2))
                            {
                                task.DownloadingFiles.Remove(dlfi2);
                                break;
                            }
                        }
                    }
                }
            }
            return task;
        }

        /// <summary>
        ///  是否两个下载任务是相同的内容？比较文件大小和长度,服务器上最后修改日期
        /// </summary>
        /// <param name="dlfi1"></param>
        /// <param name="dlfi2"></param>
        /// <returns></returns>
        bool IsTwoDownLoadFileSame(T8DownLoadFileInfo dlfi1, T8DownLoadFileInfo dlfi2)
        {
            return ((dlfi1.IsSysFile == dlfi2.IsSysFile) &&
                        (dlfi1.LocalFileName == dlfi2.LocalFileName) &&
                        (dlfi1.TotalLength == dlfi2.TotalLength) &&
                        (dlfi1.RemoteLastModifiedTime == dlfi2.RemoteLastModifiedTime)
                        );
        }

        /// <summary>
        /// 是否两个下载任务是相同的内容？比较文件大小和长度,服务器上最后修改日期
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        bool IsBothTaskSameDownload(T8Task t1, T8Task t2)
        {
            if (t1.TaskType != t2.TaskType)
                return false;
            if (t1.DownloadingFiles.Count != t2.DownloadingFiles.Count)
                return false;

            for (int i = 0; i < t1.DownloadingFiles.Count; i++)
            {
                T8DownLoadFileInfo dlfi1 = t1.DownloadingFiles[i];
                bool find = false;
                for (int j = 0; j < t2.DownloadingFiles.Count; j++)
                {
                    T8DownLoadFileInfo dlfi2 = t2.DownloadingFiles[j];
                    if (IsTwoDownLoadFileSame(dlfi2, dlfi1))
                        find = true;
                }
                if (find == false)
                    return false;
            }
            return true;
        }

        #endregion

        #region 启动区域，启动的动作，恢复Task（Task列表初始化调用从Start()搬动到构造函数那里了），线程等
        /// <summary>
        /// 功能：1 TasksProcessing队列状态为TaskStatus.Complete移到_TasksFinished队列 
        /// 2TasksProcessing未完成符合条件的重新执行
        /// </summary>
        void CheckUnfinishedWorkAndReStarted()
        {
            try
            {
                LogUtil.WriteLog("T8Service::CheckUnfinishedWorkAndReStarted() Start 开始执行");
                LogUtil.WriteLog("T8Service::CheckUnfinishedWorkAndReStarted()功能：<1>TasksProcessing队列状态为TaskStatus.Complete移到_TasksFinished队列 <2>TasksProcessing未完成符合条件的重新执行");
                if (this.GetConfig() != null)
                {
                    for (int i = TasksProcessing.Count; i > 0; i--)
                    {
                        T8Task t8t = TasksProcessing[i - 1];
                        if (t8t.TaskStatus == TaskStatus.Complete)
                        {
                            LogUtil.WriteLog(string.Format("\r\n T8Service::CheckUnfinishedWorkAndReStarted() Task:(状态:{0}){1} 已经完成，被移动 \r\n", t8t.TaskStatus.ToString(), t8t.ToString()));
                            this.RemoveTaskToFinishedList(t8t);
                        }

                        if (t8t.TaskStatus == TaskStatus.ErrorFileNotFind || t8t.TaskStatus == TaskStatus.ExecTiemExceed || t8t.TaskStatus == TaskStatus.MaxErrorAutoCanceled || t8t.TaskStatus == TaskStatus.ErrorWhileFileGenerating)
                        {
                            LogUtil.WriteLog(string.Format("\r\n T8Service::CheckUnfinishedWorkAndReStarted() Task:(状态:{0}){1} 任务被移动错误队列 \r\n", t8t.TaskStatus.ToString(), t8t.ToString()));
                            this.RemoveTaskToErrorList(t8t);
                        }
                    }

                    for (int j = _TasksFinished.Count; j > 0; j--)
                    {
                        T8Task t8t = _TasksFinished[j - 1];

                        if (t8t.TaskStatus == TaskStatus.ErrorFileNotFind || t8t.TaskStatus == TaskStatus.ExecTiemExceed || t8t.TaskStatus == TaskStatus.MaxErrorAutoCanceled || t8t.TaskStatus == TaskStatus.ErrorWhileFileGenerating)
                        {
                            LogUtil.WriteLog(string.Format("\r\n T8Service::CheckUnfinishedWorkAndReStarted() Task:(状态:{0}){1} 任务被移动错误队列 \r\n", t8t.TaskStatus.ToString(), t8t.ToString()));
                            this.RemoveTaskToErrorList(t8t);
                        }
                    }

                    LogUtil.WriteLog("T8Service::CheckUnfinishedWorkAndReStarted() 检查已经完成，将检查是否有没有完成的任务，如果存在，将重新启动");
                    for (int i = TasksProcessing.Count; i > 0; i--)
                    {
                        T8Task t8t = TasksProcessing[i - 1];
                        if ((t8t.TaskStatus != TaskStatus.WaittingUserConfirm) && (t8t.TaskSource == TaskSourceType.System))
                            ReStartTask(t8t);
                    }

                    Thread.Sleep(1000);
                }
                LogUtil.WriteLog("T8Service::CheckUnfinishedWorkAndReStarted() End 结束执行");
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(string.Format("CheckUnfinishedWorkAndReStarted():({0}{1}{2})", ex.Message, ex.StackTrace, ex.Source));
            }
        }

        /// <summary>
        /// 重新执行任务如 生成文件 下载文件 生成并上传文件
        /// </summary>
        /// <param name="task"></param>
        void ReStartTask(T8Task task)
        {
            try
            {
                LogUtil.WriteLog("T8Service::ReStartTask() Start");
                if (this.GetConfig() == null)
                    return;

                task.IsProcessing = true;
                if (task.TaskType == T8TaskType.SendFile)
                {
                    this.SendFile(task);
                }
                else if (task.TaskType == T8TaskType.DownLoadFiles)
                {
                    this.DownLoadFile(task);

                }
                else if (task.TaskType == T8TaskType.GenerateFile)
                {
                    this.GenerateFile(task);
                }
                else if (task.TaskType == T8TaskType.GenerateAndSendFile)
                {
                    GenerateAndSendFile(task);
                }

                LogUtil.WriteLog("T8Service::ReStartTask() End");
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(string.Format("ReStartTask():({0}{1}{2})", ex.Message, ex.StackTrace, ex.Source));
            }
        }

        /// <summary>
        /// T8Service服务初始化执行 1序列化processing_task.dat当前处理任务文件 2序列化finisheding_task.dat已完成任务文件 3
        /// </summary>
        void InitTaskAndNotification()
        {
            try
            {
                #region
                LogUtil.WriteLog("T8Service::InitTaskAndNotification() Start");

                string file_name = Path.GetDirectoryName(typeof(T8Service).Assembly.Location) + "\\" + processing_task;
                IFormatter formatter = GetFormatter();
                Stream stream = null;
                if (File.Exists(file_name))
                {
                    try
                    {
                        stream = new FileStream(file_name, FileMode.Open, FileAccess.Read, FileShare.Read);
                        _TasksProcessing = (List<T8Task>)formatter.Deserialize(stream);
                        LogUtil.WriteLog(String.Format("\r\n T8Service::InitTaskAndNotification(), 成功序列化文件{0}，共{1}条任务 \r\n", file_name, _TasksProcessing.Count));
                    }
                    catch (Exception e)
                    {
                        LogUtil.WriteLog(String.Format("\r\n T8Service::InitTaskAndNotification(), Load _TasksProcessing Error:{0} \r\n", e.ToString()));
                    }
                    if (stream != null)
                        stream.Close();
                }
                if (_TasksProcessing == null)
                    _TasksProcessing = new List<T8Task>();

                //---------------
                file_name = Path.GetDirectoryName(typeof(T8Service).Assembly.Location) + "\\" + finished_task;
                if (File.Exists(file_name))
                {
                    try
                    {
                        stream = new FileStream(file_name, FileMode.Open, FileAccess.Read, FileShare.Read);
                        _TasksFinished = (List<T8Task>)formatter.Deserialize(stream);
                        LogUtil.WriteLog(String.Format("\r\n T8Service::InitTaskAndNotification(), 成功序列化文件{0}，共{1}条任务 \r\n", file_name, _TasksFinished.Count));
                    }
                    catch (Exception e)
                    {
                        LogUtil.WriteLog(String.Format("\r\nT8Service::InitTaskAndNotification(), Load _TasksFinished Error:{0} \r\n", e.ToString()));
                    }
                    if (stream != null)
                        stream.Close();
                }
                if (_TasksFinished == null)
                {
                    _TasksFinished = new List<T8Task>();
                }
                //---------------
                file_name = Path.GetDirectoryName(typeof(T8Service).Assembly.Location) + "\\" + processing_notifaction;
                if (File.Exists(file_name))
                {
                    try
                    {
                        stream = new FileStream(file_name, FileMode.Open, FileAccess.Read, FileShare.Read);
                        _NotifactionProcessing = (List<T8Notification>)formatter.Deserialize(stream);
                        LogUtil.WriteLog(String.Format("\r\n T8Service::InitTaskAndNotification(), 成功序列化文件{0}，共{1}条任务 \r\n", file_name, _NotifactionProcessing.Count));
                    }
                    catch (Exception e)
                    {
                        LogUtil.WriteLog(String.Format("\r\n T8Service::InitTaskAndNotification(), Load _NotifactionProcessing Error:{0} \r\n", e.ToString()));

                    }
                    if (stream != null)
                        stream.Close();
                }
                if (_NotifactionProcessing == null)
                {
                    _NotifactionProcessing = new List<T8Notification>();
                }
                //--------------
                file_name = Path.GetDirectoryName(typeof(T8Service).Assembly.Location) + "\\" + finished_notifaction;
                if (File.Exists(file_name))
                {
                    try
                    {
                        stream = new FileStream(file_name, FileMode.Open, FileAccess.Read, FileShare.Read);
                        _NotifactionFinished = (List<T8Notification>)formatter.Deserialize(stream);
                        LogUtil.WriteLog(String.Format("\r\n T8Service::InitTaskAndNotification(), 成功序列化文件{0}，共{1}条任务 \r\n", file_name, _NotifactionFinished.Count));
                    }
                    catch (Exception e)
                    {
                        LogUtil.WriteLog(String.Format("\r\n T8Service::InitTaskAndNotification(), Load _NotifactionFinished Error:{0} \r\n", e.ToString()));
                    }
                    if (stream != null)
                        stream.Close();
                }
                if (_NotifactionFinished == null)
                {
                    _NotifactionFinished = new List<T8Notification>();
                }

                file_name = Path.GetDirectoryName(typeof(T8Service).Assembly.Location) + "\\" + error_task;
                if (File.Exists(file_name))
                {
                    try
                    {
                        stream = new FileStream(file_name, FileMode.Open, FileAccess.Read, FileShare.Read);
                        _TaskError = (List<T8Task>)formatter.Deserialize(stream);

                        LogUtil.WriteLog(String.Format("\r\n T8Service::InitTaskAndNotification(), 成功序列化文件{0}，共{1}条任务 \r\n", file_name, _TaskError.Count));

                    }
                    catch (Exception e)
                    {
                        LogUtil.WriteLog(String.Format("\r\n T8Service::InitTaskAndNotification(), Load _NotifactionFinished Error:{0} \r\n", e.ToString()));
                    }
                    finally
                    {
                        if (stream != null)
                            stream.Close();
                    }
                }

                if (_TaskError == null)
                {
                    _TaskError = new List<T8Task>();
                }

                LogUtil.WriteLog("T8Service::InitTaskAndNotification() End");
                #endregion
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog("InitTaskAndNotification():" + ex.Message);
            }
        }
        #endregion

        internal void AddNotifacation(T8Notification notifaction)
        {
            try
            {
                lock (NOTIFY_LOCKER)
                {
                    _NotifactionProcessing.Add(notifaction);
                    SaveNotifactionToDisk(_NotifactionProcessing, true);
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog("AddNotifacation():" + ex.Message);
            }
        }

        internal ServerDir GetServerDir()
        {
            return FTPThread.GetServerDir(this.GetConfig());
        }

        void AddTaskSendFile(T8Task task)
        {
            AddTask(task);
            SendFile(task);
        }

        /// <summary>
        /// 同步方法，不应当使用，如果要使用应当修改成异步的再使用
        /// </summary>
        /// <param name="task"></param>
        [System.Obsolete]
        void AddTaskGenerateFile(T8Task task)
        {
            AddTask(task);
            GenerateFile(task);
        }

        void AddTaskGenerateAndSendFile(T8Task task)
        {
            AddTask(task);
            GenerateAndSendFile(task);
        }

        /// <summary>
        /// 异步执行
        /// </summary>
        /// <param name="task"></param>
        void GenerateAndSendFile(T8Task task)
        {
            Thread t = new Thread(new ParameterizedThreadStart(GenerateAndSendProc));
            t.Name = str_generate_and_send_thread;
            t.Start(task);
        }

        void GenerateAndSendProc(object task_obj)
        {
            try
            {
                LogUtil.WriteLog("T8Service::GenerateAndSendProc() Start 开始执行");

                T8Task task = (T8Task)task_obj;
                if (task.GenerateFileInfo.IsGenerated == false)
                {
                    this.GenerateFile(task);
                    LogUtil.WriteLog("T8Service::GenerateAndSendProc()成功生成数据文件");
                }

                //检查生成的文件状态
                if (task.GenerateFileInfo.IsGenerated == true)
                {
                    SendFile(task);
                    LogUtil.WriteLog("T8Service::GenerateAndSendProc()成功上传数据文件");
                }
                //else //TODO：生成失败，如何处理？把任务搬走，不再执行？
                //{
                //    this.RemoveTaskToFinishedList(task);
                //}
                LogUtil.WriteLog("T8Service::GenerateAndSendProc() End 结束执行");
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog("GenerateAndSendProc():" + ex.Message);
            }
        }

        T8TimeSpan GetT8TimeSpanFromServiceObj(CyberResource.Transfer8.T8RemoteObjects.T8Server.T8TimeSpan t8ts)
        {
            T8TimeSpan rtn = new T8TimeSpan(t8ts.Days, t8ts.Hour, t8ts.Minute, t8ts.Second, t8ts.MillSecond);
            return rtn;
        }

        TimeSpan T8TimeSpan2TimeSpan(T8TimeSpan t8ts)
        {
            return new TimeSpan(t8ts.Days, t8ts.Hour, t8ts.Minute, t8ts.Second, t8ts.MillSecond);
        }

        TimeSpan T8TimeSpan2TimeSpan(CyberResource.Transfer8.T8RemoteObjects.T8Server.T8TimeSpan t8ts)
        {
            return new TimeSpan(t8ts.Days, t8ts.Hour, t8ts.Minute, t8ts.Second, t8ts.MillSecond);
        }

        /// <summary>
        /// 返回的名称是当前工作目录全局常量work_temp_dir+guid+.ext
        /// </summary>
        /// <param name="ext">ext 不包括.,如mdb是合法,.mdb是不合法的</param>
        /// <returns></returns>
        string GetTempFileName(string ext)
        {
            string path = Path.GetDirectoryName(this.GetType().Assembly.Location);
            path = Path.Combine(path, work_temp_dir);
            path = Path.Combine(path, Guid.NewGuid().ToString() + "." + ext);
            return path;
        }

        string GetTempDir()
        {
            string path = Path.GetDirectoryName(this.GetType().Assembly.Location);
            path = Path.Combine(path, work_temp_dir);
            path = Path.Combine(path, Guid.NewGuid().ToString());
            return path;
        }

        string GetTempDirFile(string ext)
        {
            string path = Path.GetDirectoryName(this.GetType().Assembly.Location);
            path = Path.Combine(path, work_temp_dir);
            path = Path.Combine(path, Guid.NewGuid().ToString());
            return path;

        }

        /// <summary>
        /// 包括路径和文件名，路经是work_tmp_dir/guid/store1_yyyymmdd_yyyymmdd_m.mdb.zip,Month需要减一个月
        /// </summary>
        /// <param name="dt">dt是数据时间的一个部分</param>
        /// <param name="rdi"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        string GetSendFileName(DateTime dt, ReportDataItem rdi, T8Config config)
        {
            string path = Path.GetDirectoryName(this.GetType().Assembly.Location);
            path = Path.Combine(Path.Combine(path, work_temp_dir), Guid.NewGuid().ToString());
            DateTime end_date;
            if (rdi.Frequency == ReportFrequencyType.Week || rdi.Frequency == ReportFrequencyType.WeekStock)
                end_date = dt + new TimeSpan(6, 0, 0, 0, 0);
            else if (rdi.Frequency == ReportFrequencyType.Month)
                end_date = new DateTime(dt.Year, dt.Month, dt.Day).AddMonths(1).AddDays(-1);
            else
                end_date = dt;
            //TODO：文件名中的时间是数据发生时间？当前月/周时间？
            //[书店标识+开始时间（YYYYMMDD）+”-” + 结束时间（YYYYMMDD）+ “-”+ 模版后缀]
            string file_name = "";
            if (config.DataType == "Access文件" || string.IsNullOrEmpty(config.DataType))
            {
                file_name = string.Format("{0}_{1}_{2}_{3}.mdb.zip", config.FTPUserName, dt.ToString("yyyyMMdd"), end_date.ToString("yyyyMMdd"), rdi.Suffix);
            }
            else
            {
                file_name = string.Format("{0}_{1}_{2}_{3}.db.zip", config.FTPUserName, dt.ToString("yyyyMMdd"), end_date.ToString("yyyyMMdd"), rdi.Suffix);
            }

            if (rdi.IsCurrentDay)
            {
                if (config.DataType == "Access文件" || string.IsNullOrEmpty(config.DataType))
                {
                    file_name = string.Format("{0}_{2}.mdb.zip", config.FTPUserName, rdi.Suffix);
                }
                else
                {
                    file_name = string.Format("{0}_{2}.db.zip", config.FTPUserName, rdi.Suffix);
                }
            }
            //string file_name = string.Format("{0}_{1}_{2}_{3}.mdb.zip", config.FTPUserName, dt.ToString("yyyyMMdd"), end_date.ToString("yyyyMMdd"), rdi.Suffix);
            //if (rdi.IsCurrentDay)
            //    file_name = string.Format("{0}_{2}.mdb.zip", config.FTPUserName, rdi.Suffix);
            path = Path.Combine(path, file_name);
            return path;
        }

        /// <summary>
        /// 包括路径和文件名，路经是upload_bak_dir/store1_yyyymmdd_yyyymmdd_m.mdb.zip
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="rdi"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        string GetSendFileBakName(DateTime dt, ReportDataItem rdi, T8Config config)
        {
            string path = Path.GetDirectoryName(this.GetType().Assembly.Location);
            path = Path.Combine(path, work_dir_for_upload_bak);
            DateTime end_date;
            if (rdi.Frequency == ReportFrequencyType.Week || rdi.Frequency == ReportFrequencyType.WeekStock)
                end_date = dt + new TimeSpan(6, 0, 0, 0, 0);
            else if (rdi.Frequency == ReportFrequencyType.Month)
                end_date = new DateTime(dt.Year, dt.Month, dt.Day).AddMonths(1).AddDays(-1);
            else
                end_date = dt;
            //TODO：文件名中的时间是数据发生时间？当前月/周时间？
            //[书店标识+开始时间（YYYYMMDD）+”-” + 结束时间（YYYYMMDD）+ “-”+ 模版后缀]
            string file_name = "";
            if (config.DataType == "Access文件" || string.IsNullOrEmpty(config.DataType))
            {
                file_name = string.Format("{0}_{1}_{2}_{3}.mdb.zip", config.FTPUserName, dt.ToString("yyyyMMdd"), end_date.ToString("yyyyMMdd"), rdi.Suffix);
            }
            else
            {
                file_name = string.Format("{0}_{1}_{2}_{3}.db.zip", config.FTPUserName, dt.ToString("yyyyMMdd"), end_date.ToString("yyyyMMdd"), rdi.Suffix);
            }
            if (rdi.IsCurrentDay)
            {
                if (config.DataType == "Access文件" || string.IsNullOrEmpty(config.DataType))
                {
                    file_name = string.Format("{0}_{2}.mdb.zip", config.FTPUserName, rdi.Suffix);
                }
                else
                {
                    file_name = string.Format("{0}_{2}.db.zip", config.FTPUserName, rdi.Suffix);
                }
            }
            //string file_name = string.Format("{0}_{1}_{2}_{3}.mdb.zip", config.FTPUserName, dt.ToString("yyyyMMdd"), end_date.ToString("yyyyMMdd"), rdi.Suffix);
            //if (rdi.IsCurrentDay)
            //    file_name = string.Format("{0}_{2}.mdb.zip", config.FTPUserName, rdi.Suffix);
            path = Path.Combine(path, file_name);
            return path;
        }

        void RemoveTaskToFinishedList(T8Task task)
        {
            try
            {
                lock (TASK_LOCKER)
                {
                    _TasksProcessing.Remove(task);
                    SaveTasksToDisk(_TasksProcessing, true);
                    _TasksFinished.Add(task);
                    SaveTasksToDisk(_TasksFinished, false);
                }

                if (task.TaskType == T8TaskType.DownLoadFiles)
                {
                    foreach (T8DownLoadFileInfo t8dlfi in task.DownloadingFiles)
                    {
                        if (File.Exists(t8dlfi.LocalTempFileName))
                            File.Delete(t8dlfi.LocalTempFileName);
                    }
                    foreach (T8DownLoadFileInfo t8dlfi in task.DownloadingFiles)
                    {
                        if (Directory.Exists(Path.GetDirectoryName(t8dlfi.LocalTempFileName)))
                            Directory.Delete(Path.GetDirectoryName(t8dlfi.LocalTempFileName), true);
                    }
                }
                if (task.TaskType == T8TaskType.GenerateAndSendFile)
                {

                    foreach (T8SendFileInfo tsfi in task.SendFilesInfo)
                    {
                        if (File.Exists(tsfi.LocalFileName))
                            File.Delete(tsfi.LocalFileName);
                        if (Directory.Exists(Path.GetDirectoryName(tsfi.LocalFileName)))
                            Directory.Delete(Path.GetDirectoryName(tsfi.LocalFileName), true);
                    }
                }
            }
            catch (Exception e)
            {
                LogUtil.WriteLog(string.Format("T8Service::RemoveTaskToFinishedList Remove Temp File Error:{0},Task={1}", e.ToString(), task));
            }
        }

        void RemoveTaskToErrorList(T8Task task)
        {
            try
            {
                lock (TASK_LOCKER)
                {
                    _TasksProcessing.Remove(task);
                    SaveTasksToDisk(_TasksProcessing, true);
                    _TaskError.Add(task);
                    SaveErrorTasksToDisk(_TaskError);
                }

                if (task.TaskType == T8TaskType.DownLoadFiles)
                {
                    foreach (T8DownLoadFileInfo t8dlfi in task.DownloadingFiles)
                    {
                        if (File.Exists(t8dlfi.LocalTempFileName))
                            File.Delete(t8dlfi.LocalTempFileName);
                    }
                    foreach (T8DownLoadFileInfo t8dlfi in task.DownloadingFiles)
                    {
                        if (Directory.Exists(Path.GetDirectoryName(t8dlfi.LocalTempFileName)))
                            Directory.Delete(Path.GetDirectoryName(t8dlfi.LocalTempFileName), true);
                    }
                }
                if (task.TaskType == T8TaskType.GenerateAndSendFile)
                {

                    foreach (T8SendFileInfo tsfi in task.SendFilesInfo)
                    {
                        if (File.Exists(tsfi.LocalFileName))
                            File.Delete(tsfi.LocalFileName);
                        if (Directory.Exists(Path.GetDirectoryName(tsfi.LocalFileName)))
                            Directory.Delete(Path.GetDirectoryName(tsfi.LocalFileName), true);
                    }
                }
            }
            catch (Exception e)
            {
                LogUtil.WriteLog(string.Format("T8Service::RemoveTaskToFinishedList Remove Temp File Error:{0},Task={1}", e.ToString(), task));
            }
        }

        void RemoveErrorTask(T8Task task)
        {
            try
            {
                lock (TASK_LOCKER)
                {
                    _TaskError.Remove(task);
                    SaveErrorTasksToDisk(_TaskError);
                }

                if (task.TaskType == T8TaskType.DownLoadFiles)
                {
                    foreach (T8DownLoadFileInfo t8dlfi in task.DownloadingFiles)
                    {
                        if (File.Exists(t8dlfi.LocalTempFileName))
                            File.Delete(t8dlfi.LocalTempFileName);
                    }
                    foreach (T8DownLoadFileInfo t8dlfi in task.DownloadingFiles)
                    {
                        if (Directory.Exists(Path.GetDirectoryName(t8dlfi.LocalTempFileName)))
                            Directory.Delete(Path.GetDirectoryName(t8dlfi.LocalTempFileName), true);
                    }
                }
                if (task.TaskType == T8TaskType.GenerateAndSendFile)
                {
                    foreach (T8SendFileInfo tsfi in task.SendFilesInfo)
                    {
                        if (File.Exists(tsfi.LocalFileName))
                            File.Delete(tsfi.LocalFileName);
                        if (Directory.Exists(Path.GetDirectoryName(tsfi.LocalFileName)))
                            Directory.Delete(Path.GetDirectoryName(tsfi.LocalFileName), true);
                    }
                }
            }
            catch (Exception e)
            {
                LogUtil.WriteLog(string.Format("T8Service::RemoveErrorTask Remove Temp File Error:{0},Task={1}", e.ToString(), task));
            }
        }

        void RemoveNotifactionToFinished(T8Notification notifaction)
        {
            try
            {
                lock (NOTIFY_LOCKER)
                {
                    _NotifactionProcessing.Remove(notifaction);
                    SaveNotifactionToDisk(_NotifactionProcessing, true);
                    _NotifactionFinished.Add(notifaction);
                    SaveNotifactionToDisk(_NotifactionFinished, false);
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog("RemoveNotifactionToFinished():" + ex.Message);
            }
        }

        bool SetRemotingConfig()
        {
            LogUtil.WriteLog("T8Service::StartConfigRemoting() Start");
            string path = Path.GetDirectoryName(typeof(T8Service).Assembly.Location);
            string file = path + "\\" + remoting_server_config;
            LogUtil.WriteLog(String.Format("T8Service::StartConfigRemoting(), File {0} Exists={1}", file, File.Exists(file)));
            bool rtn = true;
            if (File.Exists(file))
            {
                RemotingConfiguration.Configure(file, false);
            }
            else
            {
                rtn = false;
            }
            LogUtil.WriteLog(string.Format("T8Service::StartConfigRemoting() End,return {0}", rtn));
            return rtn;
        }

        void SaveTasksToDisk(List<T8Task> task_list, bool is_processing)
        {
            try
            {
                if (task_list == null)
                    return;
                else
                {
                    lock (TASK_LOCKER)
                    {
                        try
                        {
                            string path = Path.GetDirectoryName(typeof(T8Service).Assembly.Location);
                            if (is_processing)
                                path = path + "\\" + processing_task;
                            else
                                path = path + "\\" + finished_task;
                            IFormatter formatter = GetFormatter();
                            Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read);
                            formatter.Serialize(stream, task_list);
                            stream.Close();
                        }
                        catch (Exception e)
                        {
                            LogUtil.WriteLog(e.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog("SaveTasksToDisk():" + ex.Message);
            }
        }

        void SaveErrorTasksToDisk(List<T8Task> task_list)
        {
            try
            {
                if (task_list == null)
                    return;
                else
                {
                    lock (TASK_LOCKER)
                    {
                        string path = Path.GetDirectoryName(typeof(T8Service).Assembly.Location);
                        path = path + "\\" + error_task;
                        IFormatter formatter = GetFormatter();
                        Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read);
                        formatter.Serialize(stream, task_list);
                        stream.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog("SaveErrorTasksToDisk():" + ex.Message);
            }
        }

        void SaveNotifactionToDisk(List<T8Notification> notifaction_list, bool is_processing)
        {
            if (notifaction_list == null)
                return;
            else
            {
                lock (NOTIFY_LOCKER)
                {
                    try
                    {
                        string path = Path.GetDirectoryName(typeof(T8Service).Assembly.Location);
                        if (is_processing)
                            path = path + "\\" + processing_notifaction;
                        else
                            path = path + "\\" + finished_notifaction;
                        IFormatter formatter = GetFormatter();
                        Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read);
                        formatter.Serialize(stream, notifaction_list);
                        stream.Close();
                    }
                    catch (Exception e)
                    {
                        LogUtil.WriteLog(e.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// 要求异步执行，如果Task.T8SendFileInfo.WaitFileGenerated为真，则等待文件生成完毕后才发送
        /// </summary>
        /// <param name="task"></param>
        void SendFile(T8Task task)
        {
            try
            {
                LogUtil.WriteLog("T8Service::SendFile() Start，will start a Uplaod thread");
                lock (FTPThreads)
                {
                    FTPThread ft = new FTPThread(task.TaskGuid);
                    ft.Service = this;
                    ft.Task = task;
                    ft.StartUploadFile();
                    FTPThreads.Add(ft);
                }
                LogUtil.WriteLog("T8Service::SendFile() End");
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog("SendFile():" + ex.Message);
            }
        }

        /// <summary>
        /// 要求同步执行，完成后更改Task的生成属性
        /// </summary>
        /// <param name="task"></param>
        void GenerateFile(T8Task task)
        {
            try
            {
                #region
                LogUtil.WriteLog("T8Service::GenerateFile() Start");
                if (Directory.Exists(Path.GetDirectoryName(task.GenerateFileInfo.FileName)) == false)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(task.GenerateFileInfo.FileName));
                }
                try
                {
                    string tmp = task.GenerateFileInfo.LocalTempFileName;
                    if (tmp.EndsWith(".zip"))
                        tmp = tmp.Substring(0, tmp.Length - 4);
                    if (File.Exists(task.GenerateFileInfo.LocalTempFileName))
                        File.Delete(task.GenerateFileInfo.LocalTempFileName);
                    if (File.Exists(tmp))
                        File.Delete(tmp);
                    if (Directory.Exists(Path.GetDirectoryName(task.GenerateFileInfo.LocalTempFileName)) == false)
                        Directory.CreateDirectory(Path.GetDirectoryName(task.GenerateFileInfo.LocalTempFileName));

                    AccessHelper.GenerateFile(tmp, task.GenerateFileInfo.Sql, task.Config, task.GenerateFileInfo.ReportDataItemID);
                    if (File.Exists(task.GenerateFileInfo.LocalTempFileName) == false)
                    {
                        task.TaskStatus = TaskStatus.ErrorFileNotFind;
                        task.IsProcessing = false;
                    }
                    else
                    {
                        task.TaskStatus = TaskStatus.FileGenerated;
                        task.GenerateFileInfo.IsGenerated = true;
                        //把生成的文件备份一下。
                        File.Copy(task.GenerateFileInfo.LocalTempFileName, task.GenerateFileInfo.FileName, true);
                        LogUtil.WriteLog(string.Format("\r\n T8Service::GenerateFile() 成功生成本地数据文件：{0} \r\n", task.GenerateFileInfo.LocalTempFileName));
                    }                    
                }
                catch (Exception e)
                {
                    task.IsProcessing = false;
                    task.TaskStatus = TaskStatus.ErrorWhileFileGenerating;
                    LogUtil.WriteLog(String.Format("\r\n T8Service::GenerateFile()生成本地数据文件异常,数据文件{0} 异常：{1} \r\n", task.GenerateFileInfo.LocalTempFileName, e.ToString()));

                }

                #endregion
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog("GenerateFile():" + ex.Message);
                task.IsProcessing = false;
                task.TaskStatus = TaskStatus.ErrorWhileFileGenerating;
            }
        }

        /// <summary>
        /// 要求异步执行，完成后更改Task属性
        /// </summary>
        /// <param name="task"></param>
        void DownLoadFile(T8Task task)
        {
            LogUtil.WriteLog("T8Service::DownLoadFile() Start");
            lock (FTPThreads)
            {
                FTPThread ft = new FTPThread(task.TaskGuid);
                ft.Service = this;
                ft.Task = task;
                ft.StartDownLoadFile();
                FTPThreads.Add(ft);
            }
            LogUtil.WriteLog("T8Service::DownLoadFile() End");
        }

        /// <summary>
        /// WEB 服务初始化
        /// </summary>
        public class WebServiceWapper
        {
            string _Msg;
            string _Account;
            string _ModelName;
            CyberResource.Transfer8.T8RemoteObjects.T8Server.ClientMsgType _ClientMsgType;
            CyberResource.Transfer8.T8RemoteObjects.T8Server.T8Config _Config;
            public void ReportMsgToServer(string Msg, string Account, CyberResource.Transfer8.T8RemoteObjects.T8Server.ClientMsgType cmt, string Model_Name)
            {
                this._Account = Account;
                this._Msg = Msg;
                this._ClientMsgType = cmt;
                this._ModelName = Model_Name;
                //Thread t = new Thread(new ThreadStart(ReportMsgToServerProc));
                //t.Name = str_webservice_thread;
                //t.Start();
                ReportMsgToServerProc();
            }

            void ReportMsgToServerProc()
            {
                try
                {
                    T8Server.T8WebService service = new CyberResource.Transfer8.T8RemoteObjects.T8Server.T8WebService();
#if DEBUG
                    service.Url = ConstString.T8WebServiceUrl_Test;
#else
                    service.Url = ConstString.T8WebServiceUrl;
#endif
                    service.LogClientMsg(_Account, _ClientMsgType, _Msg, _ModelName);
                    LogUtil.WriteLog(string.Format("WebServiceWapper::ReportMsgToServerProc,url={0}, Msg =>{1}", service.Url, _Msg));
                }
                catch (Exception e)
                {
                    LogUtil.WriteLog("WebServiceWapper::ReportMsgToServerProc error=>" + e.ToString());
                }
            }

            public void SaveConfigToServer(string account, T8Config config)
            {
                try
                {
                    this._Account = account;
                    this._Config = T8ConfigToT8WebServiceConfig(config);
                    Thread t = new Thread(new ThreadStart(SaveConfigToServerProc));
                    t.Name = str_webservice_thread;
                    t.Start();
                }
                catch (Exception e)
                {
                    LogUtil.WriteLog(e.ToString());
                }
            }

            void SaveConfigToServerProc()
            {
                try
                {

                    T8Server.T8WebService service = new CyberResource.Transfer8.T8RemoteObjects.T8Server.T8WebService();
#if DEBUG
                    service.Url = "http://localhost:2613/T8OBWebSerivce/T8WebService.asmx";
#else
                    service.Url = ConstString.T8WebServiceUrl;
#endif

                    service.SaveConfigsInServer(_Account, _Config);
                }
                catch (Exception e)
                {
                    LogUtil.WriteLog("WebServiceWapper::SaveConfigToServerProc error=>" + e.ToString());
                }
            }

            static CyberResource.Transfer8.T8RemoteObjects.T8Server.T8TimeSpan T8TimeSpan2T8WebServiceTimeSpan(T8TimeSpan ts)
            {
                CyberResource.Transfer8.T8RemoteObjects.T8Server.T8TimeSpan web_ts = new CyberResource.Transfer8.T8RemoteObjects.T8Server.T8TimeSpan();
                if (ts != null)
                {
                    web_ts.Days = ts.Days;
                    web_ts.Hour = ts.Hour;
                    web_ts.MillSecond = ts.MillSecond;
                    web_ts.Minute = ts.Minute;
                    web_ts.Second = ts.Second;
                }
                return web_ts;
            }

            static CyberResource.Transfer8.T8RemoteObjects.T8Server.T8Config T8ConfigToT8WebServiceConfig(T8Config config)
            {
                CyberResource.Transfer8.T8RemoteObjects.T8Server.T8Config rtn = new CyberResource.Transfer8.T8RemoteObjects.T8Server.T8Config();
                try
                {

                    rtn.ConnectionStringParts = new string[config.ConnectionStringParts.Count];
                    config.ConnectionStringParts.CopyTo(rtn.ConnectionStringParts);
                    rtn.DatabaseType = (CyberResource.Transfer8.T8RemoteObjects.T8Server.SupportedDatabaseType)config.DatabaseType;
                    rtn.FTPPassword = config.FTPPassword;
                    rtn.FTPServerAddress = config.FTPServerAddress;
                    rtn.FTPUserName = config.FTPUserName;
                    rtn.PersonalDownloadFolder = config.PersonalDownloadFolder;
                    rtn.SelectedMenufactureID = config.SelectedMenufactureID;
                    rtn.IsAutoDownload = config.IsAutoDownload;
                    rtn.IsAutoRunUpload = config.IsAutoRunUpload;
                    rtn.DataType = config.DataType;
                    rtn.Items = new CyberResource.Transfer8.T8RemoteObjects.T8Server.ReportDataItem[config.Items.Count];
                    for (int i = 0; i < config.Items.Count; i++)
                    {
                        ReportDataItem rdi = config.Items[i];
                        rtn.Items[i] = new CyberResource.Transfer8.T8RemoteObjects.T8Server.ReportDataItem();
                        rtn.Items[i].Description = rdi.Description;
                        rtn.Items[i].Frequency = (CyberResource.Transfer8.T8RemoteObjects.T8Server.ReportFrequencyType)rdi.Frequency;
                        rtn.Items[i].ID = rdi.ID;
                        rtn.Items[i].IsCurrentDay = rdi.IsCurrentDay;
                        rtn.Items[i].IsUserCanChangeRunTime = rdi.IsUserCanChangeRunTime;
                        rtn.Items[i].ReportEndTimeSpan = T8TimeSpan2T8WebServiceTimeSpan(rdi.ReportEndTimeSpan);
                        rtn.Items[i].ReportStartTime = T8TimeSpan2T8WebServiceTimeSpan(rdi.ReportStartTime);
                        rtn.Items[i].Suffix = rdi.Suffix;
                        rtn.Items[i].Version = rdi.Version;
                        rtn.Items[i].UserChangedReportSetting = new CyberResource.Transfer8.T8RemoteObjects.T8Server.UserChangedReportSetting();
                        rtn.Items[i].UserChangedReportSetting.SavedSql = rdi.UserChangedReportSetting.SavedSql;
                        rtn.Items[i].UserChangedReportSetting.StartTime = T8TimeSpan2T8WebServiceTimeSpan(rdi.UserChangedReportSetting.StartTime);
                        rtn.Items[i].UserChangedReportSetting.IsChecked = rdi.UserChangedReportSetting.IsChecked;

                    }
                }
                catch (Exception e)
                {
                    LogUtil.WriteLog(e.ToString());
                }
                return rtn;
            }
        }

        class FTPThread
        {
            const int GB2312 = 936;
            public static object[] TaskGuidLock = new object[0];
            public FTPThread(string TaskGuid)
            {
                lock (TaskGuidLock)
                {
                    this._ExecingTaskGuid = TaskGuid;
                }
            }
            Thread _Thread;
            T8Service service;
            bool _Stoped = false;
            string _ExecingTaskGuid;
            /// <summary>
            /// 跟执行线程关联的TaskGuid，只读，这个属性在FTP的执行线程中，在构造函数中自动赋值，线程退出时自动清理掉，可以检查这个自动来判断是否有线程正在执行当前的Task,可以因此来在失败一段时间后恢复一个Task，访问者必须锁定FTPThread.TaskGuidLock
            /// </summary>
            public string ExecingTaskGuid
            {
                get { return _ExecingTaskGuid; }
            }

            /// <summary>
            /// 只读
            /// </summary>
            public bool Stoped
            {
                get { return _Stoped; }
            }
            public T8Service Service
            {
                get { return service; }
                set { service = value; }
            }
            /// <summary>
            /// 只读
            /// </summary>
            public Thread Thread
            {
                get { return _Thread; }
            }
            T8Task _Task;

            public T8Task Task
            {
                get { return _Task; }
                set { _Task = value; }
            }
            FTPConnection ftp = new FTPConnection();

            public FTPConnection Ftp
            {
                get { return ftp; }
            }
            T8DownLoadFileInfo _DownlodingFileInfo;
            T8SendFileInfo _SendingFileInfo;
            /// <summary>
            /// 只读
            /// </summary>
            public T8SendFileInfo SendingFileInfo
            {
                get { return _SendingFileInfo; }
            }
            /// <summary>
            /// 只读
            /// </summary>
            public T8DownLoadFileInfo DownlodingFileInfo
            {
                get { return _DownlodingFileInfo; }
            }
            public void StartDownLoadFile()
            {
                _Stoped = false;
                _Thread = new Thread(new ThreadStart(this.DownLoadProc));
                _Thread.Name = str_ftp_download_thread;
                _Thread.Start();
            }
            public void StartUploadFile()
            {
                _Stoped = false;
                _Thread = new Thread(new ThreadStart(this.UploadProc));
                _Thread.Name = str_ftp_upload_thread;
                _Thread.Start();
            }
            public void Stop()
            {
                LogUtil.WriteLog(string.Format("FTPThread::Stop Start,Task Info is:{0}", Task.ToString()));

                if (ftp.IsConnected && ftp.IsTransferring)
                {
                    ftp.CancelTransfer();
                }
                if (Thread != null)
                    Thread.Join();
                _Stoped = true;
                LogUtil.WriteLog(string.Format("FTPThread::Stop End,Task Info is:{0}", Task.ToString()));
            }
            public static ServerDir GetServerDir(T8Config config)
            {
                ServerDir sd = new ServerDir();
                try
                {
                    FTPConnection ftp = new FTPConnection();
                    ftp.ControlEncoding = Encoding.GetEncoding(GB2312);
                    ftp.ServerAddress = config.FTPServerAddress;
                    //------------------------
                    ftp.UserName = ConstString.PUB_ACCOUNT_USER;
                    ftp.Password = ConstString.PUB_ACCOUNT_PWD;
                    ftp.Connect();
                    FTPFile[] ftpfiles = ftp.GetFileInfos(ConstString.DIR_NAME_PUBLIC_DOWNLOAD);
                    foreach (FTPFile f in ftpfiles)
                    {
                        if (!f.Dir)
                        {
                            ServerFileInfo sfi = new ServerFileInfo();
                            sfi.FileLength = (int)f.Size;
                            sfi.FileName = f.Name;
                            sfi.IsPublicFile = true;
                            sfi.RemoteDir = ConstString.DIR_NAME_PUBLIC_DOWNLOAD;
                            sfi.LastModifiedTime = f.LastModified;
                            sd.PublicFiles.Add(sfi);
                        }
                    }
                    CloseFtp(ftp);
                    //--------------------------
                    ftp = new FTPConnection();
                    ftp.ControlEncoding = Encoding.GetEncoding(GB2312);
                    ftp.ServerAddress = config.FTPServerAddress;
                    ftp.UserName = config.FTPUserName;
                    ftp.Password = config.FTPPassword;
                    ftp.Connect();
                    ftpfiles = ftp.GetFileInfos(ConstString.DIR_NAME_PRIVATE_DOWNLOAD);
                    foreach (FTPFile f in ftpfiles)
                    {
                        if (!f.Dir)
                        {
                            ServerFileInfo sfi = new ServerFileInfo();
                            sfi.FileLength = (int)f.Size;
                            sfi.FileName = f.Name;
                            sfi.IsPublicFile = false;
                            sfi.RemoteDir = ConstString.DIR_NAME_PRIVATE_DOWNLOAD;
                            sfi.LastModifiedTime = f.LastModified;
                            sd.PrivateFiles.Add(sfi);
                        }
                    }
                    CloseFtp(ftp);
                    //----------------------------
                    //TODO：关闭自动升级
                    if (ConstString.IsAutoDownloadUpgrade)
                    {
                        ftp = new FTPConnection();
                        ftp.ControlEncoding = Encoding.GetEncoding(GB2312);
                        ftp.ServerAddress = config.FTPServerAddress;
                        ftp.UserName = ConstString.UPGRADE_ACCOUNT_USER;
                        ftp.Password = ConstString.UPGRADE_ACCOUNT_PWD;
                        ftp.Connect();
                        ftpfiles = ftp.GetFileInfos(ConstString.DIR_NAME_SYSTEM_DOWNLOAD);
                        foreach (FTPFile f in ftpfiles)
                        {
                            if (!f.Dir)
                            {
                                ServerFileInfo sfi = new ServerFileInfo();
                                sfi.FileLength = (int)f.Size;
                                sfi.FileName = f.Name;
                                sfi.IsPublicFile = false;
                                sfi.RemoteDir = ConstString.DIR_NAME_SYSTEM_DOWNLOAD;
                                sfi.LastModifiedTime = f.LastModified;
                                sd.SystemFiles.Add(sfi);
                            }
                        }
                        CloseFtp(ftp);
                    }
                }
                catch (Exception e)
                {
                    LogUtil.WriteLog(string.Format("FTPThread::GetServerDir Error:{0}", e.ToString()));
                }
                return sd;
            }
            void CloseFtp()
            {
                if (ftp == null)
                    return;
                if (ftp.IsConnected)
                {
                    try
                    {
                        ftp.Close();
                    }
                    catch (Exception ex_close)
                    {
                        LogUtil.WriteLog("Close Exception," + ex_close.ToString());
                    }
                }
            }
            static void CloseFtp(FTPConnection ftp)
            {
                if (ftp == null)
                    return;
                if (ftp.IsConnected)
                {
                    try
                    {
                        ftp.Close();
                    }
                    catch (Exception ex_close)
                    {
                        LogUtil.WriteLog("Static Close Exception," + ex_close.ToString());
                    }
                }
            }

            /// <summary>
            /// 上传文件处理线程
            /// </summary>
            void UploadProc()
            {
                try
                {
                    #region
                    LogUtil.WriteLog(string.Format("FTPThread::UploadProc 开始上传文件"));

                    T8Task task = Task;
                    task.TaskStatus = TaskStatus.Started;

                    WebServiceWapper wsw = new WebServiceWapper();
                    string model = "";
                    if ((task.GenerateFileInfo != null) && (task.Config != null))
                    {
                        model = task.GenerateFileInfo.ReportDataItemID;
                        wsw.ReportMsgToServer("开始上传", task.Config.FTPUserName, CyberResource.Transfer8.T8RemoteObjects.T8Server.ClientMsgType.UploadingFile, model);
                    }

                    foreach (T8SendFileInfo tsfi in task.SendFilesInfo)
                    {
                        if (tsfi.IsComplete == false)
                        {
                            try
                            {

                                ftp = new FTPConnection();
                                ftp.UserName = task.Config.FTPUserName;
                                ftp.Password = task.Config.FTPPassword;
                                ftp.ControlEncoding = Encoding.GetEncoding(GB2312);
                                ftp.ServerAddress = task.Config.FTPServerAddress;
                                ftp.TransferType = FTPTransferType.BINARY;

                                //-------------------------------
                                if (ftp.IsConnected == false)
                                    ftp.Connect();
                                ftp.ServerDirectory = "/" + tsfi.ServerDir;
                                _SendingFileInfo = tsfi;
                                bool exist_file = ftp.Exists(tsfi.RemoteFileName);
                                if (exist_file)
                                {
                                    LogUtil.WriteLog("Detect exist same file, will Resume Transfer");
                                    ftp.ResumeTransfer();
                                }

                                task.TaskStatus = TaskStatus.SendingFile;
                                ftp.UploadFile(tsfi.LocalFileName, tsfi.RemoteFileName, exist_file);
                                tsfi.IsComplete = true;

                                LogUtil.WriteLog(string.Format("\r\n FTPThread::UploadProc 成功上传文件:{0} to {1}\\{2} \r\n", tsfi.LocalFileName, tsfi.ServerDir, tsfi.RemoteFileName));
                                try
                                {
                                    if (File.Exists(tsfi.LocalFileName))
                                        File.Delete(tsfi.LocalFileName);
                                    if (Directory.Exists(Path.GetDirectoryName(tsfi.LocalFileName)))
                                        Directory.Delete(Path.GetDirectoryName(tsfi.LocalFileName));
                                }
                                catch (Exception edelete)
                                {
                                    LogUtil.WriteLog(string.Format("\r\n FTPThread::UploadProc 成功上传但删除本地文件时异常：上传文件{0},异常:{0} \r\n", tsfi.LocalFileName, edelete.ToString()));
                                }
                                CloseFtp();
                            }
                            catch (Exception e)
                            {
                                task.ExecFailureTime++;
                                task.TaskStatus = TaskStatus.ErrorNeedRetry;
                                task.EndExecTime = DateTime.Now;
                                LogUtil.WriteLog(string.Format("FTPThread::UploadProc 异常=>{0} \r\n ,ServerDir={1},ServerAddress={2},LocalFile={3},RemoteFile={4},\r\n task={5} \r\n", e.ToString(), ftp.ServerDirectory, ftp.ServerAddress, tsfi.LocalFileName, tsfi.RemoteFileName, task));
                                CloseFtp();
                                if (e is FTPTransferCancelledException)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    CloseFtp();


                    bool isAllTaskFinished = true;
                    foreach (T8SendFileInfo t8sfi in task.SendFilesInfo)
                    {
                        if (t8sfi.IsComplete == false)
                        {
                            isAllTaskFinished = false;
                            break;
                        }
                    }
                    if (isAllTaskFinished)
                    {
                        task.TaskStatus = TaskStatus.Complete;
                        if (task.SendFilesInfo.Count >= 1)
                        {
                            T8Notification notify = new T8Notification();
                            notify.NotifactionGuid = Guid.NewGuid().ToString();
                            notify.NotificationType = T8NotificationType.FileUploaded;

                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine("下列文件已经上传到开卷:");
                            sb.AppendLine(task.GenerateFileInfo.FileName);
                            notify.NotifactionDesc = sb.ToString();
                            notify.IsNeedUserInteractive = true;
                            notify.GenerateTime = DateTime.Now;
                            notify.DialogResult = DialogResult.None;
                            service.AddNotifacation(notify);
                            //WebServiceWapper wsw = new WebServiceWapper();
                            model = "";
                            if (task.GenerateFileInfo != null)
                                model = task.GenerateFileInfo.ReportDataItemID;
                            wsw.ReportMsgToServer(notify.NotifactionDesc, task.Config.FTPUserName, CyberResource.Transfer8.T8RemoteObjects.T8Server.ClientMsgType.FileUploaded, model);

                        }
                    }
                    task.EndExecTime = DateTime.Now;
                    task.IsProcessing = false;
                    lock (TASK_LOCKER)
                    {
                        service.UpdateTask(task);
                    }
                    LogUtil.WriteLog(string.Format("FTPThread::UploadProc End"));
                    lock (TaskGuidLock)
                    {
                        _ExecingTaskGuid = null;
                    }
                    _Stoped = true;
                    #endregion
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog("UploadProc()上传出现异常:" + ex.Message);
                    T8Task task = Task;
                    task.TaskStatus = TaskStatus.ErrorFileNotFind;
                    task.ExecFailureTime++;
                    task.EndExecTime = DateTime.Now;
                }
            }

            /// <summary>
            /// 下载文件处理线程
            /// </summary>
            void DownLoadProc()
            {
                try
                {
                    #region
                    LogUtil.WriteLog(string.Format("FTPThread::DownLoadProc Start"));

                    T8Task task = Task;
                    task.TaskStatus = TaskStatus.Started;
                    try
                    {
                        foreach (T8DownLoadFileInfo dlfi in task.DownloadingFiles)
                        {

                            if (dlfi.IsDownLoadComplete == false)
                            {
                                try
                                {
                                    ftp = new FTPConnection();

                                    ftp.TransferType = FTPTransferType.BINARY;
                                    ftp.ControlEncoding = Encoding.GetEncoding(GB2312);
                                    ftp.ServerAddress = task.Config.FTPServerAddress;

                                    if (dlfi.IsPublicFile)
                                    {

                                        ftp.UserName = ConstString.PUB_ACCOUNT_USER;
                                        ftp.Password = ConstString.PUB_ACCOUNT_PWD;
                                    }
                                    else if (dlfi.IsSysFile)
                                    {
                                        ftp.UserName = ConstString.UPGRADE_ACCOUNT_USER;
                                        ftp.Password = ConstString.UPGRADE_ACCOUNT_PWD;
                                    }
                                    else
                                    {
                                        ftp.UserName = task.Config.FTPUserName;
                                        ftp.Password = task.Config.FTPPassword;
                                    }
                                    if (ftp.IsConnected == false)
                                        ftp.Connect();
                                    ftp.ServerDirectory = "/" + dlfi.ServerDir;
                                    _DownlodingFileInfo = dlfi;
                                    //当两个系统升级靠的很近时，产生第一个不能下载，需要处理
                                    if (IsServerExistsFile(dlfi, ftp.GetFileInfos()))
                                    {
                                        if (dlfi.DownLoadedSize > 0)
                                        {
                                            ftp.ResumeTransfer();
                                        }
                                        LogUtil.WriteLog(string.Format("FTPThread::DownLoadProc Begin download file:{0} to {1} \r\n", dlfi.RemoteFileUrl, dlfi.LocalTempFileName));
                                        if (Directory.Exists(dlfi.LocalTempDir) == false)
                                            Directory.CreateDirectory(dlfi.LocalTempDir);
                                        task.TaskStatus = TaskStatus.DownLoadingFile;
                                        ftp.DownloadFile(dlfi.LocalTempDir, dlfi.RemoteFileName);
                                        LogUtil.WriteLog(string.Format("FTPThread::DownLoadProc success download file:{0} to {1} \r\n", dlfi.RemoteFileUrl, dlfi.LocalTempFileName));
                                        dlfi.IsDownLoadComplete = true;
                                        dlfi.DownLoadedSize = (int)new FileInfo(dlfi.LocalTempFileName).Length;
                                        if (Directory.Exists(Path.GetDirectoryName(dlfi.LocalFullFileName)) == false)
                                            Directory.CreateDirectory(Path.GetDirectoryName(dlfi.LocalFullFileName));
                                        File.Copy(dlfi.LocalTempFileName, dlfi.LocalFullFileName, true);
                                        LogUtil.WriteLog(string.Format("FTPThread::DownLoadProc  success download file:{0} \r\n ", dlfi.LocalFullFileName));
                                        File.Delete(dlfi.LocalTempFileName);
                                        Directory.Delete(dlfi.LocalTempDir);
                                    }
                                    else
                                    {
                                        if (dlfi.IsSysFile == true)
                                        {
                                            //系统文件丢失，任务失败，
                                            task.TaskStatus = TaskStatus.ErrorFileNotFind;
                                            break;
                                        }
                                        else//非系统文件,继续
                                        {
                                            if (File.Exists(dlfi.LocalTempFileName))
                                                File.Delete(dlfi.LocalTempFileName);
                                            if (Directory.Exists(dlfi.LocalTempDir))
                                                Directory.Delete(dlfi.LocalTempDir);
                                            dlfi.IsDownLoadComplete = true;
                                        }
                                    }

                                    CloseFtp();

                                }
                                catch (Exception e)
                                {

                                    if (File.Exists(dlfi.LocalTempFileName))
                                        dlfi.DownLoadedSize = (int)new FileInfo(dlfi.LocalTempFileName).Length;
                                    task.TaskStatus = TaskStatus.ErrorNeedRetry;
                                    task.ExecFailureTime++;
                                    task.EndExecTime = DateTime.Now;
                                    LogUtil.WriteLog(string.Format("FTPThread::DownLoadProc Error=>{0} \r\n", e.ToString()));
                                    CloseFtp();
                                    if (e is FTPTransferCancelledException)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        CloseFtp();
                        bool isAllTaskFinished = true;
                        foreach (T8DownLoadFileInfo dlfi in task.DownloadingFiles)
                        {
                            if (dlfi.IsDownLoadComplete == false)
                            {
                                isAllTaskFinished = false;
                                break;
                            }
                        }
                        if (isAllTaskFinished == true)
                        {
                            task.TaskStatus = TaskStatus.Complete;
                            if (task.DownloadingFiles.Count >= 1)
                            {
                                T8Notification notify = new T8Notification();
                                notify.NotifactionGuid = Guid.NewGuid().ToString();
                                notify.NotificationType = T8NotificationType.FileDownLoaded;

                                if (task.DownloadingFiles[0].IsSysFile == true)
                                {
                                    notify.RelTaskGuid = task.TaskGuid;
                                    notify.NotificationType = T8NotificationType.SystemUpgraded;
                                    notify.NotifactionDesc = "升级文件下载完成，系统准备升级...";
                                }
                                else
                                {
                                    StringBuilder sb = new StringBuilder();
                                    sb.AppendLine(string.Format("来自开卷的新资料已经下载，保存在{0},它们是：", task.Config.PersonalDownloadFolder));
                                    foreach (T8DownLoadFileInfo tdlfi in task.DownloadingFiles)
                                    {
                                        sb.AppendLine(tdlfi.LocalFileName);
                                    }
                                    notify.NotifactionDesc = sb.ToString();
                                }
                                notify.IsNeedUserInteractive = true;
                                notify.GenerateTime = DateTime.Now;
                                notify.DialogResult = DialogResult.None;
                                service.AddNotifacation(notify);
                                WebServiceWapper wsw = new WebServiceWapper();

                                wsw.ReportMsgToServer(notify.NotifactionDesc, task.Config.FTPUserName, CyberResource.Transfer8.T8RemoteObjects.T8Server.ClientMsgType.FileDownloaded, "");

                            }
                        }
                    }
                    catch (Exception ex_unexcept)
                    {
                        LogUtil.WriteLog(string.Format("FTPThread::DownLoadProc ex_unexcept:{0}", ex_unexcept.ToString()));
                    }
                    task.EndExecTime = DateTime.Now;
                    lock (TASK_LOCKER)
                    {
                        service.UpdateTask(task);
                    }

                    LogUtil.WriteLog(string.Format("FTPThread::DownLoadProc End"));
                    lock (TaskGuidLock)
                    {
                        _ExecingTaskGuid = null;
                    }
                    task.IsProcessing = false;
                    _Stoped = true;
                    #endregion
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog("DownLoadProc():" + ex.Message);
                }
            }

            /// <summary>
            /// 服务器上是否存在文件？大小，名称，最后修改时间相同，认为相同
            /// </summary>
            /// <param name="dlfi"></param>
            /// <param name="server_files"></param>
            /// <returns></returns>
            private bool IsServerExistsFile(T8DownLoadFileInfo dlfi, FTPFile[] server_files)
            {
                if ((server_files == null) || (server_files.Length == 0))
                    return false;
                foreach (FTPFile f in server_files)
                {
                    if ((f.LastModified == dlfi.RemoteLastModifiedTime) && (dlfi.RemoteFileName == f.Name) && (dlfi.TotalLength == ((int)f.Size)))
                        return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 判断是否可以生成并上传（月销售 周销售 月在架 周在架）
        /// </summary>
        /// <param name="rdi"></param>      
        /// <param name="nowUploadFileName"></param>
        /// <param name="t8TaskList"></param>
        /// <returns>true上传 false不上传</returns>
        private bool IsCanUploadDataByDate(ReportDataItem rdi, string nowUploadFileName, string reportDataItemID)
        {
            try
            {
                bool isSuccess = true;

                if (TaskError != null && TaskError.Count > 0)
                {
                    for (int i = TaskError.Count; i > 0; i--)
                    {
                        T8Task task = TaskError[i - 1];
                        #region
                        if (task.GenerateFileInfo == null || task.GenerateFileInfo.ReportDataItemID != reportDataItemID || string.IsNullOrEmpty(task.GenerateFileInfo.LocalTempFileName))
                        {
                            continue;
                        }

                        try
                        {
                            string t8FullName = task.GenerateFileInfo.LocalTempFileName.Substring(task.GenerateFileInfo.LocalTempFileName.LastIndexOf("\\") + 1);
                            string t8FileName = t8FullName.Substring(0, t8FullName.IndexOf("."));

                            string nowFullName = nowUploadFileName.Substring(nowUploadFileName.LastIndexOf("\\") + 1);
                            string nowFileName = nowFullName.Substring(0, nowFullName.IndexOf("."));

                            //当天生成的上传文件 且达到最达错误次数，则今天该类型的不再执行上传
                            if (t8FileName == nowFileName && task.GenerateTime.ToString("yyyyMMdd") == DateTime.Now.ToString("yyyyMMdd"))
                            {
                                if ((DateTime.Now.Hour == 10 && DateTime.Now.Minute <= 5) || (DateTime.Now.Hour == 11 && DateTime.Now.Minute <= 5) || (DateTime.Now.Hour == 14 && DateTime.Now.Minute <= 5) || (DateTime.Now.Hour == 15 && DateTime.Now.Minute <= 5))
                                {
                                    LogUtil.WriteLog(string.Format("T8Service IsCanUploadDataByDate()检测到TaskError队列文件{0}，状态{1},当天取消[{2}]自动上传", nowUploadFileName, task.TaskStatus.ToString(), reportDataItemID));
                                }

                                isSuccess = false;
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            LogUtil.WriteLog("T8Service IsCanUploadDataByDate() TaskError块附近异常:" + ex.Message);
                        }
                        #endregion
                    }
                }

                if (!isSuccess)
                {
                    return isSuccess;
                }

                if (TasksProcessing != null && TasksProcessing.Count > 0)
                {
                    for (int i = TasksProcessing.Count; i > 0; i--)
                    {
                        T8Task task = TasksProcessing[i - 1];
                        #region
                        if (task.GenerateFileInfo == null || task.GenerateFileInfo.ReportDataItemID != reportDataItemID || string.IsNullOrEmpty(task.GenerateFileInfo.LocalTempFileName))
                        {
                            continue;
                        }

                        try
                        {
                            //if (task.TaskStatus == TaskStatus.Complete || task.TaskStatus == TaskStatus.NotStart || task.TaskStatus == TaskStatus.FileGenerated || task.TaskStatus == TaskStatus.SendingFile)
                            //{
                            string t8FullName = task.GenerateFileInfo.LocalTempFileName.Substring(task.GenerateFileInfo.LocalTempFileName.LastIndexOf("\\") + 1);
                            string t8FileName = t8FullName.Substring(0, t8FullName.IndexOf("."));

                            string nowFullName = nowUploadFileName.Substring(nowUploadFileName.LastIndexOf("\\") + 1);
                            string nowFileName = nowFullName.Substring(0, nowFullName.IndexOf("."));

                            if (t8FileName == nowFileName)
                            {
                                if ((DateTime.Now.Hour == 10 && DateTime.Now.Minute <= 5) || (DateTime.Now.Hour == 11 && DateTime.Now.Minute <= 5) || (DateTime.Now.Hour == 14 && DateTime.Now.Minute <= 5) || (DateTime.Now.Hour == 15 && DateTime.Now.Minute <= 5))
                                {
                                    LogUtil.WriteLog(string.Format("T8Service IsCanUploadDataByDate()检测到文件{0}，状态{1},取消[{2}]本期自动上传", nowUploadFileName, task.TaskStatus.ToString(), reportDataItemID));
                                }

                                isSuccess = false;
                                break;
                            }
                            //}
                        }
                        catch (Exception ex)
                        {
                            LogUtil.WriteLog("T8Service IsCanUploadDataByDate() TasksProcessing块异常：" + ex.Message);
                        }
                        #endregion
                    }
                }

                if (!isSuccess)
                {
                    return isSuccess;
                }

                if (TasksFinished != null && TasksFinished.Count > 0)
                {
                    for (int i = TasksFinished.Count; i > 0; i--)
                    {
                        #region
                        T8Task task = TasksFinished[i - 1];
                        if (task.GenerateFileInfo == null || task.GenerateFileInfo.ReportDataItemID != reportDataItemID || string.IsNullOrEmpty(task.GenerateFileInfo.LocalTempFileName))
                        {
                            continue;
                        }

                        try
                        {
                            //if (task.TaskStatus == TaskStatus.Complete || task.TaskStatus == TaskStatus.NotStart || task.TaskStatus == TaskStatus.FileGenerated || task.TaskStatus == TaskStatus.SendingFile)
                            //{
                            string t8FullName = task.GenerateFileInfo.LocalTempFileName.Substring(task.GenerateFileInfo.LocalTempFileName.LastIndexOf("\\") + 1);
                            string t8FileName = t8FullName.Substring(0, t8FullName.IndexOf("."));

                            string nowFullName = nowUploadFileName.Substring(nowUploadFileName.LastIndexOf("\\") + 1);
                            string nowFileName = nowFullName.Substring(0, nowFullName.IndexOf("."));

                            if (t8FileName == nowFileName)
                            {
                                if ((DateTime.Now.Hour == 10 && DateTime.Now.Minute <= 5) || (DateTime.Now.Hour == 11 && DateTime.Now.Minute <= 5) || (DateTime.Now.Hour == 14 && DateTime.Now.Minute <= 5) || (DateTime.Now.Hour == 15 && DateTime.Now.Minute <= 5))
                                {
                                    LogUtil.WriteLog(string.Format("T8Service IsCanUploadDataByDate()检测到文件{0}，状态{1},取消[{2}]本期自动上传", nowUploadFileName, task.TaskStatus.ToString(), reportDataItemID));
                                }

                                isSuccess = false;
                                break;
                            }
                            //}
                        }
                        catch (Exception ex)
                        {
                            LogUtil.WriteLog("T8Service IsCanUploadDataByDate() TasksFinished块异常：" + ex.Message);
                        }
                        #endregion
                    }
                }

                return isSuccess;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog("IsCanUploadDataByDate():" + ex.Message);
                return false;
            }
        }

        public void T8StoreServiceTest(bool isSuccess)
        {
            T8Config tc = this.GetConfig();

            foreach (ReportDataItem rdi in tc.Items)
            {
                DateTime dtSchedule = new DateTime(2018, 7, 1, 0, 0, 0, 0);
                DateTime dtData = new DateTime(dtSchedule.Ticks).AddMonths(-1);
                string fileName = GetSendFileName(dtData, rdi, tc);
                if (!IsCanUploadDataByDate(rdi, fileName, rdi.ID))
                {
                    continue;
                }
            }
        }

        /// <summary>
        /// 删除日志文件 留最近2个月
        /// </summary>
        public void DeleteLogFile()
        {
            LogUtil.WriteLog("DeleteLogFile()删除日志及备份文件线程启动");
            while (!IsWaittingStop)
            {
                try
                {
                    string filePath = Path.GetDirectoryName(typeof(T8Service).Assembly.Location);
                    //string logpath = ConfigurationManager.AppSettings["LogDir"];
                    //删除服务日志文件                    
                    DeleteFile(Path.Combine(filePath, "ServerLogs"));

                    //删除传吧客服端日志                  
                    DeleteFile(Path.Combine(filePath, "StoreUILogs"));

                    //删除临时生成文件
                    string wordtempPath = Path.Combine(filePath, "work_temp");
                    DeleteFile(wordtempPath);

                    //删除上传文件备份
                    string uploadBakPath = Path.Combine(filePath, "UploadBak");
                    DeleteFile(uploadBakPath);

                    LogUtil.WriteLog("DeleteLogFile()删除日志及备份文件本次执行完成");

                    //休眠5个小时再执行
                    int intervalTime = 1000 * 60 * 60 * 5;
                    for (int index = 0; index < intervalTime; index++)
                    {
                        if (IsWaittingStop)
                        {
                            break;
                        }
                        Thread.Sleep(1000);
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog("DeleteLog()异常:" + ex.Message);
                }
            }
        }

        /// <summary>
        /// 删除文件2个月前文件
        /// </summary>
        /// <param name="filePath"></param>
        private void DeleteFile(string filePath)
        {
            try
            {
                LogUtil.WriteLog("DeleteFile()：路径" + filePath);
                if (Directory.Exists(filePath))
                {
                    DateTime baseTime = DateTime.Now.AddMonths(-2);
                    DirectoryInfo dir = new DirectoryInfo(filePath);
                    FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
                    foreach (FileSystemInfo file in fileinfo)
                    {
                        if (file is DirectoryInfo)
                        {
                            string[] files = Directory.GetFiles(file.FullName);
                            if (files.Length == 0 && file.CreationTime <= baseTime)
                            {
                                DirectoryInfo subdir = new DirectoryInfo(file.FullName);
                                subdir.Delete(true);
                                LogUtil.WriteLog("DeleteFile()删除目录：" + file.FullName);
                            }
                            else
                            {
                                DeleteFile(file.FullName);
                            }
                        }
                        else
                        {
                            if (file.CreationTime <= baseTime)
                            {
                                File.Delete(file.FullName);
                                LogUtil.WriteLog("DeleteFile()删除文件：" + file.FullName);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog("DeleteFile()" + ex.Message);
            }
        }
    }
    [Serializable]
    public class AutoDownloadStatus : MarshalByRefObject
    {
        DateTime _LastExecTime = DateTime.MinValue;

        public DateTime LastExecTime
        {
            get { return _LastExecTime; }
            set { _LastExecTime = value; }
        }
        bool _LastExecSuccess = false;

        public bool LastExecSuccess
        {
            get { return _LastExecSuccess; }
            set { _LastExecSuccess = value; }
        }
    }
}

     */

    /*
     using System;
using System.Collections.Generic;
using System.Text;

namespace CyberResource.Transfer8.T8RemoteObjects
{
    [Serializable]
    public class T8SendFileInfo : ICloneable
    {
        string _LocalFileName;
        /// <summary>
        /// 本地文件的全名称，包括路径和文件名,读写,这个文件一般应当是临时文件夹中的文件，用完后会被删除
        /// </summary>
        public string LocalFileName
        {
            get { return _LocalFileName; }
            set { _LocalFileName = value; }
        }

        private bool _IsComplete;
        /// <summary>
        /// ，读写
        /// </summary>
        public bool IsComplete
        {
            get { return _IsComplete; }
            set { _IsComplete = value; }
        }
        string _ServerDir;
        string _ServerAddress;
        /// <summary>
        /// 服务器的地址：127.0.0.1，读写
        /// </summary>
        public string ServerAddress
        {
            get { return _ServerAddress; }
            set { _ServerAddress = value; }
        }
        /// <summary>
        /// 服务器的目录，不包括最前面和最后面的"\"，读写
        /// </summary>
        public string ServerDir
        {
            get { return _ServerDir; }
            set { _ServerDir = value; }
        }
        string _RemoteFileName;
        /// <summary>
        /// 仅仅是远程的文件名，如：123.jpg，读写
        /// </summary>
        public string RemoteFileName
        {
            get { return _RemoteFileName; }
            set { _RemoteFileName = value; }
        }
        /// <summary>
        /// 全路径： ServerAddress + "\" + ServerDir + "\" + RemoteFileName,只读
        /// </summary>
        public string RemoteFileUrl
        {
            get { return ServerAddress + "\\" + ServerDir + "\\" + RemoteFileName; }
        }
        int _CurrentLength;

        public int CurrentLength
        {
            get { return _CurrentLength; }
            set { _CurrentLength = value; }
        }
        int _TotalLength;

        public int TotalLength
        {
            get { return _TotalLength; }
            set { _TotalLength = value; }
        }
        int _FileHash;

        public int FileHash
        {
            get { return _FileHash; }
            set { _FileHash = value; }
        } 
        private bool _WaitFileGenerated;


        public bool WaitFileGenerated
        {
            get { return _WaitFileGenerated; }
            set { _WaitFileGenerated = value; }
        }

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}

    */

    /*
     using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using CyberResource.Transfer8.T8ProcessLib;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.SQLite;
using CyberResource.Transfer8.T8Common;
using CyberResource.Transfer8.T8Utils;

namespace CyberResource.Transfer8.T8StoreUI.DatabaseSupport
{
    public partial class SqlServerSupport : UserControl, IDatabaseSupport
    {
        List<string> _Conn_strs = new List<string>();
        string SqlConnectionString = null;

        public SqlServerSupport()
        {
            InitializeComponent();
        }
        ~SqlServerSupport()
        {
            Close();
        }
        #region IDatabaseSupport 成员

        public void SetConnectionString(List<string> conn_strs)
        {
            if (conn_strs == null)
                return;
            if (conn_strs.Count != 4)
                return;
            _Conn_strs.AddRange(conn_strs);
            InitControl();
        }

        private void InitControl()
        {
            txtServerName.Text = _Conn_strs[0];
            txtDataBase.Text = _Conn_strs[1];
            txtUserName.Text = _Conn_strs[3];
            txtPassword.Text = _Conn_strs[2];
            //SqlConnectionString = "Provider=SQLOLEDB.1;";
            //SqlConnectionString = SqlConnectionString + "Data Source=" + _Conn_strs[0] + ";Initial Catalog=" + _Conn_strs[1] + ";";
            //SqlConnectionString = SqlConnectionString + "uid=" + _Conn_strs[2] + ";password=" + _Conn_strs[3];
        }

        SqlConnection _Conn = null;
        SqlDataReader _Reader = null;


        public DataTable GetDt(string sql, string starttime, string endtime)
        {
            //string str = this.GetConnectionString();
            //_Conn = new SqlConnection(str);
            //if (_Conn.State == ConnectionState.Closed)
            //{
            //    _Conn.Open();
            //}
            //DataSet ds = new DataSet();
            //SqlDataAdapter adp = new SqlDataAdapter(sql, _Conn);
            //adp.Fill(ds);
            //return ds.Tables[0];
            string str = this.GetConnectionString();

            _Conn = new SqlConnection(str);
            if (_Conn.State == ConnectionState.Closed)
            {
                _Conn.Open();
            }
            SqlCommand com = new SqlCommand();
            if (sql.Contains("@StartTime"))
            {
                SqlParameter sqlp = new SqlParameter("@StartTime", SqlDbType.DateTime, 4);
                sqlp.Value = starttime;
                com.Parameters.Add(sqlp);
            }
            if (sql.Contains("@EndTime"))
            {
                SqlParameter sqlp = new SqlParameter("@EndTime", SqlDbType.DateTime, 4);
                sqlp.Value = endtime;
                com.Parameters.Add(sqlp);
            }
            com.Connection = _Conn;
            com.CommandTimeout = 1200;
            com.CommandText = sql;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            Close();
            return ds.Tables[0];
        }

        public void Close()
        {
            if (_Reader != null)
            {
                _Reader.Close();
                _Reader = null;
            }
            if ((_Conn != null) && _Conn.State != ConnectionState.Closed)
            {
                _Conn.Close();
                _Conn.Dispose();
                _Conn = null;
            }
        }

        public string GetParamName(string name)
        {
            return "@" + name;
        }

        public string GetDesc()
        {
            return "Sql Sever 2000/2005";
        }


        public string GetConnectionString()
        {
            //SqlConnectionString = "Provider=SQLOLEDB.1;";
            SqlConnectionString = "Data Source=" + _Conn_strs[0] + ";Initial Catalog=" + _Conn_strs[1] + ";";
            SqlConnectionString = SqlConnectionString + "uid=" + _Conn_strs[3] + ";password=" + _Conn_strs[2];
            return SqlConnectionString;
        }



        public List<string> GetConnectionParts()
        {
            _Conn_strs.Clear();
            _Conn_strs.Add(txtServerName.Text.ToString());
            _Conn_strs.Add(txtDataBase.Text.ToString());
            _Conn_strs.Add(txtPassword.Text.ToString());
            _Conn_strs.Add(txtUserName.Text.ToString());

            return _Conn_strs;
        }

        #endregion

        #region IDatabaseSupport 成员


        public DataTable GetTableStructure(string sql, string starttime, string endtime)
        {
            try
            {
                string jointSql = string.Format("SELECT * FROM ({0}) TB WHERE 1<>1", sql);
                return this.GetDt(jointSql, starttime, endtime);
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(string.Format("SqlServerSupport.GetTableStructure()异常：({0}),{1}{2}", sql, ex.Message, ex.StackTrace));
                return null;
            }
        }

        public bool FillDataToMDB(string dbpath, string sql, string starttime, string endtime)
        {
            string constr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + dbpath + ";";
            bool isSuccess = false;
            OleDbTransaction tran = null;

            try
            {
                DataTable dt = this.GetTableStructure(sql, starttime, endtime);
                if (dt == null)
                    return false;

                DataTypeCollection dataTypeCollection = AccessHelper.GetDataStruct(dt);
                AccessHelper.CreateDataBase(dbpath, dataTypeCollection);


                using (OleDbConnection targetconn = new OleDbConnection(constr))
                {
                    try
                    {
                        targetconn.Open();
                        tran = targetconn.BeginTransaction();

                        #region 设置带参SQL语句
                        string insertSql = "insert into [mytable] values(";

                        IList<string> prms = AccessHelper.GetPrms(dt);

                        foreach (string prm in prms)
                        {
                            insertSql += prm + ",";
                        }
                        insertSql = insertSql.Remove(insertSql.Length - 1, 1);
                        insertSql += ")";
                        #endregion

                        OleDbCommand targetcmd = new OleDbCommand(insertSql, targetconn);
                        targetcmd.Transaction = tran;

                        #region SqlServer数据操作部分
                        using (SqlConnection sourceconn = new SqlConnection(this.GetConnectionString()))
                        {
                            try
                            {
                                sourceconn.Open();
                                IDbCommand sourcecommand = this.GetCommand(sql, starttime, endtime);
                                sourcecommand.Connection = sourceconn;

                                IDataReader sourceReader = sourcecommand.ExecuteReader(CommandBehavior.CloseConnection);
                                sourcecommand.Parameters.Clear();

                                while (sourceReader.Read())
                                {
                                    foreach (DataType type in dataTypeCollection)
                                    {
                                        int len = sourceReader[type.FiledName].ToString().Length;
                                        if (len > 255)
                                        {
                                            OleDbParameter oldb = new OleDbParameter("@" + type.FiledName, OleDbType.VarChar, len + 5);
                                            oldb.Value = sourceReader[type.FiledName].ToString();
                                            targetcmd.Parameters.Add(oldb);
                                        }
                                        else
                                            targetcmd.Parameters.Add(new OleDbParameter("@" + type.FiledName, sourceReader[type.FiledName].ToString()));
                                    }
                                    
                                    targetcmd.ExecuteNonQuery();
                                    targetcmd.Parameters.Clear();
                                }
                            }
                            catch (Exception ex)
                            {
                                LogUtil.WriteLog(string.Format("SqlServerSupport.FillDataToMDB()数据库操作部分异常：{0},{1}", ex.Message, ex.InnerException));
                                throw new Exception(ex.Message, ex.InnerException);
                            }
                            finally
                            {
                                if (sourceconn.State != ConnectionState.Closed)
                                {
                                    sourceconn.Close();
                                    sourceconn.Dispose();
                                }
                            }
                        }
                        #endregion

                        isSuccess = true;
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        isSuccess = false;
                        if (tran != null)
                        {
                            tran.Rollback();
                        }
                        LogUtil.WriteLog(string.Format("SqlServerSupport.FillDataToMDB()异常：{0},{1}", ex.Message, ex.InnerException));
                    }
                    finally
                    {
                        if (targetconn.State != ConnectionState.Closed)
                        {
                            targetconn.Close();
                            targetconn.Dispose();
                        }
                    }
                }

                return isSuccess;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(string.Format("SqlServerSupport.FillDataToMDB()异常{0}{1}", ex.Message, ex.StackTrace));
                return false;
            }
        }

        public bool FillDataToSQLlite(string dbpath, string sql, string starttime, string endtime)
        {
            string constr = "Data Source=" + dbpath + ";Version=3;";
            bool isSuccess = false;
            SQLiteTransaction tran = null;

            try
            {
                DataTable dt = this.GetTableStructure(sql, starttime, endtime);
                if (dt == null)
                    return false;

                DataTypeCollection dataTypeCollection = SQLiteHelper.GetDataStruct(dt);
                SQLiteHelper.CreateDataBase(dbpath, dataTypeCollection);

                using (SQLiteConnection targetconn = new SQLiteConnection(constr))
                {
                    try
                    {
                        targetconn.Open();
                        tran = targetconn.BeginTransaction();

                        #region 设置带参SQL语句
                        string insertSql = "insert into [mytable] values(";

                        IList<string> prms = SQLiteHelper.GetPrms(dt);

                        foreach (string prm in prms)
                        {
                            insertSql += prm + ",";
                        }
                        insertSql = insertSql.Remove(insertSql.Length - 1, 1);
                        insertSql += ")";
                        #endregion

                        SQLiteCommand targetcmd = new SQLiteCommand(targetconn);
                        targetcmd.Transaction = tran;
                        targetcmd.CommandText = insertSql;

                        #region 数据库操作部分
                        using (SqlConnection sourceconn = new SqlConnection(this.GetConnectionString()))
                        {
                            try
                            {
                                sourceconn.Open();
                                IDbCommand sourcecommand = this.GetCommand(sql, starttime, endtime);
                                sourcecommand.Connection = sourceconn;

                                IDataReader sourceReader = sourcecommand.ExecuteReader(CommandBehavior.CloseConnection);
                                sourcecommand.Parameters.Clear();

                                while (sourceReader.Read())
                                {
                                    foreach (DataType type in dataTypeCollection)
                                    {
                                        if (type.Type.ToLower() == "datetime")
                                        {
                                            //解决SQLite查询时,日期数据报错
                                            string dateTime = "1899/12/30";
                                            if (!string.IsNullOrEmpty(sourceReader[type.FiledName].ToString()))
                                            {
                                                dateTime = Convert.ToDateTime(sourceReader[type.FiledName].ToString()).ToString("s");
                                            }
                                            targetcmd.Parameters.Add(new SQLiteParameter("@" + type.FiledName, dateTime));
                                        }
                                        else
                                        {
                                            targetcmd.Parameters.Add(new SQLiteParameter("@" + type.FiledName, sourceReader[type.FiledName].ToString()));
                                        }
                                    }
                                   
                                    targetcmd.ExecuteNonQuery();
                                    targetcmd.Parameters.Clear();
                                }
                            }
                            catch (Exception ex)
                            {
                                LogUtil.WriteLog(string.Format("SqlServerSupport.FillDataToSQLlite()数据库操作部分异常,{0},{1}", ex.Message, ex.InnerException));
                                throw;
                            }
                            finally
                            {
                                if (sourceconn.State != ConnectionState.Closed)
                                {
                                    sourceconn.Close();
                                    sourceconn.Dispose();
                                }
                            }
                        }
                        #endregion

                        isSuccess = true;
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        isSuccess = false;
                        if (tran != null)
                        {
                            tran.Rollback();
                        }
                        LogUtil.WriteLog(string.Format("SqlServerSupport.FillDataToSQLlite()异常：{0},{1}", ex.Message, ex.InnerException));
                    }
                    finally {
                        if (targetconn.State != ConnectionState.Closed)
                        {
                            targetconn.Close();
                            targetconn.Dispose();
                        }
                    }                    
                }

                return isSuccess;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(string.Format("SqlServerSupport.FillDataToSQLlite()异常{0}{1}", ex.Message, ex.StackTrace));
                return false;
            }        
        }

        public IDbCommand GetCommand(string sql, string starttime, string endtime)
        {
            SqlCommand com = new SqlCommand();
            if (sql.Contains("@StartTime"))
            {
                SqlParameter sqlp = new SqlParameter("@StartTime", SqlDbType.DateTime, 4);
                sqlp.Value = starttime;
                com.Parameters.Add(sqlp);
            }
            if (sql.Contains("@EndTime"))
            {
                SqlParameter sqlp = new SqlParameter("@EndTime", SqlDbType.DateTime, 4);
                sqlp.Value = endtime;
                com.Parameters.Add(sqlp);
            }

            com.CommandTimeout = 36000;
            com.CommandText = sql;
            return com;
        }

        #endregion
    }
}

    */

    /*
     using System;
using System.Collections.Generic;
using System.Text;


namespace CyberResource.Transfer8.T8RemoteObjects
{
    public enum TaskSourceType
    {
        System,
        User,
    }
    [Serializable]
    public class T8Task :  ICloneable
    {
        bool _IsProcessing = false;
        /// <summary>
        /// 是否正在处理，更新时应当锁定当前对象
        /// </summary>
        public bool IsProcessing
        {
            get { return _IsProcessing; }
            set { _IsProcessing = value; }
        }
        TaskSourceType _TaskSource;
        DateTime _TaskOriginalTime;
        /// <summary>
        /// 用来判定是否已经执行过，不是具体的生成时间，对于自动上传适用
        /// </summary>
        public DateTime TaskOriginalTime
        {
            get { return _TaskOriginalTime; }
            set { _TaskOriginalTime = value; }
        }
        private DateTime _GenerateTime;
        /// <summary>
        /// 生成Task的时间
        /// </summary>
        public DateTime GenerateTime
        {
            get { return _GenerateTime; }
            set { _GenerateTime = value; }
        }
        private DateTime _EndExecTime;
        /// <summary>
        /// 结束执行的时间
        /// </summary>
        public DateTime EndExecTime
        {
            get { return _EndExecTime; }
            set { _EndExecTime = value; }
        }
        public TaskSourceType TaskSource
        {
            get { return _TaskSource; }
            set { _TaskSource = value; }
        }
        int _ExecFailureTime = 0;
        /// <summary>
        /// 方法执行失败的次数
        /// </summary>
        public int ExecFailureTime
        {
            get { return _ExecFailureTime; }
            set { _ExecFailureTime = value; }
        }
        T8TaskType _TaskType;

        string _TaskGuid;
        TaskStatus _TaskStatus;

        public TaskStatus TaskStatus
        {
            get { return _TaskStatus; }
            set { _TaskStatus = value; }
        }
        /// <summary>
        /// 任务的ID，是一个随机生成的GUID
        /// </summary>
        public string TaskGuid
        {
            get { return _TaskGuid; }
            set { _TaskGuid = value; }
        }
        public T8TaskType TaskType
        {
            get { return _TaskType; }
            set { _TaskType = value; }
        }
        List<T8SendFileInfo> _SendFilesInfo;
        List<T8DownLoadFileInfo> _DownloadingFiles;

        public List<T8DownLoadFileInfo> DownloadingFiles
        {
            get
            {
                if (_DownloadingFiles == null)
                    _DownloadingFiles = new List<T8DownLoadFileInfo>();
                return _DownloadingFiles;
            }
            set { _DownloadingFiles = value; }
        }
        public List<T8SendFileInfo> SendFilesInfo
        {
            get
            {
                if (_SendFilesInfo == null)
                    _SendFilesInfo = new List<T8SendFileInfo>();
                return _SendFilesInfo;
            }
            set { _SendFilesInfo = value; }
        }
        T8GenerateFileInfo _GenerateFileInfo;

        public T8GenerateFileInfo GenerateFileInfo
        {
            get
            {
                return _GenerateFileInfo;
            }
            set { _GenerateFileInfo = value; }
        }
        [NonSerialized]
        T8Config _Config;
        public T8Config Config
        {
            get 
            {
                if (this._Config == null)
                    _Config = T8Service.GetConfigInternal();
                return _Config; 
            }
            set { _Config = value; }
        }
        /// <summary>
        /// 已经重载，返回Task信息
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("TaskInfo=>{");
            sb.Append(string.Format("TaskType={0},TaskStatus={1},TaskGuid={2}", Enum.GetName(typeof(T8TaskType), TaskType), Enum.GetName(typeof(CyberResource.Transfer8.T8RemoteObjects.TaskStatus), this.TaskStatus), TaskGuid));
            sb.Append(string.Format(",Task TaskOriginalTime={0},TaskGenerateTime={1}", TaskOriginalTime, GenerateTime));
            if ((TaskType == T8TaskType.GenerateFile) || (TaskType == T8TaskType.GenerateAndSendFile))
            {
                T8GenerateFileInfo t = GenerateFileInfo;
                sb.Append(string.Format(",Task GenerateFile Info=>FileName={0},IsGenerated={1},LocalTempFileName={2},ReportDataItemID={3},Sql={4}", t.FileName, t.IsGenerated, t.LocalTempFileName, t.ReportDataItemID, t.Sql));
                ReportDataItem r = Config[t.ReportDataItemID];
                sb.Append(string.Format(",Task ReportDataItem Info=>Description={0},ID={1},IsCurrentDay={2},ReportStartTime={3},ReportEndTimeSpan={4},UserChangedReportSetting.StartTime={5},UserChangedReportSetting.SavedSql={6}", r.Description, r.ID, r.IsCurrentDay, r.ReportStartTime, r.ReportEndTimeSpan, r.UserChangedReportSetting.StartTime, r.UserChangedReportSetting.SavedSql));
            }
            sb.Append("}");
            return sb.ToString();
        }
        public string TaskDesc
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("任务类型\t：" + Enum.GetName(typeof(T8TaskType), this.TaskType));
                sb.AppendLine("任务状态\t：" + Enum.GetName(typeof(TaskStatus), this.TaskStatus));
                sb.AppendLine("任务来源\t：" + Enum.GetName(typeof(TaskSourceType), this.TaskSource));
                sb.AppendLine("是否还在执行\t：" + this.IsProcessing.ToString());
                if(this.TaskType==T8TaskType.DownLoadFiles)
                {
                    foreach (T8DownLoadFileInfo tdlfi in this.DownloadingFiles)
                        sb.AppendLine("已经下载\t：" + tdlfi.LocalFullFileName);
                }
                if (this.TaskType == T8TaskType.GenerateAndSendFile)
                {
                    sb.AppendLine(string.Format("任务属于\t：{0}",GenerateFileInfo.ReportDataItemID));
                    sb.AppendLine("上传的文件在\t：" + this.GenerateFileInfo.FileName);
                }
                return sb.ToString();
            }
        }
        #region ICloneable 成员
        /// <summary>
        /// 返回深层拷贝
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            T8Task task =(T8Task) this.MemberwiseClone();
            task.SendFilesInfo = new List<T8SendFileInfo>();
            foreach (T8SendFileInfo sfi in this.SendFilesInfo)
            {
                task.SendFilesInfo.Add((T8SendFileInfo)sfi.Clone());
            }
            if(GenerateFileInfo!=null)
                task.GenerateFileInfo =(T8GenerateFileInfo) this.GenerateFileInfo.Clone();
            task.DownloadingFiles = new List<T8DownLoadFileInfo>();
            foreach (T8DownLoadFileInfo tdlfi in this.DownloadingFiles)
            {
                task.DownloadingFiles.Add((T8DownLoadFileInfo)tdlfi.Clone());
            }
            return task;
        }

        #endregion
    }
}

     */
}
