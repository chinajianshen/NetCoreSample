var app = getApp();

Page({

  /**
   * 页面的初始数据
   */
  data: {
    showModal: false,
    phoneNum: '',
    mscode: '',
    smsbtnText: '发送验证码',
    smsbtnEnabled: true
  },

  inputPhoneNum: function(e){
    this.setData({
      phoneNum: e.detail.value
    });
  },

  inputSmsCode: function(e){
    this.setData({
      mscode: e.detail.value
    });
  },

  /*
   * 登录
   */
  login: function() {
    var getSession = function (code, getBack) {
      app.smartPost({
        url: '/auth/login',
        data: {
          code: code
        },
        callBack: function (json) {
          getBack(json);
        }
      });
    }

    //判断本地是否有sessionid
    wx.login({
      success: function (res) {
        if (res.code) {
          getSession(res.code, function (json) {
            if (json.data.Status == 0) {
              //校验失败
              wx.showModal({
                content: json.data.Msg,
                showCancel: false
              });
              return;
            }

            //保存会话
            wx.setStorage({
              key: 'session',
              data: json.data.SessionID
            });
            app.wx_st_sid = json.data.SessionID;

          });
        } else {
          wx.showModal({
            content: '系统异常，请稍后再试...',
            showCancel: false
          });
        }
      }
    });
    

    this.setData({
      showModal: true
    });

  },
  closeModol: function() {
    this.setData({
      showModal: false,
      phoneNum: '',
      mscode: ''
    });
  },

  //发送手机验证码
  sendMsCode: function() {
    if(!this.data.smsbtnEnabled){
      return;
    }
    var that = this;
    var phoneRegex = /^1\d{10}$/;
    var mscodeRegex = /^\d{4}$/;
    var phone = this.data.phoneNum;
    if (!phoneRegex.test(phone)){
      wx.showModal({
        content: '请输入正确的手机号！',
        showCancel: false
      });
      return;
    }

    //发送验证码
    app.smartPost({
      url: '/auth/sendsmscode',
      data: { phone: phone  },
      callBack: function(json){
        if(json.data.Status > 0){
          wx.showToast({
            title: '发送成功',
            icon: 'success',
            duration: 2000
          })
          that.setData({
            smsbtnText: '(120秒)重发',
            smsbtnEnabled: false
          });
          var index = 120;
          var intId = setInterval(function(){
            if(index > 0){
              index--;
              that.setData({
                smsbtnText: '('+ index +'秒)重发'
              });
            }else{
              clearInterval(intId);
              that.setData({
                smsbtnText: '发送验证码',
                smsbtnEnabled: true
              });
            }
            
          }, 1000);
        }else{
          wx.showModal({
            title: '错误',
            content: json.data.Msg
          })
        }
      }
    });
  },

  getAuthorName: function(){
    app.smartPost({
      url: '/auth/getauthorname',
      data: {},
      callBack: function (json) {
        if (json.data.Status == 999) {
          //会话丢失 重新登录
          login();
        } else {

          //未设置作者
          if (json.data.Status == 2) {
            wx.showModal({
              content: '您好！您的账号尚未开通作者权限，请您稍后再试，或者您可以电话联系。开卷客服：(010)64242820',
              showCancel: false
            });
            return;
          }

          app.wx_author_name = json.data.AuthorName;

          wx.redirectTo({
            url: '/pages/main/main',
          })
        }
      }
    });
  },

  //确定登录
  confirmLogin: function() {
    var that = this;
    app.smartPost({
      url: '/auth/login2',
      data: {
        LoginName: that.data.phoneNum,
        SmsCode: that.data.mscode
      },
      callBack: function (json) {
        if(json.data.Status == 1){
          //登录成功
          app.wx_push_notify = json.data.IsPushNotifyMsg;
          app.wx_expired_days = json.data.Day;
          app.wx_expired_time = json.data.IsPushNotifyMsg == 1 ? json.data.ExpiredTime.substr(0, 10) : json.data.ExpiredTime;

          that.setData({
            showModal: false,
            phoneNum: '',
            mscode: ''
          });

          that.getAuthorName();
        }else{
          //登录失败
          wx.showModal({
            content: json.data.Msg,
            showCancel: false
          });
          that.setData({
            showModal: false,
            phoneNum: '',
            mscode: ''
          });
          return;
        }
      }
    })
  },

  /*
   * 注册
   */
  regist: function() {
    var that = this;

    wx.navigateTo({
      url: '/pages/register/index/register',
    })
  },

  //使用临时码登录之后的回调方法
  loginCallBack: function() {
    var that = this;
    //登录获取用户的过期时间 及是否推送消息
    app.smartPost({
      url: '/auth/login3',
      callBack: function(json) {
        if(json.data.Status == 0){
          wx.removeStorage({
            key: 'session'
          })
        }
        
        if (json.data.Status != 1) {
          wx.showModal({
            content: json.data.Msg,
            showCancel: false
          });
          return;
        }

        //判断是否过期
        if (json.data.Status == 0){
          wx.showModal({
            content: json.data.Msg,
            showCancel: false
          });
          return;
        }

        if (json.data.ManualLogin == 1){
          //需要通过手机验证码登陆
          // that.setData({
          //   showModal: true
          // });
          return;
        }

        app.wx_push_notify = json.data.IsPushNotifyMsg;
        app.wx_expired_days = json.data.Day;
        app.wx_role_type = json.data.RoleType;
        app.wx_expired_time = json.data.IsPushNotifyMsg == 1 ? json.data.ExpiredTime.substr(0, 10) : json.data.ExpiredTime;

        that.getAuthorName();
      }
    });
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function(options) {
    var that = this;

    wx.showToast({
      title: '处理中',
      icon: 'loading',
      duration: 0
    })

    var login = function(loginCallBack) {

      var getSession = function(code, getBack) {
        app.smartPost({
          url: '/auth/login',
          data: {
            code: code
          },
          callBack: function(json) {
            getBack(json);
          }
        });
      }

      wx.login({
        success: function(res) {
          if (res.code) {
            getSession(res.code, function(json) {
              if (json.data.Status == 0) {
                //校验失败
                wx.showModal({
                  content: json.data.Msg,
                  showCancel: false
                });
                return;
              }

              //保存会话
              wx.setStorage({
                key: 'session',
                data: json.data.SessionID
              });
              app.wx_st_sid = json.data.SessionID;

              if (json.data.Status > 0) {
                if (json.data.Status == 1) {
                  //已注册账户 判断审核状态(1--待审核  2--注册审核通过  3--身份认证审核通过 4--未审核通过)
                  if (json.data.UserState == 1) {
                    wx.showModal({
                      content: '您的账号还在审核中，请耐心登录，我们会短信通知您。开卷客服：(010)64242820',
                      showCancel: false
                    });
                    return;
                  }

                  if (json.data.UserState == 4) {
                    wx.showModal({
                      content: '抱歉！您的账号审核未通过，如有疑问，请咨询开卷客服：（010）64242820',
                      showCancel: false
                    });
                    return;
                  }

                  that.loginCallBack();
                } else {
                  //未注册账户
                  //如果用户选择登录  那么登录之后 会将已有的用用户OpenID和当前手机号关联
                  //如果用户选择注册  弹出注册窗口
                  return;
                }
                
              } else {

                wx.showModal({
                  content: json.data.Msg,
                  showCancel: false
                });
              }

            });
          } else {
            wx.showModal({
              content: '系统异常，请稍后再试...',
              showCancel: false
            });
          }
        }
      });
    }

    //1. 查看本地是否存有SessionID  
    try {
      var sessionId = wx.getStorageSync('session');

      //本地存有SessionID 赋值给全局变量 获取用户信息
      if (sessionId) {
        app.wx_st_sid = sessionId;
        that.loginCallBack();
      } else {
        //本地没有 用户可以登录换取SessionID
        login();
      }
    } catch (e) {}
  },

  /**
   * 生命周期函数--监听页面初次渲染完成
   */
  onReady: function() {

  },

  /**
   * 生命周期函数--监听页面显示
   */
  onShow: function() {

  },

  /**
   * 生命周期函数--监听页面隐藏
   */
  onHide: function() {

  },

  /**
   * 生命周期函数--监听页面卸载
   */
  onUnload: function() {

  },

  /**
   * 页面相关事件处理函数--监听用户下拉动作
   */
  onPullDownRefresh: function() {

  },

  /**
   * 页面上拉触底事件的处理函数
   */
  onReachBottom: function() {

  },

  /**
   * 用户点击右上角分享
   */
  onShareAppMessage: function() {

  }
})